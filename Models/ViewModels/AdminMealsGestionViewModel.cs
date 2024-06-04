using RanchDuBonheur.Models.Pocos.Artists;
using RanchDuBonheur.Models.Pocos.Meals;

namespace RanchDuBonheur.Models.ViewModels;

public class AdminMealsGestionViewModel
{
    public List<Artist> Artists { get; set; } = [];
    public List<Dish> Dishes { get; set; } = [];
    public Meal NewMeal { get; set; } = new();
}