using System;
using Atenz.Domain.Identity;

namespace Atenz.Domain.Entities
{
    public class WatchLater
    {
        public long Id { get; set; }
        public User User { get; set; }
        public long UserId { get; set; }
        public Lesson Lesson { get; set; }
        public long LessonId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}