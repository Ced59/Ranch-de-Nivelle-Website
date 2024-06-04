﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RanchDuBonheur.Models.Pocos.Artists;
using RanchDuBonheur.Models.Pocos.Meals;

namespace RanchDuBonheur.Data
{
    public class RanchDbContext : IdentityDbContext
    {
        public RanchDbContext(DbContextOptions<RanchDbContext> options)
            : base(options)
        {
        }

        public DbSet<Artist> Artists { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Meal> Meals { get; set; }

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
                    .HasColumnType("nvarchar(7000)");

                entity.Property(a => a.PhotoUrl)
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
