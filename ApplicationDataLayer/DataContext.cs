using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationDataLayer
{
    public class DataContext
    {
        public List<Book> Books;
        public List<Reader> Readers;
        public List<Loan> Loans;

        public DataContext()
        {
            Books = new List<Book>();
            Readers = new List<Reader>();
            Loans = new List<Loan>();
        }
    }
}
