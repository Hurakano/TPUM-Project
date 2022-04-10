using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
using ApplicationDataLayer;

namespace BusinessLogicLayer.Test
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
        public void TestGetBookById()
        {
            DataMock.Setup(p => p.GetBook(3)).Returns(new Book("MyTitle", new DateTime(2010, 2, 2, 0, 0, 0), "MyAuthor"));
            Book bookCheck = Logic.GetBookById(3);
            Assert.AreEqual(bookCheck.Title, "MyTitle");
        }
    }
}
