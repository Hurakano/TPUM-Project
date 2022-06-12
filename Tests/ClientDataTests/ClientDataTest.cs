using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;

namespace LibraryClient.LibraryClientData.Test
{
    [TestClass]
    public class ClientDataTest
    {
        private IClientData Client;

        [TestInitialize]
        public void SetUp()
        {
            Client = ClientDataFactory.GetClient();
        }

        [TestMethod]
        public void TestGetReaders()
        {
            string readerName = "ReaderNameTest";
            Reader testReader = new ReaderImplTest { Id = Guid.NewGuid(), Name = readerName };
            List<Reader> readers = new List<Reader> { testReader };
            
            string readerMessage = "Readers\n" + DataSerializer.ListReaderToJson(readers);
            ((ClientData)Client).HandleMessage(readerMessage);

            List<Reader> readersCheck = Client.GetReaders();
            Assert.AreEqual(readersCheck.Count, 1);
            Assert.AreEqual(readersCheck[0].Name, readerName);
        }

        [TestMethod]
        public void TestGetBooks()
        {
            Guid bookId = Guid.NewGuid();
            string bookTitle = "BookTitleTest";
            Book testBook = new BookImplTest { Id = bookId, Title = bookTitle, Author = "Author" };
            List<Book> books = new List<Book> { testBook };
            string bookMessage = "Books\n" + DataSerializer.ListBookToJson(books);
            ((ClientData)Client).HandleMessage(bookMessage);

            Book bookCheck = Client.GetBookById(bookId);
            Assert.IsNotNull(bookCheck);
            Assert.AreEqual(bookCheck.Title, bookTitle);
        }

        [TestMethod]
        public void TestGetLoan()
        {
            Guid bookId = Guid.NewGuid();
            Guid loanId = Guid.NewGuid();
            Loan testLoan = new LoanImplTest { Id = loanId, BookId = bookId, ReaderId = Guid.NewGuid(), BorrowDate = new DateTime(), ReturnDate = new DateTime() };
            List<Loan> loans = new List<Loan> { testLoan };
            string loanMessage = "Loans\n" + DataSerializer.ListLoanToJson(loans);
            ((ClientData)Client).HandleMessage(loanMessage);

            Loan loanCheck = Client.GetLoanById(loanId);
            Assert.IsNotNull(loanCheck);
            Assert.AreEqual(loanCheck.BookId, bookId);
        }
    }

    class BookImplTest: Book
    {

    }

    class ReaderImplTest: Reader
    {

    }

    class LoanImplTest: Loan
    {

    }
}
