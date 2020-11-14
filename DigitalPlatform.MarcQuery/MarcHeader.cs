using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalPlatform.Marc
{
    /// <summary>
    /// MARC 记录的头标区
    /// </summary>
    public class MarcHeader
    {
        string m_strContent = "";

        /// <summary>
        /// 头标区所含字符数。固定为 24
        /// </summary>
        public static int FixedLength = 24;

        #region 各种功能位

        // 记录长度
        public string reclen
        {
            get
            {
                return this[0, 5];
            }
            set
            {
                this[0, 5] = value;
            }
        }

        public string status
        {
            get
            {
                return this[5, 1];
            }
            set
            {
                this[5, 1] = value;
            }
        }

        public string type
        {
            get
            {
                return this[6, 1];
            }
            set
            {
                this[6, 1] = value;
            }
        }

        public string level
        {
            get
            {
                return this[7, 1];
            }
            set
            {
                this[7, 1] = value;
            }
        }

        public string control
        {
            get
            {
                return this[8, 1];
            }
            set
            {
                this[8, 1] = value;
            }
        }

        public string reserve
        {
            get
            {
                return this[9, 1];
            }
            set
            {
                this[9, 1] = value;
            }
        }

        // 字段指示符长度
        public string indicount
        {
            get
            {
                return this[10, 1];
            }
            set
            {
                this[10, 1] = value;
            }
        }

        // 子字段标识符长度
        public string subfldcodecount
        {
            get
            {
                return this[11, 1];
            }
            set
            {
                this[11, 1] = value;
            }
        }

        // 数据基地址
        public string baseaddr
        {
            get
            {
                return this[12, 5];
            }
            set
            {
                this[12, 5] = value;
            }
        }

        public string res1
        {
            get
            {
                return this[17, 3];
            }
            set
            {
                this[17, 3] = value;
            }
        }

        // 目次区中字段长度部分
        public string lenoffld
        {
            get
            {
                return this[20, 1];
            }
            set
            {
                this[20, 1] = value;
            }
        }

        // 目次区中字段起始位置部分
        public string startposoffld
        {
            get
            {
                return this[21, 1];
            }
            set
            {
                this[21, 1] = value;
            }
        }

        // 实现者定义部分
        public string impdef
        {
            get
            {
                return this[22, 1];
            }
            set
            {
                this[22, 1] = value;
            }
        }

        public string res2
        {
            get
            {
                return this[23, 1];
            }
            set
            {
                this[23, 1] = value;
            }
        }

        #endregion

        // 2015/5/31
        // 按照UNIMARC惯例强制填充ISO2709头标区
        /// <summary>
        /// 按照 UNIMARC 惯例强制填充 ISO2709 头标区
        /// </summary>
        public void ForceUNIMARCHeader()
        {
            indicount = "2";
            subfldcodecount = "2";
            lenoffld = "4";   // 目次区中字段长度部分
            startposoffld = "5"; // 目次区中字段起始位置部分
        }

        /// <summary>
        /// 获取或设置头标区中任意一段长度的子字符串
        /// </summary>
        /// <param name="nStart">开始位置</param>
        /// <param name="nLength">长度。如果为-1，表示尽可能多。本参数可以缺省，缺省值为1</param>
        /// <returns>nStart 和 nLength 参数所表示范围的字符串</returns>
        public string this[int nStart, int nLength = 1]
        {
            get
            {
                if (nStart < 0 || nStart >= FixedLength)
                    throw new ArgumentException("nStart的取值范围应该是大于或等于 0，小于 " + FixedLength);
                if (nLength == -1)
                    nLength = FixedLength;
                if (nStart + nLength > FixedLength)
                    throw new ArgumentException("nStart + nLength 应该小于或等于 " + FixedLength);
                if (nLength == 0)
                    return "";

                EnsureFixedLength();

                return this.m_strContent.Substring(nStart, nLength);
            }
            set
            {
                if (nStart < 0 || nStart >= FixedLength)
                    throw new ArgumentException("nStart的取值范围应该是大于或等于 0，小于 " + FixedLength);
                if (nLength == -1)
                    nLength = FixedLength;
                if (nStart + nLength > FixedLength)
                    throw new ArgumentException("nStart + nLength 应该小于或等于 " + FixedLength);
                if (value == null)
                {
                    if (nLength == 0)
                        return;
                    throw new ArgumentException("value 不应为 null");
                }
                if (value.Length < nLength)
                    throw new ArgumentException("value 的字符数不足 nLength 参数所指定的字符数 " + nLength);

                EnsureFixedLength();

                string strLeft = this.m_strContent.Substring(0, nStart);
                string strRight = this.m_strContent.Substring(nStart + nLength);
                this.m_strContent = strLeft + value.Substring(0, nLength) + strRight;
            }
        }

        // 确保内容为固定字符数
        void EnsureFixedLength()
        {
            if (this.m_strContent.Length < FixedLength)
                this.m_strContent = this.m_strContent.PadRight(FixedLength, MarcQuery.DefaultChar);
            else if (this.m_strContent.Length > FixedLength)
                this.m_strContent = this.m_strContent.Substring(0, FixedLength);
        }

        /// <summary>
        /// 获得表示整个头标区内容的字符串
        /// </summary>
        /// <returns>表示整个头标区内容的字符串</returns>
        public override string ToString()
        {
            EnsureFixedLength();
            return this.m_strContent;
        }

        // TODO: 设置头标区为UNIMARC或者MARC21缺省值的功能。和文献类型等参数有关，可能需要一个可变参数的函数
    }


}
