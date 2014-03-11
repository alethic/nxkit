using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// This element provides a descriptive label for the containing form control. The descriptive label can be
    /// presented visually and made available to accessibility software so the visually-impaired user can obtain a
    /// short description of form controls while navigating among them.
    /// </summary>
    [Visual("label")]
    public class XFormsLabelVisual :
        XFormsSingleNodeBindingVisual,
        ISupportsCommonAttributes,
        ITextVisual
    {

        protected override IEnumerable<Visual> CreateChildren()
        {
            return CreateElementChildren(Element, includeTextContent: true);
        }

        public void WriteText(TextWriter w)
        {
            if (Binding != null)
                w.Write(Binding.Value ?? "");
            else
                foreach (var c in Children.OfType<ITextVisual>())
                    c.WriteText(w);
        }

    }

}
