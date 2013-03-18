using System.Collections.Generic;

namespace NXKit.XForms
{

    [Visual("help")]
    public class XFormsHelpVisual : XFormsSingleNodeBindingVisual
    {

        protected override IEnumerable<Visual> CreateChildren()
        {
            return CreateElementChildren(Element, includeTextContent: true);
        }

    }

}
