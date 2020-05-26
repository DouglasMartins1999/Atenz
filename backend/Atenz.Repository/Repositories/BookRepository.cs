using System;
using System.Linq;
using System.Threading.Tasks;
using Atenz.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Atenz.Repository.Repositories
{
    public class BookRepository
    {
        private readonly AtenzDBContext context;

        public BookRepository(AtenzDBContext Context) {
            context = Context;
        }

        public async Task<Book> GetById(long id){
            return await context.Books.FindAsync(id);
        }

        public async Task<dynamic> Query(string param, long user, int page = 1, int limit = 8)
        {
            var query = param.ToLower();
            return await context.Books
                .Include(b => b.Reads)
                .Where(b => 
                    b.Title.ToLower().Contains(query) ||
                    b.Subtitle.ToLower().Contains(query) ||
                    b.Author.ToLower().Contains(query) ||
                    b.Publisher.ToLower().Contains(query) ||
                    b.Keywords.Contains(param)
                )
                .Select(b => new {
                    id = b.Id,
                    cover = b.Cover,
                    title = b.Title,
                    author = b.Author,
                    pages = b.Pages,
                    wasReaded = b.Reads.Where(r => r.UserId == user).Count() > 0
                })
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<bool> MarkAsFavorite(long book, long user)
        {
            try {
                var fav = await context.FavoriteBooks
                    .Where(f => f.UserId == user && f.BookId == book)
                    .SingleOrDefaultAsync();

                if(fav == null && user != 0 && book != 0){
                    var favorite = new FavoriteBook(user, book);
                    await context.FavoriteBooks.AddAsync(favorite);
                }
                return (await context.SaveChangesAsync()) > 0;

            } catch(Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> MarkAsReaded(long book, long user)
        {
            try {
                var fav = await context.ReadHistory
                    .Where(f => f.UserId == user && f.BookId == book)
                    .SingleOrDefaultAsync();

                if(fav == null && user != 0 && book != 0){
                    var entry = new Read(user, book);
                    await context.ReadHistory.AddAsync(entry);
                }
                return (await context.SaveChangesAsync()) > 0;
                
            } catch(Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}