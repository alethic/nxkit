using System;
using System.ComponentModel.Composition;

namespace XEngine.Forms.Web.UI
{

    [AttributeUsage(AttributeTargets.Class)]
    [MetadataAttribute]
    public class VisualControlTypeDescriptorAttribute : ExportAttribute, IVisualControlDescriptorMetadata
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visualType"></param>
        public VisualControlTypeDescriptorAttribute()
            : base(typeof(VisualControlTypeDescriptor))
        {

        }

    }

    public interface IVisualControlDescriptorMetadata
    {



    }

}
