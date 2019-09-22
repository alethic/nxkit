using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;
using System.Xml.XPath;

using NXKit.Composition;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'insert' properties.
    /// </summary>
    [Extension(typeof(InsertProperties), "{http://www.w3.org/2002/xforms}insert")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class InsertProperties :
        ElementExtension
    {

        readonly InsertAttributes attributes;
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
            InsertAttributes attributes,
            Extension<EvaluationContextResolver> context)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
            this.context = context ?? throw new ArgumentNullException(nameof(context));

            this.origin = new Lazy<XPathExpression>(() =>
                !string.IsNullOrEmpty(attributes.Origin) ? context.Value.Context.CompileXPath(element, attributes.Origin) : null);

            this.at = new Lazy<XPathExpression>(() =>
                !string.IsNullOrEmpty(attributes.At) ? context.Value.Context.CompileXPath(element, attributes.At) : null);

            this.position = new Lazy<InsertPosition>(() =>
                !string.IsNullOrEmpty(attributes.Postition) ? (InsertPosition)Enum.Parse(typeof(InsertPosition), attributes.Postition, true) : InsertPosition.After);
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