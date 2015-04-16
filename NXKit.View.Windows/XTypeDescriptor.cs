using System;
using System.ComponentModel;
using System.Xml.Linq;

namespace NXKit.View.Windows
{

    class XTypeDescriptor<T> :
        CustomTypeDescriptor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        public XTypeDescriptor(ICustomTypeDescriptor parent)
            : base(parent)
        {

        }

        public override PropertyDescriptorCollection GetProperties()
        {
            return GetProperties(null);
        }

        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            var properties = new PropertyDescriptorCollection(null);
            if (attributes == null)
            {
                if (typeof(T) == typeof(XContainer))
                {
                    properties.Add(new XContainerNodesPropertyDescriptor());
                }
            }

            foreach (PropertyDescriptor property in base.GetProperties(attributes))
                properties.Add(property);

            return properties;
        }

    }

}
