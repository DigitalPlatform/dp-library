using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Xml.XPath;
using System.Text.RegularExpressions;

namespace DigitalPlatform.Marc
{
    /// <summary>
    /// MARC 基本节点
    /// </summary>
    public class MarcNode
    {
        /// <summary>
        /// 父节点
        /// </summary>
        public MarcNode Parent = null;

        /// <summary>
        /// 节点类型
        /// </summary>
        public NodeType NodeType = NodeType.None;

        /// <summary>
        /// 子节点集合
        /// </summary>
        public ChildNodeList ChildNodes = new ChildNodeList();

        #region 构造函数

        /// <summary>
        /// 初始化一个 MarcNode 对象
        /// </summary>
        public MarcNode()
        {
            this.Parent = null;
            this.ChildNodes.owner = this;
        }

        /// <summary>
        /// 初始化一个 MarcNode对象，并设置好其 Parent 成员
        /// </summary>
        /// <param name="parent">上级 MarcNode 对象</param>
        public MarcNode(MarcNode parent)
        {
            this.Parent = parent;
            this.ChildNodes.owner = this;
        }

        #endregion

        // Name
        internal string m_strName = "";
        /// <summary>
        /// 节点的名字
        /// </summary>
        public virtual string Name
        {
            get
            {
                return this.m_strName;
            }
            set
            {
                this.m_strName = value;
            }
        }

        // Indicator
        internal string m_strIndicator = "";
        /// <summary>
        /// 节点的指示符
        /// </summary>
        public virtual string Indicator
        {
            get
            {
                return this.m_strIndicator;
            }
            set
            {
                this.m_strIndicator = value;
            }
        }

        /// <summary>
        /// 指示符的第一个字符
        /// </summary>
        public virtual char Indicator1
        {
            get
            {
                if (string.IsNullOrEmpty(m_strIndicator) == true)
                    return (char)0;
                return this.m_strIndicator[0];
            }
            set
            {
                // 没有动作。需要派生类实现
            }
        }

        /// <summary>
        /// 指示符的第二个字符
        /// </summary>
        public virtual char Indicator2
        {
            get
            {
                if (string.IsNullOrEmpty(m_strIndicator) == true)
                    return (char)0;
                if (m_strIndicator.Length < 2)
                    return (char)0;
                return this.m_strIndicator[1];
            }
            set
            {
                // 没有动作。需要派生类实现
            }
        }

        // Content
        // 这个是缺省的实现方式，可以直接用于没有下级的纯内容节点
        internal string m_strContent = "";
        /// <summary>
        /// 节点的正文内容
        /// </summary>
        public virtual string Content
        {
            get
            {
                return this.m_strContent;
            }
            set
            {
                this.m_strContent = value;
            }
        }

        // Text 用于构造MARC机内格式字符串的表示当前节点部分的字符串
        //
        /// <summary>
        /// 节点的全部文字，MARC 机内格式表现形态
        /// </summary>
        public virtual string Text
        {
            get
            {
                return this.Name + this.Indicator + this.Content;
            }
            set
            {
                this.Content = value;   // 这是个草率的实现，需要具体节点重载本函数
            }
        }

        /// <summary>
        /// 创建一个新的节点对象，从当前对象复制出全部内容
        /// </summary>
        /// <returns>新的节点对象</returns>
        public virtual MarcNode clone()
        {
            throw new Exception("not implemented");
        }


        // 看一个字段名是否是控制字段。所谓控制字段没有指示符概念
        // parameters:
        //		strFieldName	字段名
        // return:
        //		true	是控制字段
        //		false	不是控制字段
        /// <summary>
        /// 检测一个字段名是否为控制字段(的字段名)
        /// </summary>
        /// <param name="strFieldName">要检测的字段名</param>
        /// <returns>true表示是控制字段，false表示不是控制字段</returns>
        public static bool isControlFieldName(string strFieldName)
        {
            if (String.Compare(strFieldName, "hdr", true) == 0)
                return true;

            if (String.Compare(strFieldName, "###", true) == 0)
                return true;

            if (
                (
                String.Compare(strFieldName, "001") >= 0
                && String.Compare(strFieldName, "009") <= 0
                )

                || String.Compare(strFieldName, "-01") == 0
                )
                return true;

            return false;
        }

