using System;
using System.Xml.Linq;

using NXKit.Diagnostics;

namespace NXKit.XForms
{

    [Extension("{http://www.w3.org/2002/xforms}value", PredicateType = typeof(HeaderValuePredicate))]
    public class HeaderValue :
        ElementExtension
    {

        class HeaderValuePredicate : IExtensionPredicate
        {

            public bool IsMatch(XObject obj)
            {
                return obj.Parent != null && obj.Parent.Name == Constants.XForms_1_0 + "header";
            }

        }

        readonly HeaderValueAttributes attributes;
        readonly Lazy<IBindingNode> bindingNode;
        readonly ITraceService trace;
        readonly Lazy<Binding> valueBinding;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        /// <param name="bindingNode"></param>
        /// <param name="trace"></param>
        public HeaderValue(
            XElement element,
            HeaderValueAttributes attributes,
            Lazy<IBindingNode> bindingNode,
            ITraceService trace)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
            this.bindingNode = bindingNode ?? throw new ArgumentNullException(nameof(bindingNode));
            this.trace = trace ?? throw new ArgumentNullException(nameof(trace));
            this.valueBinding = new Lazy<Binding>(() => BindingUtil.ForAttribute(attributes.Value,trace));
        }

        /// <summary>
        /// Gets the binding of the element.
        /// </summary>
        Binding Binding => bindingNode.Value?.Binding;

        /// <summary>
        /// Gets the 'value' attribute binding.
        /// </summary>
        Binding ValueBinding => valueBinding.Value;

        /// <summary>
        /// Gets the appropriate value to use when selecting the item.
        /// </summary>
        /// <returns></returns>
        string GetValue()
        {
            if (Binding != null)
                return Binding.Value;

            if (ValueBinding != null)
                return ValueBinding.Value;

            return Element.Value;
        }

    }

}
