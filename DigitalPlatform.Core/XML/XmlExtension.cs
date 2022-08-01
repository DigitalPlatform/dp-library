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
    }
}