using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DigitalPlatform.Marc
{
    // MARC 子字段
    /// <summary>
    /// MARC 子字段节点
    /// </summary>
    public class MarcSubfield : MarcNode
    {
        /// <summary>
        /// 缺省的子字段名。当没有指定子字段名的时候，会自动用这个值来填充
        /// </summary>
        public static string DefaultFieldName
        {
            get
            {
                return new string(MarcQuery.DefaultChar, 1);
            }
        }

        #region 构造函数

        /// <summary>
        /// 初始化一个 MarcSubfield 对象
        /// </summary>
        public MarcSubfield()
        {
            this.Parent = null;
            this.NodeType = NodeType.Subfield;
            this.Name = DefaultFieldName;
        }


        // 使用一个字符串构造
        // parameters:
        //      strText 可以为 SUBFLED + "aAAA" 形态，也可以为 "aAAA"形态
        /// <summary>
        /// 初始化一个 MarcSubfield 对象，并根据指定的字符串设置好全部内容
        /// </summary>
        /// <param name="strText">表示一个完整的 MARC 子字段的 MARC 机内格式字符串。第一字符可以为 ASCII 31，也可以为子字段名字符</param>
        public MarcSubfield(string strText)
        {
            this.NodeType = NodeType.Subfield;

            string strName = "";
            string strContent = "";
            if (string.IsNullOrEmpty(strText) == false)
            {
                if (strText[0] == (char)31)
                {
                    if (strText.Length > 1)
                    {
                        strName = strText.Substring(1, 1);
                        strContent = strText.Substring(2);
                    }
                }
                else
                {
                    strName = strText.Substring(0, 1);
                    strContent = strText.Substring(1);
                }
            }

            if (String.IsNullOrEmpty(strName) == true)
                this.Name = DefaultFieldName;
            else
            {
                if (strName.Length != 1)
                    throw new Exception("Subfield的Name必须为1字符");
                if (strName[0] == (char)31)
                    throw new Exception("子字段名不允许包含 ASCII 31 字符");

                this.Name = strName;
            }
            if (strContent.IndexOf((char)31) != -1)
                throw new Exception("子字段内容字符串中不允许包含 ASCII 31 字符");

            this.Content = strContent;
        }

        // 使用两个字符串构造
        /// <summary>
        /// 初始化一个 MarcSubfield 对象，并根据指定的字符串设置好全部内容
        /// </summary>
        /// <param name="strName">子字段名。一个字符</param>
        /// <param name="strContent">子字段正文</param>
        public MarcSubfield(string strName,
            string strContent)
        {
            this.NodeType = NodeType.Subfield;

            if (String.IsNullOrEmpty(strName) == true)
                this.Name = DefaultFieldName;
            else
            {
                if (strName.Length != 1)
                    throw new Exception("Subfield的Name必须为1字符");
                if (strName[0] == (char)31)
                    throw new Exception("子字段名不允许包含 ASCII 31 字符");

                this.Name = strName;
            }
            if (strContent != null && strContent.IndexOf((char)31) != -1)
                throw new Exception("子字段内容字符串中不允许包含 ASCII 31 字符");

            this.Content = strContent;
        }

        #endregion

        // 至少2字符
        /// <summary>
        /// 当前节点的全部文字。表现了一个完整的 MARC 子字段
        /// </summary>
        public override string Text
        {
            get
            {
                return MarcQuery.SUBFLD + this.Name + this.Content;
            }
            set
            {
                if (string.IsNullOrEmpty(value) == true)
                    throw new Exception("子字段的 Text 不能设置为空");
                if (value.Length <= 1)
                    throw new Exception("子字段的 Text 不能设置为 1 字符的内容。至少要 2 字符，并且第一个字符必须为ASCII 31");
                if (value[0] != (char)31)
                    throw new Exception("子字段的 Text 第一个字符必须设置为ASCII 31");

                Debug.Assert(value.Length >= 2, "");
                this.Name = value.Substring(1, 1);
                this.Content = value.Substring(2);
            }
        }

        /// <summary>
        /// 子字段名。一字符
        /// </summary>
        public override string Name
        {
            get
            {
                return base.Name;
            }
            set
            {
                if (string.IsNullOrEmpty(value) == true
                    || value.Length != 1)
                    throw new ArgumentException("MarcSubfield 的 Name 属性只允许用 1 个字符来设置", "Name");

                base.Name = value;
            }
        }

        /// <summary>
        /// 子字段正文。即子字段名以后的全部内容
        /// </summary>
        public override string Content
        {
            get
            {
                return this.m_strContent;
            }
            set
            {
                this.ChildNodes.clearAndDetach();
                this.m_strContent = value;
            }
        }



        /// <summary>
        /// 输出当前对象的调试用字符串
        /// </summary>
        /// <returns>表示内容的字符串</returns>
        public override string dump()
        {
            Debug.Assert(string.IsNullOrEmpty(this.Indicator) == true, "");
            return "$" + this.Name + this.Content;
        }

        /// <summary>
        /// 创建一个新的 MarcSubfield 节点对象，从当前对象复制出全部内容
        /// </summary>
        /// <returns>新的节点对象</returns>
        public override MarcNode clone()
        {
            MarcNode new_node = new MarcSubfield();
            new_node.Text = this.Text;
            new_node.Parent = null; // 尚未和任何对象连接
            return new_node;
        }
    }

}
