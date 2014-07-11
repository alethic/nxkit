using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using System.Xml.XPath;

using NXKit.Util;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'var' properties.
    /// </summary>
    [Interface("{http://www.w3.org/2002/xforms}var")]
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
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(attributes != null);
            Contract.Requires<ArgumentNullException>(context != null);

            this.attributes = attributes;
            this.context = context;

            this.value = new Lazy<XPathExpression>(() =>
                !string.IsNullOrEmpty(attributes.Value) ? context.Value.Context.CompileXPath(element, attributes.Value) : null);
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