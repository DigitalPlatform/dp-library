using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DigitalPlatform.Xml
{
    public static class ElementExtension
    {
        public static string GetElementText(this XmlElement nodeRoot,
    string strXpath)
        {
            XmlNode node = nodeRoot.SelectSingleNode(strXpath);
            if (node == null)
                return "";

            XmlNode nodeText;
            nodeText = node.SelectSingleNode("text()");

            if (nodeText == null)
                return "";
            else
                return nodeText.Value;
        }

        // 2022/8/1
        public static string GetElementText(this XmlElement nodeRoot,
    string strXpath,
    out XmlElement element)
        {
            element = nodeRoot.SelectSingleNode(strXpath) as XmlElement;
            if (element == null)
                return "";

            return element.InnerText;
        }

        // 把表示布尔值的字符串翻译为布尔值
        // 注意，strValue不能为空，本函数无法解释缺省值
        public static bool IsBooleanTrue(string strValue)
        {
            // 2008/6/4
            if (String.IsNullOrEmpty(strValue) == true)
                throw new Exception("IsBoolean() 不能接受空字符串参数");

            strValue = strValue.ToLower();

            if (strValue == "yes" || strValue == "on"
                    || strValue == "1" || strValue == "true")
                return true;

            return false;
        }

        public static bool IsBooleanTrue(string strValue, bool bDefaultValue)
        {
            if (string.IsNullOrEmpty(strValue) == true)
                return bDefaultValue;
            return IsBooleanTrue(strValue);
        }

        // 包装版本
        public static bool GetBooleanParam(this XmlElement node,
            string strParamName,
            bool bDefaultValue)
        {
            bool bValue = bDefaultValue;
            string strError = "";
            GetBooleanParam(node,
                strParamName,
                bDefaultValue,
                out bValue,
                out strError);
            return bValue;
        }

        // 包装后的版本。不用事先获得元素的 Node
        public static bool GetBooleanParam(
            this XmlElement root,
            string strElementPath,
            string strParamName,
            bool bDefaultValue)
        {
            var node = root.SelectSingleNode(strElementPath) as XmlElement;
            if (node == null)
                return bDefaultValue;
            return GetBooleanParam(node,
                strParamName,
                bDefaultValue);
        }

        // 获得布尔型的属性参数值
        // return:
        //      -1  出错。但是bValue中已经有了bDefaultValue值，可以不加警告而直接使用
        //      0   正常获得明确定义的参数值
        //      1   参数没有定义，因此代替以缺省参数值返回
        public static int GetBooleanParam(this XmlElement node,
            string strParamName,
            bool bDefaultValue,
            out bool bValue,
            out string strError)
        {
            strError = "";
            bValue = bDefaultValue;

            string strValue = node.GetAttribute(strParamName);

            strValue = strValue.Trim();

            if (String.IsNullOrEmpty(strValue) == true)
            {
                bValue = bDefaultValue;
                return 1;
            }

            strValue = strValue.ToLower();

            if (strValue == "yes" || strValue == "on"
                || strValue == "1" || strValue == "true")
            {
                bValue = true;
                return 0;
            }

            // TODO: 可以检查字符串，要在规定的值范围内

            bValue = false;
            return 0;
        }

        // 包装版本
        public static string GetStringParam(this XmlElement node,
            string strParamName,
            string strDefaultValue)
        {
            string strValue = strDefaultValue;
            string strError = "";
            GetStringParam(node,
                strParamName,
                strDefaultValue,
                out strValue,
                out strError);
            return strValue;
        }

        // 2016/10/13
        // 包装后的版本。不用事先获得元素的 Node
        public static string GetStringParam(
            this XmlElement root,
            string strElementPath,
            string strParamName,
            string strDefaultValue)
        {
            XmlElement node = root.SelectSingleNode(strElementPath) as XmlElement;
            if (node == null)
                return strDefaultValue;
            return GetStringParam(node,
                strParamName,
                strDefaultValue);
        }

        // 获得字符串的属性参数值
        // 注：属性节点不具备的时候，返回 strDefaultValue。否则，就要返回属性值，哪怕属性值为 ""
        // return:
        //      -1  出错。但是nValue中已经有了nDefaultValue值，可以不加警告而直接使用
        //      0   正常获得明确定义的参数值
        //      1   参数没有定义，因此代替以缺省参数值返回
        public static int GetStringParam(this XmlElement node,
            string strParamName,
            string strDefaultValue,
            out string strValue,
            out string strError)
        {
            strError = "";

            XmlAttribute attr = node.GetAttributeNode(strParamName);
            if (attr == null)
            {
                strValue = strDefaultValue;
                return 1;
            }
            strValue = attr.Value;
            if (string.IsNullOrEmpty(strValue) == false)
                strValue = strValue.Trim();

            return 0;
        }

        // 获得整数型的属性参数值
        // return:
        //      -1  出错。但是nValue中已经有了nDefaultValue值，可以不加警告而直接使用
        //      0   正常获得明确定义的参数值
        //      1   参数没有定义，因此代替以缺省参数值返回
        public static int GetIntegerParam(this XmlElement node,
            string strParamName,
            int nDefaultValue,
            out int nValue,
            out string strError)
        {
            strError = "";
            nValue = nDefaultValue;

            string strValue = node.GetAttribute(strParamName);


            if (String.IsNullOrEmpty(strValue) == true)
            {
                nValue = nDefaultValue;
                return 1;
            }

            try
            {
                nValue = Convert.ToInt32(strValue);
            }
            catch (Exception ex)
            {
                strError = "属性 " + strParamName + " 的值应当为数值型。出错信息: " + ex.Message;
                return -1;
            }

            return 0;
        }

        // 获得整数型的属性参数值
        // return:
        //      -1  出错。但是nValue中已经有了nDefaultValue值，可以不加警告而直接使用
        //      0   正常获得明确定义的参数值
        //      1   参数没有定义，因此代替以缺省参数值返回
        public static int GetIntegerParam(this XmlElement node,
            string strParamName,
            long nDefaultValue,
            out long nValue,
            out string strError)
        {
            strError = "";
            nValue = nDefaultValue;

            string strValue = node.GetAttribute(strParamName);

            if (String.IsNullOrEmpty(strValue) == true)
            {
                nValue = nDefaultValue;
                return 1;
            }

            try
            {
                nValue = Convert.ToInt64(strValue);
            }
            catch (Exception ex)
            {
                strError = "属性 " + strParamName + " 的值应当为数值型。出错信息: " + ex.Message;
                return -1;
            }

            return 0;
        }

        // 获得浮点数型的属性参数值
        // return:
        //      -1  出错。但是nValue中已经有了nDefaultValue值，可以不加警告而直接使用
        //      0   正常获得明确定义的参数值
        //      1   参数没有定义，因此代替以缺省参数值返回
        public static int GetDoubleParam(this XmlElement node,
            string strParamName,
            double nDefaultValue,
            out double nValue,
            out string strError)
        {
            strError = "";
            nValue = nDefaultValue;

            string strValue = node.GetAttribute(strParamName);

            if (String.IsNullOrEmpty(strValue) == true)
            {
                nValue = nDefaultValue;
                return 1;
            }

            try
            {
                nValue = Convert.ToDouble(strValue);
            }
            catch (Exception ex)
            {
                strError = "属性 " + strParamName + " 的值应当为(浮点)数值型。出错信息: " + ex.Message;
                return -1;
            }

            return 0;
        }

    }
}