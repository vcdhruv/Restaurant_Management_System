namespace Restaurant_Management_System
{
    public static class ConnectionString
    {
        private static string dbcs = "Server=DESKTOP-OMDSLBM\\SQLEXPRESS; Database=Restaurant_Management_System; Trusted_Connection=true;";
        public static string cs { get => dbcs; }
    }
}
