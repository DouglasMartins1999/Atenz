using System;
using Atenz.Domain.Identity;

namespace Atenz.Domain.Entities
{
    public class FavoriteCourse
    {
        public long Id { get; set; }
        public User User { get; set; }
        public long UserId { get; set; }
        public Course Course { get; set; }
        public long CourseId { get; set; }
        public DateTime CreatedAt { get; set; }

        public FavoriteCourse(){}
        
        public FavoriteCourse(long user, long course)
        {
            this.UserId = user;
            this.CourseId = course;
        }

    }
}