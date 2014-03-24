using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [Visual("alert")]
    public class XFormsAlertVisual :
        XFormsSingleNodeBindingVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public XFormsAlertVisual(XElement element)
            : base(element)
        {

        }

        public void WriteText(TextWriter w)
        {
            if (Binding != null)
                w.Write(Binding.Value ?? "");
            else
                foreach (var c in Elements.OfType<NXText>())
                    c.WriteText(w);
        }

    }

}
