using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using DigitalPlatform.Core;

namespace UnitTestCompactLog
{
    // 针对 RangeList 和 Range 的单元测试
    [TestClass]
    public class UnitTestRange
    {
        [TestMethod]
        public void Test_rangeList_01()
        {
            string range = "";
            var range_list = new RangeList(range);

            Assert.AreEqual(0, range_list.Count);
        }

        [TestMethod]
        public void Test_rangeList_02()
        {
            string range = "0-0";
            var range_list = new RangeList(range);

            Assert.AreEqual(1, range_list.Count);

            Assert.AreEqual(0, range_list[0].lStart);
            Assert.AreEqual(1, range_list[0].lLength);
        }

        [TestMethod]
        public void Test_rangeList_03()
        {
            string range = "0-0,1-2";
            var range_list = new RangeList(range);

            Assert.AreEqual(2, range_list.Count);

            Assert.AreEqual(0, range_list[0].lStart);
            Assert.AreEqual(1, range_list[0].lLength);

            Assert.AreEqual(1, range_list[1].lStart);
            Assert.AreEqual(2, range_list[1].lLength);
        }

        [TestMethod]
        public void Test_rangeList_10()
        {
            string range = "1:0";
            var range_list = new RangeList(range);

            Assert.AreEqual(1, range_list.Count);

            Assert.AreEqual(1, range_list[0].lStart);
            Assert.AreEqual(0, range_list[0].lLength);
        }

        [TestMethod]
        public void Test_rangeList_11()
        {
            string range = "1:0";
            var range_list = new RangeList(range);
            var result = range_list.ToString();
            Assert.AreEqual("1:0", result);
        }

        [TestMethod]
        public void Test_rangeList_20()
        {
            string range = "-2";    // 效果相当于 "(-1)-2"。起点为 -1，表示一个尽可能小的起点
            var range_list = new RangeList(range);

            Assert.AreEqual(1, range_list.Count);

            Assert.AreEqual(-1, range_list[0].lStart);
            Assert.AreEqual(2, range_list[0].lLength);
        }

        [TestMethod]
        public void Test_rangeList_21()
        {
            string range = "5-";    // 效果相当于 "5-(-1)"。长度为 -1 表示无穷大，或者说期望长度尽可能大
            var range_list = new RangeList(range);

            Assert.AreEqual(1, range_list.Count);

            Assert.AreEqual(5, range_list[0].lStart);
            Assert.AreEqual(-1, range_list[0].lLength);
        }

        [TestMethod]
        public void Test_rangeList_22()
        {
            string range = "5-";    // 效果相当于 "5-(-1)"。长度为 -1 表示无穷大，或者说期望长度尽可能大
            var range_list = new RangeList(range);
            var result = range_list.ToString();
            Assert.AreEqual("5-", result);
        }

        [TestMethod]
        public void Test_rangeList_23()
        {
            string range = "-2";    // 效果相当于 "(-1)-2"。起点为 -1，表示一个尽可能小的起点
            var range_list = new RangeList(range);
            var result = range_list.ToString();
            Assert.AreEqual("-2", result);
        }

        [TestMethod]
        public void Test_rangeList_24()
        {
            string range = "5-,-2";    // 效果相当于 "(-1)-2"。起点为 -1，表示一个尽可能小的起点
            var range_list = new RangeList(range);
            var result = range_list.ToString();
            Assert.AreEqual("5-,-2", result);
        }
    }
}
