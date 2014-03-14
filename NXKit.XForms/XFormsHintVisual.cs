using System.Collections.Generic;

namespace NXKit.XForms
{

    /// <summary>
    /// The author-optional element hint provides a convenient way to attach hint information to a form control.
    /// </summary>
    [Visual("hint")]
    public class XFormsHintVisual : 
        XFormsSingleNodeBindingVisual,
        ISupportsCommonAttributes
    {

        protected override IEnumerable<Visual> CreateVisuals()
        {
            return CreateElementVisuals(Element, includeTextContent: true);
        }

    }

}
