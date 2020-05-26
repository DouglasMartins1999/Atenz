using Atenz.Domain.Entities;
using Atenz.Domain.Identity;
using Microsoft.EntityFrameworkCore;

namespace Atenz.Repository
{
    public class AtenzDBContext : DbContext
    {
        public AtenzDBContext(DbContextOptions options) : base(options)
        {}
        public DbSet<Course> Courses { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Book> Books { get; set; }


        public DbSet<User> Users { get; set; }


        public DbSet<History> History { get; set; }
        public DbSet<FavoriteCourse> FavoriteCourses { get; set; }
        public DbSet<WatchLater> WatchLater { get; set; }
        public DbSet<Read> ReadHistory { get; set; }
        public DbSet<FavoriteBook> FavoriteBooks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder){
            builder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("NOW()");

            builder.Entity<History>()
                .Property(h => h.CreatedAt)
                .HasDefaultValueSql("NOW()");

            builder.Entity<FavoriteCourse>()
                .Property(fc => fc.CreatedAt)
                .HasDefaultValueSql("NOW()");

            builder.Entity<WatchLater>()
                .Property(wl => wl.CreatedAt)
                .HasDefaultValueSql("NOW()");

            builder.Entity<Read>()
                .Property(r => r.CreatedAt)
                .HasDefaultValueSql("NOW()");

            builder.Entity<FavoriteBook>()
                .Property(fb => fb.CreatedAt)
                .HasDefaultValueSql("NOW()");
        }
    }
}