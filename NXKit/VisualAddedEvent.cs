namespace NXKit
{

    public class VisualAddedEvent : Event
    {

        public static readonly string Name = "visual-added";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public VisualAddedEvent()
            : base(Name, true, false)
        {

        }

    }

}
