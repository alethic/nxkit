using System.Collections.Generic;
using System.Xml.Linq;

namespace NXKit.View.Windows
{

    class XContainerNodesPropertyDescriptor :
        XPropertyDescriptor<XContainer, IEnumerable<XNode>>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XContainerNodesPropertyDescriptor()
            : base("Nodes")
        {

        }

        public override object GetValue(object component)
        {
            var container = component as XContainer;
            if (container != null)
                return container.Nodes();

            return null;
        }

        protected override void OnChanged(object sender, XObjectChangeEventArgs args)
        {
            base.OnChanged(sender, args);
        }

        protected override void OnChanging(object sender, XObjectChangeEventArgs args)
        {
            base.OnChanging(sender, args);
        }

    }

}
