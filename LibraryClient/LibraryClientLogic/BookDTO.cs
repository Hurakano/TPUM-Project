using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryClient.LibraryClientLogic
{
    public abstract class BookDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
    }
}
