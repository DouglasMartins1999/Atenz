using System;
using System.Collections.Generic;

namespace Atenz.Domain.Entities
{
    public class Lesson
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public TimeSpan Duration { get; set; }
        public Module Module { get; set; }
        public long ModuleId { get; set; }
        public int Position { get; set; }
        public List<History> Watches { get; set; }
    }
}