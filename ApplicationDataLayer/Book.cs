using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationDataLayer
{
    public class Book
    {
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Author { get; set; }

        public Book(string _title, DateTime _publicationDate, string _author)
        {
            Title = _title;
            PublicationDate = _publicationDate;
            Author = _author;
        }
    }
}
