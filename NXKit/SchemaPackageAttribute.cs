using System.ComponentModel.Composition;

namespace NXKit
{

    /// <summary>
    /// Marks a <see cref="SchemaPackage"/>.
    /// </summary>
    public class SchemaPackageAttribute : ExportAttribute
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public SchemaPackageAttribute()
            : base(typeof(SchemaPackage))
        {

        }

    }

}
