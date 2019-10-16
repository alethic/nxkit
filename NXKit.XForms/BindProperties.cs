using System;
using System.Xml.Linq;
using System.Xml.XPath;

using NXKit.Xml;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'bind' properties.
    /// </summary>
    [Extension(typeof(BindProperties), "{http://www.w3.org/2002/xforms}bind")]
    public class BindProperties :
        ElementExtension
    {

        readonly BindAttributes attributes;
        readonly Lazy<EvaluationContextResolver> context;
        readonly Lazy<XName> type;
        readonly Lazy<XPathExpression> readOnly;
        readonly Lazy<XPathExpression> required;
        readonly Lazy<XPathExpression> relevant;
        readonly Lazy<XPathExpression> calculate;
        readonly Lazy<XPathExpression> constraint;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        /// <param name="context"></param>
        public BindProperties(
            XElement element,
            BindAttributes attributes,
            Lazy<EvaluationContextResolver> context)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.attributes = attributes;
            this.context = context ?? throw new ArgumentNullException(nameof(context));

            type = new Lazy<XName>(() => !string.IsNullOrEmpty(attributes.Type) ? Element.ResolvePrefixedName(attributes.Type) : null);
            readOnly = new Lazy<XPathExpression>(() => !string.IsNullOrEmpty(attributes.ReadOnly) ? context.Value.Context.CompileXPath(element, attributes.ReadOnly) : null);
            required = new Lazy<XPathExpression>(() => !string.IsNullOrEmpty(attributes.Required) ? context.Value.Context.CompileXPath(element, attributes.Required) : null);
            relevant = new Lazy<XPathExpression>(() => !string.IsNullOrEmpty(attributes.Relevant) ? context.Value.Context.CompileXPath(element, attributes.Relevant) : null);
            calculate = new Lazy<XPathExpression>(() => !string.IsNullOrEmpty(attributes.Calculate) ? context.Value.Context.CompileXPath(element, attributes.Calculate) : null);
            constraint = new Lazy<XPathExpression>(() => !string.IsNullOrEmpty(attributes.Constraint) ? context.Value.Context.CompileXPath(element, attributes.Constraint) : null);
        }

        public XName Type
        {
            get { return type.Value; }
        }

        public XPathExpression ReadOnly
        {
            get { return readOnly.Value; }
        }

        public XPathExpression Required
        {
            get { return required.Value; }
        }

        public XPathExpression Relevant
        {
            get { return relevant.Value; }
        }

        public XPathExpression Calculate
        {
            get { return calculate.Value; }
        }

        public XPathExpression Constraint
        {
            get { return constraint.Value; }
        }

        public string P3PType
        {
            get { return attributes.P3PType; }
        }

    }

}