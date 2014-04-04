using System;
using System.Diagnostics.Contracts;

using NXKit.DOMEvents;

namespace NXKit.XForms
{

    [NXElementInterface("{}refresh")]
    public class Refresh :
        IAction
    {

        readonly NXElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Refresh(NXElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
        }

        /// <summary>
        /// Gets the associated element.
        /// </summary>
        public NXElement Element
        {
            get { return element; }
        }

        /// <summary>
        /// Gets the XForms module.
        /// </summary>
        XFormsModule Module
        {
            get { return element.Document.Module<XFormsModule>(); }
        }

        public void Handle(Event ev)
        {
            Invoke();
        }

        public void Invoke()
        {
            var modelAttr = Module.GetAttributeValue(Element.Xml, "model");
            if (modelAttr != null)
            {
                var element = Element.ResolveId(modelAttr);
                if (element != null)
                    element.Interface<Model>().OnRefresh();
                else
                {
                    Element.Interface<INXEventTarget>().DispatchEvent(Events.BindingException);
                    return;
                }
            }
        }

    }

}
