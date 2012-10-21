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
        public void IranSystem_Normal_Test()
        {
            Assert.AreEqual("محمد", ConvertTo.Unicode("¢ُںُ"));
        }

        [Test]
        public void IranSystem_Number_Test()
        {
            Assert.AreEqual("123", ConvertTo.Unicode("123"));
        }

        [Test]
        public void IranSystem_TwoBytesCharacters_One_Accurance_Test()
        {
            Assert.AreEqual("کلاف", ConvertTo.Unicode(" éٍî").Trim());
        }

        [Test]
        public void IranSystem_TwoBytesCharacters_More_Than_One_Accurance_Test()
        {
            Assert.AreEqual("کلاف کلاف", ConvertTo.Unicode("éٍîéٍî").Trim());
        }
    }
}
