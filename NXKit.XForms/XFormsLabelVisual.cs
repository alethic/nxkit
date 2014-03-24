﻿using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// This element provides a descriptive label for the containing form control. The descriptive label can be
    /// presented visually and made available to accessibility software so the visually-impaired user can obtain a
    /// short description of form controls while navigating among them.
    /// </summary>
    [Visual("label")]
    public class XFormsLabelVisual :
        XFormsSingleNodeBindingVisual,
        ISupportsCommonAttributes
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public XFormsLabelVisual(XElement element)
            : base(element)
        {

        }

    }

}
