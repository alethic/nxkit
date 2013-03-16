using System;

namespace ISIS.Forms.Web.UI
{

    public class VisualControlAddedEventArgs : EventArgs
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="control"></param>
        internal VisualControlAddedEventArgs(VisualControl control)
        {
            Control = control;
        }

        /// <summary>
        /// Gets a reference to the control that was just created.
        /// </summary>
        public VisualControl Control { get; private set; }

    }

}
