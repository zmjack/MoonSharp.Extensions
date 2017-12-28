using System;

namespace MoonSharp.Extensions
{
    public static class ArraySupport_ArrayExtentions
    {
        public static T[] Shuffle<T>(this T[] @this)
        {
            var length = @this.Length;
            var random = new Random();

            for (int i = 0; i < length; i++)
            {
                var rnd = random.Next(length);
                var take = @this[i];
                @this[i] = @this[rnd];
                @this[rnd] = take;
            }
            return @this;
        }

        public static T[][] Group<T>(this T[] @this, int count)
        {
            if (@this.Length == 0) return new T[0][];

            var maxIndex = @this.Length - 1;
            var ret = new T[maxIndex / count + 1][];

            for (int page = 0; page < ret.Length; page++)
            {
                if (page != ret.Length - 1)
                    ret[page] = new T[count];
                else ret[page] = new T[maxIndex % count + 1];
            }

            for (int i = 0; i < @this.Length; i++)
            {
                ret[i / count][i % count] = @this[i];
            }

            return ret;
        }

    }
}
