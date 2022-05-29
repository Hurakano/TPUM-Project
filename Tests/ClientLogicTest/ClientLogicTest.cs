using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using LibraryClient.LibraryClientData;

namespace LibraryClient.LibraryClientLogic.Test
{
    [TestClass]
    public class ClientLogicTest
    {
        private Mock<IClientData> ClientDataMock;
        private ClientLogic Logic;

        [TestInitialize]
        public void SetUp()
        {
            ClientDataMock = new Mock<IClientData>();
            Logic = new ClientLogic(ClientDataMock.Object);
        }

        [TestMethod]
        public void TestGetBooks()
        {
            Guid bookId = Guid.NewGuid();
            string bookTitle = "BookTitleTest";
            Book testBook = new Book { Id = bookId, Title = bookTitle };
            ClientDataMock.Setup(p => p.GetBookById(bookId)).Returns(testBook);

            BookDTO bookCheck = Logic.GetBookById(bookId);
            Assert.IsNotNull(bookCheck);
            Assert.AreEqual(bookCheck.Title, bookTitle);
        }

        [TestMethod]
        public void TestGetLoans()
        {
            Guid bookId = Guid.NewGuid();
            Guid loanId = Guid.NewGuid();
            Loan loanTest = new Loan { Id = loanId, BookId = bookId, ReaderId = Guid.NewGuid() };
            ClientDataMock.Setup(p => p.GetLoanById(loanId)).Returns(loanTest);

            LoanDTO loanCheck = Logic.GetLoanById(loanId);
            Assert.IsNotNull(loanCheck);
            Assert.AreEqual(loanCheck.BookId, bookId);
        }

        [TestMethod]
        public void TestBorrowBook()
        {
            Guid bookId = Guid.NewGuid();
            Guid readerId = Guid.NewGuid();
            DateTime returnTime = new DateTime(2001);
            Book book = new Book { Id = bookId };
            ClientDataMock.Setup(p => p.GetAvailableBooks()).Returns(new List<Book> { book });
            Logic.BorrowBook(bookId, readerId, returnTime);

            ClientDataMock.Verify(p => p.BorrowBook(readerId, bookId, returnTime), Times.Once());
        }
    }
}
