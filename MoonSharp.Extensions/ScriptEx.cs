using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MoonSharp.Extensions
{
    public static class ScriptEx
    {
        public static DynValue CallFunction(this Script @this, string function, params object[] args)
        {
            return @this.Call(@this.Globals[function], args);
        }

        public static DynValue Var(this Script @this, string var)
        {
            return @this.Globals.Pairs.FirstOrDefault(a => a.Key.String == var).Value;
        }

        public static void LoadSupport<TLuaSupport>(this Script @this)
            where TLuaSupport : LuaSupport, new()
        {
            var supportInstance = new TLuaSupport();
            var supportName = new Regex("^(.*)Support$").Replace(supportInstance.GetType().Name, "$1");

            UserData.RegisterType<TLuaSupport>();
            @this.Globals[supportName.ToLower()] = UserData.Create(supportInstance);
        }

        public static void DumpSupport<TLuaSupport>(this Script @this, string culture = null)
            where TLuaSupport : LuaSupport, new()
        {
            var supportInstance = new TLuaSupport();
            if (culture == null)
                culture = CultureInfo.CurrentUICulture.ToString();

            Console.WriteLine(typeof(TLuaSupport).Name + " Functions:");
            Console.Write(supportInstance.DumpSupport(culture));
        }

        public static void DumpMethod<TLuaSupport>(this Script @this, string methodName, string culture = null)
            where TLuaSupport : LuaSupport, new()
        {
            var supportInstance = new TLuaSupport();
            if (culture == null)
                culture = CultureInfo.CurrentUICulture.ToString();

            Console.Write(supportInstance.DumpMethod(methodName, culture));
        }

    }
}
