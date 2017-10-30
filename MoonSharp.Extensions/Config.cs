using System;
using System.Collections.Generic;
using System.Text;

namespace MoonSharp.Extensions
{
    public class Config
    {
        public static string Get(Dictionary<string, string> configTable, string key, string @default)
        {
            var value = configTable.ContainsKey(key) ? configTable[key] : @default;
            return value;
        }

        public static T Get<T>(Dictionary<string, string> configTable, string key, T @default)
            where T : new()
        {
            if (new T() is Enum)
            {
                return (T)Enum.ToObject(typeof(T), int.Parse(configTable[key]));
            }

            if (configTable.ContainsKey(key))
                return (T)Convert.ChangeType(configTable[key], typeof(T));
            else return @default;
        }

    }
}
