using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using System.Xml.XPath;

using NXKit.Xml;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms binding properties.
    /// </summary>
    public class BindingProperties :
        ElementExtension
    {

        readonly BindingAttributes attributes;
        readonly Lazy<EvaluationContextResolver> context;
        readonly Lazy<XPathExpression> ref_;
        readonly Lazy<XPathExpression> nodeset;
        readonly IdRef bind;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="context"></param>
        public BindingProperties(
            XElement element,
            Lazy<EvaluationContextResolver> context)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(context != null);

            this.attributes = element.AnnotationOrCreate<BindingAttributes>(() => new BindingAttributes(element));
            this.context = context;

            this.ref_ = new Lazy<XPathExpression>(() =>
                !string.IsNullOrEmpty(attributes.Ref) ? context.Value.Context.CompileXPath(element, attributes.Ref) : null);

            this.nodeset = new Lazy<XPathExpression>(() =>
                !string.IsNullOrEmpty(attributes.NodeSet) ? context.Value.Context.CompileXPath(element, attributes.NodeSet) : null);

            this.bind = attributes.Bind;
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
            get { return bind; }
        }

    }

}