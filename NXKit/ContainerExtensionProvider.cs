using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;
using NXKit.Composition;
using NXKit.Util;

namespace NXKit
{

    public interface IExtension<TObject, TExtension>
        where TObject : XObject
        where TExtension : class
    {

        TExtension Value { get; }

    }

    /// <summary>
    /// Exports <see cref="XElement"/> extensions.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Export(typeof(IExtension<,>))]
    public class ElementExtensionExport<T> :
        IExtension<XElement, T>
        where T : class
    {

        readonly XObject obj;
        readonly IEnumerable<IInterfaceProvider> providers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="obj"></param>
        [ImportingConstructor]
        public ElementExtensionExport(
            [ImportMany] IEnumerable<IInterfaceProvider> providers)
        {
            Contract.Requires<ArgumentNullException>(providers != null);

            this.obj = null;
            this.providers = providers;
        }

        public T Value
        {
            get { return providers.SelectMany(i => i.GetInterfaces(obj)).OfType<T>().FirstOrDefault(); }
        }

    }

    [Export(typeof(IInterfaceProvider))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class ContainerExtensionProvider :
        IInterfaceProvider
    {

        readonly XObject obj;
        readonly IEnumerable<Lazy<IExtension<XDocument>, IDictionary<string, object>>> documentExtensions;
        readonly IEnumerable<Lazy<IExtension<XElement>, IDictionary<string, object>>> elementExtensions;
        readonly IEnumerable<Lazy<IExtension<XText>, IDictionary<string, object>>> textExtensions;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="predicates"></param>
        [ImportingConstructor]
        public ContainerExtensionProvider(
            XObject obj,
            [ImportMany] IEnumerable<Lazy<IExtension<XDocument>, IDictionary<string, object>>> documentExtensions,
            [ImportMany] IEnumerable<Lazy<IExtension<XElement>, IDictionary<string, object>>> elementExtensions,
            [ImportMany] IEnumerable<Lazy<IExtension<XText>, IDictionary<string, object>>> textExtensions)
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
            // only applicable to this object
            if (obj != this.obj)
                yield break;

            // document extensions
            if (obj is XDocument)
                foreach (var i in documentExtensions)
                    if (i.Value != null)
                        yield return i.Value;

            // element extensions
            if (obj is XElement)
                foreach (var i in elementExtensions)
                    if (Predicate((XElement)obj, i.Metadata))
                        if (i.Value != null)
                            yield return i.Value;

            // text extensions
            if (obj is XText)
                foreach (var i in textExtensions)
                    if (i.Value != null)
                        yield return i.Value;
        }

        bool Predicate(XElement element, IDictionary<string, object> metadata)
        {
            var localNames = (string[])metadata.GetOrDefault("LocalName");
            var namespaceNames = (string[])metadata.GetOrDefault("NamespaceName");
            var predicateTypes = (Type[])metadata.GetOrDefault("PredicateType");
            var items = localNames
                .Zip(namespaceNames, (i, j) => new { LocalName = i, NamespaceName = j })
                .Zip(predicateTypes, (i, j) => new { i.LocalName, i.NamespaceName, PredicateType = j });

            foreach (var i in items)
                if (IsMatch(element, i.NamespaceName, i.LocalName, i.PredicateType))
                    return true;

            return false;
        }

        bool IsMatch(XElement element, string namespaceName, string localName, Type predicateType)
        {
            if (namespaceName != null &&
                namespaceName != element.Name.NamespaceName)
                return false;

            if (localName != null &&
                localName != element.Name.LocalName)
                return false;

            return true;
        }

    }

}
