using System;
using System.Xml.Linq;

using NXKit.Diagnostics;

namespace NXKit.XForms
{

    [Extension("{http://www.w3.org/2002/xforms}var")]
    [Remote]
    public class Var :
        ElementExtension
    {

        readonly VarProperties properties;
        readonly Lazy<EvaluationContextResolver> context;
        readonly ITraceService trace;
        readonly Lazy<Binding> value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="properties"></param>
        /// <param name="context"></param>
        /// <param name="trace"></param>
        public Var(
            XElement element,
            VarProperties properties,
            Lazy<EvaluationContextResolver> context,
            ITraceService trace)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.properties = properties ?? throw new ArgumentNullException(nameof(properties));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.trace = trace ?? throw new ArgumentNullException(nameof(trace));
            this.value = new Lazy<Binding>(() => properties.Value != null ? new Binding(Element, context.Value.Context, properties.Value, trace) : null);
        }

        [Remote]
        public string Name => properties.Name;

        public object Value => value.Value?.Result;

    }

}
