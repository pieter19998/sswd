using System;
using System.ComponentModel.DataAnnotations;
using Core;

namespace Dashboard.Models
{
    public class SessionEditModel
    {
        [Required] public int SessionId { get; set; }
        [Required] public bool Active { get; set; }
        [Required] public string Type { get; set; }
        [Required] public RoomType RoomType { get; set; }
        [Required] public int PatientId { get; set; }
        [Required] public int SessionEmployeeId { get; set; }
        [Required] public DateTime SessionDate { get; set; }
        [Required] public int DossierId { get; set; }
        [Required] public string Text { get; set; }
        [Required] public bool Visible { get; set; }
        [Required] public bool Additional { get; set; }
    }
}