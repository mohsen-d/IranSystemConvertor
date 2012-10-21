// -----------------------------------------------------------------------
// <copyright file="ConvertTo.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace IranSystemConvertor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class ConvertTo
    {
        #region private Members (2)

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
                                             242, // لا
                                             244, // م
                                             246, // ن
                                             249, // ه
                                             252, //ﯽ
                                             253 // ی
                                         };


        #endregion


        /// <summary>
        /// تبدیل یک رشته ایران سیستم به یونیکد
        /// </summary>
        /// <param name="iranSystemEncodedString">رشته ایران سیستم</param>
        /// <returns></returns>
        public static string Unicode(string iranSystemEncodedString)
        {
            // انکود عربی برای تبدیل رشته ایران سیستم به بایت
            Encoding arabic = Encoding.GetEncoding(1256);

            // تبدیل رشته به بایت
            byte[] stringBytes = arabic.GetBytes(iranSystemEncodedString.Trim());

            // تغییر ترتیب بایت هااز آخر به اول در صورتی که رشته تماماً عدد نباشد
            if (!IsNumber(iranSystemEncodedString))
            {
                stringBytes = stringBytes.Reverse().ToArray();
            }

            // آرایه ای که بایت های معادل را در آن قرار می دهیم
            // مجموع تعداد بایت های رشته + بایت های اضافی محاسبه شده 
            byte[] newStringBytes = new byte[stringBytes.Length + GetCharacterssWithSpaceAfterCount(stringBytes)];

            int j = 0;

            // بررسی هر بایت و پیدا کردن بایت (های ) معادل آن
            for (int i = 0; i < stringBytes.Length; ++i)
            {
                byte charByte = stringBytes[i];

                // اگر جز 128 بیت اول باشد که نیازی به تبدیل ندارد چون کد اسکی است
                if (charByte < 128)
                {
                    newStringBytes[j] = charByte;
                }
                else
                {
                    // اگر جز حروف یا اعداد بود معادلش رو قرار می دیم
                    if (CharactersMapper.ContainsKey(charByte))
                    {
                        newStringBytes[j] = CharactersMapper[charByte];
                    }
                    else
                    {
                        //newStringBytes[j] = charByte;
                    }
                }

                // اگر کاراکتر ایران سیستم "لا" بود چون کاراکتر متناظرش در عربی 1256 "ل" است و باید یک "ا" هم بعدش اضافه کنیم
                if (charByte == 242)
                {
                    j = j + 1;

                    newStringBytes[j] = 199;
                }

                // اگر کاراکتر یکی از انواعی بود که بعدشان باید یک فاصله باشد
                if (charactersWithSpaceAfter.Contains(charByte))
                {
                    j = j + 1;
                    // یک فاصله بعد ان اضافه می کنیم
                    newStringBytes[j] = 32;
                }

                j = j + 1;
            }

            // تبدیل آرایه معادل از عربی به یونیکد
            byte[] unicodeContent = Encoding.Convert(arabic, Encoding.Unicode, newStringBytes);

            // تبدیل به رشته و ارسال به فراخواننده
            return Encoding.Unicode.GetString(unicodeContent).Trim();

        }

        #region Private Methods (2)

        /// <summary>
        /// رشته ارسال شده تنها حاوی اعداد است یا نه
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static bool IsNumber(string str)
        {
            int i;
            return int.TryParse(str, out i);
        }

        /// <summary>
        ///  محاسبه تعداد کاراکترهایی که بعد از آنها یک کاراکتر باید اضافه شود
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        static int GetCharacterssWithSpaceAfterCount(byte[] irTextBytes)
        {
            return (from b in irTextBytes
                    where charactersWithSpaceAfter.Contains(b)
                    select b).Count();

        }

        #endregion

    }
}
