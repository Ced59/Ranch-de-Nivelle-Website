using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RanchDuBonheur.Models.Pocos.Meals
{
    public class Dish
    {
        public Guid Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = "";

        [Required]
        public DishCategory Category { get; set; }

        public ICollection<MealDish> MealDishes { get; set; } = new List<MealDish>();
    }


    public enum DishCategory
    {
        [Description("Apéritif")]
        APERITIF,

        [Description("Entrée")]
        STARTER,

        [Description("Plat Principal")]
        MAIN_COURSE,

        [Description("Dessert")]
        DESSERT
    }

}
