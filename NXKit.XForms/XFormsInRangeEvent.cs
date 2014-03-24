﻿namespace NXKit.XForms
{

    public class XFormsInRangeEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-in-range";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsInRangeEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
