using IranSystemConvertor;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class ToIranSystemTests
    {
        [Test]
        public void IranSystem_Normal_Test_1()
        {
            var actual = "سرخرود".ToIranSystem();
            var expected = "¢ّ¤،¤¨";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IranSystem_Normal_Test_2()
        {
            var actual = "الحسنه".ToIranSystem();
            var expected = "ù÷¨ںَگ";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IranSystem_Normal_Test_3()
        {
            var actual = "قرض".ToIranSystem();
            var expected = "­¤ى";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IranSystem_Normal_Test_4()
        {
            var actual = "صندوق".ToIranSystem();
            var expected = "ëّ¢÷¬";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IranSystem_Normal_Test_5()
        {
            var actual = "على".ToIranSystem();
            var expected = "üَن";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IranSystem_Normal_Test_7()
        {
            var actual = "ميرزاعلى".ToIranSystem();
            var expected = "üَنگ¥¤‏ُ";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IranSystem_Normal_Test_999()
        {
            var actual = "-نصيري".ToIranSystem();
            var expected = "‎¤‏¬÷-";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IranSystem_Normal_Test()
        {
            var actual = "محمد".ToIranSystem();
            var expected = "¢ُںُ";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IranSystem_Number_Test()
        {
            var actual = "123".ToIranSystem();
            var expected = "123";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IranSystem_Long_Number_Test()
        {
            var actual = "3000000000000000000".ToIranSystem();
            var expected = "3000000000000000000";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IranSystem_Long_Number_Test2()
        {
            var actual = "تست يك 123 تست دو 345".ToIranSystem();
            var expected = "345 ّ¢ –¨— 123 ي‏ –¨—";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IranSystem_TwoBytesCharacters_One_Accurance_Test()
        {
            var actual = "کلاف".ToIranSystem();
            var expected = "éٍî";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IranSystem_TwoBytesCharacters_More_Than_One_Accurance_Test()
        {
            var actual = "کلاف کلاف".ToIranSystem();
            var expected = "éٍî éٍî";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IranSystem_Mixed_String_And_Number()
        {
            var actual = "مناطق 123تهران".ToIranSystem();
            var expected = "ِگ¤ْ—123 ë¯‘÷ُ";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IranSystem_Mixed_String_And_English()
        {
            var actual = "مناطق ABCتهران".ToIranSystem();
            var expected = "ِگ¤ْ—ABC ë¯‘÷ُ";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IranSystem_Mixed_English_And_Numbers()
        {
            var actual = "Ab123CD456".ToIranSystem();
            var expected = "Ab123CD456";
            Assert.AreEqual(expected, actual);
        }
    }
}