using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DigitalPlatform.Marc;
using System.Collections.Generic;

namespace UnitTestMarcQuery
{
    [TestClass]
    public class TestMarcNodeList_OtherMethodsTests
    {
        // clone() 应产生深拷贝：新集合元素文本相同但引用不同，且克隆节点的 Parent 应为 null
        [TestMethod]
        public void Clone_CreatesDeepCopy()
        {
            var list = new MarcNodeList();
            var f1 = new MarcField("100", "  ");
            f1.append(new MarcSubfield("a", "AAA"));
            var f2 = new MarcField("200", "  ");
            f2.append(new MarcSubfield("b", "BBB"));
            list.add(f1).add(f2);

            var cloned = list.clone();

            Assert.AreEqual(list.count, cloned.count);
            for (int i = 0; i < list.count; i++)
            {
                Assert.AreNotSame(list[i], cloned[i], "克隆后的节点不应与原节点为同一引用");
                Assert.AreEqual(list[i].Text, cloned[i].Text, "克隆节点的 Text 应相同");
                Assert.IsNull(cloned[i].Parent, "克隆出的节点 Parent 应为 null");
            }
        }

        // add(MarcNodeList) 应把节点逐个追加（引用保留）
        [TestMethod]
        public void Add_List_AddsReferences()
        {
            var src = new MarcNodeList();
            var a = new MarcField("100", "  ");
            var b = new MarcField("200", "  ");
            src.add(a).add(b);

            var dest = new MarcNodeList();
            dest.add(src);

            Assert.AreEqual(2, dest.count);
            Assert.AreSame(a, dest[0]);
            Assert.AreSame(b, dest[1]);
        }

        // remove(MarcNode) 应移除指定元素并返回包含被移除元素的新集合
        [TestMethod]
        public void Remove_Node_RemovesAndReturnsRemoved()
        {
            var list = new MarcNodeList();
            var a = new MarcField("100", "  ");
            var b = new MarcField("200", "  ");
            list.add(a).add(b);

            var removedList = list.remove(a);
            Assert.AreEqual(1, removedList.count);
            Assert.AreSame(a, removedList[0]);
            Assert.AreEqual(1, list.count);
            Assert.AreSame(b, list[0]);
        }

        // insertSequence 与 insertSequenceReverse 按 Name 排序插入
        [TestMethod]
        public void InsertSequence_InsertsInNameOrder()
        {
            var list = new MarcNodeList();
            list.add(new MarcField("100", "  "));
            list.add(new MarcField("300", "  "));

            var mid = new MarcField("200", "  ");
            list.insertSequence(mid);

            Assert.AreEqual("100", list[0].Name);
            Assert.AreEqual("200", list[1].Name);
            Assert.AreEqual("300", list[2].Name);

            // reverse searching insertion
            var list2 = new MarcNodeList();
            list2.add(new MarcField("100", "  "));
            list2.add(new MarcField("300", "  "));
            var mid2 = new MarcField("200", "  ");
            list2.insertSequenceReverse(mid2);
            Assert.AreEqual("100", list2[0].Name);
            Assert.AreEqual("200", list2[1].Name);
            Assert.AreEqual("300", list2[2].Name);
        }

        // insertSequence 对相同 Name 的 PreferHead / PreferTail 行为
        [TestMethod]
        public void InsertSequence_PreferHeadAndTail_Behavior()
        {
            var list = new MarcNodeList();
            list.add(new MarcField("200", "  ")); // existing

            var head = new MarcField("200", "  ");
            list.insertSequence(head, InsertSequenceStyle.PreferHead);
            // PreferHead 应当插入到相等组的前面
            Assert.AreSame(head, list[0]);

            var tail = new MarcField("200", "  ");
            // insertSequence 会继续在末尾添加（等于 PreferTail 的情形），此处用 PreferTail，期望加到末尾
            list.insertSequence(tail, InsertSequenceStyle.PreferTail);
            Assert.AreSame(tail, list[list.count - 1]);
        }

        // sort() 与自定义 Comparison 排序
        [TestMethod]
        public void Sort_SortsByName_And_CustomComparison()
        {
            var list = new MarcNodeList();
            list.add(new MarcField("300", "  "));
            list.add(new MarcField("100", "  "));
            list.add(new MarcField("200", "  "));

            list.sort();
            Assert.AreEqual("100", list[0].Name);
            Assert.AreEqual("200", list[1].Name);
            Assert.AreEqual("300", list[2].Name);

            // 自定义比较：降序
            list.sort((x, y) => string.Compare(y.Name, x.Name));
            Assert.AreEqual("300", list[0].Name);
            Assert.AreEqual("200", list[1].Name);
            Assert.AreEqual("100", list[2].Name);
        }

