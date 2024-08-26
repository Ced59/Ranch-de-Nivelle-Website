using RanchDuBonheur.Models.Pocos.Artists;
using RanchDuBonheur.Models.ViewModels.Shared;

namespace RanchDuBonheur.Models.ViewModels;

public class ArtistDetailsViewModel
{
    public required Artist Artist { get; set; }
    public List<MealInfo>? Meals { get; set; }
    public List<VideoInfo>? Videos { get; set; }
}

public class MealInfo
{
    public DateOnly Date { get; set; }
    public Guid Id { get; set; }
}