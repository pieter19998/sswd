using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class TreatmentPlanModel
    {
        [Required] public int SessionsPerWeek { get; set; }
        [Required] public int DossierId { get; set; }
        [Required] public double SessionDuration { get; set; }
    }
}