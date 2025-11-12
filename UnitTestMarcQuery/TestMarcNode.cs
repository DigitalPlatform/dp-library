using System;

using DigitalPlatform.Marc;
using DigitalPlatform.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestMarcQuery
{
    [TestClass]
    public class TestMarcNode
    {
        // 加入下级尾部
        [TestMethod]
        public void append_01()
        {
            string worksheet = @"012345678901234567890123
200  ǂaAAAǂbBBB
300  ǂaAAA
400  ǂaAAA";
            string target = @"012345678901234567890123
200  ǂaAAAǂbBBBǂcCCC
300  ǂaAAA
400  ǂaAAA";

            var record = MarcRecord.FromWorksheet(worksheet);
            var fields = record.select("field[@name='200']");
            Assert.IsNotNull(fields);
            Assert.AreEqual(1, fields.count);

            fields[0].append(new MarcSubfield("c", "CCC"));

            var errors = record.VerifyOwner();
            if (errors.Count > 0)
                Console.WriteLine(StringUtil.MakePathList(errors, "\r\n"));
            Assert.AreEqual(0, errors.Count);
            Assert.AreEqual(target, record.ToWorksheet());
        }

        // 插入一个新字段，若干子字段
        [TestMethod]
        public void append_02()
        {
            string target = @"????????????????????????
200  ǂaAAAǂbBBBǂcCCC";

            var record = new MarcRecord();
            var field = new MarcField("200", "  ");
            record.append(field);

            field.append(new MarcSubfield("a", "AAA"));
            field.append(new MarcSubfield("b", "BBB"));
            field.append(new MarcSubfield("c", "CCC"));

            var errors = record.VerifyOwner();
            if (errors.Count > 0)
                Console.WriteLine(StringUtil.MakePathList(errors, "\r\n"));
            Assert.AreEqual(0, errors.Count);
            Assert.AreEqual(target, record.ToWorksheet());
        }

        // 先插入子字段，再插入字段
        [TestMethod]
        public void append_03()
        {
            string target = @"????????????????????????
200  ǂaAAAǂbBBBǂcCCC";

            var record = new MarcRecord();
            var field = new MarcField("200", "  ");

            field.append(new MarcSubfield("a", "AAA"));
            field.append(new MarcSubfield("b", "BBB"));
            field.append(new MarcSubfield("c", "CCC"));

            record.append(field);

            var errors = record.VerifyOwner();
            if (errors.Count > 0)
                Console.WriteLine(StringUtil.MakePathList(errors, "\r\n"));
            Assert.AreEqual(0, errors.Count);
            Assert.AreEqual(target, record.ToWorksheet());
        }


        [TestMethod]
        public void appendTo_01()
        {
            string target = @"????????????????????????
200  ǂaAAAǂbBBBǂcCCC";

            var record = new MarcRecord();
            var field = new MarcField("200", "  ");

            field.append(new MarcSubfield("a", "AAA"));
            field.append(new MarcSubfield("b", "BBB"));
            field.append(new MarcSubfield("c", "CCC"));

            // field 插入 record 的下级末尾
            field.appendTo(record);

            var errors = record.VerifyOwner();
            if (errors.Count > 0)
                Console.WriteLine(StringUtil.MakePathList(errors, "\r\n"));
            Assert.AreEqual(0, errors.Count);
            Assert.AreEqual(target, record.ToWorksheet());
        }

        // prepend 是前插
        [TestMethod]
        public void prepend_01()
        {
            string target = @"????????????????????????
200  ǂcCCCǂbBBBǂaAAA";

            var record = new MarcRecord();
            var field = new MarcField("200", "  ");
            record.append(field);

            field.prepend(new MarcSubfield("a", "AAA"));
            field.prepend(new MarcSubfield("b", "BBB"));
            field.prepend(new MarcSubfield("c", "CCC"));

            var errors = record.VerifyOwner();
            if (errors.Count > 0)
                Console.WriteLine(StringUtil.MakePathList(errors, "\r\n"));
            Assert.AreEqual(0, errors.Count);
            Assert.AreEqual(target, record.ToWorksheet());
        }

        // 先插入子字段，再把字段加入记录
        [TestMethod]
        public void prepend_02()
        {
            string target = @"????????????????????????
200  ǂcCCCǂbBBBǂaAAA";

            var field = new MarcField("200", "  ");

            field.prepend(new MarcSubfield("a", "AAA"));
            field.prepend(new MarcSubfield("b", "BBB"));
            field.prepend(new MarcSubfield("c", "CCC"));

            var record = new MarcRecord();
            record.append(field);

            var errors = record.VerifyOwner();
            if (errors.Count > 0)
                Console.WriteLine(StringUtil.MakePathList(errors, "\r\n"));
            Assert.AreEqual(0, errors.Count);
            Assert.AreEqual(target, record.ToWorksheet());
        }


        // prependTo 是前插
        [TestMethod]
        public void prependTo_01()
        {
            string target = @"????????????????????????
200  ǂcCCCǂbBBBǂaAAA";

            var record = new MarcRecord();
            var field = new MarcField("200", "  ");
            field.prependTo(record);

            (new MarcSubfield("a", "AAA")).prependTo(field);
            (new MarcSubfield("b", "BBB")).prependTo(field); 
            (new MarcSubfield("c", "CCC")).prependTo(field); 

            var errors = record.VerifyOwner();
            if (errors.Count > 0)
                Console.WriteLine(StringUtil.MakePathList(errors, "\r\n"));
            Assert.AreEqual(0, errors.Count);
            Assert.AreEqual(target, record.ToWorksheet());
        }

        // 先插入子字段，再把字段加入记录
        [TestMethod]
        public void prependTo_02()
        {
            string target = @"????????????????????????
200  ǂcCCCǂbBBBǂaAAA";

            var field = new MarcField("200", "  ");

            (new MarcSubfield("a", "AAA")).prependTo(field);
            (new MarcSubfield("b", "BBB")).prependTo(field);
            (new MarcSubfield("c", "CCC")).prependTo(field);

            var record = new MarcRecord();
            field.prependTo(record);

            var errors = record.VerifyOwner();
            if (errors.Count > 0)
                Console.WriteLine(StringUtil.MakePathList(errors, "\r\n"));
            Assert.AreEqual(0, errors.Count);
            Assert.AreEqual(target, record.ToWorksheet());
        }

    }
}
