namespace Atenz.Domain.DTOs
{
    public class LessonDTO
    {
        public long Id { get; set; }
        public int Position { get; set; }
        public string Name { get; set; }
        public string Module { get; set; }
        public string Duration { get; set; }
        public string Link { get; set; }
        public string Size { get; set; }
    }
}