        /// <summary>
        /// 输出当前对象的全部子对象的调试用字符串
        /// </summary>
        /// <returns>表示内容的字符串</returns>
        public virtual string dumpChildren()
        {
            StringBuilder strResult = new StringBuilder(4096);
            for (int i = 0; i < this.ChildNodes.count; i++)
            {
                MarcNode child = this.ChildNodes[i];
                strResult.Append(child.dump());
            }

            return strResult.ToString();
        }

        /// <summary>
        /// 输出当前对象的调试用字符串
        /// </summary>
        /// <returns>表示内容的字符串</returns>
        public virtual string dump()
        {
            // 一般实现
            return this.Name + this.Indicator
                + dumpChildren();
        }

        /// <summary>
        /// 获得根节点
        /// </summary>
        /// <returns>根节点</returns>
        public MarcNode getRootNode()
        {
            MarcNode node = this;
            while (node.Parent != null)
                node = node.Parent;

            Debug.Assert(node.Parent == null, "");
            return node;
        }

        // 根
        /// <summary>
        /// 根节点
        /// </summary>
        public MarcNode Root
        {
            get
            {
                MarcNode node = this;
                while (node.Parent != null)
                {
                    node = node.Parent;
                }

#if DEBUG
                if (node != this)
                {
                    Debug.Assert(node.NodeType == Marc.NodeType.Record || node.NodeType == Marc.NodeType.None, "");
                }
#endif
                return node;
            }
        }

        // 
        /// <summary>
        /// 获得表示当前对象的位置的路径。用于比较节点之间的位置关系
        /// </summary>
        /// <returns>路径字符串</returns>
        public string getPath()
        {
            MarcNode parent = this.Parent;
            if (parent == null)
                return "0";
            int index = parent.ChildNodes.indexOf(this);
            if (index == -1)
                throw new Exception("在父节点的 ChildNodes 中没有找到自己");

            string strParentPath = this.Parent.getPath();
            return strParentPath + "/" + index.ToString();
        }

