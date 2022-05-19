using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryServer.ApplicationDataLayer
{
    internal class DataContext
    {
        public Dictionary<Guid, Book> Books;
        public Dictionary<Guid, Reader> Readers;
        public Dictionary<Guid, Loan> Loans;

        public DataContext()
        {
            Books = new Dictionary<Guid, Book>();
            Readers = new Dictionary<Guid, Reader>();
            Loans = new Dictionary<Guid, Loan>();
        }
    }
}
