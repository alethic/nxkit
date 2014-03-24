using System.Xml.Linq;

namespace NXKit.XForms
{

    [Visual("trigger")]
    public class XFormsTriggerVisual :
        XFormsSingleNodeBindingVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public XFormsTriggerVisual(XElement element)
            : base(element)
        {

        }

    }

}
