using System.Xml.Linq;

namespace XEngine.Forms.Events
{

    [VisualTypeDescriptor(Constants.Events_1_0_NS, "listener")]
    public class EventsEventListenerVisualTypeDescriptor : VisualTypeDescriptor
    {

        public override Visual CreateVisual(IFormProcessor form, StructuralVisual parent, XNode node)
        {
            return new EventsEventListenerVisual(parent, (XElement)node);
        }

    }

    public class EventsEventListenerVisual : StructuralVisual, IEventHandlerVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public EventsEventListenerVisual(StructuralVisual parent, XElement element)
            : base(parent, element)
        {

        }

        public override string Id
        {
            get { return Form.GetElementId(Element); }
        }

        public void Handle(Event ev)
        {

        }

        public XElement Element
        {
            get { return (XElement)Node; }
        }

    }

}
