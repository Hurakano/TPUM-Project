using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryServer.ApplicationDataLayer
{
    public class ExampleFiller : IDataFiller
    {
        public void FillData(ILibraryData data)
        {
            data.AddBook("Brave New World", new DateTime(1932), "Aldous Huxley");
            Guid book2 = data.AddBook("Limes inferior", new DateTime(1982), "Janusz A. Zajdel");
            data.AddBook("Paradyzja", new DateTime(1984), "Janusz A. Zajdel");
            Guid book4 = data.AddBook("Animal Farm: A Fairy Story", new DateTime(1945, 8, 17), "George Orwell");
            Guid book5 = data.AddBook("The Da Vinci Code", new DateTime(2003, 4, 1), "Dan Brown");

            Guid reader1 = data.AddReader("Jan Kowalski", 32, "Uliczna 2");
            data.AddReader("Kararzyna Łęcka", 26, "Mostowa 5");
            Guid reader3 = data.AddReader("Stefan Kowalczyk", 40, "Miejska 43");

            data.AddLoan(book2, reader1, new DateTime(2022, 2, 1, 10, 25, 0), new DateTime(2022, 5, 2, 0, 0, 0));
            data.AddLoan(book4, reader1, new DateTime(2022, 2, 10, 15, 16, 0), new DateTime(2022, 5, 11, 0, 0, 0));
            data.AddLoan(book5, reader3, new DateTime(2022, 4, 6, 12, 43, 0), new DateTime(2022, 7, 7, 0, 0, 0));
        }
    }
}
