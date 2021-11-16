using System;
using System.ComponentModel.DataAnnotations;
using Core;

namespace Dashboard.Models
{
    public class DossierModel
    {
        //DiagnoseCode and Description
        [Required] public int Code { get; set; }
        public string BodyLocation { get; set; }
        public string Pathology { get; set; }
        [Required] public string DiagnoseDescription { get; set; }
        [Required] public PatientType PatientType { get; set; }
        [Required] public int HeadPractitioner { get; set; }
        [Required] public int PatientId { get; set; }
        [Required] public string Description { get; set; }

        [Required] public DateTime DismissalDay { get; set; }
    }
}