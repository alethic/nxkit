using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit
{

    [ScopeExport(typeof(IInterfaceProvider), Scope.Host)]
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
                return documentExtensions
                    .Select(i => i.Value);
            if (obj is XElement)
                return elementExtensions
                    .Where(i => ElementPredicate(i.Metadata))
                    .Select(i => i.Value);
            if (obj is XText)
                return textExtensions
                    .Select(i => i.Value);

        }

        bool ElementPredicate(IDictionary<string, object> metadata)
        {
            return true;
        }

    }

}
