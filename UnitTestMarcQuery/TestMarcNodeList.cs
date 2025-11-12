using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DigitalPlatform.Text;
using DigitalPlatform.Marc;

namespace UnitTestMarcQuery
{
    [TestClass]
    public class TestMarcNodeList
    {
        // 测试直接替换 .ChildNodes
        [TestMethod]
        public void setChildNodeList_01()
        {
            string worksheet = @"012345678901234567890123
200  ǂaAAAǂbBBB
300  ǂaAAA
400  ǂaAAA";
            string target = @"012345678901234567890123
200  ǂcCCC
300  ǂaAAA
400  ǂaAAA";

            var record = MarcRecord.FromWorksheet(worksheet);
            var fields = record.select("field[@name='200']");
            Assert.IsNotNull(fields);
            Assert.AreEqual(1, fields.count);

            var children = new ChildNodeList();
            children.add(new MarcSubfield("c", "CCC"));
            fields[0].ChildNodes = children;

            var errors = record.VerifyOwner();
            if (errors.Count > 0)
                Console.WriteLine(StringUtil.MakePathList(errors, "\r\n"));
            Assert.AreEqual(0, errors.Count);
            Assert.AreEqual(target, record.ToWorksheet());
        }

        // 作为对照，用 .clear() .add() 方法
        [TestMethod]
        public void setChildNodeList_02()
        {
            string worksheet = @"012345678901234567890123
200  ǂaAAAǂbBBB
300  ǂaAAA
400  ǂaAAA";
            string target = @"012345678901234567890123
200  ǂcCCC
300  ǂaAAA
400  ǂaAAA";

            var record = MarcRecord.FromWorksheet(worksheet);
            var fields = record.select("field[@name='200']");
            Assert.IsNotNull(fields);
            Assert.AreEqual(1, fields.count);

            fields[0].ChildNodes.clear();
            fields[0].ChildNodes.add(new MarcSubfield("c", "CCC"));

            var errors = record.VerifyOwner();
            if (errors.Count > 0)
                Console.WriteLine(StringUtil.MakePathList(errors, "\r\n"));
            Assert.AreEqual(0, errors.Count);
            Assert.AreEqual(target, record.ToWorksheet());
        }

        // 替换后还可以再改
        [TestMethod]
        public void setChildNodeList_03()
        {
            string worksheet = @"012345678901234567890123
200  ǂaAAAǂbBBB
300  ǂaAAA
400  ǂaAAA";
            string target = @"012345678901234567890123
200  ǂcCCCǂdDDD
300  ǂaAAA
400  ǂaAAA";

            var record = MarcRecord.FromWorksheet(worksheet);
            var fields = record.select("field[@name='200']");
            Assert.IsNotNull(fields);
            Assert.AreEqual(1, fields.count);

            var children = new ChildNodeList();
            children.add(new MarcSubfield("c", "CCC"));
            fields[0].ChildNodes = children;

            // 直接在 children 上追加。这个要求比较高。如果降低要求，也可以只允许重新获得 .ChildNodes 再改
            children.add(new MarcSubfield("d", "DDD"));

            var errors = record.VerifyOwner();
            if (errors.Count > 0)
                Console.WriteLine(StringUtil.MakePathList(errors, "\r\n"));
            Assert.AreEqual(0, errors.Count);
            Assert.AreEqual(target, record.ToWorksheet());
        }

        // 剪枝
        [TestMethod]
        public void setChildNodeList_04()
        {
            string source1 = @"012345678901234567890123
200  ǂaAAAǂbBBB
300  ǂaAAA
400  ǂaAAA";
            string source2 = @"012345678901234567890123
200  ǂcCCCǂdDDD
300  ǂaAAA
400  ǂaAAA";
            string target1 = @"012345678901234567890123
200  ǂcCCCǂdDDD
300  ǂaAAA
400  ǂaAAA";
            string target2 = @"012345678901234567890123
200  
300  ǂaAAA
400  ǂaAAA";

            var record1 = MarcRecord.FromWorksheet(source1);
            var fields1 = record1.select("field[@name='200']");
            Assert.IsNotNull(fields1);
            Assert.AreEqual(1, fields1.count);

            var record2 = MarcRecord.FromWorksheet(source2);
            var fields2 = record2.select("field[@name='200']");
            Assert.IsNotNull(fields2);
            Assert.AreEqual(1, fields2.count);

            fields1[0].ChildNodes = fields2[0].ChildNodes;

            {
                var errors = record1.VerifyOwner();
                if (errors.Count > 0)
                    Console.WriteLine(StringUtil.MakePathList(errors, "\r\n"));
                Assert.AreEqual(0, errors.Count);
            }
            Assert.AreEqual(target1, record1.ToWorksheet());

            /*
            // 摘除以前，出现 .Parent 不匹配的情况
            {
                var errors = record2.VerifyOwner();
                if (errors.Count > 0)
                    Console.WriteLine(StringUtil.MakePathList(errors, "\r\n"));
                Assert.AreEqual(2, errors.Count);
            }

            fields2[0].ChildNodes.detach();
            // 摘除以后就没有问题了
            */
            {
                var errors = record2.VerifyOwner();
                if (errors.Count > 0)
                    Console.WriteLine(StringUtil.MakePathList(errors, "\r\n"));
                Assert.AreEqual(0, errors.Count);
            }

            Assert.AreEqual(target2, record2.ToWorksheet());

        }

    }
}
