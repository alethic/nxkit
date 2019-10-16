using System;
using System.Xml.Linq;
using System.Xml.XPath;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms binding properties.
    /// </summary>
    [Extension(typeof(BindingProperties))]
    public class BindingProperties :
        ElementExtension
    {

        readonly BindingAttributes attributes;
        readonly Lazy<EvaluationContextResolver> context;
        readonly Lazy<XPathExpression> ref_;
        readonly Lazy<XPathExpression> nodeset;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        /// <param name="context"></param>
        public BindingProperties(
            XElement element,
            BindingAttributes attributes,
            Lazy<EvaluationContextResolver> context)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
            this.context = context ?? throw new ArgumentNullException(nameof(context));

            ref_ = new Lazy<XPathExpression>(() => !string.IsNullOrEmpty(attributes.Ref) ? context.Value.Context.CompileXPath(element, attributes.Ref) : null);
            nodeset = new Lazy<XPathExpression>(() => !string.IsNullOrEmpty(attributes.NodeSet) ? context.Value.Context.CompileXPath(element, attributes.NodeSet) : null);
        }

        public XPathExpression Ref
        {
            get { return ref_.Value; }
        }

        public XPathExpression NodeSet
        {
            get { return nodeset.Value; }
        }

        public IdRef Bind
        {
            get { return attributes.Bind; }
        }

    }

}
