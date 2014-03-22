using NXKit.DOMEvents;

namespace NXKit
{

    public class VisualRemovedEvent : 
        Event
    {

        public static readonly string Name = "visual-removed";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public VisualRemovedEvent()
            : base(Name, true, false)
        {

        }

    }

}
