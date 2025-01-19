namespace Restaurant_Management_System.Areas.User.Models
{
    public class OrderItemsModel
    {
        public int ItemID { get; set; }
        public int OrderID { get; set; }
        public int PizzaID { get; set; }
        public int Quantity { get; set; }
    }
}
