using System.ComponentModel.Composition;
using System.Xml.Linq;

namespace NXKit.Events
{

    [Module]
    public class EventsModule : Module
    {

        private class EventListener : IEventListener
        {

            private IEventHandlerVisual handler;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="observer" />
            /// <param name="target" />
            /// <param name="handler" />
            public EventListener(Visual observer, Visual target, IEventHandlerVisual handler)
            {
                this.handler = handler;
            }

            public void HandleEvent(Event ev)
            {
                handler.Handle(ev);
            }

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        [ImportingConstructor]
        public EventsModule([Import(typeof(FormProcessor))] FormProcessor form)
            : base(form)
        {

        }

        public override bool Run()
        {
            return false;
        }

        public override void AnnotateVisual(Visual visual)
        {
            string eventAttr = null;
            string observerAttr = null;
            string targetAttr = null;
            string handlerAttr = null;
            string phaseAttr = null;
            string propagateAttr = null;
            string defaultActionAttr = null;

            if (visual is EventsEventListenerVisual)
            {
                var listenerVisual = (EventsEventListenerVisual)visual;
                eventAttr = (string)listenerVisual.Element.Attribute("event");

                // required attribute for events
                if (eventAttr == null)
                    return;

                observerAttr = (string)listenerVisual.Element.Attribute("observer");
                targetAttr = (string)listenerVisual.Element.Attribute("target");
                handlerAttr = (string)listenerVisual.Element.Attribute("handler");
                phaseAttr = (string)listenerVisual.Element.Attribute("phase");
                propagateAttr = (string)listenerVisual.Element.Attribute("propagate");
                defaultActionAttr = (string)listenerVisual.Element.Attribute("defaultAction");
            }
            else if (visual.Node is XElement)
            {
                var element = (XElement)visual.Node;
                eventAttr = (string)element.Attribute(Constants.Events_1_0 + "event");

                // required attribute for events
                if (eventAttr == null)
                    return;

                observerAttr = (string)element.Attribute(Constants.Events_1_0 + "observer");
                targetAttr = (string)element.Attribute(Constants.Events_1_0 + "target");
                handlerAttr = (string)element.Attribute(Constants.Events_1_0 + "handler");
                phaseAttr = (string)element.Attribute(Constants.Events_1_0 + "phase");
                propagateAttr = (string)element.Attribute(Constants.Events_1_0 + "propagate");
                defaultActionAttr = (string)element.Attribute(Constants.Events_1_0 + "defaultAction");
            }

            var observer = observerAttr != null ? visual.ResolveId(observerAttr) : null;
            var target = targetAttr != null ? visual.ResolveId(targetAttr) : null;
            var handler = (handlerAttr != null && handlerAttr.StartsWith("#") ? visual.ResolveId(handlerAttr.TrimStart('#')) : null) as IEventHandlerVisual;
            var capture = phaseAttr == "capture";
            var propagate = propagateAttr != "stop";
            var defaultAction = defaultActionAttr != "cancel";

            if (observer != null && handler == null)
                handler = visual as IEventHandlerVisual;
            else if (observer == null && handler != null)
                observer = visual;
            else if (observer == null && handler == null)
            {
                handler = visual as IEventHandlerVisual;
                observer = visual.Parent;
            }

            if (handler != null)
                observer.AddEventListener(eventAttr, new EventListener(observer, target, handler), capture);
        }

    }

}