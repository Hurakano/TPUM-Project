using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
using LibraryServer.ApplicationDataLayer;
using System.Collections.Generic;

namespace LibraryServer.BusinessLogicLayer.Test
{
    [TestClass]
    public class LibraryLogicTest
    {
        private Mock<ILibraryData> DataMock;
        private IBusinessLogic Logic;

        [TestInitialize]
        public void SetUp()
        {
            DataMock = new Mock<ILibraryData>();
            Logic = new LibraryLogic(DataMock.Object);
        }

        [TestMethod]
        public void TestAddMethods()
        {
            string title = "Title";
            string name = "Name";
            BookDTO book = new BookDTO { Title = title, Author = "Author" };
            ReaderDTO reader = new ReaderDTO { Name = name };

            Logic.AddNewBook(book);
            Logic.AddNewReader(reader);
            
            DataMock.Verify(p => p.AddBook(It.Is<Book>(obj => obj.Title == title)), Times.Once());
            DataMock.Verify(p => p.AddReader(It.Is<Reader>(obj => obj.Name == name)), Times.Once());
        }

        [TestMethod]
        public void TestRemoveMethods()
        {
            Guid testId = new Guid();
            DataMock.Setup(p => p.GetLoans()).Returns(new Dictionary<Guid, Loan>());
            Loan loan = new Loan(new Guid(), new Guid(), new DateTime(), new DateTime());
            DataMock.Setup(p => p.GetLoan(testId)).Returns(loan);

            Logic.RemoveBookById(testId);
            Logic.RemoveReaderById(testId);
            Logic.RemoveLoanById(testId);

            DataMock.Verify(p => p.RemoveBook(testId), Times.Once());
            DataMock.Verify(p => p.RemoveReader(testId), Times.Once());
            DataMock.Verify(p => p.RemoveLoan(testId), Times.Once());
        }

        [TestMethod]
        public void TestRemoveWithActiveLoan()
        {
            Guid bookId = Guid.NewGuid();
            Guid readerId = Guid.NewGuid();
            Guid loanId = Guid.NewGuid();
            Loan loan = new Loan(bookId, readerId, new DateTime(), new DateTime());
            Dictionary<Guid, Loan> loans = new Dictionary<Guid, Loan> { { loanId, loan } };
            DataMock.Setup(p => p.GetLoans()).Returns(loans);

            Guid otherId = Guid.NewGuid();
            Logic.RemoveBookById(otherId);
            DataMock.Verify(p => p.RemoveBook(otherId), Times.Once());

            Logic.RemoveBookById(bookId);
            Logic.RemoveReaderById(readerId);
            DataMock.Verify(p => p.RemoveBook(It.Is<Guid>(id => id != otherId)), Times.Never());
            DataMock.Verify(p => p.RemoveReader(It.IsAny<Guid>()), Times.Never());
        }

        [TestMethod]
        public void TestGetBookById()
        {
            Guid testId = Guid.NewGuid();
            string title = "MyTitle";
            DataMock.Setup(p => p.GetBook(testId)).Returns(new Book(title, new DateTime(2010, 2, 2, 0, 0, 0), "MyAuthor"));
            BookDTO bookCheck = Logic.GetBookById(testId);
            Assert.AreEqual(bookCheck.Title, title);
        }

        [TestMethod]
        public void TestGetBookByTitle()
        {
            string myTitle = "MyTitle";
            Book book1 = new Book("OtherTitle", new DateTime(), "Author1");
            Book book2 = new Book(myTitle, new DateTime(), "Authore2");
            Dictionary<Guid, Book> books = new Dictionary<Guid, Book>
            {
                { Guid.NewGuid(), book1 },
                { Guid.NewGuid(), book2 }
            };
            DataMock.Setup(p => p.GetBooks()).Returns(books);

            BookDTO bookCheck = Logic.GetBookByTitle(myTitle);
            Assert.IsNotNull(bookCheck);
            Assert.AreEqual(bookCheck.Title, myTitle);
        }

        [TestMethod]
        public void TestGetAllBooks()
        {
            Book testBook = new Book("Title", new DateTime(), "Author");
            Dictionary<Guid, Book> books = new Dictionary<Guid, Book>();
            int bookCount = 10;
            for (int i = 0; i < bookCount; i++)
                books.Add(Guid.NewGuid(), testBook);
            DataMock.Setup(p => p.GetBooks()).Returns(books);

            List<BookDTO> bookList = Logic.GetAllBooks();
            DataMock.Verify(p => p.GetBooks(), Times.Once());
            Assert.AreEqual(bookList.Count, bookCount);
        }

