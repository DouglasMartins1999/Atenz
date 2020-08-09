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

        public async Task<User> Auth(string username, string password)
        {
            var user = await context.Users
                .Where(u => u.Username == username)
                .FirstOrDefaultAsync();

            if(user == null) {
                throw new Exception("Usuário não encontrado");
            }

            var isValid = BCrypt.Net.BCrypt.Verify(password, user.Password);

            if(!isValid) {
                throw new Exception("Senha inválida");
            }

            return user;
        }

        public async Task<User> Register(User user)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Password = hashedPassword;
            await context.Users.AddAsync(user);
            var data = await context.SaveChangesAsync();
            return user;
        }

        public async Task<int> CleanHistory(long user, bool lessons, bool books, DateTime mindate, DateTime maxdate)
        {
            if(maxdate < mindate) return 0;

            if(lessons) {
                var lessonsToRemove = context.History
                    .Where(i => i.CreatedAt >= mindate && i.CreatedAt <= maxdate && i.UserId == user);

                context.History.RemoveRange(lessonsToRemove);
            }

            if(books){
                var booksToRemove = context.ReadHistory
                    .Where(i => i.CreatedAt >= mindate && i.CreatedAt <= maxdate && i.UserId == user);

                context.ReadHistory.RemoveRange(booksToRemove);
            }

            return await context.SaveChangesAsync();
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

        public async Task<dynamic> RecentsCourses(long userId, int pag = 1, int limit = 12)
        {
            return await context.History
                .Where(h => h.UserId == userId)
                .Include(h => h.Lesson)
                .ThenInclude(l => l.Module)
                .ThenInclude(m => m.Course)
                .GroupBy(h => new { 
                    h.Lesson.Module.Course.Id, 
                    h.Lesson.Module.Course.Name,
                    h.Lesson.Module.Course.Teacher,
                    h.Lesson.Module.Course.Banner,
                })
                .OrderByDescending(h => h.Max(n => n.CreatedAt))
                .Select(h => new {
                    h.Key.Id,
                    h.Key.Name,
                    h.Key.Teacher,
                    h.Key.Banner
                })
                .Take(limit * pag)
                .Skip((pag - 1) * limit)
                .ToListAsync();
        }

        public async Task<List<Course>> FavoriteCourses(long userId, int pag = 1, int limit = 12)
        {
            return await context.FavoriteCourses
                .Where(f => f.UserId == userId)
                .Include(f => f.Course)
                .OrderByDescending(h => h.CreatedAt)
                .Select(f => f.Course)
                .Distinct()
                .Take(limit * pag)
                .Skip((pag - 1) * limit)
                .ToListAsync();
        }

        public async Task<List<Lesson>> LessonsToWatchLater(long userId, int pag = 1, int limit = 12)
        {
            return await context.WatchLater
                .Where(wl => wl.UserId == userId)
                .Include(wl => wl.Lesson)
                .ThenInclude(l => l.Module)
                .ThenInclude(m => m.Course)
                .OrderByDescending(wl => wl.CreatedAt)
                .Select(wl => wl.Lesson)
                .Distinct()
                .Take(limit * pag)
                .Skip((pag - 1) * limit)
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

            var books = await context.ReadHistory
                .Where(h => h.UserId == userId)
                .Select(i => i.BookId)
                .Distinct()
                .CountAsync();

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
                .Select(i => i.LessonId)
                .Distinct()
                .CountAsync();

            var booksAmount = await context.ReadHistory
                .Where(h => h.UserId == userId)
                .Select(h => h.BookId)
                .Distinct()
                .CountAsync();

            Console.WriteLine(lessonsAmount);

            return new List<bool>(){
                lessonsAmount > 0,
                booksAmount > 0,
                lessonsAmount > 30,
                booksAmount > 10,
                lessonsAmount > 500
            };
        }
    
        public async Task<List<Book>> RecentsBooks(long userId, int pag = 1, int limit = 12)
        {
            return await context.ReadHistory
                .Include(h => h.Book)
                .Where(h => h.UserId == userId)
                .OrderByDescending(h => h.CreatedAt)
                .Select(h => h.Book)
                .Distinct()
                .Take(limit * pag)
                .Skip((pag - 1) * limit)
                .ToListAsync();
        }

        public async Task<List<Book>> FavoriteBooks(long userId, int pag = 1, int limit = 12)
        {
            return await context.FavoriteBooks
                .Include(h => h.Book)
                .Where(h => h.UserId == userId)
                .OrderByDescending(h => h.CreatedAt)
                .Select(h => h.Book)
                .Distinct()
                .Take(limit * pag)
                .Skip((pag - 1) * limit)
                .ToListAsync();
        }
    }
}