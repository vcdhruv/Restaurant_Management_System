using Restaurant_Management_System.Areas.Register.Models;
using Restaurant_Management_System.BAL;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Restaurant_Management_System.DAL
{
    public class DAL_Registration
    {
        string cs = ConnectionString.cs;

        public string addUser(RegisterationModel lm)
        {
            String role;
            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_User_Insert";

                // Encrypt the password 
                string hashedPassword = HashPassword(lm.Password);
                
                cmd.Parameters.AddWithValue("@Name", lm.Name);
                cmd.Parameters.AddWithValue("@Email", lm.Email);
                cmd.Parameters.AddWithValue("@Password", hashedPassword);
                cmd.Parameters.AddWithValue("@Phone", lm.Phone);
                cmd.Parameters.AddWithValue("@Address", lm.Address);
                cmd.Parameters.AddWithValue("@Role", lm.Role);
                role = lm.Role;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return role;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
