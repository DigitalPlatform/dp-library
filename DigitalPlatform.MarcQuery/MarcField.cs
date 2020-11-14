using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DigitalPlatform.Marc
{
    /// <summary>
    /// MARC 外围字段节点
    /// </summary>
    public class MarcOuterField : MarcField
    {
        #region 构造函数

        /// <summary>
        /// 初始化一个 MarcOuterField 对象
        /// </summary>
        public MarcOuterField()
        {
            this.Parent = null;
            this.NodeType = NodeType.Field;
            Debug.Assert(this.ChildNodes.owner == this, "");

            this.Text = DefaultFieldName;
        }

        // 使用一个字符串构造
        /// <summary>
        /// 初始化一个 MarcOuterField 对象，并根据指定的字符串设置好全部内容
        /// </summary>
        /// <param name="strText">表示一个完整的 MARC 字段的 MARC 机内格式字符串</param>
        public MarcOuterField(string strText)
        {
            this.Parent = null;
            this.NodeType = NodeType.Field;
            Debug.Assert(this.ChildNodes.owner == this, "");

            this.Text = strText;
        }

        // 使用两个或者三个字符串构造
        // 创建001等控制字段的时候，可以只使用前面两个参数，这时候第二参数表示内容部分
        // 如果在创建001等控制字段的时候，一共使用了三个参数，则 strIndicator + strContent 一起当作字段内容
        /// <summary>
        /// 初始化一个 MarcOuterField 对象，并根据指定的字符串设置好全部内容
        /// </summary>
        /// <param name="strName">字段名。3字符的字符串</param>
        /// <param name="strIndicator">字段指示符。为2字符的字符串，或者空字符串</param>
        /// <param name="strContent">字段正文</param>
        public MarcOuterField(string strName,
            string strIndicator,
            string strContent = "")
        {
            this.NodeType = NodeType.Field;

            if (String.IsNullOrEmpty(strName) == true)
                this.m_strName = DefaultFieldName;
            else
            {
                if (strName.Length != 3)
                    throw new Exception("Field的Name必须为3字符");
                this.m_strName = strName;
            }

            if (isControlFieldName(strName) == true)
            {
                // strIndicator 和 strContent 连接在一起当作内容
                // 因为这个缘故，可以省略第三个参数
                this.Content = strIndicator + strContent;
                return;
            }
            this.m_strIndicator = strIndicator;
            this.Content = strContent;
        }

        // parameters:
        //      subfields   字符串数组，指定了一系列要创建的子字段文字。每个字符串型态需要这样 “axxx” 表示子字段名为a，内容为xxx。注意，不需要包含子字段符号 SUBFLD
        /// <summary>
        /// 初始化一个 MarcOuterField 对象，并根据指定的字符串设置好全部内容和下级对象
        /// </summary>
        /// <param name="strName">字段名。3字符的字符串</param>
        /// <param name="strIndicator">字段指示符。为2字符的字符串，或者空字符串</param>
        /// <param name="fields">表示若干内嵌字段的字符串数组。每个字符串开始就是字段名</param>
        public MarcOuterField(string strName,
            string strIndicator,
            List<string> fields)
        {
            string[] temp = new string[fields.Count];
            fields.CopyTo(temp);

            newMarcField(strName,
                strIndicator,
                temp);
        }

        // 另一字符串数组版本
        /// <summary>
        /// 初始化一个 MarcOuterField 对象，并根据指定的字符串设置好全部内容和下级对象
        /// </summary>
        /// <param name="strName">字段名。3字符的字符串</param>
        /// <param name="strIndicator">字段指示符。为2字符的字符串，或者空字符串</param>
        /// <param name="fields">表示若干内嵌字段的字符串数组。每个字符串开始就是字段名</param>
        public MarcOuterField(string strName,
            string strIndicator,
            string[] fields)
        {
            newMarcField(strName, strIndicator, fields);
        }

        void newMarcField(string strName,
            string strIndicator,
            string[] fields)
        {
            this.NodeType = NodeType.Field;

            if (String.IsNullOrEmpty(strName) == true)
                this.m_strName = DefaultFieldName;
            else
            {
                if (strName.Length != 3)
                    throw new Exception("Field的Name必须为3字符");
                this.m_strName = strName;
            }

            if (isControlFieldName(strName) == true)
            {
                throw new ArgumentException("外围字段不能使用控制字段名", "strName");
            }

            this.m_strIndicator = strIndicator;

            StringBuilder content = new StringBuilder(4096);
            foreach (string s in fields)
            {
                content.Append(MarcQuery.SUBFLD);
                content.Append("1");
                content.Append(s);
            }

            this.Content = content.ToString();
        }

        // 使用一个字符串构造，指定了后面参数中所使用的(代用)子字段符号
        /// <summary>
        /// 初始化一个 MarcOuterField 对象，并根据指定的字符串设置好全部内容
        /// </summary>
        /// <param name="chSubfield">子字段符号的代用字符。在 strText 参数中可以用这个代用字符来表示子字段符号 (ASCII 31)</param>
        /// <param name="strText">表示一个完整的 MARC 字段的 MARC 机内格式字符串</param>
        public MarcOuterField(char chSubfield, string strText)
        {
            this.Parent = null;
            this.NodeType = NodeType.Field;
            Debug.Assert(this.ChildNodes.owner == this, "");

            this.Text = strText.Replace(chSubfield, MarcQuery.SUBFLD[0]);
        }

        #endregion

        /// <summary>
        /// 当前节点的全部文字。表现了一个完整的 MARC 外围字段
        /// </summary>
        public override string Text
        {
            get
            {
#if NO
                // 头标区
                if (this.IsHeader == true)
                {
                    Debug.Assert(string.IsNullOrEmpty(this.Name) == true, "");
                    Debug.Assert(string.IsNullOrEmpty(this.Indicator) == true, "");
                    return this.Content;
                }
#endif

                return this.Name + this.Indicator + this.Content + MarcQuery.FLDEND;
            }
            set
            {
                setFieldText(value);
            }
        }

        // 将机内格式的字符串设置到字段
        // 最后一个字符可以是 30 (表示字段结束)，也可以没有这个字符
        void setFieldText(string strText)
        {
            // 去掉末尾的 30 字符
            if (strText != null && strText.Length >= 1)
            {
                if (strText[strText.Length - 1] == (char)30)
                    strText = strText.Substring(0, strText.Length - 1);
            }

            if (string.IsNullOrEmpty(strText) == true)
                throw new Exception("字段 Text 不能设置为空");

            if (strText.Length < 3)
                throw new Exception("字段 Text 不能设置为小于 3 字符");

            string strFieldName = strText.Substring(0, 3);
            strText = strText.Substring(3); // 剩余部分

            this.m_strName = strFieldName;
            if (MarcNode.isControlFieldName(strFieldName) == true)
            {
                throw new Exception("MARC 外围字段的字段名不能使用控制字段名 '" + strFieldName + "'");
            }
            else
            {
                // 普通字段

                // 剩下的内容为空
                if (string.IsNullOrEmpty(strText) == true)
                {
                    this.Indicator = DefaultIndicator;
                    return;
                }

                // 还剩下一个字符
                if (strText.Length < 2)
                {
                    Debug.Assert(strText.Length == 1, "");
                    this.Indicator = strText + new string(MarcQuery.DefaultChar, 1);
                    return;
                }

                // 剩下两个字符以上
                this.m_strIndicator = strText.Substring(0, 2);
                this.Content = strText.Substring(2);
            }
        }

        /// <summary>
        /// 字段正文。即字段指示符以后的全部内容
        /// </summary>
        public override string Content
        {
            get
            {
                StringBuilder result = new StringBuilder(4096);
                result.Append(this.m_strContent);   // 第一个子字段符号以前的内容
                // 合成下级元素
                for (int i = 0; i < this.ChildNodes.count; i++)
                {
                    MarcNode node = this.ChildNodes[i];
                    result.Append(node.Text);
                    // strResult += new string((char)31, 1) + node.Name + node.Content;
                }
                return result.ToString();
            }
            set
            {
                // 拆分为子字段
                this.ChildNodes.clearAndDetach();
                this.m_strContent = "";

                string strLeadingString = "";
                MarcNodeList inner_fields = MarcQuery.createInnerFields(
                    value, out strLeadingString);
                this.ChildNodes.add(inner_fields);
                this.m_strContent = strLeadingString;
            }
        }

    }

    /// <summary>
    /// MARC 内嵌字段节点
    /// </summary>
    public class MarcInnerField : MarcField
    {
        #region 构造函数

        /// <summary>
        /// 初始化一个 MarcInnerField 对象
        /// </summary>
        public MarcInnerField() : base()
        {
        }

        // 使用一个字符串构造
        /// <summary>
        /// 初始化一个 MarcInnerField 对象，并根据指定的字符串设置好全部内容
        /// </summary>
        /// <param name="strText">表示一个完整的 MARC 字段的 MARC 机内格式字符串</param>
        public MarcInnerField(string strText) : base(strText)
        {
        }

        /// <summary>
        /// 初始化一个 MarcInnerField 对象，并根据指定的字符串设置好全部内容
        /// </summary>
        /// <param name="strName">字段名。3字符的字符串</param>
        /// <param name="strIndicator">字段指示符。为2字符的字符串，或者空字符串</param>
        /// <param name="strContent">字段正文</param>
        public MarcInnerField(string strName,
            string strIndicator,
            string strContent = "") : base(strName, strIndicator, strContent)
        {
        }

        // parameters:
        //      subfields   字符串数组，指定了一系列要创建的子字段文字。每个字符串型态需要这样 “axxx” 表示子字段名为a，内容为xxx。注意，不需要包含子字段符号 SUBFLD
        /// <summary>
        /// 初始化一个 MarcInnerField 对象，并根据指定的字符串设置好全部内容和下级对象
        /// </summary>
        /// <param name="strName">字段名。3字符的字符串</param>
        /// <param name="strIndicator">字段指示符。为2字符的字符串，或者空字符串</param>
        /// <param name="subfields">表示若干子字段的字符串数组。每个字符串的第一字符为子字段名，其余为子字段内容</param>
        public MarcInnerField(string strName,
            string strIndicator,
            List<string> subfields) : base(strName, strIndicator, subfields)
        {
        }

        // 另一字符串数组版本
        /// <summary>
        /// 初始化一个 MarcInnerField 对象，并根据指定的字符串设置好全部内容和下级对象
        /// </summary>
        /// <param name="strName">字段名。3字符的字符串</param>
        /// <param name="strIndicator">字段指示符。为2字符的字符串，或者空字符串</param>
        /// <param name="subfields">表示若干子字段的字符串数组。每个字符串的第一字符为子字段名，其余为子字段内容</param>
        public MarcInnerField(string strName,
            string strIndicator,
            string[] subfields) : base(strName, strIndicator, subfields)
        {
        }

        // 使用一个字符串构造，指定了后面参数中所使用的(代用)子字段符号
        /// <summary>
        /// 初始化一个 MarcInnerField 对象，并根据指定的字符串设置好全部内容
        /// </summary>
        /// <param name="chSubfield">子字段符号的代用字符。在 strText 参数中可以用这个代用字符来表示子字段符号 (ASCII 31)</param>
        /// <param name="strText">表示一个完整的 MARC 字段的 MARC 机内格式字符串</param>
        public MarcInnerField(char chSubfield, string strText) : base(chSubfield, strText)
        {
        }

        #endregion


        /// <summary>
        /// 当前节点的全部文字。表现了一个完整的 MARC 内嵌字段
        /// </summary>
        public override string Text
        {
            get
            {
#if NO
                // 头标区
                if (this.IsHeader == true)
                {
                    Debug.Assert(string.IsNullOrEmpty(this.Name) == true, "");
                    Debug.Assert(string.IsNullOrEmpty(this.Indicator) == true, "");
                    return this.Content;
                }
#endif

                // 普通字段
                return MarcQuery.SUBFLD + "1" + this.Name + this.Indicator + this.Content;
            }
            set
            {
#if NO
                if (this.IsHeader == true)
                {
                    if (string.IsNullOrEmpty(value) == true)
                        throw new Exception("头标区内容不能设置为空");
                    if (value.Length != 24)
                        throw new Exception("头标区内容只能设置为24字符");

                    this.Content = value;
                    return;
                }
#endif

                setFieldText(value);
            }
        }

        // 将机内格式的字符串设置到字段
        // $1200  $axxx$bxxx
        void setFieldText(string strText)
        {
            if (string.IsNullOrEmpty(strText) == true)
                throw new Exception("内嵌字段 Text 不能设置为空");
            if (strText[0] == MarcQuery.SUBFLD[0])
            {
                if (strText.Length < 5)
                    throw new Exception("内嵌字段 Text 不能设置为小于 5 字符");
                if (strText[0] != MarcQuery.SUBFLD[0])
                    throw new Exception("内嵌字段 Text 第一字符必须是子字段符号");
                if (strText[1] != '1')
                    throw new Exception("内嵌字段 Text 第二字符必须为 '1'");

                // 去掉头部的 $1  字符
                strText = strText.Substring(2);
                if (strText.Length < 3)
                    throw new Exception("字段 Text 剥离前二字符后，不能设置为小于 3 字符");
            }
            else
            {
                if (strText.Length < 3)
                    throw new Exception("内嵌字段 Text 不能设置为小于 3 字符");
            }

            string strFieldName = strText.Substring(0, 3);
            strText = strText.Substring(3); // 剩余部分

            this.m_strName = strFieldName;
            if (MarcNode.isControlFieldName(strFieldName) == true)
            {
                // 控制字段
                this.m_strIndicator = "";

                // 剩下的内容为空
                if (string.IsNullOrEmpty(strText) == true)
                {
                    this.Content = "";
                    return;
                }

                this.m_strContent = strText;    // 不要用this.Content，因为会惊扰到重新创建下级的机制
            }
            else
            {
                // 普通字段

                // 剩下的内容为空
                if (string.IsNullOrEmpty(strText) == true)
                {
                    this.Indicator = DefaultIndicator;
                    return;
                }

                // 还剩下一个字符
                if (strText.Length < 2)
                {
                    Debug.Assert(strText.Length == 1, "");
                    this.Indicator = strText + new string(MarcQuery.DefaultChar, 1);
                    return;
                }

                // 剩下两个字符以上
                this.m_strIndicator = strText.Substring(0, 2);
                this.Content = strText.Substring(2);
            }
        }

        /// <summary>
        /// 输出当前对象的调试用字符串
        /// </summary>
        /// <returns>表示内容的字符串</returns>
        public override string dump()
        {
            return "$1" + this.Name + this.Indicator + this.m_strContent
                + dumpChildren();
        }
    }

    // MARC 字段
    /// <summary>
    /// MARC 字段节点
    /// </summary>
    public class MarcField : MarcNode
    {
        // 缺省的字段名
        /// <summary>
        /// 缺省的字段名。当没有指定字段名的时候，会自动用这个值来填充
        /// </summary>
        public static string DefaultFieldName
        {
            get
            {
                return new string(MarcQuery.DefaultChar, 3);
            }
        }

        /// <summary>
        /// 缺省的字段指示符值。当没有明确指定指示符值的时候，会自动用这个值来填充
        /// </summary>
        public static string DefaultIndicator
        {
            get
            {
                return new string(MarcQuery.DefaultChar, 2);
            }
        }

        #region 构造函数

        /// <summary>
        /// 初始化一个 MarcField 对象
        /// </summary>
        public MarcField()
        {
            this.Parent = null;
            this.NodeType = NodeType.Field;
            Debug.Assert(this.ChildNodes.owner == this, "");

            this.Text = DefaultFieldName;
        }

        // 使用一个字符串构造
        /// <summary>
        /// 初始化一个 MarcField 对象，并根据指定的字符串设置好全部内容
        /// </summary>
        /// <param name="strText">表示一个完整的 MARC 字段的 MARC 机内格式字符串</param>
        public MarcField(string strText)
        {
            this.Parent = null;
            this.NodeType = NodeType.Field;
            Debug.Assert(this.ChildNodes.owner == this, "");

            this.Text = strText;
        }

        // 使用两个或者三个字符串构造
        // 创建001等控制字段的时候，可以只使用前面两个参数，这时候第二参数表示内容部分
        // 如果在创建001等控制字段的时候，一共使用了三个参数，则 strIndicator + strContent 一起当作字段内容
        /// <summary>
        /// 初始化一个 MarcField 对象，并根据指定的字符串设置好全部内容
        /// </summary>
        /// <param name="strName">字段名。3字符的字符串</param>
        /// <param name="strIndicator">字段指示符。为2字符的字符串，或者空字符串</param>
        /// <param name="strContent">字段正文</param>
        public MarcField(string strName,
            string strIndicator,
            string strContent = "")
        {
            this.NodeType = NodeType.Field;

            if (String.IsNullOrEmpty(strName) == true)
                this.m_strName = DefaultFieldName;
            else
            {
                if (strName.Length != 3)
                    throw new Exception("Field的Name必须为3字符");
                this.m_strName = strName;
            }

            if (isControlFieldName(strName) == true)
            {
                // strIndicator 和 strContent 连接在一起当作内容
                // 因为这个缘故，可以省略第三个参数
                this.Content = strIndicator + strContent;
                return;
            }
            this.m_strIndicator = strIndicator;
            this.Content = strContent;
        }

        // parameters:
        //      subfields   字符串数组，指定了一系列要创建的子字段文字。每个字符串型态需要这样 “axxx” 表示子字段名为a，内容为xxx。注意，不需要包含子字段符号 SUBFLD
        /// <summary>
        /// 初始化一个 MarcField 对象，并根据指定的字符串设置好全部内容和下级对象
        /// </summary>
        /// <param name="strName">字段名。3字符的字符串</param>
        /// <param name="strIndicator">字段指示符。为2字符的字符串，或者空字符串</param>
        /// <param name="subfields">表示若干子字段的字符串数组。每个字符串的第一字符为子字段名，其余为子字段内容</param>
        public MarcField(string strName,
            string strIndicator,
            List<string> subfields)
        {
            string[] temp = new string[subfields.Count];
            subfields.CopyTo(temp);

            newMarcField(strName,
                strIndicator,
                temp);
        }

        // 另一字符串数组版本
        /// <summary>
        /// 初始化一个 MarcField 对象，并根据指定的字符串设置好全部内容和下级对象
        /// </summary>
        /// <param name="strName">字段名。3字符的字符串</param>
        /// <param name="strIndicator">字段指示符。为2字符的字符串，或者空字符串</param>
        /// <param name="subfields">表示若干子字段的字符串数组。每个字符串的第一字符为子字段名，其余为子字段内容</param>
        public MarcField(string strName,
            string strIndicator,
            string[] subfields)
        {
            newMarcField(strName, strIndicator, subfields);
        }

        void newMarcField(string strName,
            string strIndicator,
            string[] subfields)
        {
            this.NodeType = NodeType.Field;

            if (String.IsNullOrEmpty(strName) == true)
                this.m_strName = DefaultFieldName;
            else
            {
                if (strName.Length != 3)
                    throw new Exception("Field的Name必须为3字符");
                this.m_strName = strName;
            }

            if (isControlFieldName(strName) == true)
            {
                throw new ArgumentException("不能用构造函数 NewMarcField(string strName, string strIndicator, string [] subfields) 创建控制字段", "strName");
            }

            this.m_strIndicator = strIndicator;

            StringBuilder content = new StringBuilder(4096);
            foreach (string s in subfields)
            {
                content.Append(MarcQuery.SUBFLD);
                content.Append(s);
            }

            this.Content = content.ToString();
        }

        // 使用一个字符串构造，指定了后面参数中所使用的(代用)子字段符号
        /// <summary>
        /// 初始化一个 MarcField 对象，并根据指定的字符串设置好全部内容
        /// </summary>
        /// <param name="chSubfield">子字段符号的代用字符。在 strText 参数中可以用这个代用字符来表示子字段符号 (ASCII 31)</param>
        /// <param name="strText">表示一个完整的 MARC 字段的 MARC 机内格式字符串</param>
        public MarcField(char chSubfield, string strText)
        {
            this.Parent = null;
            this.NodeType = NodeType.Field;
            Debug.Assert(this.ChildNodes.owner == this, "");

            this.Text = strText.Replace(chSubfield, MarcQuery.SUBFLD[0]);
        }

        #endregion

        /// <summary>
        /// 当前节点的全部文字。表现了一个完整的 MARC 字段
        /// </summary>
        public override string Text
        {
            get
            {
#if NO
                // 头标区
                if (this.IsHeader == true)
                {
                    Debug.Assert(string.IsNullOrEmpty(this.Name) == true, "");
                    Debug.Assert(string.IsNullOrEmpty(this.Indicator) == true, "");
                    return this.Content;
                }
#endif

                // 普通字段
                return this.Name + this.Indicator + this.Content + MarcQuery.FLDEND;
            }
            set
            {
#if NO
                if (this.IsHeader == true)
                {
                    if (string.IsNullOrEmpty(value) == true)
                        throw new Exception("头标区内容不能设置为空");
                    if (value.Length != 24)
                        throw new Exception("头标区内容只能设置为24字符");

                    this.Content = value;
                    return;
                }
#endif

                setFieldText(value);
            }
        }

        // 将机内格式的字符串设置到字段
        // 最后一个字符可以是 30 (表示字段结束)，也可以没有这个字符
        void setFieldText(string strText)
        {
            // 去掉末尾的 30 字符
            if (strText != null && strText.Length >= 1)
            {
                if (strText[strText.Length - 1] == (char)30)
                    strText = strText.Substring(0, strText.Length - 1);
            }

            if (string.IsNullOrEmpty(strText) == true)
                throw new Exception("字段 Text 不能设置为空");

            if (strText.Length < 3)
                throw new Exception("字段 Text 不能设置为小于 3 字符");

            string strFieldName = strText.Substring(0, 3);
            strText = strText.Substring(3); // 剩余部分

            this.m_strName = strFieldName;
            if (MarcNode.isControlFieldName(strFieldName) == true)
            {
                // 控制字段
                this.m_strIndicator = "";

                // 剩下的内容为空
                if (string.IsNullOrEmpty(strText) == true)
                {
                    this.Content = "";
                    return;
                }

                this.m_strContent = strText;    // 不要用this.Content，因为会惊扰到重新创建下级的机制
            }
            else
            {
                // 普通字段

                // 剩下的内容为空
                if (string.IsNullOrEmpty(strText) == true)
                {
                    this.Indicator = DefaultIndicator;
                    return;
                }

                // 还剩下一个字符
                if (strText.Length < 2)
                {
                    Debug.Assert(strText.Length == 1, "");
                    this.Indicator = strText + new string(MarcQuery.DefaultChar, 1);
                    return;
                }

                // 剩下两个字符以上
                this.m_strIndicator = strText.Substring(0, 2);
                this.Content = strText.Substring(2);
            }
        }

        /// <summary>
        /// 字段名
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
                    || value.Length != 3)
                    throw new ArgumentException("MarcField 的 Name 属性只允许用 3 个字符来设置", "Name");

                string strOldName = this.Name;
                bool bOldIsControlField = isControlFieldName(strOldName);

                bool bNewIsControlField = isControlFieldName(value);

                string strOldContent = "";
                // 预先存储内容字符串
                if (bOldIsControlField != bNewIsControlField)
                {
                    if (bOldIsControlField == false)
                        strOldContent = this.m_strContent + this.Content;
                    else
                        strOldContent = this.Content;
                }

                base.Name = value;
                // 如果从控制字段转换为普通字段(或者反之)，Indicator要妥善处理
                if (bOldIsControlField != bNewIsControlField)
                {
                    if (bOldIsControlField == false)
                    {
                        // 200 --> 001，重构下级，重构时 Indicator归入m_strContent
                        Debug.Assert(this.m_strIndicator.Length == 2, "");
                        this.ChildNodes.clearAndDetach();
                        this.Content = this.m_strIndicator + strOldContent;
                        this.m_strIndicator = "";
                    }
                    else
                    {
                        // 001 --> 200，重构下级，重构时 Indicator不算在m_strContent内
                        Debug.Assert(this.m_strIndicator.Length == 0, "");
                        this.ChildNodes.clearAndDetach();
                        this.m_strIndicator = m_strContent.Substring(0, 2);
                        this.Content = strOldContent.Substring(2);
                    }
                }
            }
        }

        // 内容字符串。不包括指示符部分，不包括字段结束符
        /// <summary>
        /// 字段正文。即字段指示符以后的全部内容
        /// </summary>
        public override string Content
        {
            get
            {
                StringBuilder result = new StringBuilder(4096);
                result.Append(this.m_strContent);   // 第一个子字段符号以前的内容
                // 合成下级元素
                for (int i = 0; i < this.ChildNodes.count; i++)
                {
                    MarcNode node = this.ChildNodes[i];
                    result.Append(node.Text);
                    // strResult += new string((char)31, 1) + node.Name + node.Content;
                }
                return result.ToString();
            }
            set
            {
                // 拆分为子字段
                this.ChildNodes.clearAndDetach();
                this.m_strContent = "";

                if (isControlFieldName(this.Name) == true)
                {
                    // 需要检查内容里面的 31字符？其实也不必检查，因为来回修改字段名的时候，可能内容中会包含 31 字符，属于正常情况
                    this.m_strContent = value;
                    return;
                }

#if NO
                List<string> segments = new List<string>();
                StringBuilder prefix = new StringBuilder(); // 第一个 31 出现以前的一段文字
                StringBuilder segment = new StringBuilder(); // 正在追加的内容段落

                for (int i = 0; i < value.Length; i++)
                {
                    char ch = value[i];
                    if (ch == 31)
                    {
                        // 如果先前有累积的，推走
                        if (segment.Length > 0)
                        {
                            segments.Add(segment.ToString());
                            segment.Clear();
                        }

                        segment.Append(ch);
                    }
                    else
                    {
                        if (segment.Length > 0 || segments.Count > 0)
                            segment.Append(ch);
                        else
                            prefix.Append(ch);// 第一个子字段符号以前的内容放在这里
                    }
                }

                if (segment.Length > 0)
                {
                    segments.Add(segment.ToString());
                    segment.Clear();
                }

                if (prefix.Length > 0)
                    this.m_strContent = prefix.ToString();
                foreach (string s in segments)
                {
                    MarcSubfield subfield = new MarcSubfield(this);
                    if (s.Length < 2)
                        subfield.Text = MarcNode.SUBFLD + "?";  // TODO: 或者可以忽略?
                    else
                        subfield.Text = s;
                    this.ChildNodes.Add(subfield);
                    Debug.Assert(subfield.Parent == this, "");
                }
#endif

                string strLeadingString = "";
                MarcNodeList subfields = MarcQuery.createSubfields(
                    // this,
                    value, out strLeadingString);
                this.ChildNodes.add(subfields);
                this.m_strContent = strLeadingString;
            }
        }

        /// <summary>
        /// 字段正文前导字符串。字段指示符以后，第一个子字段符号以前的一段特殊内容。控制字段不具备这个部分
        /// </summary>
        public string Leading
        {
            get
            {
                // 控制字段没有leading内容
                if (this.IsControlField == true)
                    return "";

                return this.m_strContent;
            }
            set
            {
                if (this.IsControlField == true)
                {
                    if (string.IsNullOrEmpty(value) == true)
                        return;
                    throw new Exception("控制字段没有不能设置(非空的) Leading 内容");
                }
                this.m_strContent = value;
            }
        }

        internal void ensureIndicatorChars()
        {
            if (string.IsNullOrEmpty(this.m_strIndicator) == true
    || this.m_strIndicator.Length < 2)
                this.m_strIndicator.PadRight(2, MarcQuery.DefaultChar);
        }

        /// <summary>
        /// 字段指示符的第一个字符
        /// </summary>
        public override char Indicator1
        {
            get
            {
                if (this.IsControlField == true)
                    return (char)0;
                ensureIndicatorChars();
                return this.m_strIndicator[0];
            }
            set
            {
                if (this.IsControlField == true)
                    return;
                ensureIndicatorChars();
                this.m_strIndicator = new string(value, 1) + this.m_strIndicator[1];
            }
        }

        /// <summary>
        /// 字段指示符的第二个字符
        /// </summary>
        public override char Indicator2
        {
            get
            {
                if (this.IsControlField == true)
                    return (char)0;
                ensureIndicatorChars();
                return this.m_strIndicator[1];
            }
            set
            {
                if (this.IsControlField == true)
                    return;
                ensureIndicatorChars();
                this.m_strIndicator = new string(this.m_strIndicator[0], 1) + value;
            }
        }

        /// <summary>
        /// 当前字段节点是否为控制字段
        /// </summary>
        public bool IsControlField
        {
            get
            {
                return isControlFieldName(this.Name);
            }
        }

        // 常用名。等同于ChildNodes
        /// <summary>
        /// 当前字段节点的下级节点集合。相当于 ChildNodes 的别名
        /// </summary>
        public MarcNodeList Subfields
        {
            get
            {
                return this.ChildNodes;
            }
            set
            {
                this.ChildNodes.clearAndDetach();
                this.ChildNodes.add(value);
            }
        }

        // fangbian diaoyong
        /// <summary>
        /// 在下级节点末尾追加一个子字段节点
        /// </summary>
        /// <param name="subfield">字段节点</param>
        public void add(MarcSubfield subfield)
        {
            this.ChildNodes.add(subfield);

        }

        /// <summary>
        /// 输出当前对象的调试用字符串
        /// </summary>
        /// <returns>表示内容的字符串</returns>
        public override string dump()
        {
            return this.Name + this.Indicator + this.m_strContent
                + dumpChildren();
        }

        /// <summary>
        /// 创建一个新的 MarcField 节点对象，从当前对象复制出全部内容和下级节点
        /// </summary>
        /// <returns>新的节点对象</returns>
        public override MarcNode clone()
        {
            MarcNode new_node = new MarcField();
            new_node.Text = this.Text;
            new_node.Parent = null; // 尚未和任何对象连接
            return new_node;
        }
    }

}
