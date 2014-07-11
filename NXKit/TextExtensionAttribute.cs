using System.Xml.Linq;

namespace NXKit
{

    public class TextExtensionAttribute :
        ObjectExtensionAttribute
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public TextExtensionAttribute()
            : base(ExtensionObjectType.Text)
        {

        }

    }

}
