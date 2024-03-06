using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using DigitalPlatform.Text;

namespace UnitTestCompactLog
{
    [TestClass]
    public class UnitTestStringUtil
    {
        [TestMethod]
        public void Test_compactNumber_01()
        {
            List<string> source = new List<string>();
            var result = StringUtil.CompactNumbers(source);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Test_compactNumber_02()
        {
            List<string> source = new List<string>() { "0000001" };
            var result = StringUtil.CompactNumbers(source);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("0000001", result[0]);
        }

        [TestMethod]
        public void Test_compactNumber_03()
        {
            List<string> source = new List<string>() { "0000001", 
                "0000002" };
            var result = StringUtil.CompactNumbers(source);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("0000001-2", result[0]);
        }

        [TestMethod]
        public void Test_compactNumber_04()
        {
            List<string> source = new List<string>() {
                "0000001",
                "0000002",
                "0000003",
                "0000004",
                "0000005",
                "0000006",
                "0000007",
                "0000008",
                "0000009",
                "0000010" };
            var result = StringUtil.CompactNumbers(source);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("0000001-10", result[0]);
        }

        [TestMethod]
        public void Test_compactNumber_05()
        {
            List<string> source = new List<string>() {
                "0000001",
                "0000002",
                "0000009",
                "0000010" };
            var result = StringUtil.CompactNumbers(source);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("0000001-2", result[0]);
            Assert.AreEqual("0000009-10", result[1]);
        }

        [TestMethod]
        public void Test_compactNumber_06()
        {
            List<string> source = new List<string>() {
                "0000001",
                "0000009",
                "0000010" };
            var result = StringUtil.CompactNumbers(source);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("0000001", result[0]);
            Assert.AreEqual("0000009-10", result[1]);
        }

        [TestMethod]
        public void Test_compactNumber_07()
        {
            List<string> source = new List<string>() {
                "0000001",
                "0000002",
                "0000010" };
            var result = StringUtil.CompactNumbers(source);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("0000001-2", result[0]);
            Assert.AreEqual("0000010", result[1]);
        }

        [TestMethod]
        public void Test_compactNumber_08()
        {
            List<string> source = new List<string>() {
                "0000001",
                "0000003",
                "0000005" };
            var result = StringUtil.CompactNumbers(source);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("0000001", result[0]);
            Assert.AreEqual("0000003", result[1]);
            Assert.AreEqual("0000005", result[2]);
        }

        // 

        [TestMethod]
        public void Test_compactNumber_12()
        {
            List<string> source = new List<string>() { "B0000001" };
            var result = StringUtil.CompactNumbers(source);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("B0000001", result[0]);
        }

        [TestMethod]
        public void Test_compactNumber_13()
        {
            List<string> source = new List<string>() { "B0000001",
                "B0000002" };
            var result = StringUtil.CompactNumbers(source);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("B0000001-2", result[0]);
        }

        [TestMethod]
        public void Test_compactNumber_14()
        {
            List<string> source = new List<string>() {
                "B0000001",
                "B0000002",
                "B0000003",
                "B0000004",
                "B0000005",
                "B0000006",
                "B0000007",
                "B0000008",
                "B0000009",
                "B0000010" };
            var result = StringUtil.CompactNumbers(source);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("B0000001-10", result[0]);
        }

        [TestMethod]
        public void Test_compactNumber_15()
        {
            List<string> source = new List<string>() {
                "B0000001",
                "B0000002",
                "B0000009",
                "B0000010" };
            var result = StringUtil.CompactNumbers(source);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("B0000001-2", result[0]);
            Assert.AreEqual("B0000009-10", result[1]);
        }

        [TestMethod]
        public void Test_compactNumber_16()
        {
            List<string> source = new List<string>() {
                "B0000001",
                "B0000009",
                "B0000010" };
            var result = StringUtil.CompactNumbers(source);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("B0000001", result[0]);
            Assert.AreEqual("B0000009-10", result[1]);
        }

        [TestMethod]
        public void Test_compactNumber_17()
        {
            List<string> source = new List<string>() {
                "B0000001",
                "B0000002",
                "B0000010" };
            var result = StringUtil.CompactNumbers(source);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("B0000001-2", result[0]);
            Assert.AreEqual("B0000010", result[1]);
        }

        [TestMethod]
        public void Test_unquote_01()
        {
            string source = "全2册";
            string target = "2";
            var ret = StringUtil.Unquote(source, "全册共册全卷共卷");
            Assert.AreEqual(target, ret);
        }

        [TestMethod]
        public void Test_unquote_02()
        {
            string source = "共2册";
            string target = "2";
            var ret = StringUtil.Unquote(source, "全册共册全卷共卷");
            Assert.AreEqual(target, ret);
        }


        [TestMethod]
        public void Test_unquote_03()
        {
            string source = "全2卷";
            string target = "2";
            var ret = StringUtil.Unquote(source, "全册共册全卷共卷");
            Assert.AreEqual(target, ret);
        }

        [TestMethod]
        public void Test_unquote_04()
        {
            string source = "共2卷";
            string target = "2";
            var ret = StringUtil.Unquote(source, "全册共册全卷共卷");
            Assert.AreEqual(target, ret);
        }
    }
}
