using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryClient.LibraryClientData
{
    public abstract class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
    }
}
