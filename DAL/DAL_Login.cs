using Restaurant_Management_System.Areas.Login.Models;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace Restaurant_Management_System.DAL
{
    public class DAL_Login
    {
        string cs = ConnectionString.cs;
        public List<string> loginUser(LoginModel lm)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_User_Login";
                string hashedPassword = HashPassword(lm.password);
                cmd.Parameters.AddWithValue("@email", lm.email);
                cmd.Parameters.AddWithValue("@password", hashedPassword);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return new List<string>() { "true", reader["Name"].ToString(), reader["Role"].ToString(), reader["Is_Active"].ToString(), reader["UserID"].ToString() }; // true because in our query name is given as output so if there are no names coming as output then it will give nothing else if output is there then it will give true (i.e HasRows has atmost 1 row )
                    }
                }
                return new List<string>() { "false", "", "no", "" };
            }
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
