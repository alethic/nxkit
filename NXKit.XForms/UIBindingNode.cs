using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.Xml;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides a <see cref="UIBinding"/> for a node.
    /// </summary>
    [Extension(PredicateType = typeof(UIBindingNodePredicate))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class UIBindingNode :
        ElementExtension,
        IUIBindingNode,
        IOnRefresh
    {

        public class UIBindingNodePredicate :
            ExtensionPredicateBase
        {

            public override bool IsMatch(XObject obj)
            {
                var element = obj as XElement;
                if (element != null)
                    return element.Name != Constants.XForms_1_0 + "bind";

                return true;
            }

        }

        readonly IInvoker invoker;
        readonly Lazy<UIBinding> uiBinding;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="invoker"></param>
        [ImportingConstructor]
        public UIBindingNode(XElement element, IInvoker invoker)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.invoker = invoker ?? throw new ArgumentNullException(nameof(invoker));
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
                return new UIBinding(Element, invoker, b.Binding);

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
