using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.DOMEvents;
using NXKit.Scripting;
using NXKit.Xml;

namespace NXKit.XForms
{

    /// <summary>
    /// XForms 2.0 script tag.
    /// </summary>
    [Interface("{http://www.w3.org/2002/xforms}script")]
    [Remote]
    public class Script :
        IAction
    {

        readonly XElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Script(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
        }

        public string Type
        {
            get { return (string)element.Attribute("type"); }
        }

        IDocumentScript DocumentScript
        {
            get { return element.Document.Interface<IDocumentScript>(); }
        }

        public void Invoke()
        {
            DocumentScript.Execute(Type, element.Value);
        }

        public void Handle(Event evt)
        {
            Invoke();
        }

    }

}
