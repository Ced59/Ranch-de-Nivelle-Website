using System.ComponentModel.DataAnnotations;

namespace RanchDuBonheur.Models.Pocos.Artists
{
    public class Video
    {
        public Guid Id { get; set; }

        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(2500)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(255)]
        public string YtLink { get; set; } = string.Empty;

        public Artist Artist { get; set; }

        public Guid ArtistId { get; set; }
    }
}
