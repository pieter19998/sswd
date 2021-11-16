using System;
using System.ComponentModel.DataAnnotations;
using Core;

namespace Dashboard.Models
{
    public class DossierEdit
    {
        [Required] public int DossierId { get; set; }
        [Required] public uint Age { get; set; }
        [Required] public int Code { get; set; }
        [Required] public string Description { get; set; }
        [Required] public string DiagnoseCode { get; set; }
        [Required] public string DiagnoseDescription { get; set; }
        [Required] public PatientType PatientType { get; set; }
        [Required] public int HeadPractitionerId { get; set; }
        [Required] public DateTime ApplicationDay { get; set; }
        [Required] public DateTime DismissalDay { get; set; }
        public bool Active { get; set; } = true;
        [Required] public int? TreatmentPlanId { get; set; }
        [Required] public int PatientId { get; set; }


        public string BodyLocation { get; set; }
        public string Pathology { get; set; }
    }
}