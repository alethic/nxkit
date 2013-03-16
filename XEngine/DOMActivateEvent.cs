namespace ISIS.Forms
{

    public class DOMActivateEvent : Event
    {

        public static readonly string Name = "DOMActivate";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public DOMActivateEvent()
            : base(Name, true, true)
        {

        }

    }

}
