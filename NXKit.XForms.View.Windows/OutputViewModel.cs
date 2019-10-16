using System;
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
            if (element is null)
                throw new ArgumentNullException(nameof(element));
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
