using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Core
{
    public class Dossier
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DossierId { get; set; }

        public uint Age { get; set; }
        [Required] public string Description { get; set; }
        [Required] public string DiagnoseCode { get; set; }
        public string DiagnoseDescription { get; set; }
        public Employee HeadPractitioner { get; set; }
        [Required] public int HeadPractitionerId { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime ApplicationDay { get; set; }

        [Column(TypeName = "datetime2")] public DateTime DismissalDay { get; set; }
        public bool Active { get; set; } = true;
        [AllowNull] public ICollection<Notes>? Notices { get; set; }

        [AllowNull] public ICollection<Session>? Sessions { get; set; }
        [AllowNull] public TreatmentPlan? TreatmentPlan { get; set; }
        [AllowNull] public int? TreatmentPlanId { get; set; }
        public Patient Patient { get; set; }
        public int PatientId { get; set; }
    }
}