using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.stam
{
    public class Diagnose
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Code { get; set; }

        [Required] public string BodyLocation { get; set; }
        [Required] public string Pathology { get; set; }
    }
}