using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NXKit.XForms
{

    [Visual("label")]
    public class XFormsLabelVisual : XFormsSingleNodeBindingVisual, ITextVisual
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
