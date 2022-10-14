using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using DigitalPlatform.Script;

namespace UnitTestCompactLog
{
    [TestClass]
    public class UnitTestIsbnSplitter
    {
        [TestMethod]
        public void Test_issn_01()
        {
            string issn = "1006-3358";

            // -1:出错; 0:未修改校验位; 1:修改了校验位
            int ret = IsbnSplitter.IssnInsertHyphen(
    issn,
    "force8",
    out string target,
    out string strError);
            Assert.AreEqual(1, ret);
            Assert.AreEqual("1006-3358", target);
        }

        [TestMethod]
        public void Test_issn_01_1()
        {
            string issn = "10063358";

            // -1:出错; 0:未修改校验位; 1:修改了校验位
            int ret = IsbnSplitter.IssnInsertHyphen(
    issn,
    "force8",
    out string target,
    out string strError);
            Assert.AreEqual(1, ret);
            Assert.AreEqual("1006-3358", target);
        }

        [TestMethod]
        public void Test_issn_01_2()
        {
            string issn = "1006~3358";  // 对不是横杠的符号也能适应

            // -1:出错; 0:未修改校验位; 1:修改了校验位
            int ret = IsbnSplitter.IssnInsertHyphen(
    issn,
    "force8",
    out string target,
    out string strError);
            Assert.AreEqual(1, ret);
            Assert.AreEqual("1006-3358", target);
        }

        // 对不是 977 开头、并且(数字部分)也不是 8 字符的号码，会报错
        [TestMethod]
        public void Test_issn_01_3()
        {
            string issn = "9781672666221";

            // -1:出错; 0:未修改校验位; 1:修改了校验位
            int ret = IsbnSplitter.IssnInsertHyphen(
    issn,
    "force8",
    out string target,
    out string strError);
            Assert.AreEqual(-1, ret);
            Assert.AreEqual("", target);
        }

        [TestMethod]
        public void Test_issn_02()
        {
            string issn = "9771672666221";

            // -1:出错; 0:未修改校验位; 1:修改了校验位
            int ret = IsbnSplitter.IssnInsertHyphen(
    issn,
    "force8",
    out string target,
    out string strError);
            Assert.AreEqual(0, ret);
            Assert.AreEqual("1672-6669", target);
        }

        [TestMethod]
        public void Test_issn_03()
        {
            string issn = "9771672666221";

            // -1:出错; 0:未修改校验位; 1:修改了校验位
            int ret = IsbnSplitter.IssnInsertHyphen(
    issn,
    "", // force8 和 force13 都没有指定，默认为 auto 效果
    out string target,
    out string strError);
            Assert.AreEqual(1, ret);
            Assert.AreEqual("1672-6669", target);
        }

        [TestMethod]
        public void Test_issn_04()
        {
            string issn = "9771672666221";

            // -1:出错; 0:未修改校验位; 1:修改了校验位
            int ret = IsbnSplitter.IssnInsertHyphen(
    issn,
    "force13",
    out string target,
    out string strError);
            Assert.AreEqual(0, ret);
            Assert.AreEqual("977-1672-666-22-1", target);
        }

        [TestMethod]
        public void Test_issn_05()
        {
            string issn = "9771672666221";

            // -1:出错; 0:未修改校验位; 1:修改了校验位
            int ret = IsbnSplitter.IssnInsertHyphen(
    issn,
    "auto",
    out string target,
    out string strError);
            Assert.AreEqual(0, ret);
            Assert.AreEqual("977-1672-666-22-1", target);
        }

        [TestMethod]
        public void Test_issn_06()
        {
            string issn = "1672-6669";

            // -1:出错; 0:未修改校验位; 1:修改了校验位
            int ret = IsbnSplitter.IssnInsertHyphen(
    issn,
    "auto",
    out string target,
    out string strError);
            Assert.AreEqual(1, ret);
            Assert.AreEqual("1672-6669", target);
        }

        [TestMethod]
        public void Test_issn_07()
        {
            string issn = "16726669";

            // -1:出错; 0:未修改校验位; 1:修改了校验位
            int ret = IsbnSplitter.IssnInsertHyphen(
    issn,
    "auto",
    out string target,
    out string strError);
            Assert.AreEqual(1, ret);
            Assert.AreEqual("1672-6669", target);
        }
    }
}
