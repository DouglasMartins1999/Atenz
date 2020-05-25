using System.Collections.Generic;

namespace Atenz.Domain.Entities
{
    public class Module
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Course Course { get; set; }
        public long CourseId { get; set; }
        public List<Lesson> Lessons { get; set; }
    }
}