using System.ComponentModel.DataAnnotations;

namespace Restaurant_Management_System.Areas.User.ViewModel
{
    public class OrderViewModel
    {
        public int UserID { get; set; }
        public int CartID { get; set; }
        public int OrderID { get; set; }
        public int PizzaID { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string ImagePath { get; set; }
        public string Status { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public float TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
