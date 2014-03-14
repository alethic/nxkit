using System.Collections.Generic;

namespace NXKit.XForms
{

    /// <summary>
    /// The author-optional element help provides a convenient way to attach help information to a form control.
    /// </summary>
    [Visual("help")]
    public class XFormsHelpVisual :
        XFormsSingleNodeBindingVisual,
        ISupportsCommonAttributes
    {

        protected override IEnumerable<Visual> CreateVisuals()
        {
            return CreateElementVisuals(Element, includeTextContent: true);
        }

    }

}
