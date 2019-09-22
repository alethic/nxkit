using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms
{

    [Extension("{http://www.w3.org/2002/xforms}var")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    [Remote]
    public class Var :
        ElementExtension
    {

        readonly VarProperties properties;
        readonly Extension<EvaluationContextResolver> context;
        readonly Lazy<Binding> value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="properties"></param>
        /// <param name="context"></param>
        [ImportingConstructor]
        public Var(
            XElement element,
            VarProperties properties,
            Extension<EvaluationContextResolver> context)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.properties = properties ?? throw new ArgumentNullException(nameof(properties));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.value = new Lazy<Binding>(() => properties.Value != null ? new Binding(Element, context.Value.Context, properties.Value) : null);
        }

        [Remote]
        public string Name
        {
            get { return properties.Name; }
        }

        public object Value
        {
            get { return value.Value != null ? value.Value.Result : null; }
        }

    }

}
