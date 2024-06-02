﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RanchDuBonheur.Data;
using RanchDuBonheur.Models.Pocos;
using RanchDuBonheur.Services.Implementations;
using RanchDuBonheur.Services.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public IActionResult Meals()
        {
            return View();
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
                TempData["Success"] = "Utilisateur ajouté avec succès.";
                return RedirectToAction("Users");
            }

            foreach (var error in result.Errors)
            {
                TempData["Error"] = error.Description;
            }
            return RedirectToAction("Users");
        }

        [HttpPost]
        [Route("edit-artist/{id}")]
        public async Task<IActionResult> EditArtist(string id, IFormFile photo)
        {
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

    }
}
