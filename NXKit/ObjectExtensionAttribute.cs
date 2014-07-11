using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;

namespace NXKit
{

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    [MetadataAttribute]
    public abstract class ObjectExtensionAttribute :
        ExportAttribute
    {

        readonly ExtensionObjectType objectType;
        Type predicateType;
        Type interfaceType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="objectType"></param>
        public ObjectExtensionAttribute(ExtensionObjectType objectType, Type predicateType, Type interfaceType)
            : base(typeof(IExtension))
        {
            this.objectType = objectType;
            this.predicateType = predicateType;
            this.interfaceType = interfaceType;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="objectType"></param>
        public ObjectExtensionAttribute(ExtensionObjectType objectType)
            : base(typeof(IExtension))
        {
            this.objectType = objectType;
        }

        /// <summary>
        /// Specifies the object type this extension applies to.
        /// </summary>
        public ExtensionObjectType ObjectType
        {
            get { return objectType; }
        }

        /// <summary>
        /// Specifies the predicate type to determine whether this extension applies to the decorated <see cref="XObject"/>.
        /// </summary>
        public Type PredicateType
        {
            get { return predicateType; }
            set { predicateType = value; }
        }

        /// <summary>
        /// Specifies that this type is available as the given interface type.
        /// </summary>
        public Type InterfaceType
        {
            get { return interfaceType; }
            set { interfaceType = value; }
        }

    }

}
