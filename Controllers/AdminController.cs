using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RanchDuBonheur.Controllers
{
    [Authorize]
    [Route("console-administration")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
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
        public IActionResult Artists()
        {
            return View();
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
        [Route("edit/{id}")]
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
        [Route("delete/{id}")]
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

    }
}
