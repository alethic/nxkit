using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit
{

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    [MetadataAttribute]
    public abstract class ObjectExtensionAttribute<T> :
        ScopeExportAttribute
        where T : XObject
    {

        Type predicateType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="contractType"></param>
        public ObjectExtensionAttribute()
            : base(typeof(IExtension<T>))
        {

        }

        /// <summary>
        /// Specifies the predicate type to determine whether this extension applies to the decorated <see cref="XObject"/>.
        /// </summary>
        public Type PredicateType
        {
            get { return predicateType; }
            set { predicateType = value; }
        }

    }

}
