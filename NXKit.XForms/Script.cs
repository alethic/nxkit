using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.DOMEvents;
using NXKit.Scripting;
using NXKit.Xml;
using NXKit.XMLEvents;

namespace NXKit.XForms
{

    /// <summary>
    /// XForms 2.0 script tag.
    /// </summary>
    [Interface("{http://www.w3.org/2002/xforms}script")]
    [Remote]
    public class Script :
        ElementExtension,
        IEventHandler
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Script(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        public string Type
        {
            get { return (string)Element.Attribute("type"); }
        }

        IDocumentScript DocumentScript
        {
            get { return Element.Document.Interface<IDocumentScript>(); }
        }

        public void Invoke()
        {
            DocumentScript.Execute(Type, Element.Value);
        }

        public void HandleEvent(Event evt)
        {
            Invoke();
        }

    }

}
