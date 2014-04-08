using System;
using System.Xml.Linq;

using NXKit.DOMEvents;

namespace NXKit.XForms
{

    [Interface("{}send")]
    public class Send :
        IAction
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Send(XElement element)
        {

        }

        public void Handle(Event ev)
        {
            Invoke();
        }

        public void Invoke()
        {
            throw new NotImplementedException();

            //var submissionAttr = Module.GetAttributeValue(Xml, "submission");
            //if (submissionAttr != null)
            //{
            //    var submissionVisual = ResolveId(submissionAttr);
            //    if (submissionVisual != null)
            //        submissionVisual.Interface<INXEventTarget>().DispatchEvent(Events.Submit);
            //}
        }

    }

}
