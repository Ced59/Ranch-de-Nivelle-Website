using System.ComponentModel.DataAnnotations;

namespace RanchDuBonheur.Models.Pocos.Meals
{
    public class Dish
    {
        public Guid Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = "";

        public ICollection<MealDish> MealDishes { get; set; } = new List<MealDish>();
    }
}
