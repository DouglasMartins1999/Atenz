namespace Atenz.Domain.DTOs
{
    public class Latest
    {
        public long Id { get; set; }
        public string Module { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public string Duration { get; set; }
        public string Course { get; set; }
        public string Banner { get; set; }
        public int MaxLessons { get; set; }
    }
}