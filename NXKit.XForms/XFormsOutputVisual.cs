using System.IO;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [Visual("output")]
    public class XFormsOutputVisual : 
        XFormsSingleNodeBindingVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public XFormsOutputVisual(XElement element)
            : base(element)
        {

        }

        public void WriteText(TextWriter w)
        {
            w.Write((Binding != null ? Binding.Value : null) ?? "");
        }

    }

}
