using System;
using System.ComponentModel.Composition;

namespace XEngine.Forms
{

    [AttributeUsage(AttributeTargets.Class)]
    [MetadataAttribute]
    public class ModuleAttribute : ExportAttribute, IModuleMetadata
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ModuleAttribute()
            : base(typeof(Module))
        {

        }

    }

    public interface IModuleMetadata
    {



    }

}
