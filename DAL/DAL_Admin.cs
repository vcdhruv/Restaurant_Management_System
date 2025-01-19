using Restaurant_Management_System.Areas.Register.Models;
using System.Data.SqlClient;
using System.Data;
using Restaurant_Management_System.Areas.Admin.ViewModel;
using Restaurant_Management_System.Areas.Admin.Models;
using Restaurant_Management_System.BAL;


namespace Restaurant_Management_System.DAL
{
    public class DAL_Admin
    {
        #region Connection String
        string cs = ConnectionString.cs;
        #endregion

        #region Method To Convert String To IFormFile Type
        public IFormFile ConvertFilePathToIFormFile(string relativePath,IWebHostEnvironment env)
        {
            // Combine the web root path with the relative path
            var fullPath = Path.Combine(env.WebRootPath, relativePath);

            // Ensure the file exists
            if (!File.Exists(fullPath))
                throw new FileNotFoundException("File not found", fullPath);

            // Create a FileStream and FormFile object
            var fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            var fileName = Path.GetFileName(fullPath);

            var formFile = new FormFile(fileStream, 0, fileStream.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/octet-stream"
            };

            return formFile;
        }
        #endregion

        #region Datatable approach
        //public DataTable SelectAllUsers() this is using datatable approach
        //{
        //    DataTable dt;
        //    using (SqlConnection conn = new SqlConnection(cs))
        //    {
        //        SqlCommand cmd = conn.CreateCommand();
        //        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //        cmd.CommandText = "PR_User_SelectAllUsers";
        //        conn.Open();
        //        SqlDataReader dr = cmd.ExecuteReader();
        //        dt = new DataTable();
        //        if (dr.HasRows) 
        //        {
        //            dt.Load(dr);
        //        }
        //    }
        //    return dt;
        //}
        #endregion

