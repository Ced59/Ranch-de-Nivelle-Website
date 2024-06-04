using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RanchDuBonheur.Data;
using RanchDuBonheur.Init;
using RanchDuBonheur.Services.Implementations;
using RanchDuBonheur.Services.Interfaces;

namespace RanchDuBonheur
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<RanchDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<RanchDbContext>();
            builder.Services.AddControllersWithViews();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login"; 
                options.AccessDeniedPath = "/Identity/Account/AccessDenied"; 
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            });

            builder.Services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
            });

            builder.Services.AddScoped<ImageProcessingService>();
            builder.Services.AddScoped<IPhotoService, PhotoService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            //ApplicationDbInitializer.SeedUsers(userManager, configuration).Wait();

            app.Run();
        }
    }
}
