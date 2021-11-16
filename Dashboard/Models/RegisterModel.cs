using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class RegisterModel
    {
        [Required] public string Email { get; set; }
        public string UserName { get; set; }
        [Required] public string PasswordHash { get; set; }
        [Required] public string PhoneNumber { get; set; }
        public string ReturnUrl { get; set; }
    }
}