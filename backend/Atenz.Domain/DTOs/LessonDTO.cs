namespace Atenz.Domain.DTOs
{
    public class LessonDTO
    {
        public long Id { get; set; }
        public long ModuleId { get; set; }
        public long CourseId { get; set; }
        public int Position { get; set; }
        public string Name { get; set; }
        public string Module { get; set; }
        public string Duration { get; set; }
        public string Link { get; set; }
        public string Banner { get; set; }
        public long Size { get; set; }
    }
}