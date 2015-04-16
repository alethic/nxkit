using System;
using System.ComponentModel;

namespace NXKit.View.Windows
{

    public class XTypeDescriptionProvider<T> :
        TypeDescriptionProvider
    {

        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            return new XTypeDescriptor<T>(base.GetTypeDescriptor(objectType, instance));
        }

    }

}
