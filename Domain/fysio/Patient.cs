using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace Core
{
    public class Patient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PatientId { get; set; }

        [PersonalData]
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

        [Required] public bool Active { get; set; } = true;

        [Required]
        [MaxLength(150, ErrorMessage = "Address must be less then 150 characters")]
        [MinLength(1, ErrorMessage = "Address must be at least 1 character")]
        public string Address { get; set; }

        [Required] public Sex Sex { get; set; }
        [NotNull] public string PatientNumber { get; set; }

        [AllowNull]
        [Column(TypeName = "VARCHAR(MAX)")]
        public string? Photo { get; set; }

        [PersonalData]
        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime DayOfBirth { get; set; }
        public string? PersonalNumber { get; set; }
        public string? StudentNumber { get; set; }
        [Required] public Role Role { get; set; }
        [Required] public PatientType Type { get; set; }
        [AllowNull] public ICollection<Appointment>? Appointment { get; set; }
        [AllowNull] public ICollection<Session>? Sessions { get; set; }
        [AllowNull] public Dossier Dossier { get; set; }
    }
}