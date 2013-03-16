namespace ISIS.Forms.XForms
{

    public abstract class XFormsEvent : Event
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bubbles"></param>
        /// <param name="cancelable"></param>
        public XFormsEvent(string name, bool bubbles, bool cancelable)
            : base(name, bubbles, cancelable)
        {

        }

    }

}
