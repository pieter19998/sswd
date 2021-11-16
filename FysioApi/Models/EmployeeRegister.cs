using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Core;

namespace FysioApi.Models
{
    public class EmployeeRegister
    {
        [Required] public string Firstname { get; set; }
        [Required] public string Lastname { get; set; }
        [Required] public string Email { get; set; }
        [Required] public string Password { get; set; }
        [Required] public Role Role { get; set; }
        [AllowNull] public string? BigNumber { get; set; }
        [AllowNull] public string? StudentNumber { get; set; }
    }
}