using System;
using System.Xml.Linq;
using System.Xml.XPath;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'delete' properties.
    /// </summary>
    [Extension(typeof(DeleteProperties), "{http://www.w3.org/2002/xforms}delete")]
    public class DeleteProperties :
        ElementExtension
    {

        readonly DeleteAttributes attributes;
        readonly Lazy<EvaluationContextResolver> context;
        readonly Lazy<XPathExpression> at;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        /// <param name="context"></param>
        public DeleteProperties(
            XElement element,
            DeleteAttributes attributes,
            Lazy<EvaluationContextResolver> context)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
            this.context = context ?? throw new ArgumentNullException(nameof(context));

            this.at = new Lazy<XPathExpression>(() =>
                !string.IsNullOrEmpty(attributes.At) ? context.Value.Context.CompileXPath(element, attributes.At) : null);
        }

        public XPathExpression At
        {
            get { return at.Value; }
        }

    }

}