using System;
using System.Xml.Linq;
using System.Xml.XPath;

using NXKit.IO.Media;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'var' properties.
    /// </summary>
    [Extension("{http://www.w3.org/2002/xforms}var")]
    public class VarProperties :
        ElementExtension
    {

        readonly VarAttributes attributes;
        readonly Lazy<EvaluationContextResolver> context;
        readonly Lazy<XPathExpression> value;
        readonly MediaRange mediaType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="context"></param>
        /// <param name="attributes"></param>
        public VarProperties(
            XElement element,
            VarAttributes attributes,
            Lazy<EvaluationContextResolver> context)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
            this.context = context ?? throw new ArgumentNullException(nameof(context));

            this.value = new Lazy<XPathExpression>(() => !string.IsNullOrEmpty(attributes.Value) ? context.Value.Context.CompileXPath(element, attributes.Value) : null);
        }

        public XPathExpression Value
        {
            get { return value.Value; }
        }

        public string Name
        {
            get { return attributes.Name; }
        }

    }

}