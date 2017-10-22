using System;

namespace MoonSharp.Extensions
{
    public static class DateTimeEx
    {
        /// <summary>
        /// 转换间隔天数按周数的往前或往后计算的天数值。
        /// </summary>
        /// <param name="days">间隔天数</param>
        /// <param name="isBackward">是否往前计算（true，返回负数；false，返回正数）</param>
        /// <returns></returns>
        public static int CastCycleDays(int days, bool isBackward)
        {
            days = days % 7;

            if (isBackward)
                return days > 0 ? days - 7 : days;
            else return days < 0 ? days + 7 : days;
        }

        /// <summary>
        /// 获取最近过去的指定星期的日期实例。
        /// </summary>
        /// <param name="this"></param>
        /// <param name="week">指定星期</param>
        /// <returns></returns>
        public static DateTime PastDay(this DateTime @this, DayOfWeek week, bool excludeToday)
        {
            var days = week - @this.DayOfWeek;

            if (excludeToday && days == 0)
                return @this.AddDays(-7);

            return @this.AddDays(CastCycleDays(days, true));
        }

        /// <summary>
        /// 获取未来的指定星期的日期实例。
        /// </summary>
        /// <param name="this"></param>
        /// <param name="week">指定星期</param>
        /// <returns></returns>
        public static DateTime FutureDay(this DateTime @this, DayOfWeek week, bool excludeToday)
        {
            var days = week - @this.DayOfWeek;

            if (excludeToday && days == 0)
                return @this.AddDays(7);

            return @this.AddDays(CastCycleDays(days, false));
        }

        /// <summary>
        /// 通过给定周的第一天获取当周数。
        /// </summary>
        /// <param name="this"></param>
        /// <param name="weekStart">周始于星期几</param>
        /// <returns></returns>
        public static int WeekInMonth(this DateTime @this, DayOfWeek weekStart)
        {
            var day1 = new DateTime(@this.Year, @this.Month, 1);
            var day1Week_day1 = PastDay(day1, weekStart, false);

            if (day1Week_day1.Month == @this.Month)
                return (PastDay(@this, weekStart, false) - day1Week_day1).Days / 7 + 1;
            else return (PastDay(@this, weekStart, false) - day1Week_day1).Days / 7;
        }

        /// <summary>
        /// 通过给定周的第一天获取当周数。
        /// </summary>
        /// <param name="this"></param>
        /// <param name="weekStart">周始于星期几</param>
        /// <returns></returns>
        public static int Week(this DateTime @this, DayOfWeek weekStart)
        {
            var day1 = new DateTime(@this.Year, 1, 1);
            var day1Week_day1 = PastDay(day1, weekStart, false);

            if (day1Week_day1.Year == @this.Year)
                return (PastDay(@this, weekStart, false) - day1Week_day1).Days / 7 + 1;
            else return (PastDay(@this, weekStart, false) - day1Week_day1).Days / 7;
        }

        public static DateTime FirstDayInMonth(this DateTime @this)
        {
            return @this.AddDays(1 - @this.Day);
        }

        public static DateTime LastDayInMonth(this DateTime @this)
        {
            return FirstDayInMonth(@this).AddMonths(1).AddDays(-1);
        }

    }
}
