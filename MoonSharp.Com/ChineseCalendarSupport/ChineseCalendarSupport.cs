using System;

namespace MoonSharp.Extensions
{
    public class ChineseCalendarSupport : LuaSupport
    {
        protected static DateTime Instance(string value) => DateTime.Parse(value);
        
        [LuaFunction("en-US", "Get the specfied date's chinese lunisoloar description. f(date)")]
        [LuaFunction("zh-CN", "获取指定日期的中国农历描述。f(日期)")]
        public string Desc(string value)
        {
            return Instance(value).GetChineseLunisolarDescription();
        }

        [LuaFunction("en-US", "Get the specfied date's chinese lunisoloar year description. f(date)")]
        [LuaFunction("zh-CN", "获取指定日期的中国农历年。f(日期)")]
        public string Year(string value) => Instance(value).GetChineseLunisolarYear();

        [LuaFunction("en-US", "Get the specfied date's chinese lunisoloar month description. f(date)")]
        [LuaFunction("zh-CN", "获取指定日期的中国农历月。f(日期)")]
        public string Month(string value) => Instance(value).GetChineseLunisolarMonth();

        [LuaFunction("en-US", "Get the specfied date's chinese lunisoloar day description. f(date)")]
        [LuaFunction("zh-CN", "获取指定日期的中国农历日。f(日期)")]
        public string Day(string value) => Instance(value).GetChineseLunisolarDay();

        [LuaFunction("en-US", "Get the specfied date's chinese zodiac year description. f(date)")]
        [LuaFunction("zh-CN", "获取指定日期的中国农历生肖。f(日期)")]
        public string ZodiacYear(string value) => Instance(value).GetChineseZodiacYear();

    }
}
