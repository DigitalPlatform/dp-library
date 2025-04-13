using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DigitalPlatform.Text;

namespace UnitTestCore
{
    public class TestStringUtil
    {
        [Theory]
        [InlineData("", false)]
        [InlineData(null, false)]
        [InlineData("a", false)]
        [InlineData("z", false)]
        [InlineData("A", false)]
        [InlineData("Z", false)]
        [InlineData(".", false)]
        [InlineData("~", false)]
        [InlineData("a0", false)]
        [InlineData("0z", false)]
        [InlineData("0", true)]
        [InlineData("1", true)]
        [InlineData("2", true)]
        [InlineData("3", true)]
        [InlineData("4", true)]
        [InlineData("5", true)]
        [InlineData("6", true)]
        [InlineData("7", true)]
        [InlineData("8", true)]
        [InlineData("9", true)]

        public void test_isDigital(string input, bool expected)
        {
            var result = StringUtil.IsDigital(input);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("", "-1")]
        [InlineData(null, "-1")]
        [InlineData("a", "-1")]
        [InlineData("z", "-1")]
        [InlineData("A", "-1")]
        [InlineData("Z", "-1")]
        [InlineData(".", ".")]
        [InlineData("~", "-1")]
        [InlineData("a0", "0")]
        [InlineData("0z", "0")]
        [InlineData(" 00 ", "00")]
        [InlineData("0", "0")]
        [InlineData("1", "1")]
        [InlineData("2", "2")]
        [InlineData("3", "3")]
        [InlineData("4", "4")]
        [InlineData("5", "5")]
        [InlineData("6", "6")]
        [InlineData("7", "7")]
        [InlineData("8", "8")]
        [InlineData("9", "9")]

        public void test_getStringNumber(string input, string expected)
        {
            var result = StringUtil.GetStringNumber(input);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void test_isNumSpeed()
        {
            int count = 1000;
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < count; i++)
            {
                var ret = StringUtil.IsNum("1234567890");
            }
            stopwatch.Stop();
            Console.WriteLine($"IsNum() 调用 {count} 次耗费时间: {stopwatch.ElapsedMilliseconds} ms");
        }

        [Fact]
        public void test_isDigitalSpeed()
        {
            int count = 1000;
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < count; i++)
            {
                var ret = StringUtil.IsDigital("1234567890");
            }
            stopwatch.Stop();
            Console.WriteLine($"IsDigital() 调用 {count} 次耗费时间: {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
