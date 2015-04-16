using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Input;
using System.Xml.Linq;

using NXKit.DOMEvents;
using NXKit.View.Windows;

namespace NXKit.XForms.View.Windows
{

    public class LabelViewModel :
        ElementViewModel
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public LabelViewModel(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

    }

}
