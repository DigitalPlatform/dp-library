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

            await log.Add("����{0}", new object[] { 1 });

            Assert.AreEqual(1, log.EntryCount);

            log.RemoveEntry("����{0}");

            Assert.AreEqual(0, log.EntryCount);
        }

        [TestMethod]
        public async Task TestMethod2()
        {
            CompactLog log = new CompactLog();

            await log.Add("����{0}", new object[] { 1 });

            Assert.AreEqual(1, log.EntryCount);

            var entry_create_time = log.GetEntry()[0].StartTime;

            log.WriteToLog((text) =>
            {
                Assert.AreEqual(true, text.Contains("����1"));
            });

            // entry ��ʱ���� WriteToLog() �Ժ󲻻��
            Assert.AreEqual(entry_create_time, log.GetEntry()[0].StartTime);

            log.RemoveEntry("����{0}");

            Assert.AreEqual(0, log.EntryCount);

            await log.Add("����{0}", new object[] { 2 });

            Assert.AreEqual(1, log.EntryCount);

            log.WriteToLog((text) =>
            {
                Assert.AreEqual(false, text.Contains("����1"));
                Assert.AreEqual(true, text.Contains("����2"));
            });

        }

        [TestMethod]
        public async Task TestMethod3()
        {
            CompactLog log = new CompactLog();

            await log.Add("����{0}", new object[] { 1 });

            Assert.AreEqual(1, log.EntryCount);

            var entry_create_time = log.GetEntry()[0].StartTime;

            log.WriteToLog((text) =>
            {
                Assert.AreEqual(true, text.Contains("����1"));
            },
            "reset_start_time");

            // entry ��Ȼ����
            Assert.AreEqual(1, log.EntryCount);
            // �� entry.TotalCount �Ѿ������
            Assert.AreEqual(0, log.GetEntry()[0].TotalCount);
            // entry.Datas Ҳ�����
            Assert.AreEqual(0, log.GetEntry()[0].Datas.Count);

            // entry ��ʱ���� WriteToLog() �Ժ���
            Assert.AreNotEqual(entry_create_time, log.GetEntry()[0].StartTime);
            // ��� DateTime.MinValue
            Assert.AreEqual(DateTime.MinValue, log.GetEntry()[0].StartTime);

            // entry.TotalCount Ϊ�յ�����µ��� WriteToLog() �ǲ��������
            int write_count = 0;
            log.WriteToLog((text) =>
            {
                write_count++;
            });
            Assert.AreEqual(0, write_count);
        }


    }
}