        #region Select All Users Having A Role As User
        public List<RegisterationModel> SelectAllUsers()
        {
            List<RegisterationModel> registrationModels = new List<RegisterationModel>();
            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_User_SelectAllUsers";
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    RegisterationModel user = new RegisterationModel();
                    user.UserID = Convert.ToInt32(reader["UserID"]);
                    user.Name = reader["Name"].ToString() ?? "";
                    user.Email = reader["Email"].ToString() ?? "";
                    user.Phone = reader["Phone"].ToString() ?? "";
                    user.Address = reader["Address"].ToString() ?? "";
                    user.Is_Active = Convert.ToInt32(reader["Is_Active"]);
                    registrationModels.Add(user);
                }
            }
            return registrationModels;
        }
        #endregion

        #region Select User By ID
        public RegisterationModel SelectUserByID(int? UserID)
        {
            RegisterationModel user = new RegisterationModel();
            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_User_SelectByID";
                cmd.Parameters.AddWithValue("@UserID", UserID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    user.UserID = Convert.ToInt32(reader["UserID"]);
                    user.Name = reader["Name"].ToString() ?? "";
                    user.Email = reader["Email"].ToString() ?? "";
                    user.Phone = reader["Phone"].ToString() ?? "";
                    user.Address = reader["Address"].ToString() ?? "";
                    user.Is_Active = Convert.ToInt32(reader["Is_Active"]);
                }
                return user;
            }
        }
        #endregion

        #region Select Is_Active State Of User
        public int SelectStateOfUser(int? id)
        {
            int is_active = 100;
            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_User_SelectIs_Active";
                cmd.Parameters.AddWithValue("@UserID", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    is_active = Convert.ToInt32(reader["Is_Active"]);
                }
            }
            return is_active;
        }
        #endregion

        #region Update Is_Active State Of User i.e Activating & Inactivating User
        public void ActivateDeactivateUser(int? id, int? is_active)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_User_IsActiveToggle";
                conn.Open();
                cmd.Parameters.AddWithValue("@UserID", id);
                cmd.Parameters.AddWithValue("@Is_Active", is_active);
                cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region Delete User Having Role As A User
        public void DeleteUser(int id)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_User_Delete";
                cmd.Parameters.AddWithValue("@UserID", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region Add Pizza
        public void AddPizza(MenuViewModel mm, IWebHostEnvironment env)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                #region Upload File With IFormFile
                string filename = "";
                if (mm.ImagePath != null)
                {
                    string folder = Path.Combine(env.WebRootPath, "upload_images");
                    filename = Guid.NewGuid().ToString() + "_" + mm.ImagePath.FileName; // Guid means Global unique identifier -> It will generate random character
                    string filepath = Path.Combine(folder, filename);
                    mm.ImagePath.CopyTo(new FileStream(filepath, FileMode.Create));
                }
                #endregion

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_Menu_Insert";
                cmd.Parameters.AddWithValue("@Name", mm.Name);
                cmd.Parameters.AddWithValue("@Description", mm.Description);
                cmd.Parameters.AddWithValue("@Price", mm.Price);
                cmd.Parameters.AddWithValue("@ImagePath", filename);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region Select All Pizza
        public List<MenuModel> SelectAllPizzas()
        {
            List<MenuModel> pizzaList = new List<MenuModel>();
            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_Menu_SelectAll";
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        MenuModel pizza = new MenuModel();
                        pizza.PizzaID = Convert.ToInt32(reader["PizzaID"]);
                        pizza.Name = reader["Name"].ToString();
                        pizza.Description = reader["Description"].ToString();
                        pizza.Price = (float)Convert.ToDouble(reader["Price"]);
                        pizza.ImagePath = reader["ImagePath"].ToString();
                        pizzaList.Add(pizza);
                    }
                }
            }
            return pizzaList;
        }
        #endregion

        #region Select Pizza By ID
        public MenuModel SelectPizzaByID(int id)
        {
            MenuModel pizza = new MenuModel();
            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_Menu_SelectByID";
                cmd.Parameters.AddWithValue("@PizzaID", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        pizza.PizzaID = Convert.ToInt32(reader["PizzaID"]);
                        pizza.Name = reader["Name"].ToString();
                        pizza.Description = reader["Description"].ToString();
                        pizza.Price = (float)Convert.ToDouble(reader["Price"].ToString());
                        pizza.ImagePath = reader["ImagePath"].ToString();
                    }
                }
            }
            return pizza;
        }
        #endregion

        #region Select Pizza By View Model ID
        public MenuViewModel SelectPizzaByViewModelID(int id,IWebHostEnvironment env)
        {
            MenuViewModel pizza = new MenuViewModel();
            using (SqlConnection conn = new SqlConnection(cs))
            {

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_Menu_SelectByID";
                cmd.Parameters.AddWithValue("@PizzaID", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        pizza.PizzaID = Convert.ToInt32(reader["PizzaID"]);
                        pizza.Name = reader["Name"].ToString();
                        pizza.Description = reader["Description"].ToString();
                        pizza.Price = (float)Convert.ToDouble(reader["Price"].ToString());
                        pizza.ImagePath = ConvertFilePathToIFormFile("upload_images/"+reader["ImagePath"].ToString(),env);
                    }
                }

            }
            return pizza;
        }
        #endregion

        #region Edit Pizza With IFormFile
        public void EditPizza(int id, MenuViewModel newPizza, IWebHostEnvironment env)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string filename = "";
                if (newPizza.ImagePath != null)
                {
                    string folder = Path.Combine(env.WebRootPath, "upload_images");
                    filename = Guid.NewGuid().ToString() + "_edited_" + newPizza.ImagePath.FileName; // Guid means Global unique identifier -> It will generate random character
                    string filepath = Path.Combine(folder, filename);
                    newPizza.ImagePath.CopyTo(new FileStream(filepath, FileMode.Create));
                }
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_Menu_Update";
                cmd.Parameters.AddWithValue("@PizzaID", id);
                cmd.Parameters.AddWithValue("@Name", newPizza.Name);
                cmd.Parameters.AddWithValue("@Description", newPizza.Description);
                cmd.Parameters.AddWithValue("@Price", newPizza.Price);
                cmd.Parameters.AddWithValue("@ImagePath", filename);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region Delete Pizza
        public void DeletePizza(int id)
        {
            using(SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_Menu_Delete";
                cmd.Parameters.AddWithValue("@PizzaID", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region Get History Of All Users
        public List<OrderHistoryModel> GetAllHistory()
        {
            List<OrderHistoryModel> history = new List<OrderHistoryModel>();
            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_OrderHistory_SelectAll_Admin";
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        OrderHistoryModel orderHistoryView = new OrderHistoryModel();
                        orderHistoryView.UserID = (int)reader["UserID"];
                        orderHistoryView.OrderID = (int)reader["OrderID"];
                        orderHistoryView.UserName = reader["UserName"].ToString();
                        orderHistoryView.Status = reader["Status"].ToString();
                        orderHistoryView.OrderDate = Convert.ToDateTime(reader["OrderDate"]);
                        orderHistoryView.TotalAmount = (float)Convert.ToDouble(reader["TotalAmount"]);
                        history.Add(orderHistoryView);
                    }
                }
            }
            return history;
        }
        #endregion

        #region Update Status Of Order
        public void UpdateStatus(int UserID,string status)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_Status_Update";
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@Status", status);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        #endregion
    }
}
