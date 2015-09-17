using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace IranSystemConvertor
{

    /// <summary>
    /// نحوه‌ي تبديل اعداد
    /// </summary>
    public enum IranSystemNumbers
    {
        /// <summary>
        /// اعداد موجود به فرمت ايران سيستم تبديل نشوند و به همان شكل اصلي باقي بمانند
        /// </summary>
        DontConvert,

        /// <summary>
        /// اعداد موجود هم به قالب ايران سيستم تبديل شوند
        /// </summary>
        Convert
    }

    public static class ConvertToIranSystem
    {
        const byte AlefChasban = 145;
        const byte LaaArabic = 242;
        const byte LaamAvval = 243;
        const byte QuestionMark = 191;

        private static readonly IDictionary<byte, byte> _charMapperCharIsNotFinal =
            new Dictionary<byte, byte>
        {
            {48 , 128}, // 0
            {49 , 129}, // 1
            {50 , 130}, // 2
            {51 , 131}, // 3
            {52 , 132}, // 4
            {53 , 133}, // 5
            {54 , 134}, // 6
            {55 , 135}, // 7
            {56 , 136}, // 8
            {57 , 137}, // 9
            {161, 138}, // ،
            {191, 140}, // ؟
            {193,143}, //
            {194,141}, // آ
            {195,145}, // ﺎ
            {196, /*248*/ 142}, //
            {197,145}, //
            {198,254}, //
            {199,145}, //
            {200,147}, //
            {201,250}, //
            {202,151}, //
            {203,153}, //
            {204,155}, //
            {205,159}, //
            {206,161}, //
            {207,162}, //
            {208,163}, //
            {209,164}, //
            {210,165}, //ز
            {211,168}, //
            {212,170}, //
            {213,172}, //
            {214,174}, //
            {216,175}, //
            {217,224}, //
            {218,227}, //
            {219,231}, //
            {220,139}, // -
            {221,234}, //
            {222,236}, //
            {223,238}, //
            {225,243}, //
            {227,245}, //
            {228,247}, //
            {229,250}, //
            {230,248}, //
            {236,254}, //
            {237,254}, //
            {129,149}, //
            {141,157}, //
            {142,166}, //
            {152,238}, //
            {144,240} //
        };

        private static readonly IDictionary<byte, byte> _charMapperNextCharFinal =
            new Dictionary<byte, byte>
        {
            {198, 252}, //
            {200, 146}, //
            {201, 249}, //
            {202, 150}, //
            {203, 152}, //
            {204, 154}, //
            {205, 158}, //
            {206, 160}, //
            {211, 167}, //
            {212, 169}, //
            {213, 171}, //
            {214, 173}, //
            {218, 226}, //
            {219, 230}, //
            {221, 233}, //
            {222, 235}, //
            {223, 237}, //
            {225, 241}, //
            {227, 244 /*245*/}, //
            {228, 246}, //
            {229, 249}, //
            {236, 252}, //
            {237, 252}, //
            {129, 148}, //
            {141, 156}, //
            {142, 166}, //
            {152, 237}, //
            {144, 239} //
       };

        private static readonly IDictionary<byte, byte> _charMapperPreviousCharFinal =
            new Dictionary<byte, byte>
        {
           {195, 144},//أ
           {197, 144},//إ
           {199, 144},//ا
           {201, 251},//ة
           {218, 228},//ع
           {219, 232},//غ
           {229, 251},//ه
           {170, 251}//ﻫ
        };

        private static readonly IDictionary<byte, byte> _charMapperPreviousCharNextCharFinal =
            new Dictionary<byte, byte>
        {
            {195, 144}, // أ
            {197, 144}, //إ
            {199, 144},//ا
            {200, 146}, //ب
            {201, 249}, //ة
            {202, 150}, //ت
            {203, 152}, //ث
            {204, 154}, //ﺝ
            {205, 158}, //ﺡ
            {206, 160}, //ﺥ
            {211, 167},//س
            {212, 169},//ش
            {213, 171}, //ص
            {214, 173}, //ض
            {218, 225}, //ع
            {219, 229}, //غ
            {221, 233},//ف
            {222, 235},//ق
            {223, 237},//ك
            {225, 241},//ل
            {227, 244},//م
            {228, 246},//ن
            {229, 249},//ه
            {236, /*253*/ 252},//ى
            {237, 253},//ی
            {129, 148},//پ
            {141, 156},//چ
            {152, 237},//ک
            {144, 239}//گ
           };

        private static readonly Encoding _encoding1256 = Encoding.GetEncoding("windows-1256");

        private static readonly byte[] _singles = _encoding1256.GetBytes("ءآأؤإادذرزژو");

        /// <summary>
        /// تبديل رشته‌ي يونيكد به رشته‌ي ايران سيستمي
        /// </summary>
        /// <param name="text">رشته‌ي يونيكد</param>
        /// <param name="iranSystemNumbers">تبديل اعداد</param>
        /// <returns>رشته‌ي ايران سيستمي</returns>
        public static string ToIranSystem(
            this string text,
            IranSystemNumbers iranSystemNumbers = IranSystemNumbers.DontConvert)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            text = reverseNumbersAndLetters(text);
            var iranSystemBytes = getUnicodeToIranSystem(text, iranSystemNumbers);
            var iranSystemText = _encoding1256.GetString(iranSystemBytes.ToArray()).Trim();
            return new string(iranSystemText.Reverse().ToArray());

        }

        private static byte getLattinLetter(byte c)
        {
            return char.IsNumber((char)c) ? (byte)(c + 80) : getMirroredCharacter(c);
        }

        private static byte getMapperCharIsNotFinal(byte currentChar)
        {
            byte value;
            return _charMapperCharIsNotFinal.TryGetValue(currentChar, out value) ? value : currentChar;
        }

        private static byte getMapperNextCharFinal(byte currentChar)
        {
            byte value;
            return _charMapperNextCharFinal.TryGetValue(currentChar, out value) ? value : getMapperCharIsNotFinal(currentChar);
        }

        private static byte getMapperPreviousCharFinal(byte currentChar)
        {
            byte value;
            return _charMapperPreviousCharFinal.TryGetValue(currentChar, out value) ? value : getMapperCharIsNotFinal(currentChar);
        }

        private static byte getMapperPreviousCharNextCharFinal(byte currentChar)
        {
            byte value;
            return _charMapperPreviousCharNextCharFinal.TryGetValue(currentChar, out value) ? value : getMapperCharIsNotFinal(currentChar);
        }

        private static byte getMirroredCharacter(byte c)
        {
            switch (c)
            {
                case (byte)'(': return (byte)')';
                case (byte)'{': return (byte)'}';
                case (byte)'[': return (byte)']';
                case (byte)')': return (byte)'(';
                case (byte)'}': return (byte)'{';
                case (byte)']': return (byte)'[';
                default: return c;
            }
        }

        private static List<byte> getUnicodeToIranSystem(string text, IranSystemNumbers iranSystemNumbers)
        {
            var unicodeString = string.Format(" {0} ", text);
            var textBytes = _encoding1256.GetBytes(unicodeString);

            byte pre = 0;
            var length = textBytes.Length;
            var results = new List<byte>(length);

            for (var i = 0; i < length; i++)
            {
                byte cur;
                var currentChar = textBytes[i];

                if (isNumber(currentChar) && iranSystemNumbers == IranSystemNumbers.DontConvert)
                {
                    cur = currentChar;
                    results.Add(cur);
                    pre = cur;
                }
                else if (isLattinLetter(currentChar))
                {
                    cur = getLattinLetter(currentChar);
                    results.Add(cur);
                    pre = cur;
                }
                else if (i != 0 && i != length - 1)
                {
                    cur = getUnicodeToIranSystemChar(textBytes[i - 1], currentChar, textBytes[i + 1]);
                    if (cur == AlefChasban) // برای بررسی استثنای لا
                    {
                        if (pre == LaamAvval)
                        {
                            results.RemoveAt(results.Count - 1);
                            results.Add(LaaArabic);
                        }
                        else
                        {
                            results.Add(cur);
                        }
                    }
                    else
                    {
                        results.Add(cur);
                    }

                    pre = cur;
                }
            }

            return results;
        }

        private static byte getUnicodeToIranSystemChar(byte previousChar, byte currentChar, byte nextChar)
        {
            var isPreviousCharFinal =
                isWhitespaceOrLattinOrQuestionMark(previousChar) ||
                isFinalLetter(previousChar);

            var isNextCharFinal = isWhitespaceOrLattinOrQuestionMark(nextChar);

            if (isPreviousCharFinal && isNextCharFinal)
            {
                return getMapperPreviousCharNextCharFinal(currentChar);
            }

            if (isPreviousCharFinal)
            {
                return getMapperPreviousCharFinal(currentChar);
            }

            if (isNextCharFinal)
            {
                return getMapperNextCharFinal(currentChar);
            }

            return getMapperCharIsNotFinal(currentChar);
        }

        private static bool isFinalLetter(byte c)
        {
            return _singles.Contains(c);
        }

        private static bool isLattinLetter(byte c)
        {
            return c < 128 && c > 31;
        }

        private static bool isNumber(byte currentChar)
        {
            return currentChar >= 48 && currentChar <= 57;
        }

        private static bool isWhiteSpaceLetter(byte c)
        {
            return c == 8 || c == 09 || c == 10 || c == 13 || c == 27 || c == 32 || c == 0;
        }

        private static bool isWhitespaceOrLattinOrQuestionMark(byte c)
        {
            return isWhiteSpaceLetter(c) || isLattinLetter(c) || c == QuestionMark;
        }

        private static string reverseNumbersAndLetters(string text)
        {
            return Regex.Replace(text, @"[a-zA-Z0-9]+", match =>
            {
                return new string(match.Value.Reverse().ToArray());
            }).Trim();
        }
    }
}