using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Base implementation for an XForms element which implements Single-Node Binding.
    /// </summary>
    public class SingleNodeUIBindingElement :
        UIBindingElement
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public SingleNodeUIBindingElement(XName name)
            : base(name)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public SingleNodeUIBindingElement(XElement xml)
            : base(xml)
        {

        }

        /// <summary>
        /// Provides an evaluation context for children nodes. Single-Node Bindings provide the single bound node.
        /// </summary>
        /// <returns></returns>
        protected override EvaluationContext CreateEvaluationContext()
        {
            if (Binding != null &&
                Binding.ModelItem != null &&
                Binding.ModelItem.Xml.Document != null)
            {
                var model = Binding.ModelItem.Xml.Document.Annotation<ModelElement>();
                Contract.Assert(model != null);

                var instance = Binding.ModelItem.Xml.Document.Annotation<Instance>();
                Contract.Assert(instance != null);

                return new EvaluationContext(model, instance, Binding.ModelItem, 1, 1);
            }

            return null;
        }

        /// <summary>
        /// Creates the binding.
        /// </summary>
        /// <returns></returns>
        protected override Binding CreateBinding()
        {
            return Module.ResolveSingleNodeBinding(this);
        }

    }

}
