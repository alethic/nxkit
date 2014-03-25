using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;

namespace NXKit
{

    /// <summary>
    /// Provides overrides to <see cref="NXNode"/> types.
    /// </summary>
    public abstract class NodeTypeDescriptionProvider :
        TypeDescriptionProvider
    {

        readonly TypeDescriptionProvider parent;
        readonly Type type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="type"></param>
        public NodeTypeDescriptionProvider(TypeDescriptionProvider parent, Type type)
        {
            Contract.Requires<ArgumentNullException>(parent != null);
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Requires<ArgumentException>(typeof(NXNode).IsAssignableFrom(type));

            this.parent = parent;
            this.type = type;
        }

        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            return parent.GetTypeDescriptor(objectType, instance);
        }

    }

}
