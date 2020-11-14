using Microsoft.VisualStudio.TestTools.UnitTesting;

using DigitalPlatform.Marc;

namespace UnitTestMarcQuery
{
    /// <summary>
    /// 测试 MarcRecord 类
    /// </summary>
    [TestClass]
    public class TestMarcRecord
    {
        // 测试删除空子字段
        [TestMethod]
        public void Test_DetachEmptySubfields_1()
        {
            string worksheet = @"012345678901234567890123
200  ǂaAAAǂbBBB
300  ǂaAAAǂb
400  ǂaAAA";
            var record = MarcRecord.FromWorksheet(worksheet);
            record.DetachEmptySubfields();

            string target = @"012345678901234567890123
200  ǂaAAAǂbBBB
300  ǂaAAA
400  ǂaAAA";
            Assert.AreEqual(target, record.ToWorksheet());
        }

        // 测试删除空子字段
        // 注意，删除子字段后，即便字段内容为空，字段也不会被删除
        [TestMethod]
        public void Test_DetachEmptySubfields_2()
        {
            string worksheet = @"012345678901234567890123
200  ǂaAAAǂbBBB
300  ǂaǂb
400  ǂaAAA";
            var record = MarcRecord.FromWorksheet(worksheet);
            record.DetachEmptySubfields();

            string target = @"012345678901234567890123
200  ǂaAAAǂbBBB
300  
400  ǂaAAA";
            Assert.AreEqual(target, record.ToWorksheet());
        }

        // 测试删除空字段
        [TestMethod]
        public void Test_DetachEmptyFields_1()
        {
            string worksheet = @"012345678901234567890123
200  ǂaAAAǂbBBB
300  
400  ǂaAAA";
            var record = MarcRecord.FromWorksheet(worksheet);
            record.DetachEmptyFields();

            string target = @"012345678901234567890123
200  ǂaAAAǂbBBB
400  ǂaAAA";
            Assert.AreEqual(target, record.ToWorksheet());
        }

        // 测试删除空字段
        // 注意，空子字段并不会被删除
        [TestMethod]
        public void Test_DetachEmptyFields_2()
        {
            string worksheet = @"012345678901234567890123
200  ǂaAAAǂbBBB
300  ǂaǂb
400  ǂaAAA";
            var record = MarcRecord.FromWorksheet(worksheet);
            record.DetachEmptyFields();

            string target = @"012345678901234567890123
200  ǂaAAAǂbBBB
300  ǂaǂb
400  ǂaAAA";
            Assert.AreEqual(target, record.ToWorksheet());
        }

        // 测试删除空字段、空子字段
        // 注意，如果一个字段里面的空子字段都删除后导致字段内容为空，那么字段也会随后被删除
        [TestMethod]
        public void Test_DetachEmptyFieldsSubfields_1()
        {
            string worksheet = @"012345678901234567890123
200  ǂaAAAǂbBBB
300  ǂaǂb
400  ǂaAAA";
            var record = MarcRecord.FromWorksheet(worksheet);
            record.DetachEmptyFieldsSubfields();

            string target = @"012345678901234567890123
200  ǂaAAAǂbBBB
400  ǂaAAA";
            Assert.AreEqual(target, record.ToWorksheet());
        }
    }
}
