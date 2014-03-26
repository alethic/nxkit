using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Base implementation for an XForms element which implements Single-Node Binding.
    /// </summary>
    public class SingleNodeBindingElement :
        BindingElement
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public SingleNodeBindingElement(XElement xml)
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
                Binding.Node != null &&
                Binding.Node.Document != null)
            {
                var model = Binding.Node.Document.Annotation<ModelElement>();
                Contract.Assert(model != null);

                var instance = Binding.Node.Document.Annotation<InstanceElement>();
                Contract.Assert(instance != null);

                return new EvaluationContext(model, instance, Binding.Node, 1, 1);
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
        [Interactive]
        public object Value
        {
            get { return Binding != null ? Binding.Value : null; }
            set { SetValue(value); }
        }

        /// <summary>
        /// Implements the setter for Value. Override this method to store bound values differently.
        /// </summary>
        /// <param name="value"></param>
        protected virtual void SetValue(object value)
        {
            if (Binding != null)
                Binding.SetValue(value != null ? value.ToString() : null);
        }

        /// <summary>
        /// Gets the type of the bound data.
        /// </summary>
        [Interactive]
        public XName Type
        {
            get { return Binding != null ? Binding.Type : null; }
        }

        /// <summary>
        /// Gets whether or not this visual is enabled.
        /// </summary>
        [Interactive]
        public virtual bool Relevant
        {
            get
            {
                // 8.1.1 Implementation Requirements Common to All Form Controls

                // the Single Node Binding is expressed and resolves to empty nodeset
                if (Binding == null)
                    return true;

                // the Single Node Binding is expressed and resolves to empty nodeset
                if (Binding.Node == null)
                    return false;

                // the Single Node Binding is expressed and resolves to a non-relevant instance node
                if (Binding.Relevant == false)
                    return false;

                // the form control is contained by a non-relevant switch or group (which includes a non-relevant repeat item)
                var scope = this.Ancestors().OfType<IRelevancyScope>().FirstOrDefault();
                if (scope != null)
                    if (scope.Relevant == false)
                        return false;

                // the form control is contained by a non-selected case element of a switch
                // TODO

                return true;
            }
        }

        /// <summary>
        /// Gets whether or not this visual is read-only.
        /// </summary>
        [Interactive]
        public bool ReadOnly
        {
            get { return Binding != null ? Binding.ReadOnly : true; }
        }

        /// <summary>
        /// Gets whether or not this visual is required.
        /// </summary>
        [Interactive]
        public bool Required
        {
            get { return Binding != null ? Binding.Required : false; }
        }

        /// <summary>
        /// Gets whether or not this visual is bound to valid data.
        /// </summary>
        [Interactive]
        public bool Valid
        {
            get { return Binding != null ? Binding.Valid : false; }
        }

    }

}
