using MoonSharp.Interpreter;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace MoonSharp.Extensions
{
    public static class ScriptEx
    {
        public static DynValue LCall(this Script @this, string function, params object[] args)
        {
            return @this.Call(@this.Globals[function], args);
        }

        public static DynValue LVar(this Script @this, string var)
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
            Console.Write(new TLuaSupport().DumpSupport(culture));
        }

        public static void DumpMethod<TLuaSupport>(this Script @this, string methodName, string culture = null)
            where TLuaSupport : LuaSupport, new()
        {
            Console.Write(new TLuaSupport().DumpMethod(methodName, culture));
        }

    }
}
