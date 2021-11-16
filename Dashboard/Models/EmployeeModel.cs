using System.ComponentModel.DataAnnotations;
using Core;

namespace Dashboard.Models
{
    public class EmployeeModel
    {
        [Required] public string Firstname { get; set; }
        [Required] public string Lastname { get; set; }
        [Required] public string Email { get; set; }
        [Required] public string Password { get; set; }
        [Required] public Role Role { get; set; }
        public string BigNumber { get; set; }
        public string StudentNumber { get; set; }
    }
}