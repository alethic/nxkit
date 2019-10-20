using System;
using System.Xml.Linq;

namespace NXKit.AspNetCore.Components.XHtml
{

    [Extension(typeof(INXComponentTypeProvider))]
    public class XHtmlComponentTypeProvider : AssemblyComponentTypeProviderBase
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XHtmlComponentTypeProvider() :
            base(typeof(XHtmlComponentTypeProvider).Assembly)
        {

        }

        public override Type GetComponentType(XElement element)
        {
            if (element.Name.Namespace == "http://www.w3.org/1999/xhtml")
                return base.GetComponentType(element) ?? typeof(GenericComponent);
            else
                return null;
        }

    }

}
