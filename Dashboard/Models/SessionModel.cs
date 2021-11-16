using System;
using System.ComponentModel.DataAnnotations;
using Core;

namespace Dashboard.Models
{
    public class SessionModelEmployee
    {
        [Required] public string Type { get; set; }
        [Required] public RoomType RoomType { get; set; }
        [Required] public int PatientId { get; set; }
        [Required] public int SessionEmployeeId { get; set; }
        [Required] public int DossierId { get; set; }
        [Required] public DateTime SessionDate { get; set; }

        //Note
        public string Author { get; set; }
        public bool Visible { get; set; }
        public string Text { get; set; }
    }

    public class SessionModelPatient
    {
        [Required] public int PatientId { get; set; }
        [Required] public int SessionEmployeeId { get; set; }
        [Required] public int DossierId { get; set; }
        [Required] public DateTime SessionDate { get; set; }
    }
}