        [TestMethod]
        public void TestGetAvailableBooks()
        {
            Guid loanedBookId = Guid.NewGuid();
            Book testBook = new Book("Title", new DateTime(), "Author");
            Dictionary<Guid, Book> books = new Dictionary<Guid, Book>()
            {
                { Guid.NewGuid(), testBook },
                { loanedBookId, testBook },
                { Guid.NewGuid(), testBook }
            };
            Loan loan = new Loan(loanedBookId, Guid.NewGuid(), new DateTime(), new DateTime());
            Dictionary<Guid, Loan> loans = new Dictionary<Guid, Loan> { { Guid.NewGuid(), loan } };
            DataMock.Setup(p => p.GetBooks()).Returns(books);
            DataMock.Setup(p => p.GetLoans()).Returns(loans);

            List<BookDTO> bookList = Logic.GetAvailableBooks();
            DataMock.Verify(p => p.GetBooks(), Times.Once());
            DataMock.Verify(p => p.GetLoans(), Times.AtLeastOnce());
            Assert.AreEqual(bookList.Count, 2);
        }

        [TestMethod]
        public void TestIsBookLoaned()
        {
            Guid freeBookId = Guid.NewGuid();
            Guid loanedBookId = Guid.NewGuid();
            Book testBook = new Book("Title", new DateTime(), "Author");
            Dictionary<Guid, Book> books = new Dictionary<Guid, Book>()
            {
                { Guid.NewGuid(), testBook },
                { loanedBookId, testBook },
                { freeBookId, testBook }
            };
            Loan loan = new Loan(loanedBookId, Guid.NewGuid(), new DateTime(), new DateTime());
            Dictionary<Guid, Loan> loans = new Dictionary<Guid, Loan> { { Guid.NewGuid(), loan } };
            DataMock.Setup(p => p.GetBooks()).Returns(books);
            DataMock.Setup(p => p.GetLoans()).Returns(loans);

            Assert.IsTrue(Logic.IsBookLoaned(loanedBookId));
            Assert.IsFalse(Logic.IsBookLoaned(freeBookId));
            DataMock.Verify(p => p.GetLoans(), Times.AtLeastOnce());
        }

        [TestMethod]
        public void TestGetReaderById()
        {
            Guid testId = Guid.NewGuid();
            string name = "MyName";
            DataMock.Setup(p => p.GetReader(testId)).Returns(new Reader(name, 20, "Address"));
            ReaderDTO readerCheck = Logic.GetReaderById(testId);
            Assert.AreEqual(readerCheck.Name, name);
        }

        [TestMethod]
        public void TestGetReaderByName()
        {
            string myName = "MyName";
            Reader reader1 = new Reader("OtherName", 20, "Address1");
            Reader reader2 = new Reader(myName, 20, "Address2");
            Dictionary<Guid, Reader> readers = new Dictionary<Guid, Reader>
            {
                { Guid.NewGuid(), reader1 },
                { Guid.NewGuid(), reader2 }
            };
            DataMock.Setup(p => p.GetReaders()).Returns(readers);

            ReaderDTO readerCheck = Logic.GetReaderByName(myName);
            Assert.IsNotNull(readerCheck);
            Assert.AreEqual(readerCheck.Name, myName);
        }

        [TestMethod]
        public void TestGetAllReaders()
        {
            Reader testReader = new Reader("Name", 20, "Address");
            Dictionary<Guid, Reader> readers = new Dictionary<Guid, Reader>();
            int readerCount = 10;
            for (int i = 0; i < readerCount; i++)
                readers.Add(Guid.NewGuid(), testReader);
            DataMock.Setup(p => p.GetReaders()).Returns(readers);

            List<ReaderDTO> readerList = Logic.GetAllReaders();
            DataMock.Verify(p => p.GetReaders(), Times.Once());
            Assert.AreEqual(readerList.Count, readerCount);
        }

