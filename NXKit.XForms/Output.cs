using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms
{

    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    [Extension("{http://www.w3.org/2002/xforms}output")]
    [Extension(typeof(IRemote), "{http://www.w3.org/2002/xforms}output")]
    [Remote]
    public class Output :
        ElementExtension
    {

        readonly Extension<EvaluationContextResolver> context;
        readonly OutputProperties properties;
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
            OutputProperties properties)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(context != null);
            Contract.Requires<ArgumentNullException>(properties != null);

            this.properties = properties;
            this.context = context;
            this.value = new Lazy<Binding>(() => properties.Value != null ? new Binding(Element, context.Value.Context, properties.Value) : null);
        }

        [Remote]
        public string Value
        {
            get { return value.Value != null ? value.Value.Value : null; }
        }

    }

}
