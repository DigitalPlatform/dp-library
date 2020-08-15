using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2
{
    public class VariableLengthField 
    {
        //public string Name { get; set; }
        public string ID { get; set; }
        public bool IsRequired { get; set; }
        public string Value { get; set; }

        // 是否是重复字段 2020/8/13加
        public bool IsRepeat{ get; set; }

        public VariableLengthField (string id, bool required,bool repeat=false)
        {
            //this.Name = name;
            this.ID = id;
            this.IsRequired = required;

            // 是否是可重复字段 
            this.IsRepeat = repeat;
        }

        public VariableLengthField(string id, bool required,string value, bool repeat = false)
        {
            //this.Name = name;
            this.ID = id;
            this.IsRequired = required;
            this.Value = value;

            this.IsRepeat = repeat;
        }

    }
}
