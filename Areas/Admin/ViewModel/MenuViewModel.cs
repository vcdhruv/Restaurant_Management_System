using System.ComponentModel.DataAnnotations;

namespace Restaurant_Management_System.Areas.Admin.ViewModel
{
    public class MenuViewModel
    {
        [Required]
        public int PizzaID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public IFormFile ImagePath { get; set; }
    }
}
