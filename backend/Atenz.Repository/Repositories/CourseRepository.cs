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
    }
}