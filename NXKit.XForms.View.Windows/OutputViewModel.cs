using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.View.Windows;
using NXKit.Xml;

namespace NXKit.XForms.View.Windows
{

    public class OutputViewModel :
        ElementViewModel
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public OutputViewModel(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        public string Value
        {
            get { return GetValue(); }
        }

        string GetValue()
        {
            return Element.Interface<Output>().Value;
        }

    }

}
