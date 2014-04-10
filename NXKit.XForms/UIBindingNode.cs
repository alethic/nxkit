using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Xml;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides a <see cref="UIBinding"/> for a node.
    /// </summary>
    [Interface]
    public class UIBindingNode :
        IUIBindingNode
    {

        class UIBindingNodePredicate :
            IInterfacePredicate
        {

            public bool IsMatch(XObject obj, Type type)
            {
                throw new NotImplementedException();
            }

        }

        readonly XElement element;
        readonly Lazy<UIBinding> uiBinding;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public UIBindingNode(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
            this.uiBinding = new Lazy<UIBinding>(() => CreateUIBinding());
        }

        /// <summary>
        /// Gets the element.
        /// </summary>
        public XElement Element
        {
            get { return element; }
        }

        /// <summary>
        /// Gets the <see cref="UIBinding"/> associated with the node.
        /// </summary>
        public UIBinding UIBinding
        {
            get { return uiBinding.Value; }
        }

        /// <summary>
        /// Creates the UIBinding.
        /// </summary>
        /// <returns></returns>
        UIBinding CreateUIBinding()
        {
            var b = element.InterfaceOrDefault<IBindingNode>();
            if (b != null &&
                b.Binding != null)
                return new UIBinding(element, b.Binding);

            return null;
        }

    }

}
