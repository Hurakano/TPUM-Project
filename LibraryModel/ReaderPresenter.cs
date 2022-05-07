using System;
using System.Collections.Generic;
using System.Text;

namespace PresentationLayer.LibraryModel
{
    public class ReaderPresenter
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }

        public ReaderPresenter(Guid _id, string _name)
        {
            Id = _id;
            Name = _name;
        }
    }
}
