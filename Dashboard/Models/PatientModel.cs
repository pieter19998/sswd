using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Core;

namespace Dashboard.Models
{
    public class PatientModel
    {
        [AllowNull] public string FirstName { get; set; }
        [AllowNull] public string LastName { get; set; }
        [Required] public string Email { get; set; }
        [Required] public Sex Sex { get; set; }
        [AllowNull] public string Address { get; set; }
        [Required] public DateTime DayOfBirth { get; set; }
        [Required] public PatientType Type { get; set; }
        [AllowNull] public string PersonalNumber { get; set; }
        [AllowNull] public string StudentNumber { get; set; }
    }
}