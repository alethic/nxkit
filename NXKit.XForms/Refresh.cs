using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.DOMEvents;
using NXKit.Xml;

namespace NXKit.XForms
{

    [Interface("{http://www.w3.org/2002/xforms}refresh")]
    public class Refresh :
        IAction
    {

        readonly XElement element;
        readonly AttributeAccessor attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Refresh(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
        }

        public void Handle(Event ev)
        {
            Invoke();
        }

        public void Invoke()
        {
            var modelAttr = attributes.GetAttributeValue("model");
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
