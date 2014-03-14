using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NXKit.XForms
{

    [Visual("group")]
    public class XFormsGroupVisual :
        XFormsSingleNodeBindingVisual,
        ISupportsUiCommonAttributes,
        IRelevancyScope, 
        INavigationCategoryVisual
    {

        protected override IEnumerable<Visual> CreateVisuals()
        {
            return CreateElementVisuals(Element);
        }

        public string Label
        {
            get { return GetLabel(); }
        }

        string GetLabel()
        {
            var b = new StringWriter();
            var l = Visuals.OfType<XFormsLabelVisual>().FirstOrDefault();
            if (l != null)
                l.WriteText(b);
            else
                return null;

            return b.ToString();
        }

    }

}
