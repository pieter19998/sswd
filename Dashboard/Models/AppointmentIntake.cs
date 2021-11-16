using System;
using System.ComponentModel.DataAnnotations;
using Core;

namespace Dashboard.Models
{
    public class AppointmentIntake
    {
        [Required] public DateTime Day { get; set; }
        [Required] public TimeSpan Time { get; set; }
        [Required] public AppointmentType AppointmentType { get; set; }
        [Required] public string Email { get; set; }
        [Required] public int EmployeeId { get; set; }
    }
}