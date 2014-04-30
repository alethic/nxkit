using System;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.Composition
{

    class ObjectContainer :
        Container,
        IObjectContainer
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="container"></param>
        /// <param name="catalog"></param>
        public ObjectContainer(XObject obj, CompositionContainer container, ComposablePartCatalog catalog)
            : base(container, catalog)
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            Contract.Requires<ArgumentNullException>(container != null);
            Contract.Requires<ArgumentNullException>(catalog != null);

            this.WithExport<IObjectContainer>(this);
            this.WithExport<XObject>(obj);

            if (obj is XDocument)
                this.WithExport<XDocument>((XDocument)obj);
            if (obj is XElement)
                this.WithExport<XElement>((XElement)obj);
            if (obj is XNode)
                this.WithExport<XNode>((XNode)obj);
            if (obj is XAttribute)
                this.WithExport<XAttribute>((XAttribute)obj);
        }

    }

}
