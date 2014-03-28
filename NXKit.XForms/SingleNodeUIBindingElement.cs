using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Base implementation for an XForms element which implements Single-Node Binding.
    /// </summary>
    [Public]
    public class SingleNodeUIBindingElement :
        UIBindingElement
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public SingleNodeUIBindingElement()
            : base()
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

                var instance = Binding.ModelItem.Xml.Document.Annotation<InstanceElement>();
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

        /// <summary>
        /// Gets or sets the value of the bound data.
        /// </summary>
        [Public]
        public object Value
        {
            get { return UIBinding != null ? UIBinding.Value : null; }
            set { SetValue(value); }
        }

        /// <summary>
        /// Implements the setter for Value. Override this method to store bound values differently.
        /// </summary>
        /// <param name="value"></param>
        protected virtual void SetValue(object value)
        {
            if (Binding != null &&
                Binding.ModelItem != null)
                Binding.ModelItem.Value = value != null ? value.ToString() : null;
        }

    }

}
