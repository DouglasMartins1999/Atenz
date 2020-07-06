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
                .ThenInclude(l => l.Course)
                .SingleOrDefaultAsync();
        }

        public async Task<List<Lesson>> GetLessonByModule(long moduleId)
        {
            return await Context.Lessons
                .Where(l => l.ModuleId == moduleId)
                .Include(l => l.Watches)
                .ToListAsync();
        }

        public async Task<dynamic> Query(
            string keyword, 
            string type, 
            long userId, 
            int page = 1, 
            int limit = 10, 
            bool onlyFavorites = false, 
            bool onlyWatched = false, 
            bool onlyMarked = false
        )
        {
            var querystring = keyword.ToLower().Split(' ');
            var baseCourses = Context.Courses
                .Include(c => c.Modules);

            var baseModules = Context.Courses
                .Include(c => c.Modules)
                .ThenInclude(m => m.Lessons)
                .SelectMany(c => c.Modules);

            var baseLessons = Context.Lessons
                .Include(l => l.Module)
                .ThenInclude(m => m.Course)
                .Include(l => l.Watches);

            IQueryable<Course> courses = baseCourses;
            IQueryable<Module> modules = baseModules;
            IQueryable<Lesson> lessons = baseLessons;


            var baseFavCourses = Context.FavoriteCourses
                .Include(f => f.Course)
                .ThenInclude(c => c.Modules)
                .Where(f => f.UserId == userId)
                .Select(f => f.Course);

            var baseFavModules = Context.FavoriteCourses
                .Include(f => f.Course)
                .ThenInclude(c => c.Modules)
                .ThenInclude(m => m.Lessons)
                .Where(f => f.UserId == userId)
                .Select(f => f.Course)
                .SelectMany(f => f.Modules);

            var baseFavLessons = Context.FavoriteCourses
                .Include(f => f.Course)
                .ThenInclude(c => c.Modules)
                .ThenInclude(m => m.Lessons)
                .ThenInclude(l => l.Watches)
                .Where(h => h.UserId == userId)
                .Select(f => f.Course)
                .SelectMany(c => c.Modules)
                .SelectMany(m => m.Lessons);

            var baseHistory = Context.History
                .Include(h => h.Lesson)
                .ThenInclude(l => l.Module)
                .ThenInclude(m => m.Course)
                .Include(l => l.Lesson)
                .ThenInclude(l => l.Watches)
                .Where(h => h.UserId == userId)
                .Select(h => h.Lesson);

            var baseWatchLater = Context.WatchLater
                .Include(h => h.Lesson)
                .ThenInclude(l => l.Module)
                .ThenInclude(m => m.Course)
                .Include(l => l.Lesson)
                .ThenInclude(l => l.Watches)
                .Where(h => h.UserId == userId)
                .Select(h => h.Lesson);

            foreach(var k in querystring){
                var courseQuery = baseCourses.Where(c => 
                    c.Name.ToLower().Contains(k) ||
                    c.Teacher.ToLower().Contains(k) ||
                    c.Description.ToLower().Contains(k) ||
                    c.Keywords.Contains(keyword)
                );

                var moduleQuery = baseModules
                    .Where(m => m.Name.ToLower().Contains(k));

                var lessonQuery = baseLessons
                    .Where(l => l.Name.ToLower().Contains(k))
                    .Skip(page == 1 ? 2 : 0);

                courses = courses.Intersect(courseQuery);
                modules = modules.Intersect(moduleQuery);
                lessons = lessons.Intersect(lessonQuery);



                if(onlyFavorites){
                    var favCourses = baseFavCourses.Where(c => 
                        c.Name.ToLower().Contains(k) ||
                        c.Teacher.ToLower().Contains(k) ||
                        c.Description.ToLower().Contains(k) ||
                        c.Keywords.Contains(keyword)
                    );

                    var favModules = baseFavModules
                        .Where(m => m.Name.ToLower().Contains(k));

                    var favLessons = baseFavLessons
                        .Where(l => l.Name.ToLower().Contains(k));

                    courses = courses.Intersect(favCourses);
                    modules = modules.Intersect(favModules);
                    lessons = lessons.Join(favLessons, l => l.Id, f => f.Id, (l, f) => l);
                }

                if(onlyWatched){
                    var watched = baseHistory.Where(l => l.Name.ToLower().Contains(k));
                    lessons = lessons.Intersect(watched);
                }

                if(onlyMarked){
                    var marked = baseWatchLater.Where(l => l.Name.ToLower().Contains(k));
                    lessons = lessons.Intersect(marked);
                }
            }

            if(onlyWatched || onlyMarked){
                type = "l";
            }

            var lessonResult = lessons
                .Select(m => new {
                    id = m.Id,
                    name = m.Name,
                    banner = m.Module.Course.Banner,
                    label = m.Module.Name,
                    duration = m.Duration.ToString(),
                    wasWatched = m.Watches.Where(i => i.UserId == userId).Count() > 0,
                    amount = 0,
                    course = m.Module.CourseId,
                    module = m.ModuleId,
                    lesson = m.Id,
                });

            var modulesResult = modules
                .Select(m => new {
                    id = m.Id,
                    name = m.Name,
                    banner = m.Course.Banner,
                    label = null as string,
                    duration = null as string,
                    wasWatched = false,
                    amount = m.Lessons.Count,
                    course = m.CourseId,
                    module = m.Id,
                    lesson = 0L
                });

            var coursesResult = courses
                .Select(c => new {
                    id = c.Id,
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

            var query = coursesResult
                .Union(modulesResult)
                .Union(lessonResult);

            switch(type){
                case "c":
                    query = coursesResult;
                    break;
                case "m":
                    query = modulesResult;
                    break;
                case "l":
                    query = lessonResult;
                    break;
            }

            return await query
                .OrderBy(s => s.course)
                .ThenBy(s => s.module)
                .ThenBy(s => s.lesson)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<dynamic> QueryFeatured(string query, long user, bool favs, bool watchs, bool marked)
        {
            var queryStrings = query.ToLower().Split(' ');
            var baseLessons = Context.Lessons
                .Include(l => l.Module)
                .ThenInclude(m => m.Course)
                .Include(l => l.Watches);

            var baseFavLessons = Context.FavoriteCourses
                .Include(f => f.Course)
                .ThenInclude(c => c.Modules)
                .ThenInclude(m => m.Lessons)
                .ThenInclude(l => l.Watches)
                .Where(h => h.UserId == user)
                .Select(f => f.Course)
                .SelectMany(c => c.Modules)
                .SelectMany(m => m.Lessons);

            var baseHistory = Context.History
                .Include(h => h.Lesson)
                .ThenInclude(l => l.Module)
                .ThenInclude(m => m.Course)
                .Include(l => l.Lesson)
                .ThenInclude(l => l.Watches)
                .Where(h => h.UserId == user)
                .Select(h => h.Lesson);

            var baseWatchLater = Context.WatchLater
                .Include(h => h.Lesson)
                .ThenInclude(l => l.Module)
                .ThenInclude(m => m.Course)
                .Include(l => l.Lesson)
                .ThenInclude(l => l.Watches)
                .Where(h => h.UserId == user)
                .Select(h => h.Lesson);

            IQueryable<Lesson> lessons = baseLessons;

            foreach(var k in queryStrings){
                var lesson = baseLessons.Where(l => l.Name.ToLower().Contains(k));
                lessons = lessons.Intersect(lesson);

                if(favs){
                    var favorites = baseFavLessons.Where(l => l.Name.ToLower().Contains(k));
                    lessons = lessons.Join(favorites, l => l.Id, f => f.Id, (l, f) => l);
                }

                if(watchs){
                    var history = baseHistory.Where(l => l.Name.ToLower().Contains(k));
                    lessons = lessons.Intersect(history);
                }

                if(marked){
                    var markeds = baseWatchLater.Where(l => l.Name.ToLower().Contains(k));
                    lessons = lessons.Intersect(markeds);
                }
            }

            return await lessons
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

        public async Task<bool> RemoveFavorite(long user, long course){
            try {
                if(course == 0 || user == 0) return false;
                var courses = await Context.FavoriteCourses
                    .Where(f => f.CourseId == course && f.UserId == user)
                    .ToListAsync();

                Context.FavoriteCourses.RemoveRange(courses);
                return (await Context.SaveChangesAsync()) > 0;

            } catch(Exception e){
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

        public async Task<bool> RemoveFromWatchLater(long userId, long lessonId){
            try {
                if(lessonId == 0 || userId == 0) return false;
                var lessons = await Context.WatchLater
                    .Where(f => f.LessonId == lessonId && f.UserId == userId)
                    .ToListAsync();

                Context.WatchLater.RemoveRange(lessons);
                return (await Context.SaveChangesAsync()) > 0;

            } catch(Exception e){
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> AddToHistory(long userId, long lessonId)
        {
            if(userId != 0 && lessonId != 0){
                var entry = new History(userId, lessonId);
                await Context.History.AddAsync(entry);
            }

            return (await Context.SaveChangesAsync()) > 0;
        }
    }
}