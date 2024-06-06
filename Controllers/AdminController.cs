﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RanchDuBonheur.Data;
using RanchDuBonheur.Models.Pocos.Artists;
using RanchDuBonheur.Models.Pocos.Meals;
using RanchDuBonheur.Models.ViewModels;
using RanchDuBonheur.Services.Interfaces;

namespace RanchDuBonheur.Controllers
{
    [Authorize]
    [Route("console-administration")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RanchDbContext _context;
        private readonly IPhotoService _photoService;

        public AdminController(UserManager<IdentityUser> userManager, RanchDbContext context, IPhotoService photoService)
        {
            _userManager = userManager;
            _context = context;
            _photoService = photoService;
        }


        [Route("accueil")]
        public IActionResult AdminIndex()
        {
            return View();
        }

        [Route("utilisateurs")]
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            var currentUserId = _userManager.GetUserId(User);
            ViewData["CurrentUserId"] = currentUserId;
            return View(users);
        }

        [Route("artistes")]
        public async Task<IActionResult> Artists()
        {
            var artists = await _context.Artists.ToListAsync();
            return View(artists);
        }

        [Route("repas")]
        public async Task<IActionResult> Meals()
        {
            var meals = await _context.Meals.ToListAsync();

            return View(meals);
        }

        [HttpPost]
        [Route("delete-meal/{mealId}")]
        public async Task<IActionResult> DeleteMeal(Guid mealId)
        {
            var meal = await _context.Meals.Include(m => m.MealArtists).Include(m => m.MealDishes)
                .FirstOrDefaultAsync(m => m.Id == mealId);

            if (meal == null)
            {
                TempData["Error"] = "Le repas spécifié n'a pas été trouvé.";
                return RedirectToAction("Meals");
            }

            _context.MealArtists.RemoveRange(meal.MealArtists);
            _context.MealDishes.RemoveRange(meal.MealDishes);

            _context.Meals.Remove(meal);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Le repas a été supprimé avec succès.";
            return RedirectToAction("Meals");
        }

        [HttpGet]
        [Route("edit-meal/{mealId}")]
        public async Task<IActionResult> EditMeal(Guid mealId)
        {
            // Récupération du repas à partir de l'ID
            var meal = await _context.Meals.FindAsync(mealId);

            if (meal == null)
            {
                TempData["Error"] = "Le repas spécifié n'a pas été trouvé.";
                return RedirectToAction("Meals");
            }

            // TODO: Préparer et passer les données nécessaires à la vue de modification
            // Pour l'instant, vous pouvez simplement retourner la vue avec le repas récupéré
            return RedirectToAction("Meals");
        }


