using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit
{

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    [MetadataAttribute]
    public abstract class ObjectExtensionAttribute :
        ScopeExportAttribute
    {

        Type predicateType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="objectType"></param>
        public ObjectExtensionAttribute(Type objectType)
            : base(typeof(IExtension<>).MakeGenericType(objectType))
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
