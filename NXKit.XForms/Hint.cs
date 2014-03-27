using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// The author-optional element hint provides a convenient way to attach hint information to a form control.
    /// </summary>
    [Element("hint")]
    public class Hint : 
        SingleNodeUIBindingElement,
        ISupportsCommonAttributes
    {
        
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Hint(XElement element)
            : base(element)
        {

        }

        [Interactive]
        public XName Appearance
        {
            get { return (string)Module.ResolveAttribute(Xml, "appearance"); }
        }

    }

}
