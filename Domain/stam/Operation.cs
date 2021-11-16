using System.ComponentModel.DataAnnotations;

namespace Core.stam
{
    public class Operation
    {
        [Key] public string Value { get; set; }

        [Required] public string Description { get; set; }
        [Required] public string Additional { get; set; }
    }
}