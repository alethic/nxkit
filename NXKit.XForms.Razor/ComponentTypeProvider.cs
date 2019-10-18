using System;
using System.Xml.Linq;

using NXKit.View.Razor;

namespace NXKit.XForms.Razor
{

    [Extension("{http://www.w3.org/2002/xforms}model")]
    [Extension("{http://www.w3.org/2002/xforms}group")]
    [Extension("{http://www.w3.org/2002/xforms}label")]
    [Extension("{http://www.w3.org/2002/xforms}input")]
    public class ComponentTypeProvider : INXComponentTypeProvider
    {

        public Type GetComponentType(XElement element)
        {
            switch (element.Name.LocalName)
            {
                case "model":
                    return typeof(NXKit.XForms.Razor.Model);
                case "group":
                    return typeof(NXKit.XForms.Razor.Group);
                case "label":
                    return typeof(NXKit.XForms.Razor.Label);
                case "input":
                    return typeof(NXKit.XForms.Razor.Input);
                default:
                    return null;
            }
        }

    }

}
