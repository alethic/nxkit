using System;
using System.Xml.Linq;

using NXKit.View.Razor;

namespace NXKit.XForms.Layout.Razor
{

    [Extension("{http://schemas.nxkit.org/2014/xforms-layout}form")]
    [Extension("{http://schemas.nxkit.org/2014/xforms-layout}section")]
    public class ComponentTypeProvider : INXComponentTypeProvider
    {

        public Type GetComponentType(XElement element)
        {
            switch (element.Name.LocalName)
            {
                case "form":
                    return typeof(NXKit.XForms.Layout.Razor.Form);
                case "section":
                    return typeof(NXKit.XForms.Layout.Razor.Section);
                default:
                    return null;
            }
        }

    }

}
