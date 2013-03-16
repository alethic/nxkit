using System;
using System.ComponentModel.Composition;

namespace XEngine.Forms.Web.UI
{

    /// <summary>
    /// Marks a <see cref="FormModule"/>.
    /// </summary>
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class)]
    public class FormModuleAttribute : ExportAttribute, IFormModuleMetadata
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public FormModuleAttribute()
            : base(typeof(FormModule))
        {

        }

    }

    public interface IFormModuleMetadata
    {



    }

}
