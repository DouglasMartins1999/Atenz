using System;

namespace Atenz.Domain.Identity
{
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool isAdmin { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}