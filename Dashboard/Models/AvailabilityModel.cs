using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.Models
{
    public class AvailabilityModel
    {
        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime Day { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public TimeSpan AvailableFrom { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public TimeSpan AvailableTo { get; set; }

        [Required] public int EmployeeId { get; set; }
    }
}