using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Core
{
    public class Notes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoticeId { get; set; }

        [Required] public string Text { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime Date { get; set; }

        [Required] public NoteType NoteType { get; set; }
        [Required] public string Author { get; set; }
        [Required] public bool Visible { get; set; }
        [AllowNull] public Dossier? Dossier { get; set; }
        [AllowNull] public Session? Session { get; set; }
    }
}