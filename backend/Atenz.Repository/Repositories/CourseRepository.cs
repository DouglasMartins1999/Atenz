using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Atenz.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Atenz.Repository.Repositories
{
    public class CourseRepository
    {
        public readonly AtenzDBContext Context;
        public CourseRepository(AtenzDBContext context)
        {
            Context = context;
        }

        public async Task<dynamic> GetById(long id)
        {
            return await Context.Courses
                .Include(c => c.Modules)
                .ThenInclude(m => m.Lessons)
                .Where(m => m.Id == id)
                .Select(c => new {
                    id = c.Id,
                    name = c.Name,
                    teacher = c.Teacher,
                    description = c.Description,
                    banner = c.Banner,
                    keywords = c.Keywords,
                    modules = c.Modules
                        .Select(m => new {
                            id = m.Id,
                            name = m.Name,
                            duration = TimeSpan.FromTicks(m.Lessons.Select(l => l.Duration.Ticks).ToArray().Sum()).ToString(),
                            lessonsAmount = m.Lessons.Count()
                        })
                })
                .FirstOrDefaultAsync();
        }

        public Task<Lesson> GetLessonById(long id)
        {
            return Context.Lessons
                .Where(l => l.Id == id)
                .Include(l => l.Module)
                .SingleOrDefaultAsync();
        }

        public async Task<List<Lesson>> GetLessonByModule(long moduleId)
        {
            return await Context.Lessons
                .Where(l => l.ModuleId == moduleId)
                .Include(l => l.Watches)
                .ToListAsync();
        }

        public async Task<dynamic> Query(string keyword, long userId, int page = 1, int limit = 10)
        {
            var querystring = keyword.ToLower();
            var courses = Context.Courses
                .Include(c => c.Modules)
                .Where(c => 
                    c.Name.ToLower().Contains(querystring) ||
                    c.Teacher.ToLower().Contains(querystring) ||
                    c.Description.ToLower().Contains(querystring) ||
                    c.Keywords.Contains(keyword)
                )
                .Select(c => new {
                    name = c.Name,
                    banner = c.Banner,
                    label = null as string,
                    duration = null as string,
                    wasWatched = false,
                    amount = c.Modules.Count,
                    course = c.Id,
                    module = 0L,
                    lesson = 0L,
                });


            var modules = Context.Modules
                .Include(m => m.Course)
                .Include(m => m.Lessons)
                .Where(m => m.Name.ToLower().Contains(querystring))
                .Select(m => new {
                    name = m.Name,
                    banner = m.Course.Banner,
                    label = null as string,
                    duration = null as string,
                    wasWatched = false,
                    amount = m.Lessons.Count,
                    course = m.CourseId,
                    module = m.Id,
                    lesson = 0L,
                });

            var lessons = Context.Lessons
                .Include(l => l.Module)
                .ThenInclude(m => m.Course)
                .Include(l => l.Watches)
                .Where(l => l.Name.ToLower().Contains(querystring))
                .Select(m => new {
                    name = m.Name,
                    banner = m.Module.Course.Banner,
                    label = m.Module.Name,
                    duration = m.Duration.ToString(),
                    wasWatched = m.Watches.Where(i => i.UserId == userId).Count() > 0,
                    amount = 0,
                    course = m.Module.CourseId,
                    module = m.ModuleId,
                    lesson = m.Id,
                })
                .Skip(page == 1 ? 2 : 0);

            return await courses
                .Union(modules)
                .Union(lessons)
                .OrderBy(s => s.course)
                .ThenBy(s => s.module)
                .ThenBy(s => s.lesson)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<dynamic> QueryFeatured(string query, long user)
        {
            return await Context.Lessons
                .Include(l => l.Module)
                .ThenInclude(m => m.Course)
                .Include(l => l.Watches)
                .Where(l => l.Name.ToLower().Contains(query.ToLower()))
                .Take(2)
                .Select(m => new {
                    id = m.Id,
                    name = m.Name,
                    banner = m.Module.Course.Banner,
                    course = m.Module.Course.Name,
                    module = m.Module.Name,
                    teacher = m.Module.Course.Teacher,
                    duration = m.Duration.ToString(),
                    wasWatched = m.Watches.Where(i => i.UserId == user).Count() > 0,
                    position = m.Position,
                    lessonsAmount = m.Module.Course.Modules.Select(x => x.Lessons.Count).Sum()
                })
                .ToListAsync();
        }

        public async Task<bool> AddToFavorites(long userId, long courseId)
        {
            try 
            {
                var fav = await Context.FavoriteCourses
                    .Where(f => f.UserId == userId && f.CourseId == courseId)
                    .SingleOrDefaultAsync();

                if(fav == null && userId != 0 && courseId != 0){
                    var favorite = new FavoriteCourse(userId, courseId);
                    await Context.FavoriteCourses.AddAsync(favorite);
                }

                return (await Context.SaveChangesAsync()) > 0;
            } 
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> AddToWatchLater(long userId, long lessonId)
        {
            try 
            {
                var wl = await Context.WatchLater
                    .Where(w => w.UserId == userId && w.LessonId == lessonId)
                    .SingleOrDefaultAsync();

                if(wl == null && userId != 0 && lessonId != 0){
                    var watch = new WatchLater(userId, lessonId);
                    await Context.WatchLater.AddAsync(watch);
                }

                return (await Context.SaveChangesAsync()) > 0;
            } 
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> AddToHistory(long userId, long lessonId)
        {
            try 
            {
                var history = await Context.History
                    .Where(w => w.UserId == userId && w.LessonId == lessonId)
                    .SingleOrDefaultAsync();

                if(history == null && userId != 0 && lessonId != 0){
                    var entry = new History(userId, lessonId);
                    await Context.History.AddAsync(entry);
                }

                return (await Context.SaveChangesAsync()) > 0;
            } 
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}