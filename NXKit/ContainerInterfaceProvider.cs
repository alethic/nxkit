using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit
{

    [Export(typeof(IInterfaceProvider))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class ContainerInterfaceProvider :
        IInterfaceProvider
    {

        readonly XObject obj;
        readonly IEnumerable<Lazy<IExtension<XDocument>>> documentExtensions;
        readonly IEnumerable<Lazy<IExtension<XElement>, IDictionary<string, object>>> elementExtensions;
        readonly IEnumerable<Lazy<IExtension<XText>>> textExtensions;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="predicates"></param>
        [ImportingConstructor]
        public ContainerInterfaceProvider(
            XObject obj,
            [ImportMany] IEnumerable<Lazy<IExtension<XDocument>>> documentExtensions,
            [ImportMany] IEnumerable<Lazy<IExtension<XElement>, IDictionary<string, object>>> elementExtensions,
            [ImportMany] IEnumerable<Lazy<IExtension<XText>>> textExtensions)
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            Contract.Requires<ArgumentNullException>(documentExtensions != null);
            Contract.Requires<ArgumentNullException>(elementExtensions != null);
            Contract.Requires<ArgumentNullException>(textExtensions != null);

            this.obj = obj;
            this.documentExtensions = documentExtensions;
            this.elementExtensions = elementExtensions;
            this.textExtensions = textExtensions;
        }

        public IEnumerable<object> GetInterfaces(XObject obj)
        {
            if (obj is XDocument)
                foreach (var i in documentExtensions)
                    if (i.Value != null)
                        yield return i.Value;
            if (obj is XElement)
                foreach (var i in elementExtensions)
                    if (ElementPredicate(i.Metadata))
                        if (i.Value != null)
                            yield return i.Value;
            if (obj is XText)
                foreach (var i in textExtensions)
                    if (i.Value != null)
                        yield return i.Value;
        }

        bool ElementPredicate(IDictionary<string, object> metadata)
        {
            return true;
        }

    }

}
