using System;
using System.Xml.Linq;

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
            if (element is null)
                throw new ArgumentNullException(nameof(element));
        }

    }

}