        [TestMethod]
        public void TestLoanBook()
        {
            Guid bookId = Guid.NewGuid();
            Guid readerId = Guid.NewGuid();
            DateTime dateStart = new DateTime(2020);
            DateTime dateEnd = new DateTime(2021);
            Book book = new Book("Title", new DateTime(), "Author");
            Reader reader = new Reader("Name", 20, "Address");
            Dictionary<Guid, Loan> loans = new Dictionary<Guid, Loan>();
            DataMock.Setup(p => p.GetBook(bookId)).Returns(book);
            DataMock.Setup(p => p.GetReader(readerId)).Returns(reader);
            DataMock.Setup(p => p.GetLoans()).Returns(loans);

            Logic.LoanBook(readerId, bookId, dateStart, dateEnd);
            DataMock.Verify(p => p.AddLoan(It.Is<Loan>(l => l.ReaderId == readerId &&
                                                            l.BookId == bookId &&
                                                            l.BorrowDate == dateStart &&
                                                            l.ReturnDate == dateEnd)), Times.Once());
        }

        [TestMethod]
        public void TestLoanBookNegative()
        {
            Guid bookId = Guid.NewGuid();
            Guid readerId = Guid.NewGuid();
            Guid inegsistingId = Guid.NewGuid();
            Book book = new Book("Title", new DateTime(), "Author");
            Reader reader = new Reader("Name", 20, "Address");
            Loan loan = new Loan(bookId, readerId, new DateTime(), new DateTime());
            Dictionary<Guid, Loan> loans = new Dictionary<Guid, Loan>();
            DataMock.Setup(p => p.GetBook(bookId)).Returns(book);
            DataMock.Setup(p => p.GetBook(It.Is<Guid>(g => g != bookId))).Returns(null as Book);
            DataMock.Setup(p => p.GetReader(readerId)).Returns(reader);
            DataMock.Setup(p => p.GetReader(It.Is<Guid>(g => g != readerId))).Returns(null as Reader);
            DataMock.Setup(p => p.GetLoans()).Returns(loans);

            Logic.LoanBook(inegsistingId, bookId, new DateTime(), new DateTime());
            Logic.LoanBook(readerId, inegsistingId, new DateTime(), new DateTime());
            loans.Add(Guid.NewGuid(), loan);
            Logic.LoanBook(readerId, bookId, new DateTime(), new DateTime());

            DataMock.Verify(p => p.AddLoan(It.IsAny<Loan>()), Times.Never());
        }

        [TestMethod]
        public void TestGetLoanById()
        {
            Guid bookId = Guid.NewGuid();
            Guid readerId = Guid.NewGuid();
            Guid loanId = Guid.NewGuid();
            Loan loan = new Loan(bookId, readerId, new DateTime(), new DateTime());
            DataMock.Setup(p => p.GetLoan(loanId)).Returns(loan);

            LoanDTO loanCheck = Logic.GetLoanById(loanId);
            Assert.IsNotNull(loanCheck);
            Assert.AreEqual(loanCheck.BookId, bookId);
            Assert.AreEqual(loanCheck.ReaderId, readerId);
        }

        [TestMethod]
        public void TestGetAllLoans()
        {
            Loan testLoan = new Loan(Guid.NewGuid(), Guid.NewGuid(), new DateTime(), new DateTime());
            int loanCount = 10;
            Dictionary<Guid, Loan> loans = new Dictionary<Guid, Loan>();
            for(int i = 0; i < loanCount; i++)
            {
                loans.Add(Guid.NewGuid(), testLoan);
            }
            DataMock.Setup(p => p.GetLoans()).Returns(loans);

            List<LoanDTO> loanList = Logic.GetAllLoans();
            Assert.AreEqual(loanList.Count, loanCount);
        }

        [TestMethod]
        public void TestGetLoansByReader()
        {
            Guid readerId = Guid.NewGuid();
            Guid otherReader = Guid.NewGuid();
            Loan loanWithReader = new Loan(Guid.NewGuid(), readerId, new DateTime(), new DateTime());
            Loan loanWithAnotherReader = new Loan(Guid.NewGuid(), otherReader, new DateTime(), new DateTime());
            Dictionary<Guid, Loan> loans = new Dictionary<Guid, Loan>
            {
                {Guid.NewGuid(), loanWithReader },
                {Guid.NewGuid(), loanWithAnotherReader },
                {Guid.NewGuid(), loanWithReader }
            };
            DataMock.Setup(p => p.GetLoans()).Returns(loans);

            List<LoanDTO> loanList = Logic.GetAllLoansByReader(readerId);
            Assert.AreEqual(loanList.Count, 2);
        }

