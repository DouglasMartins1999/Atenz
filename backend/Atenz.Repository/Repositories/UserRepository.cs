using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Atenz.Domain.Entities;
using Atenz.Domain.Identity;
using Microsoft.EntityFrameworkCore;

namespace Atenz.Repository.Repositories
{
    public class UserRepository
    {
        private readonly AtenzDBContext context;

        public UserRepository(AtenzDBContext Context)
        {
            context = Context;
        }

        public async Task<User> ProfileBasic(long id){
            return await context.Users
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Lesson> LatestWatched(long userId)
        {
            return await context.History
                .Where(h => h.UserId == userId)
                .Include(h => h.Lesson)
                .ThenInclude(l => l.Module)
                .ThenInclude(m => m.Course)
                .OrderByDescending(h => h.CreatedAt)
                .Select(h => h.Lesson)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Course>> RecentsCourses(long userId)
        {
            return await context.History
                .Where(h => h.UserId == userId)
                .Include(h => h.Lesson)
                .ThenInclude(l => l.Module)
                .ThenInclude(m => m.Course)
                .OrderByDescending(h => h.CreatedAt)
                .Select(h => h.Lesson.Module.Course)
                .Distinct()
                .Take(4)
                .ToListAsync();
        }

        public async Task<List<Course>> FavoriteCourses(long userId)
        {
            return await context.FavoriteCourses
                .Where(f => f.UserId == userId)
                .OrderByDescending(h => h.CreatedAt)
                .Include(f => f.Course)
                .Select(f => f.Course)
                .Distinct()
                .Take(4)
                .ToListAsync();
        }

        public async Task<List<Lesson>> LessonsToWatchLater(long userId)
        {
            return await context.WatchLater
                .Where(wl => wl.UserId == userId)
                .Include(wl => wl.Lesson)
                .ThenInclude(l => l.Module)
                .ThenInclude(m => m.Course)
                .OrderByDescending(wl => wl.CreatedAt)
                .Select(wl => wl.Lesson)
                .Distinct()
                .Take(4)
                .ToListAsync();
        }

        public async Task<List<string>> Interests(long userId)
        {
            var favs = await context.FavoriteCourses
                .Where(f => f.UserId == userId)
                .Select(f => new {
                    course = f.CourseId,
                    points = 100
                })
                .ToListAsync();

            var later = await context.WatchLater
                .Where(wl => wl.UserId == userId)
                .Include(wl => wl.Lesson)
                .ThenInclude(l => l.Module)
                .Select(wl => new {
                    course = wl.Lesson.Module.CourseId,
                    points = 20
                })
                .ToListAsync();

            var watchs = await context.History
                .Where(h => h.UserId == userId)
                .Include(h => h.Lesson)
                .ThenInclude(l => l.Module)
                .Select(h => new {
                    course = h.Lesson.Module.CourseId,
                    points = 1
                })
                .ToListAsync();

            var points = favs.Union(later).Union(watchs);

            var keywords = await context.Courses
                .Where(c => points.Select(p => p.course).Contains(c.Id))
                .Select(c => new {
                    id = c.Id,
                    keywords = c.Keywords
                })
                .ToListAsync();

            return points
                .GroupBy(c => c.course)
                .Select(p => new {
                    course = keywords.Where(i => i.id == p.Key).FirstOrDefault().keywords,
                    points = p.Sum(i => i.points)
                })
                .SelectMany(s => s.course.Select(y => new Tuple<string, int>(y, s.points)))
                .GroupBy(i => i.Item1)
                .Select(i => new {
                    interest = i.Key,
                    points = i.Sum(n => n.Item2)
                })
                .OrderByDescending(i => i.points)
                .Select(i => i.interest)
                .Take(10)
                .ToList();
        }

        public async Task<Domain.DTOs.Statistics> Statistics(long userId)
        {
            var lessons = await context.History
                .Where(h => h.UserId == userId)
                .Select(i => i.LessonId)
                .Distinct()
                .CountAsync();

            var books = 0;

            var courses = await context.History
                .Include(h => h.Lesson)
                .ThenInclude(l => l.Module)
                .Where(h => h.UserId == userId)
                .Select(i => i.Lesson.Module.CourseId)
                .Distinct()
                .CountAsync();

            return new Domain.DTOs.Statistics(){
                Lessons = lessons,
                Books = books,
                Courses = courses
            };
        }
    
        public async Task<List<bool>> Goals(long userId)
        {
            var lessonsAmount = await context.History
                .Where(h => h.UserId == userId)
                .CountAsync();

            var booksAmount = 0;

            return new List<bool>(){
                lessonsAmount > 0,
                booksAmount > 0,
                lessonsAmount > 30,
                booksAmount > 10,
                lessonsAmount > 500
            };
        }
    }
}