        [Route("add")]
        public async Task<IActionResult> AddMeal()
        {
            var artists = await _context.Artists.ToListAsync();
            var dishes = await _context.Dishes.Where(d => d.Category == DishCategory.APERITIF).ToListAsync();
            var newMeal = new Meal
            {
                Date = DateOnly.FromDateTime(DateTime.Now),
                Price = 40
            };

            var viewModel = new AdminMealsGestionViewModel
            {
                Artists = artists,
                Dishes = dishes,
                NewMeal = newMeal,
                SelectedCategory = DishCategory.APERITIF
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("add-meal")]
        public async Task<IActionResult> AddMealAction(AdminMealsGestionViewModel model)
        {
            if (model.NewMeal.Date < DateOnly.FromDateTime(DateTime.Now))
            {
                ModelState.AddModelError("NewMeal.Date", "La date doit être aujourd'hui ou une date future.");
            }

            bool mealExists = await _context.Meals.AnyAsync(m => m.Date == model.NewMeal.Date);
            if (mealExists)
            {
                TempData["Error"] = "Un repas est déjà planifié pour cette date. Veuillez choisir une autre date.";
                model.Artists = await _context.Artists.ToListAsync();
                model.Dishes = await _context.Dishes.ToListAsync();
                model.NewMeal = new Meal
                {
                    Date = DateOnly.FromDateTime(DateTime.Now) // S'assurer que la date par défaut est aujourd'hui pour le réaffichage
                };
                return View("AddMeal", model);
            }

            if (ModelState.IsValid)
            {
                // Création et configuration du repas
                var meal = new Meal
                {
                    Date = model.NewMeal.Date,
                    Price = model.NewMeal.Price,
                    Commentaires = model.NewMeal.Commentaires
                };

                // Ajouter le repas à la base de données
                _context.Meals.Add(meal);
                await _context.SaveChangesAsync();

                // Enregistrer les relations avec les artistes assignés
                foreach (var artistId in model.AssignedArtistIds)
                {
                    var mealArtist = new MealArtist
                    {
                        MealId = meal.Id,
                        ArtistId = artistId
                    };
                    _context.MealArtists.Add(mealArtist);
                }

                // Enregistrer les relations avec les plats assignés
                foreach (var dishId in model.AssignedDishIds)
                {
                    var mealDish = new MealDish
                    {
                        MealId = meal.Id,
                        DishId = dishId
                    };
                    _context.MealDishes.Add(mealDish);
                }

                // Sauvegarde des changements dans la base de données
                await _context.SaveChangesAsync();

                TempData["Success"] = "Repas ajouté avec succès.";
                return RedirectToAction("Meals");
            }

            // Recharger les dépendances nécessaires pour le modèle si le ModelState n'est pas valide
            model.Artists = await _context.Artists.ToListAsync();
            model.Dishes = await _context.Dishes.ToListAsync();
            model.NewMeal = new Meal
            {
                Date = DateOnly.FromDateTime(DateTime.Now)
            };

            return View("AddMeal", model);
        }


        [HttpPost]
        [Route("assign-dish")]
        public async Task<IActionResult> AssignDish(AdminMealsGestionViewModel model, List<Guid> newAssignedDishIds)
        {
            // Récupération des IDs déjà assignés et ajout des nouveaux
            var currentAssignedDishIds = model.AssignedDishIds ?? new List<Guid>();
            currentAssignedDishIds.AddRange(newAssignedDishIds.Distinct());
            model.AssignedDishIds = currentAssignedDishIds.Distinct().ToList();

            model = await RebuildViewModel(model);
            return View("AddMeal", model);
        }

        [HttpPost]
        [Route("unassign-dish")]
        public async Task<IActionResult> UnassignDish(Guid dishId, AdminMealsGestionViewModel model)
        {
            model.AssignedDishIds.Remove(dishId);
            model = await RebuildViewModel(model);

            return View("AddMeal", model);
        }

        [HttpPost]
        [Route("assign-artists")]
        public async Task<IActionResult> AssignArtists(AdminMealsGestionViewModel model, List<Guid> newAssignedArtistIds)
        {
            // Ajouter les nouveaux IDs sans perdre les anciens
            model.AssignedArtistIds.AddRange(newAssignedArtistIds);
            model.AssignedArtistIds = model.AssignedArtistIds.Distinct().ToList();

            // Recharger le modèle complet
            model = await RebuildViewModel(model);

            TempData["Success"] = "Artistes assignés avec succès.";
            return View("AddMeal", model);
        }

        [HttpPost]
        [Route("unassign-artist")]
        public async Task<IActionResult> UnassignArtist(Guid artistId, AdminMealsGestionViewModel model)
        {
            model.AssignedArtistIds.Remove(artistId);
            model = await RebuildViewModel(model);

            return View("AddMeal", model);
        }


        [HttpPost]
        [Route("add-dish")]
        public async Task<IActionResult> AddDish(AdminMealsGestionViewModel model, string name, DishCategory category)
        {
            // Vérifier si le plat existe déjà en base de données
            var dishExists = await _context.Dishes.AnyAsync(d => d.Name == name);
            if (dishExists)
            {
                TempData["Error"] = "Un plat avec ce nom existe déjà.";

                model = await RebuildViewModel(model);

                return View("AddMeal", model);
            }

            // Créer et ajouter le nouveau plat
            var dish = new Dish { Name = name, Category = category };
            _context.Dishes.Add(dish);
            await _context.SaveChangesAsync();

            model = await RebuildViewModel(model);

            TempData["Success"] = "Plat ajouté avec succès.";
            return View("AddMeal", model);
        }

        [HttpPost]
        [Route("delete-dish")]
        public async Task<IActionResult> DeleteDish(Guid dishId)
        {
            var dish = await _context.Dishes.FindAsync(dishId);
            if (dish != null)
            {
                _context.Dishes.Remove(dish);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Plat supprimé avec succès.";
            }
            else
            {
                TempData["Error"] = "Plat non trouvé.";
            }
            return RedirectToAction("AddMeal"); // Assurez-vous que cette redirection est correcte selon votre architecture de routing
        }




        [HttpPost]
        public async Task<IActionResult> FilterDishesByCategory(AdminMealsGestionViewModel model)
        {
            var filteredDishes = await _context.Dishes
                .Where(d => d.Category == model.SelectedCategory)
                .ToListAsync();

            model = await RebuildViewModel(model);
            model.Dishes = filteredDishes;
            model.SelectedCategory = model.SelectedCategory;

            return View("AddMeal", model);
        }





        [Route("infos-salle")]
        public IActionResult RoomInfo()
        {
            return View();
        }

        [HttpPost]
        [Route("edit-user/{id}")]
        public async Task<IActionResult> Edit(string id, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["Error"] = "Utilisateur non trouvé.";
                return RedirectToAction("Users");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (result.Succeeded)
            {
                TempData["Success"] = "Mot de passe mis à jour avec succès.";
            }
            else
            {
                TempData["Error"] = "Échec de la mise à jour du mot de passe.";
            }
            return RedirectToAction("Users");
        }



        [HttpPost]
        [Authorize]
        [Route("delete-user/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var currentUserID = _userManager.GetUserId(User);
            if (currentUserID == id)
            {

                TempData["Error"] = "Vous ne pouvez pas supprimer votre propre compte.";
                return RedirectToAction("Users");
            }

            var userToDelete = await _userManager.FindByIdAsync(id);
            if (userToDelete != null)
            {
                if (userToDelete.Email == "c.caudron59@gmail.com")
                {
                    TempData["Error"] = "Vous ne pouvez pas supprimer le compte spécial.";
                    return RedirectToAction("Users");
                }

                var result = await _userManager.DeleteAsync(userToDelete);
                if (result.Succeeded)
                {
                    TempData["Success"] = "Utilisateur supprimé avec succès.";
                }
                else
                {
                    TempData["Error"] = "Erreur lors de la suppression de l'utilisateur.";
                }
            }
            else
            {
                TempData["Error"] = "Utilisateur non trouvé.";
            }

            return RedirectToAction("Users");
        }

        [HttpPost]
        [Route("add-user")]
        public async Task<IActionResult> AddUser(string userName, string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                TempData["Error"] = "L'email est déjà utilisé par un autre compte.";
                return RedirectToAction("Users");
            }

            var user = new IdentityUser { UserName = userName, Email = email };
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                // Confirmer directement l'email de l'utilisateur
                var confirmEmailResult = await _userManager.ConfirmEmailAsync(user, await _userManager.GenerateEmailConfirmationTokenAsync(user));
                if (confirmEmailResult.Succeeded)
                {
                    TempData["Success"] = "Utilisateur ajouté avec succès.";
                    return RedirectToAction("Users");
                }
                else
                {
                    TempData["Error"] = "Erreur lors de la confirmation de l'e-mail.";
                    foreach (var error in confirmEmailResult.Errors)
                    {
                        TempData["Error"] += " " + error.Description;
                    }
                    return RedirectToAction("Users");
                }
            }

            TempData["Error"] = string.Join(" ", result.Errors.Select(e => e.Description));
            return RedirectToAction("Users");
        }


