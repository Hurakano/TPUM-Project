using System;
using System.Collections.Generic;
using System.Text;

namespace PresentationLayer.LibraryModel
{
    public class BookPresenter
    {
        public Guid Id { get; private set; }
        public string Title { get; set; }
        public string Author { get; set; }

        public string TitleWithAuthor
        {
            get => Title + ", " + Author;
        }

        public BookPresenter(Guid id, string _title, string _author)
        {
            Id = id;
            Title = _title;
            Author = _author;
        }
    }
}
