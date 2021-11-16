using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Core
{
    public class Intake
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IntakeId { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "Email must be less then 150 characters")]
        [MinLength(1, ErrorMessage = "Email must be at least 1 character")]
        public string Email { get; set; }

        public Employee IntakeBy { get; set; }
        [Required] public int IntakeById { get; set; }
        [AllowNull] public string IntakeSuperVisor { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime Date { get; set; }

        public Appointment Appointment { get; set; }
    }
}