using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using DigitalPlatform.Core;
using System;

namespace UnitTestCompactLog
{
    [TestClass]
    public class UnitTestCompactLog
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            CompactLog log = new CompactLog();

            await log.Add("文字{0}", new object[] { 1 });

            Assert.AreEqual(1, log.EntryCount);

            log.RemoveEntry("文字{0}");

            Assert.AreEqual(0, log.EntryCount);
        }

        [TestMethod]
        public async Task TestMethod2()
        {
            CompactLog log = new CompactLog();

            await log.Add("文字{0}", new object[] { 1 });

            Assert.AreEqual(1, log.EntryCount);

            var entry_create_time = log.GetEntry()[0].StartTime;

            log.WriteToLog((text) =>
            {
                Assert.AreEqual(true, text.Contains("文字1"));
            });

            // entry 的时间在 WriteToLog() 以后不会变
            Assert.AreEqual(entry_create_time, log.GetEntry()[0].StartTime);

            log.RemoveEntry("文字{0}");

            Assert.AreEqual(0, log.EntryCount);

            await log.Add("文字{0}", new object[] { 2 });

            Assert.AreEqual(1, log.EntryCount);

            log.WriteToLog((text) =>
            {
                Assert.AreEqual(false, text.Contains("文字1"));
                Assert.AreEqual(true, text.Contains("文字2"));
            });

        }

        [TestMethod]
        public async Task TestMethod3()
        {
            CompactLog log = new CompactLog();

            await log.Add("文字{0}", new object[] { 1 });

            Assert.AreEqual(1, log.EntryCount);

            var entry_create_time = log.GetEntry()[0].StartTime;

            log.WriteToLog((text) =>
            {
                Assert.AreEqual(true, text.Contains("文字1"));
            },
            "reset_start_time");

            // entry 依然存在
            Assert.AreEqual(1, log.EntryCount);
            // 但 entry.TotalCount 已经被清除
            Assert.AreEqual(0, log.GetEntry()[0].TotalCount);
            // entry.Datas 也被清除
            Assert.AreEqual(0, log.GetEntry()[0].Datas.Count);

            // entry 的时间在 WriteToLog() 以后会变
            Assert.AreNotEqual(entry_create_time, log.GetEntry()[0].StartTime);
            // 变成 DateTime.MinValue
            Assert.AreEqual(DateTime.MinValue, log.GetEntry()[0].StartTime);

            // entry.TotalCount 为空的情况下调用 WriteToLog() 是不会输出的
            int write_count = 0;
            log.WriteToLog((text) =>
            {
                write_count++;
            });
            Assert.AreEqual(0, write_count);
        }


    }
}
