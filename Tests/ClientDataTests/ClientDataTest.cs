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
            Reader testReader = new Reader { Id = Guid.NewGuid(), Name = readerName };
            string readerMessage = "Readers\n1\n" + testReader.Id.ToString() + "\n" + testReader.Name + "\n";
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
            Book testBook = new Book { Id = bookId, Title = bookTitle, Author = "Author" };
            string bookMessage = "Books\n1\n" + testBook.Id.ToString() + "\n" + testBook.Title + "\n" + testBook.Author + "\n";
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
            Loan testLoan = new Loan { Id = loanId, BookId = bookId, ReaderId = Guid.NewGuid(), BorrowDate = new DateTime(), ReturnDate = new DateTime() };
            string loanMessage = "Loans\n1\n" + testLoan.Id.ToString() + "\n" + testLoan.BookId.ToString() + "\n" + testLoan.ReaderId.ToString() + "\n" + testLoan.BorrowDate.ToString() + "\n" + testLoan.ReturnDate.ToString() + "\n";
            ((ClientData)Client).HandleMessage(loanMessage);

            Loan loanCheck = Client.GetLoanById(loanId);
            Assert.IsNotNull(loanCheck);
            Assert.AreEqual(loanCheck.BookId, bookId);
        }
    }
}
