using MoonSharp.Extensions;
using System;
using Xunit;

namespace ArraySupportTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            const int _COUNT = 5;

            Assert.Equal(new int[0][], Array(0).Group(_COUNT));

            Assert.Equal(new int[][]
            {
                new int[] { 1, 2, 3, 4 },
            }, Array(4).Group(_COUNT));

            Assert.Equal(new int[][]
            {
                new int[] { 1, 2, 3, 4, 5 },
            }, Array(5).Group(_COUNT));

            Assert.Equal(new int[][]
            {
                new int[] { 1, 2, 3, 4, 5 },
                new int[] { 6 },
            }, Array(6).Group(_COUNT));

            Assert.Equal(new int[][]
            {
                new int[] { 1, 2, 3, 4, 5 },
                new int[] { 6, 7, 8, 9 },
            }, Array(9).Group(_COUNT));

            Assert.Equal(new int[][]
            {
                new int[] { 1, 2, 3, 4, 5 },
                new int[] { 6, 7, 8, 9, 10 },
            }, Array(10).Group(_COUNT));

            Assert.Equal(new int[][]
            {
                new int[] { 1, 2, 3, 4, 5 },
                new int[] { 6, 7, 8, 9, 10 },
                new int[] { 11 },
            }, Array(11).Group(_COUNT));                        
        }

        public int[] Array(int count)
        {
            var ret = new int[count];
            for (int i = 0; i < count; i++)
            {
                ret[i] = i + 1;
            }
            return ret;
        }
    }
}
