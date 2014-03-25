using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using NXKit.DOMEvents;

namespace NXKit.XmlEvents
{

    /// <summary>
    /// Introduces XML events support into the NXKit model.
    /// </summary>
    public class EventsModule :
        Module
    {

        class EventListener :
            IEventListener
        {

            IEventHandler handler;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="observer" />
            /// <param name="target" />
            /// <param name="handler" />
            public EventListener(NXNode observer, NXNode target, IEventHandler handler)
            {
                Contract.Requires<ArgumentNullException>(handler != null);

                this.handler = handler;
            }

            public void HandleEvent(Event ev)
            {
                handler.Handle(ev);
            }

        }

        public override NXNode CreateNode(XNode node)
        {
            var element = node as XElement;
            if (element == null)
                return null;

            if (element.Name.Namespace != SchemaConstants.Events_1_0)
                return null;

            if (element.Name.LocalName == "listener")
                return new EventsEventListenerElement(element);

            return null;
        }

        //public override void AnnotateNode(NXNode visual)
        //{
        //    string eventAttr = null;
        //    string observerAttr = null;
        //    string targetAttr = null;
        //    string handlerAttr = null;
        //    string phaseAttr = null;
        //    string propagateAttr = null;
        //    string defaultActionAttr = null;

        //    if (visual is EventsEventListenerVisual)
        //    {
        //        var listenerVisual = (EventsEventListenerVisual)visual;
        //        eventAttr = (string)listenerVisual.Xml.Attribute("event");

        //        // required attribute for events
        //        if (eventAttr == null)
        //            return;

        //        observerAttr = (string)listenerVisual.Xml.Attribute("observer");
        //        targetAttr = (string)listenerVisual.Xml.Attribute("target");
        //        handlerAttr = (string)listenerVisual.Xml.Attribute("handler");
        //        phaseAttr = (string)listenerVisual.Xml.Attribute("phase");
        //        propagateAttr = (string)listenerVisual.Xml.Attribute("propagate");
        //        defaultActionAttr = (string)listenerVisual.Xml.Attribute("defaultAction");
        //    }
        //    else if (visual.Xml is XElement)
        //    {
        //        var element = (XElement)visual.Xml;
        //        eventAttr = (string)element.Attribute(SchemaConstants.Events_1_0 + "event");

        //        // required attribute for events
        //        if (eventAttr == null)
        //            return;

        //        observerAttr = (string)element.Attribute(SchemaConstants.Events_1_0 + "observer");
        //        targetAttr = (string)element.Attribute(SchemaConstants.Events_1_0 + "target");
        //        handlerAttr = (string)element.Attribute(SchemaConstants.Events_1_0 + "handler");
        //        phaseAttr = (string)element.Attribute(SchemaConstants.Events_1_0 + "phase");
        //        propagateAttr = (string)element.Attribute(SchemaConstants.Events_1_0 + "propagate");
        //        defaultActionAttr = (string)element.Attribute(SchemaConstants.Events_1_0 + "defaultAction");
        //    }

        //    var observer = observerAttr != null ? visual.ResolveId(observerAttr) : null;
        //    var target = targetAttr != null ? visual.ResolveId(targetAttr) : null;
        //    var handler = (handlerAttr != null && handlerAttr.StartsWith("#") ? visual.ResolveId(handlerAttr.TrimStart('#')) : null).Interface<IEventHandler>();
        //    var capture = phaseAttr == "capture";
        //    var propagate = propagateAttr != "stop";
        //    var defaultAction = defaultActionAttr != "cancel";

        //    if (observer != null && handler == null)
        //        handler = visual.Interface<IEventHandler>();
        //    else if (observer == null && handler != null)
        //        observer = visual;
        //    else if (observer == null && handler == null)
        //    {
        //        handler = visual.Interface<IEventHandler>();
        //        observer = visual.Parent;
        //    }

        //    if (handler != null)
        //        observer.Interface<IEventTarget>().AddEventListener(
        //            eventAttr,
        //            new EventListener(observer, target, handler),
        //            capture);
        //}

        public override bool Invoke()
        {
            return false;
        }

    }

}