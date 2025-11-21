using System;
using System.Collections;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using DigitalPlatform.Marc;
using System.Collections.Generic;

namespace UnitTestMarcQuery
{
    /*
说明（简要）：
- 新增文件 `UnitTestMarcQuery\TestMarcNodeList_EnumerableTests.cs`，包含若干 MSTest 单元测试，专门验证 `MarcNodeList` 实现的泛型和非泛型枚举器行为，以及与 LINQ 的兼容性和在集合修改后的枚举一致性。
- 这些测试使用现有的 `MarcField`/`MarcNode` 构造方式，能直接在现有工程中运行，无需额外修改或依赖。
*/
    [TestClass]
    public class TestMarcNodeList_EnumerableTests
    {
        // 测试泛型 IEnumerable<MarcNode> foreach 枚举顺序与索引一致
        [TestMethod]
        public void IEnumerable_Generic_Foreach_OrderMatchesIndex()
        {
            var list = new MarcNodeList();
            list.add(new MarcField("100", "  "));
            list.add(new MarcField("200", "  "));
            list.add(new MarcField("300", "  "));

            int idx = 0;
            foreach (MarcNode node in list)
            {
                Assert.AreSame(list[idx], node, "枚举得到的元素应与按索引访问的元素相同");
                idx++;
            }
            Assert.AreEqual(3, idx, "应枚举出 3 个元素");
        }

        // 测试可以用 LINQ 泛型扩展方法对 MarcNodeList 进行操作
        [TestMethod]
        public void IEnumerable_Linq_Extensions_Work()
        {
            var list = new MarcNodeList();
            list.add(new MarcField("100", "  "));
            list.add(new MarcField("200", "  "));
            list.add(new MarcField("300", "  "));

            // 使用 LINQ Count / First / Select 等
            int cnt = list.Count();
            Assert.AreEqual(3, cnt);

            var first = list.First();
            Assert.AreEqual("100", first.Name);

            var names = list.Select(n => n.Name).ToArray();
            CollectionAssert.AreEqual(new[] { "100", "200", "300" }, names);
        }

        // 测试非泛型 IEnumerable 枚举也能正常工作
        [TestMethod]
        public void IEnumerable_NonGeneric_Enumeration_Works()
        {
            var list = new MarcNodeList();
            list.add(new MarcField("100", "  "));
            list.add(new MarcField("200", "  "));

            IEnumerable nonGeneric = (IEnumerable)list;
            int count = 0;
            foreach (var obj in nonGeneric)
            {
                Assert.IsInstanceOfType(obj, typeof(MarcNode));
                count++;
            }
            Assert.AreEqual(list.count, count, "非泛型枚举得到的计数应等于 list.count");
        }

        // 直接使用 GetEnumerator() 的两种形式（泛型与非泛型）
        [TestMethod]
        public void IEnumerable_GetEnumerator_GenericAndNonGeneric_BothWork()
        {
            var list = new MarcNodeList();
            list.add(new MarcField("100", "  "));
            list.add(new MarcField("200", "  "));
            list.add(new MarcField("300", "  "));

            // 泛型枚举器
            var genEnum = ((IEnumerable<MarcNode>)list).GetEnumerator();
            int i = 0;
            while (genEnum.MoveNext())
            {
                Assert.AreSame(list[i], genEnum.Current);
                i++;
            }
            Assert.AreEqual(3, i);

            // 非泛型枚举器
            var nonGenEnum = ((IEnumerable)list).GetEnumerator();
            i = 0;
            while (nonGenEnum.MoveNext())
            {
                Assert.IsInstanceOfType(nonGenEnum.Current, typeof(MarcNode));
                i++;
            }
            Assert.AreEqual(3, i);
        }

        // 修改集合后再次枚举，确保枚举反映最新状态（add/remove/clear）
        [TestMethod]
        public void IEnumerable_Enumeration_ReflectsModifications()
        {
            var list = new MarcNodeList();
            list.add(new MarcField("100", "  "));
            list.add(new MarcField("200", "  "));
            list.add(new MarcField("300", "  "));

            var namesBefore = list.Select(n => n.Name).ToArray();
            CollectionAssert.AreEqual(new[] { "100", "200", "300" }, namesBefore);

            // 移除中间元素
            list.removeAt(1);
            var namesAfterRemove = list.Select(n => n.Name).ToArray();
            CollectionAssert.AreEqual(new[] { "100", "300" }, namesAfterRemove);

            // 清空并添加新元素
            list.clear();
            list.add(new MarcField("400", "  "));
            list.add(new MarcField("500", "  "));
            var namesAfterClearAdd = list.Select(n => n.Name).ToArray();
            CollectionAssert.AreEqual(new[] { "400", "500" }, namesAfterClearAdd);
        }
    }
}