        // getAt(index) 与 getAt(index, length) 的正常与异常行为
        [TestMethod]
        public void GetAt_Ranges_And_Errors()
        {
            var list = new MarcNodeList();
            list.add(new MarcField("100", "  "));
            list.add(new MarcField("200", "  "));
            list.add(new MarcField("300", "  "));

            var one = list.getAt(1);
            Assert.AreEqual(1, one.count);
            Assert.AreEqual("200", one[0].Name);

            var slice = list.getAt(1, 2);
            Assert.AreEqual(2, slice.count);
            Assert.AreEqual("200", slice[0].Name);
            Assert.AreEqual("300", slice[1].Name);

            var toEnd = list.getAt(1, -1);
            Assert.AreEqual(2, toEnd.count);
            CollectionAssert.AreEqual(new[] { "200", "300" }, toEnd.Select(n => n.Name).ToArray());

            // 异常：index 越界
            Assert.ThrowsException<ArgumentException>(() => list.getAt(-1));
            Assert.ThrowsException<ArgumentException>(() => list.getAt(10));
            // 异常：index + length 越界
            Assert.ThrowsException<ArgumentException>(() => list.getAt(2, 2));
        }

        // first()/last() 以及 FirstName / FirstIndicator / FirstContent / FirstText / AllText / Contents 属性
        [TestMethod]
        public void FirstLast_And_Properties_Work()
        {
            var f1 = new MarcField("100", "AB");
            f1.append(new MarcSubfield("a", "one"));
            var f2 = new MarcField("200", "CD");
            f2.append(new MarcSubfield("b", "two"));

            var list = new MarcNodeList();
            list.add(f1).add(f2);

            var firstList = list.first();
            Assert.AreEqual(1, firstList.count);
            Assert.AreSame(f1, firstList[0]);

            var lastList = list.last();
            Assert.AreEqual(1, lastList.count);
            Assert.AreSame(f2, lastList[0]);

            Assert.AreEqual("100", list.FirstName);
            Assert.AreEqual("AB", list.FirstIndicator);
            Assert.AreEqual('A', list.FirstIndicator1);
            Assert.AreEqual('B', list.FirstIndicator2);
            Assert.AreEqual(f1.Content, list.FirstContent);
            Assert.AreEqual(f1.Text + f2.Text, list.AllText);
            CollectionAssert.AreEqual(new[] { f1.Content, f2.Content }, list.Contents);
        }

        // FirstChild / LastChild 返回每个元素的第一个/最后一个下级
        [TestMethod]
        public void FirstChild_LastChild_ReturnChildren()
        {
            var f1 = new MarcField("100", "  ");
            f1.append(new MarcSubfield("a", "A1"));
            f1.append(new MarcSubfield("b", "A2"));

            var f2 = new MarcField("200", "  ");
            f2.append(new MarcSubfield("c", "B1"));

            var list = new MarcNodeList();
            list.add(f1).add(f2);

            var firstChildren = list.FirstChild;
            Assert.AreEqual(2, firstChildren.count);
            Assert.AreEqual("a", firstChildren[0].Name);
            Assert.AreEqual("c", firstChildren[1].Name);

            var lastChildren = list.LastChild;
            Assert.AreEqual(2, lastChildren.count);
            Assert.AreEqual("b", lastChildren[0].Name);
            Assert.AreEqual("c", lastChildren[1].Name);
        }

        // 批量修改 Name / Indicator / Content / Text / Leading 属性会作用到集合中每个元素
        [TestMethod]
        public void BulkSetters_ModifyAllElements()
        {
            var f1 = new MarcField("100", "  ");
            var f2 = new MarcField("200", "  ");
            var list = new MarcNodeList();
            list.add(f1).add(f2);

            list.Name = "999";
            Assert.AreEqual("999", f1.Name);
            Assert.AreEqual("999", f2.Name);

            list.Indicator = "XY";
            Assert.AreEqual("XY", f1.Indicator);
            Assert.AreEqual("XY", f2.Indicator);

            list.Content = "prefix";
            Assert.IsTrue(f1.Content.StartsWith("prefix"));
            Assert.IsTrue(f2.Content.StartsWith("prefix"));

            list.Text = "TTT";
            Assert.AreEqual("TTT??\u001e", f1.Text);  // 注: MarcField 类型的 Indicator 成员无法变成空，所以用 ?? 填充
            Assert.AreEqual("TTT??\u001e", f2.Text);

            // Leading 对 MarcField 生效（控制字段不生效，这里使用普通字段）
            list.Leading = "LL";
            Assert.AreEqual("LL", ((MarcField)f1).Leading);
            Assert.AreEqual("LL", ((MarcField)f2).Leading);
        }
    }
}