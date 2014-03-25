using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;

namespace NXKit
{

    /// <summary>
    /// Provides overrides to <see cref="NXNode"/> types.
    /// </summary>
    public abstract class NodeTypeDescriptor :
        CustomTypeDescriptor
    {

        readonly Type type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        public NodeTypeDescriptor(Type type)
        {
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Requires<ArgumentException>(typeof(NXNode).IsAssignableFrom(type));

            this.type = type;
        }

        public override PropertyDescriptorCollection GetProperties()
        {
            return new PropertyDescriptorCollection(GetPropertiesIter().ToArray());
        }

        IEnumerable<PropertyDescriptor> GetPropertiesIter()
        {
            yield break;
        }

    }

}
