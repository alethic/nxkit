using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Base implementation for an XForms visual which implements Single-Node Binding.
    /// </summary>
    public class XFormsSingleNodeBindingVisual :
        XFormsBindingVisual
    {

        /// <summary>
        /// Provides an evaluation context for children nodes. Single-Node Bindings provide the single bound node.
        /// </summary>
        /// <returns></returns>
        protected override XFormsEvaluationContext CreateEvaluationContext()
        {
            if (Binding != null &&
                Binding.Node != null &&
                Binding.Node.Document != null)
            {
                var model = Binding.Node.Document.Annotation<XFormsModelVisual>();
                Contract.Assert(model != null);

                var instance = Binding.Node.Document.Annotation<XFormsInstanceVisual>();
                Contract.Assert(instance != null);

                return new XFormsEvaluationContext(model, instance, Binding.Node, 1, 1);
            }

            return null;
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
                var scope = Ascendants().OfType<IRelevancyScope>().FirstOrDefault();
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
        /// Refreshes the visual's state from the model.
        /// </summary>
        public override void Refresh()
        {
            // rebuild binding
            Binding = Module.ResolveSingleNodeBinding(this);

            base.Refresh();
        }

    }

}
