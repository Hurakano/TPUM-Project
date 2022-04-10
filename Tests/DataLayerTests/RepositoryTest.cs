using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ApplicationDataLayer.Test
{
    [TestClass]
    public class RepositoryTest
    {
        [TestMethod]
        public void TestAddBook()
        {
            LibraryRepository repository = new ApplicationDataLayer.LibraryRepository();
            Book book1 = new Book("Title1", new DateTime(2010, 5, 5, 0, 0, 0), "Author1");
            Book book2 = new Book("Title2", new DateTime(2015, 1, 4, 0, 0, 0), "Author2");

            repository.AddBook(book1);
            repository.AddBook(book2);

            List<Book> booksIn = repository.GetBooks();
            Assert.AreEqual(booksIn.Count, 2);
            Assert.AreEqual(booksIn[1].Author, "Author2");

            Book bookCheck = repository.GetBook(0);
            Assert.AreEqual(bookCheck.Title, "Title1");
        }

        [TestMethod]
        public void TestRemoveBook()
        {
            LibraryRepository repository = new ApplicationDataLayer.LibraryRepository();
            Book book1 = new Book("Title1", new DateTime(2010, 5, 5, 0, 0, 0), "Author1");
            Book book2 = new Book("Title2", new DateTime(2015, 1, 4, 0, 0, 0), "Author2");

            repository.AddBook(book1);
            repository.AddBook(book2);

            repository.RemoveBook(0);

            List<Book> booksIn = repository.GetBooks();
            Assert.AreEqual(booksIn.Count, 1);
            Assert.AreEqual(booksIn[0].Title, "Title2");
        }
    }
}
