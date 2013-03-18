using System.IO;
using System.Linq;

namespace NXKit.XForms
{

    [Visual("alert")]
    public class XFormsAlertVisual : XFormsSingleNodeBindingVisual, ITextVisual
    {

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
