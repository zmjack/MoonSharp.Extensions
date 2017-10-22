using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MoonSharp.Extensions
{
    public abstract class LuaSupport
    {
        private class FunctionDefinitionWrapper
        {
            public string DeclareName { get; set; }
            public string MethodInfo { get; set; }
            public string Usage { get; set; }
        }
        public LuaSupport() { }

        private IEnumerable<(LuaFunctionAttribute Attribute, MethodInfo ReflectObject)> EnumMethodDefinitions(string culture)
        {
            return GetType()
                .GetMethods()
                .Where(method => method.GetCustomAttributes<LuaFunctionAttribute>(false).Any())
                .Select(method =>
                {
                    var attrs = method.GetCustomAttributes<LuaFunctionAttribute>(false);
                    var currentAttr = attrs.SingleOrDefault(attr => attr.Culture == culture);
                    if (currentAttr == null)
                    {
                        currentAttr = attrs.First();
                    }

                    return (currentAttr, method);
                });
        }

        public void Dump((LuaFunctionAttribute Attribute, MethodInfo ReflectObject) x)
        {
            var name = x.ReflectObject.Name;
            var usage = x.Attribute.Usage;
            var methodInfo = x.ReflectObject.ToString();

            Console.WriteLine($"+ {name}");
            Console.WriteLine($"  - {methodInfo}");
            if (!string.IsNullOrEmpty(usage))
            {
                Console.WriteLine($"  {usage}");
            }
        }

        public void DumpSupport(string culture)
        {
            var definitions = EnumMethodDefinitions(culture);

            foreach (var definition in definitions)
            {
                Dump(definition);
            }
        }

        public void DumpMethod(string methodName, string culture)
        {
            var method = GetType().GetMethod(methodName);

            var attrs = method.GetCustomAttributes<LuaFunctionAttribute>(false);
            var currentAttr = attrs.SingleOrDefault(attr => attr.Culture == culture);
            if (currentAttr == null)
            {
                currentAttr = attrs.First();
            }

            Dump((currentAttr, method));
        }

    }
}
