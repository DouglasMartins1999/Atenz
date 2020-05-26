using System;
using System.Collections.Generic;

namespace Atenz.Domain.Entities
{
    public class Book
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public DateTime ReleasedAt { get; set; }
        public string Description { get; set; }
        public string ISBN { get; set; }
        public List<String> Keywords { get; set; }
        public int Pages { get; set; }
        public string Format { get; set; }
        public string Cover { get; set; }
        public string Link { get; set; }
        public List<Read> Reads { get; set; }
    }
}