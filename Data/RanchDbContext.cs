using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RanchDuBonheur.Models.Pocos.Artists;

namespace RanchDuBonheur.Data
{
    public class RanchDbContext : IdentityDbContext
    {
        public RanchDbContext(DbContextOptions<RanchDbContext> options)
            : base(options)
        {
        }

        public DbSet<Artist> Artists { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Artist>(entity =>
            {
                entity.ToTable("Artists");

                entity.HasKey(a => a.Id);

                entity.Property(a => a.Id).HasColumnType("uniqueidentifier").IsRequired();
                entity.Property(a => a.Name)
                    .IsRequired() 
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)"); 

                entity.Property(a => a.Description)
                    .HasMaxLength(1000)
                    .HasColumnType("nvarchar(1000)");

                entity.Property(a => a.PhotoUrl)
                    .HasMaxLength(255)
                    .HasColumnType("nvarchar(255)"); 
            });
        }

    }
}
