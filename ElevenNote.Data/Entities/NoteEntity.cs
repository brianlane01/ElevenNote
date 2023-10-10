using System.ComponentModel.DataAnnotations;

namespace ElevenNote.Data.Entities
{
    public class NoteEntity
    {
        [Key]
        public int Id { get; set; } 

        [Required]
        public int OwnerId { get; set; }
        public UserEntity Owner { get; set; } = null!;

        [Required, MinLength(1), MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required, MinLength(1), MaxLength(8000)]
        public string Content { get; set; } = string.Empty;

        [Required]
        public DateTimeOffset CreatedUtc { get; set; }

        public DateTimeOffset? ModifiedUtc { get; set; }
        
    }
}