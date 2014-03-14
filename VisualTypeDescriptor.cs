using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;

namespace NXKit
{

    /// <summary>
    /// Provides overrides to <see cref="Visual"/> types.
    /// </summary>
    public abstract class VisualTypeDescriptor :
        CustomTypeDescriptor
    {

        readonly Type type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        public VisualTypeDescriptor(Type type)
        {
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Requires<ArgumentException>(typeof(Visual).IsAssignableFrom(type));

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
