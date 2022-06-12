using System;
using System.Collections.Generic;
using System.Text;

namespace PresentationLayer.LibraryModel
{
    public abstract class BookPresenter
    {
        public Guid Id { get; private set; }
        public string Title { get; set; }
        public string Author { get; set; }

        public virtual string TitleWithAuthor { get; }

        public BookPresenter(Guid id, string _title, string _author)
        {
            Id = id;
            Title = _title;
            Author = _author;
        }
    }
}
