using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Core
{
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppointmentId { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime StartTime { get; set; }

        [Column(TypeName = "datetime2")] public DateTime EndTime { get; set; }

        [Required] public AppointmentType AppointmentType { get; set; }
        [Required] public bool Cancelled { get; set; } = false;

        [AllowNull] public Intake? Intake { get; set; }
        [AllowNull] public int? IntakeId { get; set; }

        [AllowNull] public Session? Session { get; set; }
        [AllowNull] public int? SessionId { get; set; }

        [AllowNull] public int? PatientId { get; set; }
        [AllowNull] public Patient Patient { get; set; }
        [Required] public int EmployeeId { get; set; }
        public Employee EfEmployee { get; set; }
    }
}