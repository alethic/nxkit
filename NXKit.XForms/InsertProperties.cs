using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using System.Xml.XPath;

using NXKit.Composition;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'insert' properties.
    /// </summary>
    [Extension("{http://www.w3.org/2002/xforms}insert")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class InsertProperties :
        ElementExtension
    {

        readonly Extension<InsertAttributes> attributes;
        readonly Extension<EvaluationContextResolver> context;
        readonly Lazy<XPathExpression> origin;
        readonly Lazy<XPathExpression> at;
        readonly Lazy<InsertPosition> position;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        /// <param name="context"></param>
        [ImportingConstructor]
        public InsertProperties(
            XElement element,
            Extension<InsertAttributes> attributes,
            Extension<EvaluationContextResolver> context)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(attributes != null);
            Contract.Requires<ArgumentNullException>(context != null);

            this.attributes = attributes;
            this.context = context;

            this.origin = new Lazy<XPathExpression>(() =>
                !string.IsNullOrEmpty(attributes.Value.Origin) ? context.Value.Context.CompileXPath(element, attributes.Value.Origin) : null);

            this.at = new Lazy<XPathExpression>(() =>
                !string.IsNullOrEmpty(attributes.Value.At) ? context.Value.Context.CompileXPath(element, attributes.Value.At) : null);

            this.position = new Lazy<InsertPosition>(() =>
                !string.IsNullOrEmpty(attributes.Value.Postition) ? (InsertPosition)Enum.Parse(typeof(InsertPosition), attributes.Value.Postition, true) : InsertPosition.After);
        }

        public XPathExpression Origin
        {
            get { return origin.Value; }
        }

        public XPathExpression At
        {
            get { return at.Value; }
        }

        public InsertPosition Position
        {
            get { return position.Value; }
        }

    }

}