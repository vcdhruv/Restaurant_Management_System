using System.ComponentModel.DataAnnotations;

namespace Restaurant_Management_System.Areas.Login.Models
{
    public class LoginModel
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }

        public int Is_Active { get; set; }
    }
}
