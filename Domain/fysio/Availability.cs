using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core
{
    public class Availability
    {
        [Key] public int AvailabilityId { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime AvailableFrom { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime AvailableTo { get; set; }

        public Employee Employee { get; set; }
        [Required] public int EmployeeId { get; set; }
    }
}