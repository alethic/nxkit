using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using System.Xml.XPath;

using NXKit.Composition;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms binding properties.
    /// </summary>
    [Extension]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class BindingProperties :
        ElementExtension
    {

        readonly Extension<BindingAttributes> attributes;
        readonly Extension<EvaluationContextResolver> context;
        readonly Lazy<XPathExpression> ref_;
        readonly Lazy<XPathExpression> nodeset;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        /// <param name="context"></param>
        [ImportingConstructor]
        public BindingProperties(
            XElement element,
            Extension<BindingAttributes> attributes,
            Extension<EvaluationContextResolver> context)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(context != null);

            this.attributes = attributes;
            this.context = context;

            this.ref_ = new Lazy<XPathExpression>(() =>
                !string.IsNullOrEmpty(attributes.Value.Ref) ? context.Value.Context.CompileXPath(element, attributes.Value.Ref) : null);

            this.nodeset = new Lazy<XPathExpression>(() =>
                !string.IsNullOrEmpty(attributes.Value.NodeSet) ? context.Value.Context.CompileXPath(element, attributes.Value.NodeSet) : null);
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
            get { return attributes.Value.Bind; }
        }

    }

}