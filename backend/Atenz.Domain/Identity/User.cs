using System;
using System.Collections.Generic;
using Atenz.Domain.Entities;

namespace Atenz.Domain.Identity
{
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool isAdmin { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<History> History { get; set; }
        public List<FavoriteCourse> Favorites { get; set; }
        public List<WatchLater> WatchLater { get; set; }
    }
}