using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.Marc
{
    // MARC 记录
    /// <summary>
    /// MARC 记录节点
    /// </summary>
    public class MarcRecord : MarcNode
    {
        // 存储头标区 24 字符
        /// <summary>
        /// MARC记录的头标区，一共24个字符
        /// </summary>
        public MarcHeader Header = new MarcHeader();

        /// <summary>
        /// 嵌套字段的定义。
        /// 缺省为 空，表示不使用嵌套字段。
        /// 这是一个列举字段名的逗号间隔的列表('*'为通配符)，或者 '@' 字符后面携带一个正则表达式
        /// </summary>
        public string OuterFieldDef
        {
            get;
            set;
        }

        #region 构造函数

        /// <summary>
        /// 初始化一个 MarcRecord 对象
        /// </summary>
        public MarcRecord()
        {
            this.Parent = null;
            this.NodeType = NodeType.Record;
        }

        // 通过传递过来的MARC记录构造整个一棵树
        /// <summary>
        /// 初始化一个 MarcRecord 对象，并根据指定的字符串设置好全部内容
        /// </summary>
        /// <param name="strRecord">表示一条完整的 MARC 记录的 MARC 机内格式字符串</param>
        /// <param name="strOuterFieldDef">嵌套字段的定义。缺省为 null，表示不使用嵌套字段。这是一个列举字段名的逗号间隔的列表('*'为通配符)，或者 '@' 字符后面携带一个正则表达式</param>
        public MarcRecord(string strRecord,
            string strOuterFieldDef = null)
        {
            this.NodeType = NodeType.Record;

            this.OuterFieldDef = strOuterFieldDef;
            this.Content = strRecord;
        }

        #endregion

        // 根节点的 Name Indicator 都为空，内容记载在 Content
        /// <summary>
        /// 当前节点的全部文字。表现了一条完整的 MARC 记录
        /// </summary>
        public override string Text
        {
            get
            {
                return this.Content;
            }
            set
            {
                this.Content = value;
            }
        }

        /// <summary>
        /// 当前节点的正文内容。对于 MarcRoecrd 节点来说，它等同于 Text 成员值
        /// </summary>
        public override string Content
        {
            get
            {
                StringBuilder result = new StringBuilder(4096);
                result.Append(this.Header.ToString());
                // 合成下级元素
                for (int i = 0; i < this.ChildNodes.count; i++)
                {
                    MarcNode node = this.ChildNodes[i];

                    if (node.NodeType != Marc.NodeType.Field)
                        throw new Exception("根下级出现了不是 Field 类型的节点 (" + node.NodeType.ToString() + ")");
                    /*
                    if (i != 0 && node.Name != "hdr")
                        throw new Exception("MarcField同级第一个位置必须是Name为'hdr'的表示头标区的MarcField（而现在Name为 '" + node.Name + "'）");
                     * */
                    MarcField field = (MarcField)node;

                    result.Append(field.Text);
                }
                return result.ToString();
            }
            set
            {
                setContent(value);
            }
        }

        void setContent(string strValue)
        {
            // 拆分为字段
            this.ChildNodes.clearAndDetach();
            this.m_strContent = "";

            if (String.IsNullOrEmpty(strValue) == true)
                return;

            // 整理尾部字符
            char tail = strValue[strValue.Length - 1];
            if (tail == 29)
            {
                strValue = strValue.Substring(0, strValue.Length - 1);

                if (String.IsNullOrEmpty(strValue) == true)
                    return;
            }

            this.Header[0, Math.Min(strValue.Length, 24)] = strValue;

            if (strValue.Length <= 24)
            {
                // 只有头标区，没有任何字段
                return;
            }
            this.ChildNodes.add(MarcQuery.createFields(
                // this,
                strValue.Substring(24),
                this.OuterFieldDef));
        }
#if NO
        void SetContent(string strValue)
        {
            // 拆分为字段
            this.ChildNodes.Clear();
            this.m_strContent = "";

            if (String.IsNullOrEmpty(strValue) == true)
                return;

            // 整理尾部字符
            char tail = strValue[strValue.Length - 1];
            if (tail == 29)
                strValue = strValue.Substring(0, strValue.Length - 1);

            if (String.IsNullOrEmpty(strValue) == true)
                return;

            tail = strValue[strValue.Length - 1];
            if (tail != 30)
                strValue += (char)30;

            StringBuilder field_text = new StringBuilder(4096);
            MarcField field = null;
            for (int i = 0; i < strValue.Length; i++)
            {
                char ch = strValue[i];
                if (ch == 30 || ch == 29)
                {
                    // 上一个字段结束。创建一个字段的时机
                    string strText = field_text.ToString();

                    if (this.ChildNodes.Count == 0)
                    {
                        // 创建第一个字段，也就是头标区
                        string strHeader = "";  // 头标区
                        string strRest = "";    // 余下的部分
                        // 长度不足 24 字符
                        if (string.IsNullOrEmpty(strText) == true
                            || strText.Length < 24)
                        {
                            // 需要补齐 24 字符
                            strHeader = strText.PadRight(24, '?');
                        }
                        else
                        {
                            // 长度大于或者等于 24 字符
                            strHeader = strText.Substring(0, 24);
                            strRest = strText.Substring(24);
                        }

                        // header
                        Debug.Assert(strHeader.Length == 24, "");
                        field = new MarcField(this);
                        field.IsHeader = true;
                        field.Text = strHeader;
                        this.ChildNodes.Add(field);
                        Debug.Assert(field.Parent == this, "");

                        // 余下的部分再创建一个字段
                        if (string.IsNullOrEmpty(strRest) == false)
                        {
                            // 如果长度不足 3 字符，补齐?
                            if (strRest.Length < 3)
                                strRest = strRest.PadRight(3, '?');
                            field = new MarcField(this);
                            field.Text = strRest;
                            this.ChildNodes.Add(field);
                            Debug.Assert(field.Parent == this, "");
                        }

                        field_text.Clear();
                        continue;
                    }

                    // 创建头标区以后的普通字段
                    field = new MarcField(this);

                    // 如果长度不足 3 字符，补齐?
                    if (strText.Length < 3)
                        strText = strText.PadRight(3, '?');
                    field = new MarcField(this);
                    field.Text = strText;
                    this.ChildNodes.Add(field);
                    Debug.Assert(field.Parent == this, "");

                    field_text.Clear();
                }
                else
                {
                    field_text.Append(ch);
                }
            }

        }
#endif

        // 常用名。等同于ChildNodes
        /// <summary>
        /// 当前节点下属的全部字段节点。本属性相当于 CHildNodes 的别名
        /// </summary>
        public MarcNodeList Fields
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
        /// 在下级节点末尾追加一个字段节点
        /// </summary>
        /// <param name="field">字段节点</param>
        public void add(MarcField field)
        {
            this.ChildNodes.add(field);
        }

        /// <summary>
        /// 设置指定名字的第一个字段的指示符和内容。如果没有这个字段，则创建一个
        /// </summary>
        /// <param name="strFieldName">字段名。3字符</param>
        /// <param name="strIndicator">要设置的指示符。如果不想修改指定字段的指示符，则可以将本参数设置为 null；否则应当是一个 2 字符的字符串。如果要操作的字段是控制字段，则本参数不会被使用，可设置为 null</param>
        /// <param name="strContent">要设置的字段内容。如果是非控制字段，本参数中一般需要按照 MARC 格式要求包含子字段符号(MarcQuery.SUBFLD)，以指定子字段名等</param>
        /// <param name="strNewIndicator">如果指定的字段不存在，则需要创建，创建的时候将采用本参数作为字段指示符的值。控制字段(因为没有指示符所以)不使用本参数。如果本参数设置为 null，但函数执行过程中确实遇到了需要创建新字段的情况，则函数会自动采用两个空格作为新字段的指示符</param>
        public void setFirstField(
            string strFieldName,
            string strIndicator,
            string strContent,
            string strNewIndicator = null)
        {
            // 检查参数
            if (string.IsNullOrEmpty(strFieldName) == true || strFieldName.Length != 3)
                throw new ArgumentException("strFieldName不能为空，应是 3 字符内容", "strFieldName");
            if (string.IsNullOrEmpty(strIndicator) == false && strIndicator.Length != 2)
                throw new ArgumentException("strIndicator若不是空，应是 2 字符内容", "strIndicator");
            if (string.IsNullOrEmpty(strNewIndicator) == false && strNewIndicator.Length != 2)
                throw new ArgumentException("strNewIndicator若不是空，应是 2 字符内容", "strNewIndicator");

            MarcNodeList fields = this.select("field[@name='" + strFieldName + "']");
            if (fields.count == 0)
            {
                if (isControlFieldName(strFieldName) == true)
                {
                    this.ChildNodes.insertSequence(new MarcField(strFieldName, strContent));
                }
                else
                {
                    if (string.IsNullOrEmpty(strNewIndicator) == true)
                        strNewIndicator = "  ";
                    this.ChildNodes.insertSequence(new MarcField(strFieldName, strNewIndicator, strContent));
                }
            }
            else
            {
                if (string.IsNullOrEmpty(strIndicator) == false)
                    fields[0].Indicator = strIndicator;

                fields[0].Content = strContent;
            }
        }


        /// <summary>
        /// 设置指定名字的第一个字段和第一个子字段的值。如果没有这个字段，则创建一个；如果没有这个子字段，则创建一个
        /// </summary>
        /// <param name="strFieldName">字段名。3字符</param>
        /// <param name="strSubfieldName">子字段名。1字符</param>
        /// <param name="strContent">要设置的子字段内容</param>
        /// <param name="strNewIndicator">如果指定的字段不存在，则需要创建，创建的时候将采用本参数作为字段指示符的值</param>
        public void setFirstSubfield(
            string strFieldName,
            string strSubfieldName,
            string strContent,
            string strNewIndicator = "  ")
        {
            // 检查参数
            if (string.IsNullOrEmpty(strFieldName) == true || strFieldName.Length != 3)
                throw new ArgumentException("strFieldName不能为空，应是 3 字符内容", "strFieldName");
            if (string.IsNullOrEmpty(strSubfieldName) == true || strSubfieldName.Length != 1)
                throw new ArgumentException("strSubfieldName不能为空，应是 1 字符内容", "strSubfieldName");
            if (string.IsNullOrEmpty(strNewIndicator) == true || strNewIndicator.Length != 2)
                throw new ArgumentException("strNewIndicator不能为空，应是 2 字符内容", "strNewIndicator");

            MarcNodeList fields = this.select("field[@name='" + strFieldName + "']");
            if (fields.count == 0)
            {
                this.ChildNodes.insertSequence(new MarcField(strFieldName, strNewIndicator, MarcQuery.SUBFLD + strSubfieldName + strContent));
            }
            else
            {
                MarcNodeList subfields = fields[0].select("subfield[@name='" + strSubfieldName + "']");
                if (subfields.count == 0)
                {
                    fields[0].ChildNodes.insertSequence(new MarcSubfield(strSubfieldName, // bug "a", 
                        strContent));
                }
                else
                {
                    subfields[0].Content = strContent;
                }
            }
        }

        /// <summary>
        /// 输出当前对象的全部子对象的调试用字符串
        /// </summary>
        /// <returns>表示内容的字符串</returns>
        public override string dumpChildren()
        {
            StringBuilder strResult = new StringBuilder(4096);
            foreach (MarcNode node in this.ChildNodes)
            {
                if (strResult.Length > 0)
                    strResult.Append("\r\n");
                strResult.Append(node.dump());
            }

            return strResult.ToString();
        }

        /// <summary>
        /// 输出当前对象的调试用字符串
        /// </summary>
        /// <returns>表示内容的字符串</returns>
        public override string dump()
        {
            return this.Header + "\r\n" + dumpChildren();
        }

        /// <summary>
        /// 创建一个新的 MarcRecord 节点对象，从当前对象复制出全部内容和下级节点
        /// </summary>
        /// <returns>新的节点对象</returns>
        public override MarcNode clone()
        {
            MarcNode new_node = new MarcRecord();
            new_node.Text = this.Text;
            new_node.Parent = null; // 尚未和任何对象连接
            return new_node;
        }

        /// <summary>
        /// 输出工作单格式
        /// </summary>
        /// <returns>工作单格式文本。注意子字段符号为 'ǂ'</returns>
        public string ToWorksheet()
        {
            StringBuilder result = new StringBuilder();

            // 头标区
            result.AppendLine(this.Header.ToString());
            foreach (MarcField field in this.Fields)
            {
                result.AppendLine(
                    field.Text
                    .TrimEnd(new char[] { MarcQuery.FLDEND[0] })
                    .Replace(MarcQuery.SUBFLD, "ǂ")
                    );
            }

            return result.ToString().TrimEnd(new char[] { '\r','\n' });
        }

        /// <summary>
        /// 由工作单格式创建 MarcRecord 对象
        /// </summary>
        /// <param name="strText">工作单格式文本。注意子字段符号为 'ǂ'</param>
        /// <returns>MarcRecord 对象</returns>
        public static MarcRecord FromWorksheet(string strText)
        {
            MarcRecord record = new MarcRecord();

            if (string.IsNullOrEmpty(strText))
                return record;

            string[] lines = strText.Replace("\r\n", "\r").Split(new char[] { '\r' });

            // 头标区
            string first_line = lines[0];
            if (first_line.Length < 24)
                first_line = first_line.PadRight(24, ' ');
            record.Header[0, Math.Min(first_line.Length, 24)] = first_line;

            int i = 0;
            foreach (string line in lines)
            {
                if (i > 0)
                {
                    // 
                    // record.add(new MarcField("001A1234567"));
                    record.add(new MarcField('ǂ', line));
                }
                i++;
            }

            return record;
        }

        // 2020/11/14
        /// <summary>
        /// 删除全部空的字段、子字段
        /// </summary>
        /// <returns>返回已经被删除的对象(字段或者子字段)的集合</returns>
        public List<MarcNode> DetachEmptyFieldsSubfields()
        {
            var subfields = DetachEmptySubfields();
            var fields = DetachEmptyFields();
            var results = new List<MarcNode>();
            results.AddRange(fields.Cast<MarcNode>());
            results.AddRange(subfields.Cast<MarcNode>());
            return results;
        }

        // 2020/11/14
        /// <summary>
        /// 删除全部空的字段
        /// </summary>
        /// <returns>返回已经被删除的字段的集合</returns>
        public List<MarcField> DetachEmptyFields()
        {
            List<MarcField> results = new List<MarcField>();

            var fields = this.select("//field");
            foreach (MarcField field in fields)
            {
                if (string.IsNullOrEmpty(field.Content))
                {
                    field.detach();
                    results.Add(field);
                }
            }

            return results;
        }

        // 2020/11/14
        /// <summary>
        /// 删除全部空的子字段
        /// </summary>
        /// <returns>返回已经被删除的子字段的集合</returns>
        public List<MarcSubfield> DetachEmptySubfields()
        {
            List<MarcSubfield> results = new List<MarcSubfield>();

            var subfields = this.select("//subfield");
            foreach (MarcSubfield subfield in subfields)
            {
                if (string.IsNullOrEmpty(subfield.Content))
                {
                    subfield.detach();
                    results.Add(subfield);
                }
            }

            return results;
        }
    }

}
