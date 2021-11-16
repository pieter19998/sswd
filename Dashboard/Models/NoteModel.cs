using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class NoteModel
    {
        [Required] public int DossierId { get; set; }
        [Required] public string Text { get; set; }
        [Required] public bool Visible { get; set; }
    }
}