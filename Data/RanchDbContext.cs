using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RanchDuBonheur.Models.Pocos.Artists;
using RanchDuBonheur.Models.Pocos.Meals;

namespace RanchDuBonheur.Data
{
    public class RanchDbContext(DbContextOptions<RanchDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<MealDish> MealDishes { get; set; }
        public DbSet<MealArtist> MealArtists { get; set; }
        public DbSet<Video> Videos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure Artist entity
            builder.Entity<Artist>(entity =>
            {
                entity.ToTable("Artists");
                entity.HasKey(a => a.Id);

                entity.Property(a => a.Id)
                    .HasColumnType("uniqueidentifier")
                    .IsRequired();

                entity.Property(a => a.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)");

                entity.Property(a => a.Description)
                    .HasMaxLength(7000)
                    .HasColumnType("nvarchar(max)");

                entity.Property(a => a.PhotoUrl)
                    .HasMaxLength(255)
                    .HasColumnType("nvarchar(255)");

                entity.HasMany(a => a.Videos)
                    .WithOne(v => v.Artist)
                    .HasForeignKey(v => v.ArtistId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Video entity
            builder.Entity<Video>(entity =>
            {
                entity.ToTable("Videos");
                entity.HasKey(v => v.Id);

                entity.Property(v => v.Id)
                    .HasColumnType("uniqueidentifier")
                    .IsRequired();

                entity.Property(v => v.Title)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)");

                entity.Property(v => v.Description)
                    .HasMaxLength(7000)
                    .HasColumnType("nvarchar(max)");

                entity.Property(v => v.YtLink)
                    .HasMaxLength(255)
                    .HasColumnType("nvarchar(255)");
            });

            // Configure Dish entity
            builder.Entity<Dish>(entity =>
            {
                entity.ToTable("Dishes");
                entity.HasKey(d => d.Id);

                entity.Property(d => d.Id)
                    .HasColumnType("uniqueidentifier")
                    .IsRequired();

                entity.Property(d => d.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)");

                entity.Property(d => d.Category)
                    .IsRequired();
            });

            // Configure Meal entity
            builder.Entity<Meal>(entity =>
            {
                entity.ToTable("Meals");
                entity.HasKey(m => m.Id);

                entity.Property(m => m.Id)
                    .HasColumnType("uniqueidentifier")
                    .IsRequired();

                entity.Property(m => m.Date)
                    .HasColumnType("date");

                entity.Property(m => m.Price)
                    .HasColumnType("decimal");

                // Configure Many-to-Many Relationships
                entity.HasMany(m => m.MealDishes)
                    .WithOne(md => md.Meal)
                    .HasForeignKey(md => md.MealId);

                entity.HasMany(m => m.MealArtists)
                    .WithOne(ma => ma.Meal)
                    .HasForeignKey(ma => ma.MealId);
            });
        }
    }
}
