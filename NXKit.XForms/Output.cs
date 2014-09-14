using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms
{

    [Extension("{http://www.w3.org/2002/xforms}output")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    [Remote]
    public class Output :
        ElementExtension
    {

        readonly Extension<EvaluationContextResolver> context;
        readonly Extension<OutputProperties> properties;
        readonly Lazy<Binding> value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="context"></param>
        /// <param name="properties"></param>
        [ImportingConstructor]
        public Output(
            XElement element,
            Extension<EvaluationContextResolver> context,
            Extension<OutputProperties> properties)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(context != null);
            Contract.Requires<ArgumentNullException>(properties != null);

            this.properties = properties;
            this.context = context;
            this.value = new Lazy<Binding>(() => properties.Value.Value != null ? new Binding(Element, context.Value.Context, properties.Value.Value) : null);
        }

        [Remote]
        public string Value
        {
            get { return value.Value != null ? value.Value.Value : null; }
        }

    }

}
