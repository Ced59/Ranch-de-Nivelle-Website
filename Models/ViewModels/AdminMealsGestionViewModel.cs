using RanchDuBonheur.Models.Pocos.Artists;
using RanchDuBonheur.Models.Pocos.Meals;

namespace RanchDuBonheur.Models.ViewModels;

public class AdminMealsGestionViewModel
{
    public List<Artist> Artists { get; set; } = [];
    public List<Guid> AssignedArtistIds { get; set; } = [];
    public List<Artist> AssignedArtists { get; set; } = [];
    public List<Dish> Dishes { get; set; } = [];
    public List<Guid> AssignedDishIds { get; set; } = [];
    public List<Dish> AssignedDishes { get; set; } = [];
    public Meal NewMeal { get; set; } = new();
}
