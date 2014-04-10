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
        ElementExtension,
        IUIBindingNode,
        IOnRefresh
    {

        readonly Lazy<UIBinding> uiBinding;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public UIBindingNode(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.uiBinding = new Lazy<UIBinding>(() => CreateUIBinding());
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
            var b = Element.InterfaceOrDefault<IBindingNode>();
            if (b != null &&
                b.Binding != null)
                return new UIBinding(Element, b.Binding);

            return null;
        }
        
        void IOnRefresh.RefreshBinding()
        {
            if (UIBinding != null)
                UIBinding.Refresh();
        }

        void IOnRefresh.Refresh()
        {
            
        }

        void IOnRefresh.DispatchEvents()
        {
            if (UIBinding != null)
                UIBinding.DispatchEvents();
        }

        void IOnRefresh.DiscardEvents()
        {
            if (UIBinding != null)
                UIBinding.DiscardEvents();
        }

    }

}
