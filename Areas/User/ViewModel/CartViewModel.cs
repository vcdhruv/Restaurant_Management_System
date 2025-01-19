using System.ComponentModel.DataAnnotations;

namespace Restaurant_Management_System.Areas.User.ViewModel
{
    public class CartViewModel
    {
        public int UserID { get; set; }
        public int ItemID { get; set; }
        public int PizzaID { get; set; }
        public int CartID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public int Quanity { get; set; }
        [Required]
        public float TotalAmount { get; set; }
    }
}
