using System.Collections.Generic;

namespace Atenz.Domain.Entities
{
    public class Course
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Teacher { get; set; }
        public string Description { get; set; }
        public string Banner { get; set; }
        public string[] Keywords { get; set; }
        public List<Module> Modules { get; set; }
    }
}