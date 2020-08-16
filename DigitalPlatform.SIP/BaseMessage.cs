using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DigitalPlatform.SIP2
{
    public class BaseMessage
    {
        // 命令指示符
        public string CommandIdentifier { get; set; }

        //The sequence number is a single ASCII digit, '0' to '9'.  
        //When error detection is enabled, the SC will increment the sequence number field for each new message it transmits. 
        //The ACS should verify that the sequence numbers increment as new messages are received from the 3M SelfCheck system.  
        //When error detection is enabled, the ACS response to a message should include a sequence number field also, where the sequence number field’s value matches the sequence number value from the message being responded to.
        private string _sequenceNumber_AY { get; set; }

        //The checksum is four ASCII character digits representing the binary sum of the characters including the first character of the transmission and up to and including the checksum field identifier characters.
        //To calculate the checksum add each character as an unsigned binary number, take the lower 16 bits of the total and perform a 2's complement.  The checksum field is the result represented by four hex digits.
        //To verify the correct checksum on received data, simply add all the hex values including the checksum.  It should equal zero.
        // 4位16进制
        private string _checksum_AZ { get; set; }

        // 定长字段数组
        public List<FixedLengthField> FixedLengthFields = new List<FixedLengthField>();
        
        // 变长字段数组
        public List<VariableLengthField> VariableLengthFields = new List<VariableLengthField>();

        #region 定长字段

        // 获取某个定长字段
        protected FixedLengthField GetFixedField(string name)
        {
            foreach (FixedLengthField field in this.FixedLengthFields)
            {
                if (field.Name == name)
                    return field;
            }
            return null;
        }

        protected string GetFixedFieldValue(string name)
        {
            FixedLengthField field = this.GetFixedField(name);
            if (field == null)
                throw new Exception("未定义定长字段" + name);

            return field.Value;
        }

        // 设置某个定长字段的值
        protected void SetFixedFieldValue(string name, string value)
        {
            FixedLengthField field = this.GetFixedField(name);
            if (field == null)
                throw new Exception("未定义定长字段" + name);

            field.Value = value;
        }
        #endregion

        #region 变长字段



        // 获取某个定长字段
        protected VariableLengthField GetVariableField(string id)
        {
            VariableLengthField temp = null;
            foreach (VariableLengthField field in this.VariableLengthFields)
            {
                if (field.ID == id)
                {
                    //return field; 
                    temp = field;//2020/8/13 改造，找到最后一个同名字段再返回。
                }
            }

            return temp;
        }

        protected string GetVariableFieldValue(string id)
        {

            VariableLengthField field = this.GetVariableField(id);
            if (field == null)
            {
                // 20170811 jane todo
                if (id == "AY" || id == "AZ")
                    return "";

                throw new Exception("未定义变长字段" + id);
            }

            return field.Value;
        }

        // 设置某个定长字段的值
        protected void SetVariableFieldValue(string id, string value)
        {

            VariableLengthField field = this.GetVariableField(id);
            if (field == null)
            {
                // 20170811 jane todo
                if (id == "AY" || id == "AZ")
                    return; 

                throw new Exception("未定义变长字段" + id);
            }

            // 2020/8/13 如果是重复的字段，先检查字段是否已经赋值，没赋值过则赋值，
            // 如果已有值，则新new的一个字段,插在其后（这样可以保证原始的字段顺序）
            if (field.IsRepeat == false)
            {
                field.Value = value;
            }
            else
            {
                if (string.IsNullOrEmpty(field.Value) == true)
                    field.Value = value;
                else
                {
                    VariableLengthField f = new VariableLengthField(id, field.IsRepeat, field.IsRepeat);
                    f.Value = value;

                    // 插在其后,不要用增加（这样可以保证原始的字段顺序）
                    this.VariableLengthFields.Insert(this.VariableLengthFields.IndexOf(field) + 1, f);
                    //this.VariableLengthFields.Add(f);
                }
            }
        }

        protected List<VariableLengthField> GetVariableFieldList(string id)
        {
            List<VariableLengthField> list = new List<VariableLengthField>();
            foreach (VariableLengthField field in this.VariableLengthFields)
            {
                if (field.ID == id)
                {
                    list.Add(field);
                }
            }
            return list;
        }

        protected void SetVariableFieldList(string id, List<VariableLengthField> list)
        {
            if (string.IsNullOrEmpty(id) == true)
                throw new Exception("id参数不能为空");

            //传入数组字段必须与id同名
            foreach (VariableLengthField field in list)
            {
                if (field.ID != id)
                    throw new Exception("数组中有个字段名为"+field.ID+",与指定的字段名"+id+"不符。");
            }

            // 先删除原来已存在的同名字段
            List<VariableLengthField> oldList = this.GetVariableFieldList(id);
            foreach (VariableLengthField field in oldList)
            {
                this.VariableLengthFields.Remove(field);
            }

            // 再增加新的字段
            foreach (VariableLengthField field in list)
            {
                this.VariableLengthFields.Add(field);
            }

        }

        #endregion

        #region 各命令通用字段

        public List<VariableLengthField> AF_ScreenMessage_List
        {
            get
            {
                return this.GetVariableFieldList(SIPConst.F_AF_ScreenMessage);
            }
        }

        //variable-length optional field
        public string AF_ScreenMessage_o
        {
            get
            {
                // 2020/8/14，因为AF是重复字段，所以把所有值用逗号组合起来，如果前端想自己拼装，请调AF_ScreenMessage_List
                List<VariableLengthField> list = this.GetVariableFieldList(SIPConst.F_AF_ScreenMessage);
                string text = "";
                foreach (VariableLengthField one in list)
                {
                    if (text != "")
                        text += ",";
                    text += one.Value;
                }
                return text;
            }
            set
            {
                // 2020/8/16注意赋值的时候，只给一个字段赋值（GetVariableField是找最后一个字段，一般情况下dp2 response命令只有一个AF）。
                VariableLengthField field = this.GetVariableField(SIPConst.F_AF_ScreenMessage);
                if (field == null)
                {
                    field = new VariableLengthField(SIPConst.F_AF_ScreenMessage, false, true);
                    this.VariableLengthFields.Add(field);
                }
                field.Value = value;
            }
        }

        // AG是重复字段
        public List<VariableLengthField> AG_PrintLine
        {
            get
            {
                return this.GetVariableFieldList(SIPConst.F_AG_PrintLine);
            }
        }

        //variable-length optional field
        public string AG_PrintLine_o
        {
            get
            {
                // 2020/8/14，因为AG是重复字段，所以把所有值用逗号组合起来，如果前端想自己拼装，请调 AG_PrintLine
                List<VariableLengthField> list = this.GetVariableFieldList(SIPConst.F_AG_PrintLine);
                string text = "";
                foreach (VariableLengthField one in list)
                {
                    if (text != "")
                        text += ",";
                    text += one.Value;
                }
                return text;
            }
            set
            {
                // 2020/8/16注意赋值的时候，只给一个字段赋值（GetVariableField是找最后一个字段，一般情况下dp2 response命令只有一个AG）。
                VariableLengthField field = this.GetVariableField(SIPConst.F_AG_PrintLine);
                if (field == null)
                {
                    field = new VariableLengthField(SIPConst.F_AG_PrintLine, false, true);
                    this.VariableLengthFields.Add(field);
                }
                field.Value = value;
            }
        }

        #endregion

        // 解析字符串命令为对象
        public virtual int parse(string text, out string error)
        {
            error = "";

            if (text == null || text.Length < 2)
            {
                error = "命令字符串为null或长度小于2位";
                return -1;
            }
            this.CommandIdentifier = text.Substring(0, 2);  //命令指示符
            string conent = text.Substring(2); //内容

            // 给定长字段赋值
            int start = 0;
            foreach (FixedLengthField field in this.FixedLengthFields)
            {
                field.Value = conent.Substring(start, field.Length);
                start += field.Length;
            }


            //处理后面的变长字段
            string rest = conent.Substring(start);
            string[] parts = rest.Split(new char[] { '|' });
            for (int i = 0; i < parts.Length; i++)
            {
                string part = parts[i];
                if (part.Length < 2)
                {
                    continue;
                }

                string fieldId = part.Substring(0, 2);
                string value = part.Substring(2);

                this.SetVariableFieldValue(fieldId, value);
            }

            // 校验;
            int ret = this.Verify(out error);
            if (ret == -1)
                return -1;

            return 0;
        }

        // 将对象转换字符串命令
        public virtual string ToText()
        {
            Debug.Assert(String.IsNullOrEmpty(this.CommandIdentifier) == false, "命令指示符未赋值");
            StringBuilder text = new StringBuilder(this.CommandIdentifier);

            if (this.FixedLengthFields != null)
            {
                //foreach (FixedLengthField field in this.FixedLengthFields)
                for (int i = 0; i < this.FixedLengthFields.Count; i++)
                {
                    FixedLengthField field = this.FixedLengthFields[i];
                    if (field.Value == null || field.Value.Length != field.Length)
                        throw new Exception("定长字段[" + field.Name + "]的值为null或者长度不符合定义");
                    text.Append(field.Value);
                }
            }

            if (this.VariableLengthFields != null && this.VariableLengthFields.Count > 0)
            {
                //foreach (VariableLengthField field in this.VariableLengthFields)
                for (int i = 0; i < this.VariableLengthFields.Count; i++)
                {
                    VariableLengthField field = this.VariableLengthFields[i];
                    if (field.Value != null)
                    {
                        text.Append(field.ID + field.Value + SIPConst.FIELD_TERMINATOR);
                    }
                }
            }


            string result=  text.ToString();

            // 去掉字符串最后一个|
            if (string.IsNullOrEmpty(result) == false)
            {
                if (result.Substring(result.Length - 1) == SIPConst.FIELD_TERMINATOR)
                    result=result.Substring(0, result.Length - 1);
            }

            return result;
        }

        // 校验对象的各参数是否合法
        public virtual int Verify(out string error)
        {
            error = "";

            // 校验定长字段
            foreach (FixedLengthField field in this.FixedLengthFields)
            {
                if (field.Value == null || field.Value.Length != field.Length)
                {
                    error = field.Name + "的值为null或者长度不符合要求的长度";
                    return -1;
                }
            }

            foreach (VariableLengthField field in this.VariableLengthFields)
            {
                if (field.IsRequired==true &&  field.Value == null)
                {
                    error = field.ID + "是必备字段，消息中需包含该字段";
                    return -1;
                }
            }

            return 0;
        }

        public virtual void SetDefaulValue()
        {
        }
    }
}
