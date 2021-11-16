using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Core
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "FirstName must be less then 50 characters")]
        [MinLength(1, ErrorMessage = "FirstName must be at least 1 character")]
        public string Firstname { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "Lastname must be less then 150 characters")]
        [MinLength(1, ErrorMessage = "Lastname must be at least 1 character")]
        public string Lastname { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "Email must be less then 150 characters")]
        [MinLength(1, ErrorMessage = "Email must be at least 1 character")]
        public string Email { get; set; }

        // [Required] public string Password { get; set; }
        [Required] public bool Active { get; set; } = true;
        [Required] public Role Role { get; set; }
        [AllowNull] public string? BigNumber { get; set; }
        [AllowNull] public string? StudentNumber { get; set; }
        [AllowNull] public ICollection<Appointment>? Appointment { get; set; }
        [AllowNull] public ICollection<Intake>? Intake { get; set; }
        [AllowNull] public ICollection<Dossier>? HeadPractitioner { get; set; }
        [AllowNull] public ICollection<Session>? Sessions { get; set; }
    }
}