using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [Visual("group")]
    public class XFormsGroupVisual :
        XFormsSingleNodeBindingVisual,
        ISupportsUiCommonAttributes,
        IRelevancyScope, 
        INavigationCategoryVisual
    {

        protected override IEnumerable<Visual> CreateChildren()
        {
            return CreateElementChildren(Element);
        }

        public string Label
        {
            get { return GetLabel(); }
        }

        string GetLabel()
        {
            var b = new StringWriter();
            var l = Children.OfType<XFormsLabelVisual>().FirstOrDefault();
            if (l != null)
                l.WriteText(b);
            else
                return null;

            return b.ToString();
        }

    }

}
