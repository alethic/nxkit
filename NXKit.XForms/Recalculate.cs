﻿using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.DOMEvents;
using NXKit.XMLEvents;

namespace NXKit.XForms
{

    [Interface("{http://www.w3.org/2002/xforms}recalculate")]
    public class Recalculate :
        ElementExtension,
        IEventHandler
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Recalculate(XElement element)
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
            throw new NotImplementedException();

            //var modelAttr = Module.GetAttributeValue(Xml, "model");
            //if (modelAttr != null)
            //{
            //    var element = (NXElement)ResolveId(modelAttr);
            //    if (element != null)
            //        element.Interface<Model>().OnRecalculate();
            //    else
            //    {
            //        this.Interface<INXEventTarget>().DispatchEvent(Events.BindingException);
            //        return;
            //    }
            //}
        }

    }

}
