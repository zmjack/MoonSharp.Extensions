using System;

namespace MoonSharp.Extensions
{
    public class DateTimeSupport : LuaSupport
    {
        protected static DateTime Instance(string value) => DateTime.Parse(value);
        
        [LuaFunction("en-US", "Get current datetime. f()")]
        [LuaFunction("zh-CN", "获取当前日期时间。f()")]
        public static string Now() => DateTime.Now.ToString();

        [LuaFunction("en-US", "Create a date. f(year,month,day)")]
        [LuaFunction("zh-CN", "构造日期。f(年,月,日)")]
        public static string NewDate(int year, int month, int day)
        {
            return new DateTime(year, month, day).ToString();
        }

        [LuaFunction("en-US", "Create a datetime. (year,month,day,hour,minute,second)")]
        [LuaFunction("zh-CN", "构造日期时间：f(年,月,日,时,分,秒)")]
        public static string NewDateTime(int year, int month, int day, int hour, int minute, int second)
        {
            return new DateTime(year, month, day, hour, minute, second).ToString();
        }

        [LuaFunction("en-US", "Get the year of the specified date. f(date)")]
        [LuaFunction("zh-CN", "获取指定日期的年。f(日期)")]
        public static int Year(string value) => Instance(value).Year;

        [LuaFunction("en-US", "Get the month of the specified date. f(date)")]
        [LuaFunction("zh-CN", "获取指定日期的月。f(日期)")]
        public static int Month(string value) => Instance(value).Month;

        [LuaFunction("en-US", "Get the day of the specified date. f(date)")]
        [LuaFunction("zh-CN", "获取指定日期的日。f(日期)")]
        public static int Day(string value) => Instance(value).Day;

        [LuaFunction("en-US", "Get the hour of the specified time. f(datetime)")]
        [LuaFunction("zh-CN", "获取指定时间的时。f(时间)")]
        public static int Hour(string value) => Instance(value).Hour;

        [LuaFunction("en-US", "Get the minute of the specified time. f(time)")]
        [LuaFunction("zh-CN", "获取指定时间的分。f(时间)")]
        public static int Minute(string value) => Instance(value).Minute;

        [LuaFunction("en-US", "Get the second of the specified time. f(time)")]
        [LuaFunction("zh-CN", "获取指定时间的秒。f(时间)")]
        public static int Second(string value) => Instance(value).Second;

        [LuaFunction("en-US", "Get the week number in month of the specified time. f(date,weekstart[S,M,T,W,T,F,S])")]
        [LuaFunction("zh-CN", "获取指定日期的在当月的周数。f(日期, 周起始星期[S,M,T,W,T,F,S])")]
        public static int WeekInMonth(string value, int weekStart)
        {
            return Instance(value).WeekInMonth((DayOfWeek)weekStart);
        }

        [LuaFunction("en-US", "Get the week number in year of the specified time. f(date,weekstart[S,M,T,W,T,F,S])")]
        [LuaFunction("zh-CN", "获取指定日期的在当年的周数。f(日期, 周起始星期[S,M,T,W,T,F,S])")]
        public static int Week(string value, int weekStart)
        {
            return Instance(value).Week((DayOfWeek)weekStart);
        }

        [LuaFunction("en-US", "Get a new datetime that adds the specified number of years to the specified date. f(date,years)")]
        [LuaFunction("zh-CN", "获取指定日期几年后的日期。f(日期, 年数)")]
        public static string AddYears(string value, int years)
        {
            return Instance(value).AddYears(years).ToString();
        }

        [LuaFunction("en-US", "Get a new datetime that adds the specified number of months to the specified date. f(date, months)")]
        [LuaFunction("zh-CN", "获取指定日期几月后的日期。f(日期, 月数)")]
        public static string AddMonths(string value, int months)
        {
            return Instance(value).AddMonths(months).ToString();
        }

        [LuaFunction("en-US", "Get a new datetime that adds the specified number of days to the specified date. f(date,days)")]
        [LuaFunction("zh-CN", "获取指定日期几天后的日期。f(日期, 天数)")]
        public static string AddDays(string value, int days)
        {
            return Instance(value).AddDays(days).ToString();
        }

        [LuaFunction("en-US", "Get the date part of the specified datetime. f(datetime)")]
        [LuaFunction("zh-CN", "获取指定日期时间的日期部分。f(日期时间)")]
        public static string Date(string value) => Instance(value).ToString("yyyy-M-d");

        [LuaFunction("en-US", "Get the time part of the specifeid datetime. f(datetime)")]
        [LuaFunction("zh-CN", "获取指定日期时间的时间部分。f(日期时间)")]
        public static string Time(string value) => Instance(value).ToString("HH:mm:ss");

        [LuaFunction("en-US", "Get the day of week of the specified datetime. f(datetime)")]
        [LuaFunction("zh-CN", "获取指定日期的星期。f(日期)")]
        public static int DayOfWeek(string value) => (int)Instance(value).DayOfWeek;

        [LuaFunction("en-US", "Get a new datetime that which is the past day and the day of week is specified. f(date,dayOfWeek,excludeTheDate)")]
        [LuaFunction("zh-CN", "获取离指定日期最近的已过某星期的日期。f(日期, 星期, 是否包含当天)")]
        public static string PastDay(string value, int dayOfWeek, bool excludeTheValue)
        {
            return Instance(value).PastDay((DayOfWeek)dayOfWeek, excludeTheValue).ToString();
        }

        [LuaFunction("en-US", "Get a new datetime that which is the future day and the day of week is specified. f(date,dayOfWeek,excludeTheDate)")]
        [LuaFunction("zh-CN", "获取离指定日期最近的未来某星期的日期。f(日期, 星期, 是否包含当天)")]
        public static string FutureDay(string value, int dayOfWeek, bool excludeTheValue)
        {
            return Instance(value).FutureDay((DayOfWeek)dayOfWeek, excludeTheValue).ToString();
        }

        [LuaFunction("en-US", "Get the first day of the month of the specified date. f(date)")]
        [LuaFunction("zh-CN", "获取指定日期所属月第一天的日期。f(日期)")]
        public static string FirstDayInMonth(string value)
        {
            return Instance(value).FirstDayInMonth().ToString();
        }

        [LuaFunction("en-US", "Get the last day of the month of the specified date. f(date)")]
        [LuaFunction("zh-CN", "获取指定日期所属月最后一天的日期。f(日期)")]
        public static string LastDayInMonth(string value)
        {
            return Instance(value).LastDayInMonth().ToString();
        }

    }
}
