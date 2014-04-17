using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using NXKit.XForms.IO;
using NXKit.Xml;

namespace NXKit.XForms
{

    /// <summary>
    /// The method element (deprecated in favor of using an AVT in the method attribute) allows the submission method
    /// to be dynamically calculated based on instance data.
    /// </summary>
    [Interface("{http://www.w3.org/2002/xforms}method")]
    public class Method :
        ElementExtension
    {

        readonly MethodAttributes attributes;
        readonly Lazy<IBindingNode> bindingNode;
        readonly Lazy<Binding> valueBinding;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Method(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(element.Name == Constants.XForms_1_0 + "method");

            this.attributes = new MethodAttributes(element);
            this.bindingNode = new Lazy<IBindingNode>(() => Element.Interface<IBindingNode>());
            this.valueBinding = new Lazy<Binding>(() => BindingUtil.ForAttribute(attributes.ValueAttribute));
        }

        MethodAttributes Attributes
        {
            get { return attributes; }
        }

        Binding Binding
        {
            get { return bindingNode.Value != null ? bindingNode.Value.Binding : null; }
        }

        Binding ValueBinding
        {
            get { return valueBinding.Value; }
        }

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

        public RequestMethod RequestMethod
        {
            get { return !string.IsNullOrEmpty(GetValue()) ? RequestMethodHelper.Parse(GetValue()) : RequestMethod.None; }
        }

    }

}
