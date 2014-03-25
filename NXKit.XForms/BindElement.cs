using System.Xml.Linq;

namespace NXKit.XForms
{

    [Element("bind")]
    public class BindElement :
        BindingElement
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public BindElement(XElement xml)
            : base(xml)
        {

        }

        protected override EvaluationContext CreateEvaluationContext()
        {
            return null;
        }

        protected override Binding CreateBinding()
        {
            return Module.ResolveNodeSetBinding(this);
        }

    }

}
