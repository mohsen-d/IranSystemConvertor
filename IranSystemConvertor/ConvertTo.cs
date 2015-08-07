// -----------------------------------------------------------------------
// <copyright file="ConvertTo.cs">
// By: MOHSEN DORPARASTI - 1391
// </copyright>
// -----------------------------------------------------------------------

namespace IranSystemConvertor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public enum TextEncoding
    {
        Arabic1256 = 1256,
        CP1252 = 1252
    }

    public static class ConvertTo
    {
        #region private Members (3)

        // متغیری برای نگهداری اعدادی که در رشته ایران سیستم وجود دارند
        static Stack<string> NumbersInTheString;

        // کد کاراکترها در ایران سیستم و معادل آنها در عربی 1256
        static Dictionary<byte, byte> CharactersMapper = new Dictionary<byte, byte>
        {
        {128,48}, // 0
        {129,49}, // 1
        {130,50}, // 2
        {131,51}, // 3
        {132,52}, // 4
        {133,53}, // 5
        {134,54}, // 6
        {135,55}, // 7
        {136,56}, // 8
        {137,57}, // 9
        {138,161}, // ،
        {139,220}, // -
        {140,191}, // ؟
        {141,194}, // آ
        {142,196}, // ﺋ
        {143,154}, // ء
        {144,199}, // ﺍ
        {145,199}, // ﺎ
        {146,200}, // ﺏ
        {147,200}, // ﺑ
        {148,129}, // ﭖ
        {149,129}, // ﭘ
        {150,202}, // ﺕ
        {151,202}, // ﺗ
        {152,203}, // ﺙ
        {153,203}, // ﺛ
        {154,204}, //ﺝ
        {155,204},// ﺟ
        {156,141},//ﭼ
        {157,141},//ﭼ
        {158,205},//ﺡ
        {159,205},//ﺣ
        {160,206},//ﺥ
        {161,206},//ﺧ
        {162,207},//د
        {163,208},//ذ
        {164,209},//ر
        {165,210},//ز
        {166,142},//ژ
        {167,211},//ﺱ
        {168,211},//ﺳ
        {169,212},//ﺵ
        {170,212},//ﺷ
        {171,213},//ﺹ
        {172,213},//ﺻ
        {173,214},//ﺽ
        {174,214},//ﺿ
        {175,216},//ط
        {224,217},//ظ
        {225,218},//ﻉ
        {226,218},//ﻊ
        {227,218},//ﻌ
        {228,218},//ﻋ
        {229,219},//ﻍ
        {230,219},//ﻎ
        {231,219},//ﻐ
        {232,219},//ﻏ
        {233,221},//ﻑ
        {234,221},//ﻓ
        {235,222},//ﻕ
        {236,222},//ﻗ
        {237,152},//ﮎ
        {238,152},//ﮐ
        {239,144},//ﮒ
        {240,144},//ﮔ
        {241,225},//ﻝ
        {242,225},//ﻻ
        {243,225},//ﻟ
        {244,227},//ﻡ
        {245,227},//ﻣ
        {246,228},//ﻥ
        {247,228},//ﻧ
        {248,230},//و
        {249,229},//ﻩ
        {250,229},//ﻬ
        {251,170},//ﻫ
        {252,236},//ﯽ
        {253,237},//ﯼ
        {254,237},//ﯾ
        {255,160} // فاصله
        };

        /// <summary>
        /// لیست کاراکترهایی که بعد از آنها باید یک فاصله اضافه شود
        /// </summary>
        static byte[] charactersWithSpaceAfter = { 
                                             146, // ب
                                             148, // پ
                                             150, // ت
                                             152, // ث
                                             154, // ج
                                             156, // چ
                                             158, // ح
                                             160, // خ
                                             167, // س
                                             169, // ش
                                             171, // ص
                                             173, // ض
                                             225, // ع
                                             229, // غ
                                             233, // ف
                                             235, // ق
                                             237, // ک
                                             239, // گ
                                             241, // ل
                                             244, // م
                                             246, // ن
                                             249, // ه
                                             252, //ﯽ
                                             253 // ی
                                         };


        #endregion

        /// <summary>
        /// تبدیل یک رشته ایران سیستم به یونیکد با استفاده از عربی 1256
        /// </summary>
        /// <param name="iranSystemEncodedString">رشته ایران سیستم</param>
        /// <returns></returns>
        [Obsolete("بهتر است از UnicodeFrom استفاده کنید")]
        public static string Unicode(string iranSystemEncodedString)
        {
            return UnicodeFrom(TextEncoding.Arabic1256, iranSystemEncodedString);
        }

        /// <summary>
        /// تبدیل یک رشته ایران سیستم به یونیکد
        /// </summary>
        /// <param name="textEncoding">کدپیج رشته ایران سیستم</param>
        /// <param name="iranSystemEncodedString">رشته ایران سیستم</param>
        /// <returns></returns>
        public static string UnicodeFrom(TextEncoding textEncoding, string iranSystemEncodedString)
        {
            // حذف فاصله های موجود در رشته
            iranSystemEncodedString = iranSystemEncodedString.Replace(" ", "");

            /// بازگشت در صورت خالی بودن رشته
            if (string.IsNullOrWhiteSpace(iranSystemEncodedString))
            {
                return string.Empty;
            }

            // در صورتی که رشته تماماً عدد نباشد
            if (!IsNumber(iranSystemEncodedString))
            {
                /// تغییر ترتیب کاراکترها از آخر به اول 
                iranSystemEncodedString = Reverse(iranSystemEncodedString);

                /// خارج کردن اعداد درون رشته
                iranSystemEncodedString = ExcludeNumbers(iranSystemEncodedString);
            }

            // وهله سازی از انکودینگ صحیح برای تبدیل رشته ایران سیستم به بایت
            Encoding encoding = Encoding.GetEncoding((int)textEncoding);            

            // تبدیل رشته به بایت
            byte[] stringBytes = encoding.GetBytes(iranSystemEncodedString.Trim());


            // آرایه ای که بایت های معادل را در آن قرار می دهیم
            // مجموع تعداد بایت های رشته + بایت های اضافی محاسبه شده 
            byte[] newStringBytes = new byte[stringBytes.Length + CountCharactersRequireTwoBytes(stringBytes)];

            int index = 0;

            // بررسی هر بایت و پیدا کردن بایت (های) معادل آن
            for (int i = 0; i < stringBytes.Length; ++i)
            {
                byte charByte = stringBytes[i];

                // اگر جز 128 بایت اول باشد که نیازی به تبدیل ندارد چون کد اسکی است
                if (charByte < 128)
                {
                    newStringBytes[index] = charByte;
                }
                else
                {
                    // اگر جز حروف یا اعداد بود معادلش رو قرار می دیم
                    if (CharactersMapper.ContainsKey(charByte))
                    {
                        newStringBytes[index] = CharactersMapper[charByte];
                    }
                }

                // اگر کاراکتر ایران سیستم "لا" بود چون کاراکتر متناظرش در عربی 1256 "ل" است و باید یک "ا" هم بعدش اضافه کنیم
                if (charByte == 242)
                {
                    newStringBytes[++index] = 199;
                }

                // اگر کاراکتر یکی از انواعی بود که بعدشان باید یک فاصله باشد
                // و در عین حال آخرین کاراکتر رشته نبود
                if (charactersWithSpaceAfter.Contains(charByte) && Array.IndexOf(stringBytes, charByte) != stringBytes.Length - 1)
                {
                    // یک فاصله بعد ان اضافه می کنیم
                    newStringBytes[++index] = 32;
                }

                index += 1;
            }

            // تبدیل به رشته و ارسال به فراخواننده
            byte[] unicodeContent = Encoding.Convert(encoding, Encoding.Unicode, newStringBytes);

            string convertedString = Encoding.Unicode.GetString(unicodeContent).Trim();

            return IncludeNumbers(convertedString);
        }

        #region Private Methods (4)

        /// <summary>
        /// رشته ارسال شده تنها حاوی اعداد است یا نه
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static bool IsNumber(string str)
        {
            return Regex.IsMatch(str, @"^[\d]+$");
        }

        /// <summary>
        ///  محاسبه تعداد کاراکترهایی که بعد از آنها یک کاراکتر باید اضافه شود
        ///  شامل کاراکتر لا
        ///  و کاراکترهای غیرچسبان تنها در صورتی که کاراکتر پایانی رشته نباشند
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        static int CountCharactersRequireTwoBytes(byte[] irTextBytes)
        {
            return (from b in irTextBytes
                    where (
                    charactersWithSpaceAfter.Contains(b) // یکی از حروف غیرچسبان باشد
                    && Array.IndexOf(irTextBytes, b) != irTextBytes.Length - 1) // و کاراکتر آخر هم نباشد
                    || b == 242 // یا کاراکتر لا باشد
                    select b).Count();
        }

        /// <summary>
        /// خارج کردن اعدادی که در رشته ایران سیستم قرار دارند
        /// </summary>
        /// <param name="iranSystemString"></param>
        /// <returns></returns>
        static string ExcludeNumbers(string iranSystemString)
        {
            /// گرفتن لیستی از اعداد درون رشته
            NumbersInTheString = new Stack<string>(Regex.Split(iranSystemString, @"\D+"));

            /// جایگزین کردن اعداد با یک علامت جایگزین
            /// در نهایت بعد از تبدیل رشته اعداد به رشته اضافه می شوند
            return Regex.Replace(iranSystemString, @"\d+", "#");
        }

        /// <summary>
        /// اضافه کردن اعداد جدا شده پس از تبدیل رشته
        /// </summary>
        /// <param name="convertedString"></param>
        /// <returns></returns>
        static string IncludeNumbers(string convertedString)
        {
            while (convertedString.IndexOf("#") >= 0)
            {
                string number = Reverse(NumbersInTheString.Pop());
                if(!string.IsNullOrWhiteSpace(number))
                {
                    int index = convertedString.IndexOf("#");

                    convertedString = convertedString.Remove(index, 1);
                    convertedString = convertedString.Insert(index, number);
                }                
            }

            return convertedString;
        }

        static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        #endregion

    }
}
