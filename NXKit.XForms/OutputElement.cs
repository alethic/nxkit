using System.IO;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [Element("output")]
    public class OutputElement : 
        SingleNodeBindingElement
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public OutputElement(XElement element)
            : base(element)
        {

        }

        public void WriteText(TextWriter w)
        {
            w.Write((Binding != null ? Binding.Value : null) ?? "");
        }

    }

}
