using System;
using System.ComponentModel.Composition;

namespace ISIS.Forms
{

    public interface IVisualTypeDescriptorMetadata
    {

        string Namespace { get; }

        string LocalName { get; }

    }

    [AttributeUsage(AttributeTargets.Class)]
    [MetadataAttribute]
    public class VisualTypeDescriptorAttribute : ExportAttribute, IVisualTypeDescriptorMetadata
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        public VisualTypeDescriptorAttribute(string @namespace, string localName)
            : base(typeof(VisualTypeDescriptor))
        {
            Namespace = @namespace;
            LocalName = localName;
        }

        /// <summary>
        /// Gets the namespace name of entities described by this descriptor.
        /// </summary>
        public string Namespace { get; private set; }

        /// <summary>
        /// Gets the local name of entities described by this descriptor.
        /// </summary>
        public string LocalName { get; private set; }

    }

}
