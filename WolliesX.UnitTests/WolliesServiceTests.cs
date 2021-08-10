using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WolliesX.UnitTests
{
    [TestClass]
    public class WolliesServiceTests
    {
        
        [TestMethod]
        public void GetResourceShouldNotBeEqual()
        {
            var initialValue = "some value";
            var responseValue = $"{initialValue} and more";

            Assert.AreNotEqual(initialValue, responseValue);
        }
    }
}
