using System.IO;

namespace NXKit.XForms
{

    [Visual("output")]
    public class XFormsOutputVisual : XFormsSingleNodeBindingVisual, ITextVisual
    {

        public void WriteText(TextWriter w)
        {
            w.Write((Binding != null ? Binding.Value : null) ?? "");
        }

    }

}
