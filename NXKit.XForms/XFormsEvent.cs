using NXKit.DOMEvents;

namespace NXKit.XForms
{

    public abstract class XFormsEvent
    {

        readonly Event evt;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bubbles"></param>
        /// <param name="cancelable"></param>
        public XFormsEvent(NXNode visual, string name, bool bubbles, bool cancelable)
        {
            this.evt = visual.Document.Interface<IDocumentEvent>().CreateEvent(name);
            this.evt.InitEvent(name, bubbles, cancelable);
        }

        public Event Event
        {
            get { return evt; }
        }

    }

}
