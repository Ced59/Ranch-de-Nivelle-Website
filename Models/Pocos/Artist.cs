using System.ComponentModel.DataAnnotations;

namespace RanchDuBonheur.Models.Pocos
{
    public class Artist
    {
        public Guid Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = "";

        [MaxLength(1000)]
        public string Description { get; set; } = "";

        [MaxLength(255)]
        public string PhotoUrl { get; set; } = "";
    }
}