        [TestMethod]
        public void TestGetLoanByBook()
        {
            Guid bookId = Guid.NewGuid();
            Guid otherBookId = Guid.NewGuid();
            Loan loanWithBook = new Loan(bookId, Guid.NewGuid(), new DateTime(), new DateTime());
            Loan loanWithOtherBook = new Loan(otherBookId, Guid.NewGuid(), new DateTime(), new DateTime());
            Dictionary<Guid, Loan> loans = new Dictionary<Guid, Loan>
            {
                {Guid.NewGuid(), loanWithOtherBook },
                {Guid.NewGuid(), loanWithBook }
            };
            DataMock.Setup(p => p.GetLoans()).Returns(loans);

            LoanDTO loan = Logic.GetLoanByBook(bookId);
            Assert.IsNotNull(loan);
            Assert.AreEqual(loan.BookId, bookId);
        }

        [TestMethod]
        public void TestGetLoanBetweenDates()
        {
            Dictionary<Guid, Loan> loans = new Dictionary<Guid, Loan>();
            int startYear = 2000;
            for (int i = 0; i < 5; i++)
            {
                Loan loan = new Loan(Guid.NewGuid(), Guid.NewGuid(), new DateTime(startYear + i), new DateTime(2010));
                loans.Add(Guid.NewGuid(), loan);
            }
            DataMock.Setup(p => p.GetLoans()).Returns(loans);

            List<LoanDTO> loanCheck = Logic.GetLoansBetweenDates(new DateTime(2000), new DateTime(2010));
            Assert.AreEqual(loanCheck.Count, 5);
            loanCheck = Logic.GetLoansBetweenDates(new DateTime(2002), new DateTime(2003));
            Assert.AreEqual(loanCheck.Count, 2);
            loanCheck = Logic.GetLoansBetweenDates(new DateTime(2006), new DateTime(2020));
            Assert.AreEqual(loanCheck.Count, 0);
        }

        [TestMethod]
        public void TestGetOverdueLoans()
        {
            Dictionary<Guid, Loan> loans = new Dictionary<Guid, Loan>();
            int startYear = 2010;
            for (int i = 0; i < 5; i++)
            {
                Loan loan = new Loan(Guid.NewGuid(), Guid.NewGuid(), new DateTime(2000), new DateTime(startYear + i));
                loans.Add(Guid.NewGuid(), loan);
            }
            DataMock.Setup(p => p.GetLoans()).Returns(loans);

            List<LoanDTO> loanCheck = Logic.GetOverdueLoans(new DateTime(2010));
            Assert.AreEqual(loanCheck.Count, 0);
            loanCheck = Logic.GetOverdueLoans(new DateTime(2013));
            Assert.AreEqual(loanCheck.Count, 3);
            loanCheck = Logic.GetOverdueLoans(new DateTime(2020));
            Assert.AreEqual(loanCheck.Count, 5);
        }

        class ObserverTestClass : IObserver<List<LoanDTO>>
        {
            public List<LoanDTO> ReceivedEvents = new List<LoanDTO>();
            public void OnCompleted()
            {

            }

            public void OnError(Exception error)
            {

            }

            public void OnNext(List<LoanDTO> value)
            {
                ReceivedEvents = value;
            }
        }

        [TestMethod]
        public void TestOverdueSubscriber()
        {
            Dictionary<Guid, Loan> loans = new Dictionary<Guid, Loan>();
            DateTime currentDate = DateTime.Now;
            Loan loan = new Loan(Guid.NewGuid(), Guid.NewGuid(), new DateTime(2000), currentDate.AddDays(-2));
            loans.Add(Guid.NewGuid(), loan);
            loan = new Loan(Guid.NewGuid(), Guid.NewGuid(), new DateTime(2000), currentDate.AddDays(2));
            loans.Add(Guid.NewGuid(), loan);
            DataMock.Setup(p => p.GetLoans()).Returns(loans);

            ObserverTestClass observer = new ObserverTestClass();
            Logic.SubscribeToOverdueEvent(observer);

            LibraryLogic myLogic = (LibraryLogic)Logic;
            myLogic.CheckForOverdueLoans(null, null);

            Assert.AreEqual(observer.ReceivedEvents.Count, 1);
        }
    }
}
