using RanchDuBonheur.Models.Pocos.Artists;
using System.ComponentModel.DataAnnotations;

namespace RanchDuBonheur.Models.Pocos.Meals
{
    public class Meal
    {
        public Guid Id { get; set; }
        public DateOnly Date { get; set; }
        public decimal Price { get; set; }

        [MaxLength(2500)]
        public string? Commentaires { get; set; }
        public ICollection<MealDish> MealDishes { get; set; } = new List<MealDish>();
        public ICollection<MealArtist> MealArtists { get; set; } = new List<MealArtist>();
    }

    public class MealDish
    {
        [Key]
        public Guid MailDishId { get; set; }
        public Guid MealId { get; set; }
        public Meal Meal { get; set; } = null!;

        public Guid DishId { get; set; }
        public Dish Dish { get; set; } = null!;
    }

    public class MealArtist
    {
        [Key]
        public Guid MailArtistId { get; set; }
        public Guid MealId { get; set; }
        public Meal Meal { get; set; } = null!;

        public Guid ArtistId { get; set; }
        public Artist Artist { get; set; } = null!;
    }
}
