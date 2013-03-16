using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Xml.Linq;

using ISIS.Util;

namespace ISIS.Forms
{

    /// <summary>
    /// Provides the composition container to load the available <see cref="VisualTypeDescriptor"/>s.
    /// </summary>
    internal class VisualTypeDescriptorContainer
    {

        private static VisualTypeDescriptorContainer defaultInstance = new VisualTypeDescriptorContainer();

        /// <summary>
        /// Gets a reference to the default instance.
        /// </summary>
        public static VisualTypeDescriptorContainer DefaultInstance
        {
            get { return defaultInstance; }
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        private VisualTypeDescriptorContainer()
        {
            DefaultCompositionInitializer.SatisfyImports(this);

            // initialize map of XName to descriptor
            TypeDescriptorMap = TypeDescriptors.ToDictionary(i => XName.Get(i.Metadata.LocalName, i.Metadata.Namespace), i => i.Value);
        }

        /// <summary>
        /// Set of loaded <see cref="VisualTypeDescriptor"/>s.
        /// </summary>
        [ImportMany(typeof(VisualTypeDescriptor))]
        private List<Lazy<VisualTypeDescriptor, IVisualTypeDescriptorMetadata>> TypeDescriptors { get; set; }

        /// <summary>
        /// Map of <see cref="XName"/> to associated <see cref="VisualTypeDescriptor"/>.
        /// </summary>
        private IDictionary<XName, VisualTypeDescriptor> TypeDescriptorMap { get; set; }

        /// <summary>
        /// Returns the <see cref="VisualTypeDescriptor"/> registered for the given <see cref="XName"/>.
        /// </summary>
        /// <param name="xname"></param>
        /// <returns></returns>
        public VisualTypeDescriptor GetDescriptor(XName xname)
        {
            return TypeDescriptorMap.ValueOrDefault(xname);
        }

    }

}
