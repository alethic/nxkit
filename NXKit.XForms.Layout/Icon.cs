using System;
using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Extension("{http://schemas.nxkit.org/2014/xforms-layout}icon")]
    [Extension(typeof(IRemote), "{http://schemas.nxkit.org/2014/xforms-layout}icon")]
    [Remote]
    public class Icon :
        ElementExtension,
        IRemote
    {

        readonly IconAttributes attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Icon(
            XElement element,
            IconAttributes attributes)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.attributes = attributes;
        }

        /// <summary>
        /// Gets the icon name.
        /// </summary>
        [Remote]
        public string Name
        {
            get { return attributes.Name; }
        }

    }

}