        [HttpPost]
        [Route("edit-artist/{id}")]
        public async Task<IActionResult> EditArtist(Guid id, string name, string description, IFormFile photo)
        {
            var artist = await _context.Artists.FindAsync(id);
            if (artist == null)
            {
                TempData["Error"] = "Artiste non trouvé.";
                return RedirectToAction("Artists");
            }

            artist.Name = name;
            artist.Description = description;

            if (photo != null && photo.Length > 0)
            {
                if (!string.IsNullOrWhiteSpace(artist.PhotoUrl))
                {
                    _photoService.DeletePhoto(artist.PhotoUrl);
                }

                var photoResult = await _photoService.UploadPhotoAsync(name, photo, "artists");
                if (photoResult.IsSuccess)
                {
                    artist.PhotoUrl = photoResult.PhotoPath;
                }
                else
                {
                    TempData["Error"] = "Échec de l'upload de la photo.";
                    return RedirectToAction("Artists");
                }
            }

            _context.Artists.Update(artist);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Artiste modifié avec succès.";
            return RedirectToAction("Artists");
        }


        [HttpPost]
        [Route("delete-artist/{id}")]
        public async Task<IActionResult> DeleteArtist(Guid id)
        {
            var artist = await _context.Artists.FindAsync(id);
            if (artist == null)
            {
                TempData["Error"] = "Artiste non trouvé.";
                return RedirectToAction("Artists");
            }

            if (!string.IsNullOrEmpty(artist.PhotoUrl))
            {
                var deletePhotoResult = _photoService.DeletePhoto(artist.PhotoUrl);
                if (!deletePhotoResult)
                {
                    TempData["Error"] = "Erreur lors de la suppression de la photo de l'artiste.";
                    return RedirectToAction("Artists");
                }
            }

            _context.Artists.Remove(artist);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Artiste supprimé avec succès.";
            return RedirectToAction("Artists");
        }

