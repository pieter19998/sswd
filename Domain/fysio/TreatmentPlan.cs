using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core
{
    public class TreatmentPlan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TreatmentPlanId { get; set; }

        [Required] public int SessionsPerWeek { get; set; }
        [Required] public double SessionDuration { get; set; }
        [Required] public bool Active { get; set; } = true;
        public Dossier Dossier { get; set; }
    }
}