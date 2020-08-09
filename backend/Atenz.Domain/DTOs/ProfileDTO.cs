using System.Collections.Generic;

namespace Atenz.Domain.DTOs
{
    public class Statistics
    {
        public int Lessons { get; set; }
        public int Books { get; set; }
        public int Courses { get; set; }
    }

    public class ProfileDTO
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string RegisteredAt { get; set; }
        public List<string> Interests { get; set; }
        public Statistics Statistics { get; set; }
        public List<bool> Goals { get; set; }
        public Latest Latest { get; set; }
        public dynamic RecentCourses { get; set; }
        public List<MinimalBook> RecentBooks { get; set; }
        public List<MinimalCourse> FavCourses { get; set; }
        public List<MinimalLesson> WatchLater { get; set; }
        public List<MinimalBook> FavBooks { get; set; }
    }
}