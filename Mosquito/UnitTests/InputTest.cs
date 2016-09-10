using System;
using Input.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class InputTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var provider = new InputDataProvider("mosdb.xlsx");

            provider.GetInputData();
        }
    }
}
