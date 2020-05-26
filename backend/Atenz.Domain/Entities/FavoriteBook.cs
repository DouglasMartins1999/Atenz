using System;
using Atenz.Domain.Identity;

namespace Atenz.Domain.Entities
{
    public class FavoriteBook
    {
        public long Id { get; set; }
        public User User { get; set; }
        public long UserId { get; set; }
        public Book Book { get; set; }
        public long BookId { get; set; }
        public DateTime CreatedAt { get; set; }

        public FavoriteBook(){}

        public FavoriteBook(long user, long book)
        {
            this.UserId = user;
            this.BookId = book;
        }
    }
}