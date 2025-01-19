namespace Restaurant_Management_System.Areas.Admin.Models
{
    public class OrderHistoryModel
    {
        public int UserID { get; set; }
        public int OrderID { get; set; }
        public string UserName { get; set; }
        public String Status { get; set; }
        public DateTime OrderDate { get; set; }
        public float TotalAmount { get; set; }
    }
}
