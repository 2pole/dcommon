using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DCommon
{
    public static class XmlNodeExtensions
    {
        public static XmlElement AppendChild(this XmlNode node, string elementName)
        {
            return node.AppendChild(elementName, null);
        }

        public static XmlElement AppendChild(this XmlNode node, string elementName, string value)
        {
            var doc = node.GetXmlDocument();
            var element = doc.CreateElement(elementName);
            if (value != null)
                element.InnerText = value;
            node.AppendChild(element);
            return element;
        }

        public static XmlElement AppendSibling(this XmlNode node, string elementName)
        {
            return node.AppendSibling(elementName, null);
        }

        public static XmlElement AppendSibling(this XmlNode node, string elementName, string value)
        {
            return node.ParentNode.AppendChild(elementName, value);
        }

        public static XmlNode AppendClonedNode(this XmlNode node, XmlNode child)
        {
            var xmlDoc = node.GetXmlDocument();
            var clonedNode = xmlDoc.ImportNode(child, true);
            return node.AppendChild(clonedNode);
        }

        public static XmlElement Attr(this XmlElement node, string name, string value)
        {
            node.SetAttribute(name, value);
            return node;
        }

        public static string SafeValue(this XmlNode node)
        {
            if (node == null)
                return null;

            return node.Value ?? node.InnerXml;
        }

        public static XmlDocument GetXmlDocument(this XmlNode node)
        {
            return (node as XmlDocument ?? node.OwnerDocument);
        }

        public static XmlDocument ToXmlDocument(this XmlNode node)
        {
            var doc = new XmlDocument();
            var clonedNode = doc.ImportNode(node, true);
            doc.AppendChild(clonedNode);
            return doc;
        }

        public static XmlDocument LoadXml(this string xml)
        {
            var xmlDoc = new XmlDocument();
            if (!string.IsNullOrEmpty(xml))
                xmlDoc.LoadXml(xml);
            return xmlDoc;
        }
    }
}
