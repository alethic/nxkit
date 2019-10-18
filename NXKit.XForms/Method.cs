using System;
using System.Xml.Linq;

using NXKit.Diagnostics;
using NXKit.XForms.IO;

namespace NXKit.XForms
{

    /// <summary>
    /// The method element (deprecated in favor of using an AVT in the method attribute) allows the submission method
    /// to be dynamically calculated based on instance data.
    /// </summary>
    [Extension("{http://www.w3.org/2002/xforms}method")]
    public class Method :
        ElementExtension
    {

        readonly MethodAttributes attributes;
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
        public Method(
            XElement element,
            MethodAttributes attributes,
            Lazy<IBindingNode> bindingNode,
            ITraceService trace)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
            this.bindingNode = bindingNode ?? throw new ArgumentNullException(nameof(bindingNode));
            this.trace = trace ?? throw new ArgumentNullException(nameof(trace));

            valueBinding = new Lazy<Binding>(() => BindingUtil.ForAttribute(attributes.ValueAttribute, trace));
        }

        Binding Binding => bindingNode.Value?.Binding;

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

        public ModelMethod RequestMethod
        {
            get { return GetValue(); }
        }

    }

}
