using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.DOMEvents;
using NXKit.Xml;

namespace NXKit.XForms
{

    [Interface("{http://www.w3.org/2002/xforms}refresh")]
    public class Refresh :
        ElementExtension,
        IAction
    {

        readonly AttributeAccessor attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Refresh(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
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
                var model = Element.ResolveId(modelAttr);
                if (model != null)
                    model.Interface<Model>().OnRefresh();
                else
                {
                    Element.Interface<INXEventTarget>().DispatchEvent(Events.BindingException);
                    return;
                }
            }
        }

    }

}
