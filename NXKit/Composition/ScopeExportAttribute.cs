using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;

namespace NXKit.Composition
{

    /// <summary>
    /// Marks an export as requiring a container supporting the specified scope.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    [MetadataAttribute]
    public class ScopeExportAttribute :
        ExportAttribute,
        IScopeMetadata
    {

        readonly Scope scope;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="contractName"></param>
        /// <param name="contractType"></param>
        /// <param name="scope"></param>
        public ScopeExportAttribute(string contractName, Type contractType, Scope scope)
            : base(contractName, contractType)
        {
            Contract.Requires<ArgumentException>(contractName != null || contractType != null);

            this.scope = scope;
        }

        /// <summary>
        /// Initializes a new instnace.
        /// </summary>
        /// <param name="contractType"></param>
        /// <param name="scope"></param>
        public ScopeExportAttribute(Type contractType, Scope scope)
            : this(null, contractType, scope)
        {
            Contract.Requires<ArgumentNullException>(contractType != null);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="contractType"></param>
        public ScopeExportAttribute(Type contractType)
            : this(contractType, Scope.Global)
        {
            Contract.Requires<ArgumentNullException>(contractType != null);
        }

        /// <summary>
        /// Export is only available to the specified <see cref="scope"/>.
        /// </summary>
        public Scope Scope
        {
            get { return scope; }
        }

    }

}