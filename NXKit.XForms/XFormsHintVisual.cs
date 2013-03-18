using System.Collections.Generic;

namespace NXKit.XForms
{

    [Visual("hint")]
    public class XFormsHintVisual : XFormsSingleNodeBindingVisual
    {

        protected override IEnumerable<Visual> CreateChildren()
        {
            return CreateElementChildren(Element, includeTextContent: true);
        }

    }

}
