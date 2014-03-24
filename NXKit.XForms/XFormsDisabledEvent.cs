﻿namespace NXKit.XForms
{

    public class XFormsDisabledEvent :
        XFormsEvent
    {

        public static readonly string Name = "xforms-disabled";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsDisabledEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
