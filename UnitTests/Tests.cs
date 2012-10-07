using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using IranSystemConvertor;

namespace UnitTests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void IranSystemTest()
        {
            string expected0 = "محمد";
            string expected1 = "123";

            string actual0 = ConvertTo.Unicode("¢ُںُ");
            string actual1 = ConvertTo.Unicode("123");

            Assert.AreEqual(expected0, actual0);
            Assert.AreEqual(expected1, actual1);
        }

    }
}
