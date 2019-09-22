using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;
using System.Xml.XPath;

using NXKit.Composition;
using NXKit.Xml;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'bind' properties.
    /// </summary>
    [Extension(typeof(BindProperties), "{http://www.w3.org/2002/xforms}bind")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class BindProperties :
        ElementExtension
    {

        readonly BindAttributes attributes;
        readonly Extension<EvaluationContextResolver> context;
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
        [ImportingConstructor]
        public BindProperties(
            XElement element,
            BindAttributes attributes,
            Extension<EvaluationContextResolver> context)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.attributes = attributes;
            this.context = context ?? throw new ArgumentNullException(nameof(context));

            this.type = new Lazy<XName>(() =>
                !string.IsNullOrEmpty(attributes.Type) ? Element.ResolvePrefixedName(attributes.Type) : null);

            this.readOnly = new Lazy<XPathExpression>(() =>
                !string.IsNullOrEmpty(attributes.ReadOnly) ? context.Value.Context.CompileXPath(element, attributes.ReadOnly) : null);

            this.required = new Lazy<XPathExpression>(() =>
                !string.IsNullOrEmpty(attributes.Required) ? context.Value.Context.CompileXPath(element, attributes.Required) : null);

            this.relevant = new Lazy<XPathExpression>(() =>
                !string.IsNullOrEmpty(attributes.Relevant) ? context.Value.Context.CompileXPath(element, attributes.Relevant) : null);

            this.calculate = new Lazy<XPathExpression>(() =>
                !string.IsNullOrEmpty(attributes.Calculate) ? context.Value.Context.CompileXPath(element, attributes.Calculate) : null);

            this.constraint = new Lazy<XPathExpression>(() =>
                !string.IsNullOrEmpty(attributes.Constraint) ? context.Value.Context.CompileXPath(element, attributes.Constraint) : null);
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