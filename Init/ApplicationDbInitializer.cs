using Microsoft.AspNetCore.Identity;

namespace RanchDuBonheur.Init
{
    public static class ApplicationDbInitializer
    {
        public static async Task SeedUsers(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            var username = configuration["DefaultUser:Username"];
            var email = configuration["DefaultUser:Email"];
            var password = configuration["DefaultUser:Password"];

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = username,
                    Email = email,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to create default user.");
                }

                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                await userManager.ConfirmEmailAsync(user, token);
            }
        }
    }
}
