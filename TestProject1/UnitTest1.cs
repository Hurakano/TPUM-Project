using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestProgram;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test1()
        {
           Class1 testObject = new Class1();
            testObject.function1();
        }
    }
}
