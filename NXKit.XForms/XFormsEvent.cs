using NXKit.DOMEvents;

namespace NXKit.XForms
{

    public abstract class XFormsEvent
    {

        readonly IEvent evt;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bubbles"></param>
        /// <param name="cancelable"></param>
        public XFormsEvent(Visual visual, string name, bool bubbles, bool cancelable)
        {
            this.evt = visual.Interface<IDocumentEvent>().CreateEvent(name);
            this.evt.InitEvent(name, bubbles, cancelable);
        }

        public IEvent Event
        {
            get { return evt; }
        }

    }

}
