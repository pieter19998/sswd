using System;
using Core;

namespace Dashboard.Models
{
    public class AppointmentList
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public AppointmentType AppointmentType { get; set; }
        public bool Cancelled { get; set; }
    }
}