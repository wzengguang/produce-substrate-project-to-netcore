using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SubstrateUtils.Extensions
{
    public static class XElementExtensions
    {
        public static XElement GetFirstChild(this XElement element, string name)
        {
            if (element == null)
                return null;
            return element.Elements().FirstOrDefault(a => a.Name.LocalName == name);
        }

        public static IEnumerable<XElement> GetChildren(this XElement element, string name)
        {
            if (element == null)
                return null;
            return element.Elements().Where(a => a.Name.LocalName == name);
        }

        public static XElement GetFirstDescendent(this XElement element, string name)
        {
            if (element == null)
                return null;
            return element.Descendants().FirstOrDefault(a => a.Name.LocalName == name);
        }

        public static IEnumerable<XElement> GetDescendents(this XElement element, string name)
        {
            if (element == null)
                return null;
            return element.Descendants().Where(a => a.Name.LocalName == name);
        }

        public static XElement GetFirstChildByAttribute(this XElement element, string elementName, string name, bool and = true, params string[] values)
        {
            if (element != null)
            {
                foreach (var child in element.GetChildren(elementName))
                {
                    var result = child.CheckAttribute(name, and, values);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return null;
        }

        public static IEnumerable<XElement> GetChildrenByAttribute(this XElement element, string elementName, string name, bool and = true, params string[] values)
        {
            if (element != null)
            {
                foreach (var child in element.GetChildren(elementName))
                {
                    var result = child.CheckAttribute(name, and, values);
                    if (result != null)
                    {
                        yield return result;
                    }
                }
            }
        }

        public static IEnumerable<XElement> GetDescendentsByAttribute(this XElement element, string elementName, string name, bool and = true, params string[] values)
        {
            if (element != null)
            {
                foreach (var child in element.GetDescendents(elementName))
                {
                    var result = child.CheckAttribute(name, and, values);
                    if (result != null)
                    {
                        yield return result;
                    }
                }
            }
        }

        public static XElement GetFirstDescendentByAttribute(this XElement element, string elementName, string name, bool and = true, params string[] values)
        {
            if (element != null)
            {
                foreach (var child in element.GetDescendents(elementName))
                {
                    var result = child.CheckAttribute(name, and, values);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return null;
        }

        public static XElement CheckAttribute(this XElement element, string name, bool and = true, params string[] values)
        {
            foreach (var attr in element.Attributes())
            {
                if (attr.Name.LocalName.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    if (values.Length > 0)
                    {
                        if (and)
                        {
                            foreach (var v in values)
                            {
                                if (!attr.Value.Contains(v, StringComparison.OrdinalIgnoreCase))
                                {
                                    return null;
                                }
                            }

                            return element;
                        }
                        else
                        {
                            foreach (var v in values)
                            {
                                if (attr.Value.Contains(v, StringComparison.OrdinalIgnoreCase))
                                {
                                    return element;
                                }
                            }
                        }

                    }
                    else
                    {
                        return element;
                    }
                }
            }

            return null;
        }

        public static string GetAttribute(this XElement element, string name)
        {
            foreach (var attr in element.Attributes())
            {
                if (attr.Name.LocalName.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return attr.Value;
                }
            }

            return null;
        }
    }
}