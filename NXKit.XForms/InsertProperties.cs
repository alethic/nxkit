using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using System.Xml.XPath;

using NXKit.Composition;
using NXKit.Xml;

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

        readonly InsertAttributes attributes;
        readonly Lazy<EvaluationContextResolver> context;
        readonly Lazy<XPathExpression> origin;
        readonly Lazy<XPathExpression> at;
        readonly Lazy<InsertPosition> position;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="context"></param>
        public InsertProperties(
            XElement element,
            Lazy<EvaluationContextResolver> context)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(context != null);

            this.attributes = element.AnnotationOrCreate<InsertAttributes>(() => new InsertAttributes(element));
            this.context = context;

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