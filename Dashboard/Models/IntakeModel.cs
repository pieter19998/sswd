using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class IntakeModel
    {
        [Required] public int AppointmentId { get; set; }
        [Required] public string Email { get; set; }
    }
}