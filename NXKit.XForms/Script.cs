using System;
using System.Linq;
using System.Diagnostics.Contracts;
using NXKit.DOMEvents;
using NXKit.Scripting;

namespace NXKit.XForms
{

    /// <summary>
    /// XForms 2.0 script tag.
    /// </summary>
    [NXElement("{http://www.w3.org/2002/xforms}script")]
    [Public]
    public class Script :
        IAction
    {

        readonly NXElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Script(NXElement element)
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
            var code = string.Join("", element.Nodes()
                .OfType<NXText>()
                .Select(i => i.Value));
            if (code == null)
                return;

            // execute script
            DocumentScript.Execute(Type, code);
        }

        public void Handle(Event evt)
        {
            Invoke();
        }

    }

}
