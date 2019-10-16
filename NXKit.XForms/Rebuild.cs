using System;
using System.Xml.Linq;

using NXKit.DOMEvents;
using NXKit.XMLEvents;

namespace NXKit.XForms
{

    [Extension("{http://www.w3.org/2002/xforms}rebuild")]
    public class Rebuild :
        ElementExtension,
        IEventHandler
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Rebuild(XElement element)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
        }

        public void HandleEvent(Event ev)
        {
            Invoke();
        }

        public void Invoke()
        {
            throw new NotImplementedException();

            //var modelAttr = Module.GetAttributeValue(Xml, "model");
            //if (modelAttr != null)
            //{
            //    var element = (NXElement)ResolveId(modelAttr);
            //    if (element != null)
            //        element.Interface<Model>().OnRebuild();
            //    else
            //    {
            //        this.Interface<INXEventTarget>().DispatchEvent(Events.BindingException);
            //        return;
            //    }
            //}
        }

    }

}
