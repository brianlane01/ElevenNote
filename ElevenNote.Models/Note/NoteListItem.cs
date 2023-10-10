namespace ElevenNote.Models.Note
{
    public class NoteListItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTimeOffset CreatedUtc { get; set; }
    }
}