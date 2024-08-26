using RanchDuBonheur.Models.Pocos.Artists;
using RanchDuBonheur.Models.ViewModels.Shared;

namespace RanchDuBonheur.Models.ViewModels
{
    public class ArtistVideosViewModel
    {
        public required Artist Artist { get; set; }
        public List<VideoInfo>? Videos { get; set; }
        public Guid IdSelectedVideo { get; set; }
    }

}
