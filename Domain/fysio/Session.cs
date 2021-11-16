using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Core
{
    public class Session
    {
        [AllowNull] public List<Notes>? Notices { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SessionId { get; set; }

        public string Type { get; set; }
        public RoomType RoomType { get; set; }
        public Patient Patient { get; set; }
        [Required] public int PatientId { get; set; }
        public Employee SessionEmployee { get; set; }
        [Required] public int SessionEmployeeId { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime SessionDate { get; set; }

        public bool Active { get; set; } = true;
        [Required] public int DossierId { get; set; }
        public Dossier Dossier { get; set; }
        public Appointment Appointment { get; set; }
    }
}