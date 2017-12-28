using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

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

        private BindingFlags FindBindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.IgnoreCase;

        private IEnumerable<(LuaFunctionAttribute Attribute, MethodInfo ReflectObject)> EnumMethodDefinitions(string culture)
        {
            return GetType()
                .GetMethods(FindBindingFlags)
                .Where(method => method.GetCustomAttributes<LuaFunctionAttribute>(false).Any())
                .Select(method => Get_Attribute_ReflectObject(method, culture));
        }

        private (LuaFunctionAttribute Attribute, MethodInfo ReflectObject) Get_Attribute_ReflectObject(MethodInfo method, string culture)
        {
            var attrs = method.GetCustomAttributes<LuaFunctionAttribute>(false);
            var currentAttr = attrs.SingleOrDefault(attr => attr.Culture == culture);
            if (currentAttr == null)
            {
                currentAttr = attrs.First();
            }

            return (currentAttr, method);
        }

        private string Dump((LuaFunctionAttribute Attribute, MethodInfo ReflectObject) x)
        {
            var name = x.ReflectObject.Name;
            var usage = x.Attribute.Usage;
            var methodInfo = x.ReflectObject.ToString();
            var stringBuilder = new StringBuilder(256);

            stringBuilder.AppendLine($"+ {name}");
            stringBuilder.AppendLine($"  - {methodInfo}");
            if (!string.IsNullOrEmpty(usage))
            {
                stringBuilder.AppendLine($"  {usage}");
            }

            return stringBuilder.ToString();
        }

        public string DumpMethod(string methodName, string culture= null)
        {
            if (culture == null)
                culture = CultureInfo.CurrentUICulture.ToString();

            var method = GetType().GetMethod(methodName, FindBindingFlags);
            return Dump(Get_Attribute_ReflectObject(method, culture));
        }

        public string DumpSupport(string culture = null)
        {
            if (culture == null)
                culture = CultureInfo.CurrentUICulture.ToString();

            var definitions = EnumMethodDefinitions(culture);
            var stringBuilder = new StringBuilder(256);

            stringBuilder.AppendLine(GetType().Name + " Functions:\r\n");
            foreach (var definition in definitions)
            {
                stringBuilder.Append(Dump(definition));
            }
            stringBuilder.AppendLine();

            return stringBuilder.ToString();
        }

    }
}
