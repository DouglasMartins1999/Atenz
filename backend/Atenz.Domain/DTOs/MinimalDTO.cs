namespace Atenz.Domain.DTOs
{
    public class MinimalCourse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Banner { get; set; }
        public string Teacher { get; set; }
        public int Lessons { get; set; }
        public string Duration { get; set; }
    }

    public class MinimalBook
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Author { get; set; }
        public int Pages { get; set; }
        public string Cover { get; set; }
    }

    public class MinimalLesson
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Banner { get; set; }
        public string Duration { get; set; }
    }
}