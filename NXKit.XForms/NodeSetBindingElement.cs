using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Abstract base <see cref="NXNode"/> implementation for node-set binding elements.
    /// </summary>
    public abstract class NodeSetBindingElement :
        BindingElement
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public NodeSetBindingElement(XElement xml)
            : base(xml)
        {

        }

        /// <summary>
        /// Provides an evaluation context for children. Node-set bindings provide the first node result.
        /// </summary>
        /// <returns></returns>
        protected override EvaluationContext CreateEvaluationContext()
        {
            if (Binding != null &&
                Binding.Nodes != null &&
                Binding.Nodes.Length >= 1)
                return new EvaluationContext(Binding.Context.Model, Binding.Context.Instance, Binding.Nodes[0], 1, Binding.Nodes.Length);

            return null;
        }

        protected override Binding CreateBinding()
        {
            return Module.ResolveNodeSetBinding(this);
        }

    }

}
