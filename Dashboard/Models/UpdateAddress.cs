using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class UpdateAddress
    {
        [Required] public int PatientId { get; set; }
        [Required] public string Address { get; set; }
    }
}