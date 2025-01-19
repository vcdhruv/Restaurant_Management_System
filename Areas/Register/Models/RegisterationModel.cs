using System.ComponentModel.DataAnnotations;

namespace Restaurant_Management_System.Areas.Register.Models
{
    public class RegisterationModel
    {
        public int UserID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Role { get; set; }
        public int Is_Active { get; set; }
    }
}
