using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms repeat extension attributes.
    /// </summary>
    [Extension]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class RepeatExtensionAttributes :
        AttributeAccessor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public RepeatExtensionAttributes(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        /// <summary>
        /// Returns <c>true</c> if the given element posesses any repeat extension attributes.
        /// </summary>
        /// <returns></returns>
        public bool HasAttributes()
        {
            return 
                Model != null ||
                Bind != null ||
                Ref != null ||
                NodeSet != null;
        }

        public string Model
        {
            get { return (string)GetAttribute("repeat-model"); }
        }

        public string Bind
        {
            get { return (string)GetAttribute("repeat-bind"); }
        }

        public string Ref
        {
            get { return (string)GetAttribute("repeat-ref"); }
        }

        public string NodeSet
        {
            get { return (string)GetAttribute("repeat-nodeset"); }
        }

        public int? StartIndex
        {
            get { return (int?)GetAttribute("repeat-startindex"); }
        }

        public int? Number
        {
            get { return (int?)GetAttribute("repeat-number"); }
        }

        public string IndexRef
        {
            get { return (string)GetAttribute("repeat-indexref"); }
        }

    }

}