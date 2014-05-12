using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [Interface("{http://www.w3.org/2002/xforms}output")]
    [Remote]
    public class Output :
        ElementExtension
    {

        readonly Lazy<EvaluationContextResolver> context;
        readonly Lazy<OutputProperties> properties;
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
            Lazy<OutputProperties> properties)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(context != null);
            Contract.Requires<ArgumentNullException>(properties != null);

            this.properties = properties;
            this.context = context;
            this.value = new Lazy<Binding>(() => new Binding(Element, context.Value.Context, properties.Value.Value));
        }

        [Remote]
        public string Value
        {
            get { return value.Value != null ? value.Value.Value : null; }
        }

    }

}
