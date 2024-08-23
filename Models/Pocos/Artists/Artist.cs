using RanchDuBonheur.Models.Pocos.Meals;
using System.ComponentModel.DataAnnotations;

namespace RanchDuBonheur.Models.Pocos.Artists
{
    public class Artist
    {
        public Guid Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = "";

        [MaxLength(2500)]
        public string Description { get; set; } = "";

        [MaxLength(255)]
        public string PhotoUrl { get; set; } = "";

        public ICollection<MealArtist> MealArtists { get; set; } = new List<MealArtist>();
        public ICollection<Video> Videos { get; set; } = new List<Video>();
    }
}
