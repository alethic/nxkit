using System.Xml.Linq;

namespace NXKit
{

    public class DocumentExtensionAttribute :
        ObjectExtensionAttribute
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public DocumentExtensionAttribute()
            : base(ExtensionObjectType.Document)
        {

        }

    }

}
