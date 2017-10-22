using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MoonSharp.Extensions
{
    public static class DateTimeEx
    {
        private static ChineseLunisolarCalendar _Calendar = new ChineseLunisolarCalendar();

        private static string[] _HeavenlyStems
            = { "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" };
        private static string[] _TerrestrialBranches
            = { "子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉", "戌", "亥" };
        private static string[] _ChineseZodiac
            = { "鼠", "牛", "虎", "免", "龙", "蛇", "马", "羊", "猴", "鸡", "狗", "猪" };
        private static string[] _Months
            = { "正", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "腊" };
        private static string[] _Days =
        {
            "初一", "初二", "初三", "初四", "初五", "初六", "初七", "初八", "初九", "初十",
            "十一", "十二", "十三", "十四", "十五", "十六", "十七", "十八", "十九", "二十",
            "廿一", "廿二", "廿三", "廿四", "廿五", "廿六", "廿七", "廿八", "廿九", "三十",
        };

        /// <summary>
        /// 获取农历描述
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string GetChineseLunisolarDescription(this DateTime datetime)
        {
            return $"{GetChineseLunisolarYear(datetime)}【{GetChineseZodiacYear(datetime)}】{GetChineseLunisolarMonth(datetime)}{GetChineseLunisolarDay(datetime)}";
        }

        /// <summary>
        /// 获取农历年描述
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string GetChineseLunisolarYear(this DateTime datetime)
        {
            int sexagenaryYear = _Calendar.GetSexagenaryYear(datetime);
            var terrestrialBranch = _Calendar.GetTerrestrialBranch(sexagenaryYear);

            return $"{_HeavenlyStems[(sexagenaryYear - 1) % 10]}" +
                $"{_TerrestrialBranches[terrestrialBranch - 1]}年";
        }

        /// <summary>
        /// 获取农历月描述
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string GetChineseLunisolarMonth(this DateTime datetime)
        {
            int year = _Calendar.GetYear(datetime);
            int month = _Calendar.GetMonth(datetime);
            int leapMonth = _Calendar.GetLeapMonth(year);

            if (month <= leapMonth)
            {
                return $"{(month == leapMonth ? "闰" : "")}{_Months[month - 1]}月";
            }
            else return $"{_Months[month - 2]}月";
        }

        /// <summary>
        /// 获取农历日描述
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string GetChineseLunisolarDay(this DateTime datetime)
        {
            int day = _Calendar.GetDayOfMonth(datetime);

            return _Days[day - 1];
        }

        /// <summary>
        /// 获取生肖年
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string GetChineseZodiacYear(this DateTime datetime)
        {
            return $"{_ChineseZodiac[_Calendar.GetTerrestrialBranch(_Calendar.GetSexagenaryYear(datetime)) - 1]}年";
        }

    }
}
