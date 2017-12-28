using MoonSharp.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace MoonSharp.Extensions
{
    public class ArraySupport : LuaSupport
    {
        [LuaFunction("en-US", "Get a new Array of random index. f(array)")]
        [LuaFunction("zh-CN", "获取乱序数组。f(数组)")]
        public object[] Shuffle(Dictionary<string, object> array)
        {
            return array.Values.Select(a => a).ToArray().Shuffle();
        }
        
    }
}
