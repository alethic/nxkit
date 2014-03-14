using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;

namespace NXKit.XForms
{

    public class XFormsTypeDescriptionProvider :
        VisualTypeDescriptionProvider
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="type"></param>
        public XFormsTypeDescriptionProvider(TypeDescriptionProvider parent, Type type)
            : base(parent, type)
        {
            Contract.Requires<ArgumentNullException>(parent != null);
            Contract.Requires<ArgumentNullException>(type != null);
        }

        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            return base.GetTypeDescriptor(objectType, instance);
        }

    }

}