        [HttpPost]
        [Route("add-artist")]
        public async Task<IActionResult> AddArtist(string name, string description, IFormFile photo)
        {
            // Création de l'objet Artist
            var artist = new Artist
            {
                Name = name,
                Description = description
            };

            if (ModelState.IsValid)
            {
                try
                {
                    // Gestion de la photo
                    if (photo.Length > 0)
                    {
                        var photoResult = await _photoService.UploadPhotoAsync(name, photo, "artists");
                        if (photoResult.IsSuccess)
                        {
                            artist.PhotoUrl = photoResult.PhotoPath;
                        }
                        else
                        {
                            TempData["Error"] = "Échec de l'upload de la photo.";
                            return RedirectToAction("Artists");
                        }
                    }

                    _context.Artists.Add(artist);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Artists");
                }
                catch (DbUpdateException)
                {
                    TempData["Error"] = "Erreur lors de l'ajout de l'artiste";
                }
            }

            return RedirectToAction("Artists");
        }

        private async Task<AdminMealsGestionViewModel> RebuildViewModel(AdminMealsGestionViewModel model)
        {
            var allArtists = await _context.Artists.ToListAsync();
            var allDishes = await _context.Dishes.ToListAsync();

            model.Artists = allArtists.Where(a => !model.AssignedArtistIds.Contains(a.Id)).ToList();
            model.AssignedArtists = allArtists.Where(a => model.AssignedArtistIds.Contains(a.Id)).ToList();

            model.Dishes = allDishes.Where(d => !model.AssignedDishIds.Contains(d.Id) && d.Category == model.SelectedCategory).ToList();
            model.AssignedDishes = allDishes.Where(d => model.AssignedDishIds.Contains(d.Id)).ToList();

            model.NewMeal = new Meal
            {
                Date = model.NewMeal.Date,
                Price = model.NewMeal.Price,
                Commentaires = model.NewMeal.Commentaires
            };

            return model;
        }


    }
}
