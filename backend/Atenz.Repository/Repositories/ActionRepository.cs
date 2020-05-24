using System.Threading.Tasks;
using Atenz.Domain.Entities;

namespace Atenz.Repository.Repositories
{
    public class ActionRepository
    {
        private readonly AtenzDBContext Context;
        public ActionRepository(AtenzDBContext context)
        {
            this.Context = context;
        }

        public async Task<FavoriteCourse> FavoriteCourse(long user, long course){
            var favorite = new FavoriteCourse(){
                UserId = user,
                CourseId = course
            };

            Context.FavoriteCourses.Add(favorite);
            await Context.SaveChangesAsync();
            return favorite;
        }

        public async Task<WatchLater> AddToWatchLater(long user, long lesson){
            var later = new WatchLater(){
                UserId = user,
                LessonId = lesson
            };

            Context.WatchLater.Add(later);
            await Context.SaveChangesAsync();
            return later;
        }

        public async Task<History> AddToHistory(long user, long lesson){
            var entry = new History(){
                UserId = user,
                LessonId = lesson
            };

            Context.History.Add(entry);
            await Context.SaveChangesAsync();
            return entry;
        }
    }
}