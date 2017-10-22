using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoonSharp.Extensions
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class LuaFunctionAttribute : Attribute
    {
        /// <summary>
        /// Specify display name of culture
        /// </summary>
        public string Culture { get; set; }

        /// <summary>
        /// Function usage
        /// </summary>
        public string Usage { get; set; }

        public LuaFunctionAttribute() { }
        public LuaFunctionAttribute(string culture, string usage)
        {
            Culture = culture;
            Usage = usage;
        }
    }

}
