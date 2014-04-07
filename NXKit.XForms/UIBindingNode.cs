using System;
using System.Diagnostics.Contracts;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides a <see cref="UIBinding"/> for a node.
    /// </summary>
    [NXElementInterface("http://www.w3.org/2002/xforms", null)]
    public class UIBindingNode
    {

        readonly NXElement element;
        readonly Lazy<UIBinding> uiBinding;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public UIBindingNode(NXElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
            this.uiBinding = new Lazy<UIBinding>(() => CreateUIBinding());
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
