using System;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// The author-optional element hint provides a convenient way to attach hint information to a form control.
    /// </summary>
    [Extension("{http://www.w3.org/2002/xforms}hint")]
    public class Hint :
        ElementExtension
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Hint(XElement element)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
        }

    }

}
