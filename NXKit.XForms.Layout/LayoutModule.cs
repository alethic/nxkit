using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

using NXKit.Util;

namespace NXKit.XForms.Layout
{

    public class LayoutModule : Module
    {

        /// <summary>
        /// Map of <see cref="XName"/> to <see cref="NXNode"/> type.
        /// </summary>
        static readonly Dictionary<XName, Type> visualTypeMap = typeof(LayoutModule).Assembly.GetTypes()
            .Select(i => new { Type = i, Attribute = i.GetCustomAttribute<ElementAttribute>() })
            .Where(i => i.Attribute != null)
            .ToDictionary(i => Constants.Layout_1_0 + i.Attribute.Name, i => i.Type);

        public override Type[] DependsOn
        {
            get
            {
                return new[]
                {
                    typeof(XFormsModule)
                };
            }
        }

        public override NXNode CreateNode(XNode node)
        {
            var element = node as XElement;
            if (element == null)
                return null;

            if (element.Name.Namespace != Constants.Layout_1_0)
                return null;

            var type = visualTypeMap.GetOrDefault(element.Name);
            if (type == null)
                return null;

            return (NXNode)Activator.CreateInstance(type, new[] { node });
        }

        /// <summary>
        /// Resolves the XForms node for attribute <paramref name="name"/> on <paramref name="element"/>.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        internal XAttribute ResolveAttribute(XElement element, string name)
        {
            if (element.Name.Namespace == Constants.Layout_1_0)
                // only layout native elements support default-ns attributes
                return element.Attribute(Constants.Layout_1_0 + name) ?? element.Attribute(name);
            else
                // non-layout native elements must be prefixed
                return element.Attribute(Constants.Layout_1_0 + name);
        }

        /// <summary>
        /// Gets the XForms attribute value <paramref name="name"/> on <paramref name="element"/>.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        internal string GetAttributeValue(XElement element, string name)
        {
            var attr = ResolveAttribute(element, name);
            return attr != null ? (string)attr : null;
        }

    }

}