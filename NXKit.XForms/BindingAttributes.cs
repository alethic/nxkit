using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the standard XForms binding attributes.
    /// </summary>
    [Extension(typeof(BindingAttributes))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class BindingAttributes :
        AttributeAccessor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public BindingAttributes(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        /// <summary>
        /// Gets the 'ref' attribute values.
        /// </summary>
        public string Ref
        {
            get { return GetAttributeValue("ref"); }
        }

        /// <summary>
        /// Gets the 'nodeset' attribute values.
        /// </summary>
        public string NodeSet
        {
            get { return GetAttributeValue("nodeset"); }
        }

        /// <summary>
        /// Gets the 'bind' attribute value.
        /// </summary>
        public string Bind
        {
            get { return GetAttributeValue("bind"); }
        }

    }

}