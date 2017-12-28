using System;
using System.Collections.Generic;

namespace MoonSharp.Extensions
{
    public class Config
    {
        public static string Get(Dictionary<string, object> configTable, string key, string @default)
        {
            var value = configTable.ContainsKey(key) ? configTable[key] : @default;
            return value as string;
        }

        public static T Get<T>(Dictionary<string, object> configTable, string key, T @default)
            where T : new()
        {
            if (new T() is Enum)
            {
                if (configTable.ContainsKey(key))
                    return (T)Enum.ToObject(typeof(T), int.Parse(configTable[key].ToString()));
                else return @default;
            }

            if (configTable.ContainsKey(key))
                return (T)Convert.ChangeType(configTable[key], typeof(T));
            else return @default;
        }

    }
}
