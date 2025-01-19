namespace Restaurant_Management_System.Areas.User.Models
{
    public class OrdersModel
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public DateTime OrderData { get; set; }
        public float TotalAmount { get; set; }
        public string Status { get; set; }
    }
}
