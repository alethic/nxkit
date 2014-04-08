using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.DOMEvents;

namespace NXKit.XForms
{

    [NXElementInterface("{http://www.w3.org/2002/xforms}refresh")]
    public class Refresh :
        IAction
    {

        readonly XElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Refresh(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
        }

        /// <summary>
        /// Gets the XForms module.
        /// </summary>
        XFormsModule Module
        {
            get { return element.Host().Module<XFormsModule>(); }
        }

        public void Handle(Event ev)
        {
            Invoke();
        }

        public void Invoke()
        {
            var modelAttr = Module.GetAttributeValue(element, "model");
            if (modelAttr != null)
            {
                var model = element.ResolveId(modelAttr);
                if (model != null)
                    model.Interface<Model>().OnRefresh();
                else
                {
                    element.Interface<INXEventTarget>().DispatchEvent(Events.BindingException);
                    return;
                }
            }
        }

    }

}
