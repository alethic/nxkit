using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;
using System.Xml.XPath;

using NXKit.Composition;
using NXKit.Xml;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'setvalue' properties.
    /// </summary>
    [Extension(typeof(SetValueProperties), "{http://www.w3.org/2002/xforms}setvalue")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class SetValueProperties :
        ElementExtension
    {

        readonly SetValueAttributes attributes;
        readonly Extension<EvaluationContextResolver> context;
        readonly Lazy<XPathExpression> value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="context"></param>
        [ImportingConstructor]
        public SetValueProperties(
            XElement element,
            Extension<EvaluationContextResolver> context)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.attributes = element.AnnotationOrCreate<SetValueAttributes>(() => new SetValueAttributes(element));
            this.context = context ?? throw new ArgumentNullException(nameof(context));

            this.value = new Lazy<XPathExpression>(() => !string.IsNullOrEmpty(attributes.Value) ? context.Value.Context.CompileXPath(element, attributes.Value) : null);
        }

        public XPathExpression Value
        {
            get { return value.Value; }
        }

    }

}