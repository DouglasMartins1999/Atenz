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

        public async Task<dynamic> Query(string param, long user, int page = 1, int limit = 8, bool favs = false, bool read = false)
        {
            var queryStrings = param.ToLower().Split(' ');
            var baseBooks = context.Books.Include(b => b.Reads);
            var baseFavs = context.FavoriteBooks.Include(f => f.Book).ThenInclude(b => b.Reads).Select(f => f.Book);
            var baseReads = context.ReadHistory.Include(r => r.Book).ThenInclude(r => r.Reads).Select(r => r.Book);
            IQueryable<Book> books = baseBooks;

            foreach(var q in queryStrings){
                var book = baseBooks.Where(b => 
                    b.Title.ToLower().Contains(q) ||
                    b.Subtitle.ToLower().Contains(q) ||
                    b.Author.ToLower().Contains(q) ||
                    b.Publisher.ToLower().Contains(q) ||
                    b.Keywords.Contains(param)
                );

                books = books.Intersect(book);

                if(favs){
                    var favBooks = baseFavs.Where(b => 
                        b.Title.ToLower().Contains(q) ||
                        b.Subtitle.ToLower().Contains(q) ||
                        b.Author.ToLower().Contains(q) ||
                        b.Publisher.ToLower().Contains(q) ||
                        b.Keywords.Contains(param)
                    );

                    books = books.Intersect(favBooks);
                }

                if(read){
                    var readed = baseReads.Where(b => 
                        b.Title.ToLower().Contains(q) ||
                        b.Subtitle.ToLower().Contains(q) ||
                        b.Author.ToLower().Contains(q) ||
                        b.Publisher.ToLower().Contains(q) ||
                        b.Keywords.Contains(param)
                    );

                    books = books.Intersect(readed);
                }
            }

            return await books
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