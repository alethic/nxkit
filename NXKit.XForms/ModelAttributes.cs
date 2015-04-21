using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'model' attributes.
    /// </summary>
    [Extension(typeof(ModelAttributes), "{http://www.w3.org/2002/xforms}model")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class ModelAttributes :
        AttributeAccessor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public ModelAttributes(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        /// <summary>
        /// Gets the 'version' attribute values.
        /// </summary>
        public string Version
        {
            get { return GetAttributeValue("version"); }
        }

        public string Schema
        {
            get { return GetAttributeValue("schema"); }
        }

        public string Functions
        {
            get { return GetAttributeValue("functions"); }
        }

    }

}