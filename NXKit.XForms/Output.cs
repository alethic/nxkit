using System;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [Extension("{http://www.w3.org/2002/xforms}output")]
    [Extension(typeof(IRemote), "{http://www.w3.org/2002/xforms}output")]
    [Remote]
    public class Output :
        ElementExtension,
        IRemote
    {

        readonly Lazy<EvaluationContextResolver> context;
        readonly OutputProperties properties;
        readonly Lazy<Binding> value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="context"></param>
        /// <param name="properties"></param>
        public Output(
            XElement element,
            Lazy<EvaluationContextResolver> context,
            OutputProperties properties)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.properties = properties ?? throw new ArgumentNullException(nameof(properties));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.value = new Lazy<Binding>(() => properties.Value != null ? new Binding(Element, context.Value.Context, properties.Value) : null);
        }

        [Remote]
        public string Value
        {
            get { return value.Value != null ? value.Value.Value : null; }
        }

    }

}
