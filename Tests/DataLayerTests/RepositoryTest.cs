using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationDataLayer.Test
{
    [TestClass]
    public class RepositoryTest
    {
        [TestMethod]
        public void TestAddBook()
        {
            ILibraryData repository = new ApplicationDataLayer.LibraryRepository();
            string bookTitle1 = "Title1";
            string bookTitle2 = "Title2";
            string bookAuthor1 = "Author1";
            string bookAuthor2 = "Author2";
            DateTime bookDate = new DateTime(2010, 5, 5, 0, 0, 0);
            Book book1 = new Book(bookTitle1, bookDate, bookAuthor1);
            Book book2 = new Book(bookTitle2, new DateTime(2015, 1, 4, 0, 0, 0), bookAuthor2);

            Guid bookId1 = repository.AddBook(book1);
            Guid bookId2 = repository.AddBook(book2);

            Dictionary<Guid, Book> booksIn = repository.GetBooks();
            Assert.AreEqual(booksIn.Count, 2);
            Assert.AreEqual(booksIn[bookId2].Author, bookAuthor2);

            Book bookCheck = repository.GetBook(bookId1);
            Assert.IsNotNull(bookCheck);
            Assert.AreEqual(bookCheck.Title, bookTitle1);
            Assert.AreEqual(bookCheck.PublicationDate, bookDate);
        }

        [TestMethod]
        public void TestUpdateBook()
        {
            ILibraryData repository = new ApplicationDataLayer.LibraryRepository();
            Book book1 = new Book("Title1", new DateTime(2010, 5, 5, 0, 0, 0), "Author1");
            Book book2 = new Book("Title2", new DateTime(2015, 1, 4, 0, 0, 0), "Author2");

            repository.AddBook(book1);
            Guid bookId2 = repository.AddBook(book2);

            string titleUpdate = "Updated title";
            book2.Title = titleUpdate;
            repository.UpdateBook(bookId2, book2);

            Book bookCheck = repository.GetBook(bookId2);
            Assert.AreEqual(bookCheck.Title, titleUpdate);
        }

        [TestMethod]
        public void TestRemoveBook()
        {
            ILibraryData repository = new ApplicationDataLayer.LibraryRepository();
            string bookAuthor2 = "Author2";
            Book book1 = new Book("Title1", new DateTime(2010, 5, 5, 0, 0, 0), "Author1");
            Book book2 = new Book("Title2", new DateTime(2015, 1, 4, 0, 0, 0), bookAuthor2);

            Guid bookId1 = repository.AddBook(book1);
            repository.AddBook(book2);

            repository.RemoveBook(bookId1);

            Dictionary<Guid, Book> booksIn = repository.GetBooks();
            Assert.AreEqual(booksIn.Count, 1);
            Assert.IsNull(repository.GetBook(bookId1));
        }

        [TestMethod]
        public void TestAddReader()
        {
            ILibraryData repository = new ApplicationDataLayer.LibraryRepository();
            string name1 = "Name1";
            string name2 = "Name2";
            uint age1 = 20;
            uint age2 = 50;
            string address1 = "Address1";
            string address2 = "Address2";
            Reader reader1 = new Reader(name1, age1, address1);
            Reader reader2 = new Reader(name2, age2, address2);

            Guid readerId1 = repository.AddReader(reader1);
            Guid readerId2 = repository.AddReader(reader2);

            Dictionary<Guid, Reader> readers = repository.GetReaders();
            Assert.AreEqual(readers.Count, 2);
            Assert.AreEqual(readers[readerId2].Address, address2);

            Reader readerCheck = repository.GetReader(readerId1);
            Assert.IsNotNull(readerCheck);
            Assert.AreEqual(readerCheck.Name, name1);
            Assert.AreEqual(readerCheck.Age, age1);
        }

        [TestMethod]
        public void TestUpdateReader()
        {
            ILibraryData repository = new ApplicationDataLayer.LibraryRepository();
            Reader reader1 = new Reader("Name1", 20, "Address1");
            Reader reader2 = new Reader("Name2", 30, "Address2");

            repository.AddReader(reader1);
            Guid readerId2 = repository.AddReader(reader2);

            string updateAddress = "Updated address";
            reader2.Address = updateAddress;
            repository.UpdateReader(readerId2, reader2);

            Reader readerCheck = repository.GetReader(readerId2);
            Assert.AreEqual(readerCheck.Address, updateAddress);
        }

        [TestMethod]
        public void TestRemoveReader()
        {
            ILibraryData repository = new ApplicationDataLayer.LibraryRepository();
            string readerName2 = "Name2";
            Reader reader1 = new Reader("Name1", 20, "Address1");
            Reader reader2 = new Reader(readerName2, 30, "Address2");

            Guid readerId1 = repository.AddReader(reader1);
            repository.AddReader(reader2);

            repository.RemoveReader(readerId1);

            Dictionary<Guid, Reader> readersIn = repository.GetReaders();
            Assert.AreEqual(readersIn.Count, 1);
            Assert.IsNull(repository.GetReader(readerId1));
        }

        [TestMethod]
        public void TestAddLoan()
        {
            ILibraryData repository = new ApplicationDataLayer.LibraryRepository();
            Book book = new Book("Title", new DateTime(2000), "Author");
            Reader reader = new Reader("Name", 20, "Address");

            Guid bookId = repository.AddBook(book);
            Guid readerId = repository.AddReader(reader);

            DateTime date1 = new DateTime(2005);
            DateTime date2 = new DateTime(2010);
            Loan loan = new Loan(bookId, readerId, date1, date2);
            Guid loanId = repository.AddLoan(loan);

            Dictionary<Guid, Loan> loans = repository.GetLoans();
            Assert.AreEqual(loans.Count, 1);
            Assert.AreEqual(loans[loanId].BookId, bookId);
            Assert.AreEqual(loans[loanId].ReaderId, readerId);
            Assert.AreEqual(loans[loanId].BorrowDate, date1);
            Assert.AreEqual(loans[loanId].ReturnDate, date2);
        }

        [TestMethod]
        public void TestUpdateLoan()
        {
            ILibraryData repository = new ApplicationDataLayer.LibraryRepository();
            Book book = new Book("Title", new DateTime(2000), "Author");
            Reader reader = new Reader("Name", 20, "Address");

            Guid bookId = repository.AddBook(book);
            Guid readerId = repository.AddReader(reader);

            Loan loan = new Loan(bookId, readerId, new DateTime(2005), new DateTime(2010));
            Guid loanId = repository.AddLoan(loan);

            DateTime newDate = new DateTime(2020);
            loan.ReturnDate = newDate;
            repository.UpdateLoan(loanId, loan);

            Loan loanCheck = repository.GetLoan(loanId);
            Assert.AreEqual(loanCheck.ReturnDate, newDate);
        }

        [TestMethod]
        public void TestRemoveLoan()
        {
            ILibraryData repository = new ApplicationDataLayer.LibraryRepository();
            Book book = new Book("Title", new DateTime(2000), "Author");
            Reader reader = new Reader("Name", 20, "Address");

            Guid bookId = repository.AddBook(book);
            Guid readerId = repository.AddReader(reader);

            Loan loan = new Loan(bookId, readerId, new DateTime(2005), new DateTime(2010));
            Guid loanId = repository.AddLoan(loan);

            Loan loanCheck = repository.GetLoan(loanId);
            Assert.IsNotNull(loanCheck);

            repository.RemoveLoan(loanId);
            Dictionary<Guid, Loan> loans = repository.GetLoans();
            Assert.AreEqual(loans.Count, 0);
            Assert.IsNull(repository.GetLoan(loanId));
        }

        [TestMethod]
        public void TestDataFiller()
        {
            ILibraryData repository = new ApplicationDataLayer.LibraryRepository();
            IDataFiller filler = new ApplicationDataLayer.ExampleFiller();

            repository.FillData(filler);

            Dictionary<Guid, Book> books = repository.GetBooks();
            Dictionary<Guid, Reader> readers = repository.GetReaders();
            Dictionary<Guid, Loan> loans = repository.GetLoans();

            Assert.AreEqual(books.Count, 5);
            Assert.AreEqual(readers.Count, 3);
            Assert.AreEqual(loans.Count, 3);

            Assert.AreEqual(readers.ElementAt(0).Value.Name, "Jan Kowalski");
        }
    }
}
