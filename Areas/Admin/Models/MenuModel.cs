using System.ComponentModel.DataAnnotations;

namespace Restaurant_Management_System.Areas.Admin.Models
{
    public class MenuModel
    {
        public int PizzaID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public string ImagePath { get; set; }
    }
}