        // 内容是否为空?
        /// <summary>
        /// 检测节点内容是否为空
        /// </summary>
        public bool isEmpty
        {
            get
            {
                if (string.IsNullOrEmpty(this.Content) == true)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// 将当前节点从父节点摘除。但依然保留对当前节点对下级的拥有关系
        /// </summary>
        /// <returns>已经被摘除的当前节点</returns>
        public MarcNode detach()
        {
            MarcNode parent = this.Parent;
            if (parent == null)
                return this; // 自己是根节点，或者先前已经被摘除
            int index = parent.ChildNodes.indexOf(this);
            if (index == -1)
            {
                throw new Exception("parent的ChildNodes中居然没有找到自己");
                return this;
            }

            parent.ChildNodes.removeAt(index);
            this.Parent = null;
            return this;
        }

        /// <summary>
        /// 将指定节点插入到当前节点的前面兄弟位置
        /// </summary>
        /// <param name="source">要插入的节点</param>
        /// <returns>当前节点</returns>
        public MarcNode before(MarcNode source)
        {
            MarcNode parent = this.Parent;
            // 自己是根节点，无法具有兄弟
            if (parent == null)
                throw new Exception("无法在根节点同级插入新节点");

            int index = parent.ChildNodes.indexOf(this);
            if (index == -1)
            {
                throw new Exception("parent的ChildNodes中居然没有找到自己");
            }

            // 进行类型检查，同级只能插入相同类型的元素
            if (this.NodeType != source.NodeType)
                throw new Exception("无法在节点同级插入不同类型的新节点。this.NodeTYpe=" + this.NodeType.ToString() + ", source.NodeType=" + source.NodeType.ToString());

            source.detach();
            parent.ChildNodes.insert(index, source);
            source.Parent = this.Parent;
            return this;
        }

        // 把source插入到this的后面。返回this
        /// <summary>
        /// 将指定节点插入到当前节点的后面兄弟位置
        /// </summary>
        /// <param name="source">要插入的节点</param>
        /// <returns>当前节点</returns>
        public MarcNode after(MarcNode source)
        {
            MarcNode parent = this.Parent;
            // 自己是根节点，无法具有兄弟
            if (parent == null)
                throw new Exception("无法在根节点同级插入新节点");

            int index = parent.ChildNodes.indexOf(this);
            if (index == -1)
            {
                throw new Exception("parent的ChildNodes中居然没有找到自己");
            }

            // 进行类型检查，同级只能插入相同类型的元素
            if (this.NodeType != source.NodeType)
                throw new Exception("无法在节点同级插入不同类型的新节点。this.NodeTYpe=" + this.NodeType.ToString() + ", source.NodeType=" + source.NodeType.ToString());

            source.detach();
            parent.ChildNodes.insert(index + 1, source);
            source.Parent = this.Parent;
            return this;
        }

        // 把strText构造的新对象插入到this的后面。返回this
        /// <summary>
        /// 用指定的字符串构造出新的节点，插入到当前节点的后面兄弟位置
        /// </summary>
        /// <param name="strText">用于构造新节点的字符串</param>
        /// <returns>当前节点</returns>
        public MarcNode after(string strText)
        {
            MarcNodeList targets = new MarcNodeList(this);

            targets.after(strText);
            return this;
        }

        // 把source插入到this的下级末尾位置。返回this
        /// <summary>
        /// 将指定节点追加到当前节点的子节点尾部
        /// </summary>
        /// <param name="source">要追加的节点</param>
        /// <returns>当前节点</returns>
        public MarcNode append(MarcNode source)
        {
            source.detach();
            this.ChildNodes.add(source);
            source.Parent = this;
            return this;
        }

        // 把strText构造的新对象插入到this的下级末尾位置。返回this
        /// <summary>
        /// 用指定的字符串构造出新的节点，追加到当前节点的子节点末尾
        /// </summary>
        /// <param name="strText">用于构造新节点的字符串</param>
        /// <returns>当前节点</returns>
        public MarcNode append(string strText)
        {
            MarcNodeList targets = new MarcNodeList(this);

            targets.append(strText);
            return this;
        }

        // this 插入到 target 儿子的末尾
        /// <summary>
        /// 将当前节点追加到指定(目标)节点的子节点末尾
        /// </summary>
        /// <param name="target">目标节点</param>
        /// <returns>当前节点</returns>
        public MarcNode appendTo(MarcNode target)
        {
            this.detach();
            target.ChildNodes.add(this);
            this.Parent = target;
            return this;
        }

        // 把source插入到this的下级开头位置。返回this
        /// <summary>
        /// 将指定的(源)节点插入到当前节点的子节点开头位置
        /// </summary>
        /// <param name="source">源节点</param>
        /// <returns>当前节点</returns>
        public MarcNode prepend(MarcNode source)
        {
            source.detach();
            this.ChildNodes.insert(0, source);
            source.Parent = this;
            return this;
        }

        // 把strText构造的新对象插入到this的下级开头位置。返回this
        /// <summary>
        /// 用指定的字符串构造出新节点，插入到当前节点的子节点开头
        /// </summary>
        /// <param name="strText">用于构造新节点的字符串</param>
        /// <returns>当前节点</returns>
        public MarcNode prepend(string strText)
        {
            MarcNodeList targets = new MarcNodeList(this);

            targets.prepend(strText);
            return this;
        }

        // this 插入到 target 的儿子的第一个
        /// <summary>
        /// 将当前节点插入到指定的(目标)节点的子节点的开头
        /// </summary>
        /// <param name="target">目标节点</param>
        /// <returns>当前节点</returns>
        public MarcNode prependTo(MarcNode target)
        {
            this.detach();
            target.ChildNodes.insert(0, this);
            this.Parent = target;
            return this;
        }

#if NO
        public virtual MarcNavigator CreateNavigator()
        {
            return new MarcNavigator(this);
        }
#endif
        /// <summary>
        /// 用 XPath 字符串选择节点
        /// </summary>
        /// <param name="strXPath">XPath 字符串</param>
        /// <returns>被选中的节点集合</returns>
        public MarcNodeList select(string strXPath)
        {
            return select(strXPath, -1);
        }

        // 针对DOM树进行 XPath 筛选
        // parameters:
        //      nMaxCount    至多选择开头这么多个元素。-1表示不限制
        /// <summary>
        /// 用 XPath 字符串选择节点
        /// </summary>
        /// <param name="strXPath">XPath 字符串</param>
        /// <param name="nMaxCount">限制命中的最多节点数。-1表示不限制</param>
        /// <returns>被选中的节点集合</returns>
        public MarcNodeList select(string strXPath, int nMaxCount/* = -1*/)
        {
            MarcNodeList results = new MarcNodeList();

            MarcNavigator nav = new MarcNavigator(this);  // 出发点在当前对象

            XPathNodeIterator ni = nav.Select(strXPath);
            while (ni.MoveNext() && (nMaxCount == -1 || results.count < nMaxCount))
            {
                NaviItem item = ((MarcNavigator)ni.Current).Item;
                if (item.Type != NaviItemType.Element)
                {
                    // if (bSkipNoneElement == false)
                    throw new Exception("xpath '" + strXPath + "' 命中了非元素类型的节点，这是不允许的");
                    continue;
                }
                results.add(item.MarcNode);
            }
            return results;
        }

        /*
        public MarcNode SelectSingleNode(string strXpath)
        {
            MarcNavigator nav = new MarcNavigator(this);
            XPathNodeIterator ni = nav.Select(strXpath);
            ni.MoveNext();
            return ((MarcNavigator)ni.Current).Item.MarcNode;
        }
         * */

#if NO
        public MarcNodeList SelectNodes(string strPath)
        {
            string strFirstPart = GetFirstPart(ref strPath);

            if (strFirstPart == "/")
            {
                /*
                if (this.Parent == null)
                    return this.SelectNodes(strPath);
                 * */

                return GetRootNode().SelectNodes(strPath);
            }

            if (strFirstPart == "..")
            {
                return this.Parent.SelectNodes(strPath);
            }

            if (strFirstPart == ".")
            {
                return this.SelectNodes(strPath);
            }

            // tagname[@attrname='']
            string strTagName = "";
            string strCondition = "";

            int nRet = strFirstPart.IndexOf("[");
            if (nRet == -1)
                strTagName = strFirstPart;
            else
            {
                strCondition = strFirstPart.Substring(nRet + 1);
                if (strCondition.Length > 0)
                {
                    // 去掉末尾的']'
                    if (strCondition[strCondition.Length - 1] == ']')
                        strCondition.Substring(0, strCondition.Length - 1);
                }
                strTagName = strFirstPart.Substring(0, nRet);
            }

            MarcNodeList results = new MarcNodeList(null);

            for (int i = 0; i < this.ChildNodes.Count; i++)
            {
                MarcNode node = this.ChildNodes[i];
                Debug.Assert(node.Parent != null, "");
                if (strTagName == "*" || node.Name == strTagName)
                {
                    if (results.Parent == null)
                        results.Parent = node.Parent;
                    results.Add(node);
                }
            }


            if (String.IsNullOrEmpty(strPath) == true)
            {
                // 到了path的末级。用strFirstPart筛选对象
                return results;
            }

            return results.SelectNodes(strPath);
        }

                // 获得路径的第一部分
        static string GetFirstPart(ref string strPath)
        {
            if (String.IsNullOrEmpty(strPath) == true)
                return "";

            if (strPath[0] == '/')
            {
                strPath = strPath.Substring(1);
                return "/";
            }

            string strResult = "";
            int nRet = strPath.IndexOf("/");
            if (nRet == -1)
            {
                strResult = strPath;
                strPath = "";
                return strResult;
            }

            strResult = strPath.Substring(0, nRet);
            strPath = strPath.Substring(nRet + 1);
            return strResult;
        }
#endif

        // 删除自己
        // 但是this.Parent指针还是没有清除
        /// <summary>
        /// 从父节点(的子节点集合中)将当前节点移走。注意，本操作并不修改当前节点的 Parent 成员，也就是说 Parent 成员依然指向父节点
        /// </summary>
        /// <returns>当前节点</returns>
        public MarcNode remove()
        {
            if (this.Parent != null)
            {
                this.Parent.ChildNodes.remove(this);
                return this;
            }

            return null;    // biaoshi zhaobudao , yejiu wucong shanchu
        }

        #region 访问各种位置

        /// <summary>
        /// 当前节点的第一个子节点
        /// </summary>
        public MarcNode FirstChild
        {
            get
            {
                if (this.ChildNodes.count == 0)
                    return null;
                return this.ChildNodes[0];
            }
        }

        /// <summary>
        /// 当前节点的最后一个子节点
        /// </summary>
        public MarcNode LastChild
        {
            get
            {
                if (this.ChildNodes.count == 0)
                    return null;
                return this.ChildNodes[this.ChildNodes.count - 1];
            }
        }

        /// <summary>
        /// 当前节点的前一个兄弟节点
        /// </summary>
        public MarcNode PrevSibling
        {
            get
            {
                MarcNode parent = this.Parent;
                // 自己是根节点，无法具有兄弟
                if (parent == null)
                    return null;

                int index = parent.ChildNodes.indexOf(this);
                if (index == -1)
                {
                    throw new Exception("parent的ChildNodes中居然没有找到自己");
                }

                if (index == 0)
                    return null;

                return parent.ChildNodes[index - 1];
            }
        }

        /// <summary>
        /// 当前节点的下一个兄弟节点
        /// </summary>
        public MarcNode NextSibling
        {
            get
            {
                MarcNode parent = this.Parent;
                // 自己是根节点，无法具有兄弟
                if (parent == null)
                    return null;

                int index = parent.ChildNodes.indexOf(this);
                if (index == -1)
                {
                    throw new Exception("parent的ChildNodes中居然没有找到自己");
                }

                if (index >= parent.ChildNodes.count - 1)
                    return null;

                return parent.ChildNodes[index + 1];
            }
        }

        #endregion
    }





    /// <summary>
    /// 专用于存储下级节点的集合类
    /// <para></para>
    /// </summary>
    /// <remarks>
    /// 继承 MarcNodeList 类而来，完善了 add() 方法，能自动把每个元素的 Parent 成员设置好
    /// </remarks>
    public class ChildNodeList : MarcNodeList
    {
        internal MarcNode owner = null;

        // 追加
        // 对node先要摘除
        /// <summary>
        /// 在当前集合末尾追加一个节点元素
        /// </summary>
        /// <param name="node">要追加的节点</param>
        public new void add(MarcNode node)
        {
            node.detach();
            base.add(node);

            Debug.Assert(owner != null, "");
            node.Parent = owner;
        }

        // 检查加入，不去摘除 node 原来的关系，也不自动修改 node.Parent
        internal void baseAdd(MarcNode node)
        {
            base.add(node);
        }

        // 追加
        /// <summary>
        /// 在当前集合末尾追加若干节点元素
        /// </summary>
        /// <param name="list">要追加的若干节点元素</param>
        public new void add(MarcNodeList list)
        {
            base.add(list);
            Debug.Assert(owner != null, "");
            foreach (MarcNode node in list)
            {
                node.Parent = owner;
            }
        }

        /// <summary>
        /// 向当前集合中添加一个节点元素，按节点名字顺序决定加入的位置
        /// </summary>
        /// <param name="node">要加入的节点</param>
        /// <param name="style">如何加入</param>
        /// <param name="comparer">用于比较大小的接口</param>
        public override void insertSequence(MarcNode node,
    InsertSequenceStyle style = InsertSequenceStyle.PreferHead,
    IComparer<MarcNode> comparer = null)
        {
            base.insertSequence(node, style, comparer);
            node.Parent = owner;
        }

        /// <summary>
        /// 清除当前集合，并把集合中的元素全部摘除
        /// </summary>
        public new void clear()
        {
            clearAndDetach();
        }

        // 清除集合，并把原先的每个元素的Parent清空。
        // 主要用于ChildNodes摘除关系
        internal void clearAndDetach()
        {
            foreach (MarcNode node in this)
            {
                node.Parent = null;
            }
            base.clear();
        }
    }

    /// <summary>
    /// 节点类型
    /// </summary>
    public enum NodeType
    {
        /// <summary>
        /// 尚未确定
        /// </summary>
        None = 0,
        /// <summary>
        /// 记录
        /// </summary>
        Record = 1,
        /// <summary>
        /// 字段
        /// </summary>
        Field = 2,
        /// <summary>
        /// 子字段
        /// </summary>
        Subfield = 3,
    }

    /// <summary>
    /// dump()方法的操作风格
    /// </summary>
    [Flags]
    public enum DumpStyle
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,
        /// <summary>
        /// 包含行号
        /// </summary>
        LineNumber = 0x01,
    }


}
