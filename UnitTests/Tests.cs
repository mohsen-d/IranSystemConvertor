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
            Assert.AreEqual("محمد", ConvertTo.UnicodeFrom(TextEncoding.Arabic1256, "¢ُںُ"));
        }

        [Test]
        public void IranSystem_Number_Test()
        {
            Assert.AreEqual("123", ConvertTo.UnicodeFrom(TextEncoding.Arabic1256, "123"));
        }

        [Test]
        public void IranSystem_Long_Number_Test()
        {
            Assert.AreEqual("3000000000000000000", ConvertTo.UnicodeFrom(TextEncoding.Arabic1256, "3000000000000000000"));
        }

        [Test]
        public void IranSystem_TwoBytesCharacters_One_Accurance_Test()
        {
            Assert.AreEqual("کلاف", ConvertTo.UnicodeFrom(TextEncoding.Arabic1256, " éٍî").Trim());
        }

        [Test]
        public void IranSystem_TwoBytesCharacters_More_Than_One_Accurance_Test()
        {
            Assert.AreEqual("کلاف کلاف", ConvertTo.UnicodeFrom(TextEncoding.Arabic1256, "éٍîéٍî").Trim());
        }

        [Test]
        public void IranSystem_CP1252_Can_Be_Converted_By_Arabic1256()
        {
            Assert.AreEqual("ليست", ConvertTo.UnicodeFrom(TextEncoding.Arabic1256, "–¨‏َ"));
            Assert.AreEqual("مناطق تهران", ConvertTo.UnicodeFrom(TextEncoding.Arabic1256, " ِگ¤ْ— ë¯‘÷ُ"));
        }
    }
}
