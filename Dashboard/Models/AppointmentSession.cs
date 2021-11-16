using System;
using System.ComponentModel.DataAnnotations;
using Core;

namespace Dashboard.Models
{
    public class AppointmentSession
    {
        [Required] public DateTime Day { get; set; }
        [Required] public TimeSpan Time { get; set; }
        [Required] public RoomType RoomType { get; set; }
        [Required] public int EmployeeId { get; set; }
        [Required] public int PatientId { get; set; }
    }
}