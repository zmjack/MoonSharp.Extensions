using MoonSharp.Extensions;
using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var lua = new Script();
            DynValue ret;

            lua.LoadSupport<DateTimeSupport>();
            lua.LoadSupport<ChineseCalendarSupport>();

            ret = lua.DoString(@"
return datetime.now() .. ' ' .. chinesecalendar.desc(datetime.now())
");

            Console.WriteLine(ret.String);
        }
    }
}
