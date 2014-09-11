using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit
{

    class ObjectContainerAnnotation
    {

        readonly XObject obj;
        readonly IObjectContainer container;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="container"></param>
        public ObjectContainerAnnotation(XObject obj, IObjectContainer container)
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            Contract.Requires<ArgumentNullException>(container != null);

            this.obj = obj;
            this.container = container;
        }

        public XObject Object
        {
            get { return obj; }
        }

        public IObjectContainer Container
        {
            get { return container; }
        }

    }

}
