using Microsoft.AspNetCore.SignalR;
using Restaurant_Management_System.Areas.Admin.Models;
using Restaurant_Management_System.Areas.Admin.ViewModel;
using Restaurant_Management_System.Areas.User.ViewModel;
using Restaurant_Management_System.BAL;
using System.Data.SqlClient;

namespace Restaurant_Management_System.DAL
{
    public class DAL_User
    {
        string cs = ConnectionString.cs;

        public void AddToCart(int PizzaID)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = conn.CreateCommand();
                int UserID = (int)CV.UserID();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "PR_Cart_Insert";
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@PizzaID", PizzaID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<CartViewModel> SelectAllCartItems()
        {
            List<CartViewModel> cartItems = new List<CartViewModel>();
            using(SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "PR_Cart_SelectAll";
                cmd.Parameters.AddWithValue("@UserID", CV.UserID());
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CartViewModel item = new CartViewModel();
                        item.ItemID = (int)reader["ItemID"];
                        item.UserID = (int)reader["UserID"];
                        item.Price = (float)Convert.ToDouble(reader["Price"]);
                        item.Quanity = (int)reader["Quantity"];
                        item.CartID = (int)reader["CartID"];
                        item.PizzaID = (int)reader["PizzaID"];
                        item.TotalAmount = item.Price * item.Quanity;
                        item.Name = (string)reader["Name"];
                        cartItems.Add(item);
                    }
                }
            }
            return cartItems;
        }

        public void UpdateQuantity(int pizzaID , int qty)
        {
            using (SqlConnection conn = new SqlConnection(cs)){
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "PR_Cart_UpdateQty";
                cmd.Parameters.AddWithValue("@PizzaID", pizzaID);
                cmd.Parameters.AddWithValue("@Quantity", qty);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool DeleteCartItem(int itemID)
        {
            using (SqlConnection conn = new SqlConnection(cs)) 
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "PR_Delete_CartItems";
                cmd.Parameters.AddWithValue("@ItemID", itemID);
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            return false;
        }

        public void PlaceOrder(int CartID, float payable_amount)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "PR_Order_Insert";
                cmd.Parameters.AddWithValue("@UserID", CV.UserID());
                cmd.Parameters.AddWithValue("@TotalAmount", payable_amount);
                cmd.Parameters.AddWithValue("@Status", "Pending");
                cmd.Parameters.AddWithValue("@CartID", CartID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<OrderViewModel> GetOrders() 
        {
            List<OrderViewModel> orders = new List<OrderViewModel>();
            using(SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "PR_Order_SelectAll";
                cmd.Parameters.AddWithValue("@UserID", CV.UserID());
                conn.Open();
                SqlDataReader r = cmd.ExecuteReader();
                if (r.HasRows)
                {
                    while (r.Read())
                    {
                        OrderViewModel order = new OrderViewModel();
                        order.PizzaID = (int)r["PizzaID"];
                        order.ImagePath = r["ImagePath"].ToString();
                        order.Name = (string)r["Name"];
                        order.Quantity = (int)r["Quantity"];
                        orders.Add(order);
                    }
                }
            }
            return orders;
        }

        // Method to notify admins about a new order in history
        public OrderViewModel getConfirmOrderDetails()
        {
            OrderViewModel order = new OrderViewModel();
            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "PR_Order_SelectOrderDetails";
                cmd.Parameters.AddWithValue("@UserID", CV.UserID());
                cmd.Parameters.AddWithValue("@UserName", CV.UserName());
                conn.Open();
                SqlDataReader r = cmd.ExecuteReader();
                if (r.HasRows) 
                {
                    while (r.Read()) 
                    {
                        order.OrderID = (int)r["OrderID"];
                        order.TotalAmount = (float)Convert.ToDouble(r["TotalAmount"]);
                        order.Status = r["Status"].ToString();
                        order.OrderDate = Convert.ToDateTime(r["OrderDate"]);
                    }
                }
            }

            return order;
        }

        public List<OrderHistoryModel> getOrderHistory()
        {
            List<OrderHistoryModel> historyList = new List<OrderHistoryModel>();
            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "PR_OrderHistory_SelectAll";
                cmd.Parameters.AddWithValue("@UserName", CV.UserName());
                conn.Open();
                SqlDataReader r = cmd.ExecuteReader();
                if (r.HasRows)
                {
                    while (r.Read())
                    {
                        OrderHistoryModel order = new OrderHistoryModel();
                        order.OrderID = (int)r["OrderID"];
                        order.TotalAmount = (float)Convert.ToDouble(r["TotalAmount"]);
                        order.Status = r["Status"].ToString();
                        order.UserName = r["UserName"].ToString();
                        order.OrderDate = Convert.ToDateTime(r["OrderDate"]);
                        historyList.Add(order);
                    }
                }
            }
            return historyList;
        }

    }
}
