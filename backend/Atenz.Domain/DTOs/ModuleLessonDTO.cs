namespace Atenz.Domain.DTOs
{
    public class ModuleLessonDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Duration { get; set; }
        public bool WasWatched { get; set; }
    }
}