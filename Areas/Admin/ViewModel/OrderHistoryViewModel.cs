namespace Restaurant_Management_System.Areas.Admin.ViewModel
{
    public class OrderHistoryViewModel
    {
        public int UserID { get; set; }
        public int OrderID { get; set; }
        public string UserName { get; set; }
        public Status Status { get; set; }
        public DateTime OrderDate { get; set; }
        public float TotalAmount { get; set; }
    }
    public enum Status
    {
        Preparing,
        Dispatched,
        Delivered,
        Canceled
    }
}
