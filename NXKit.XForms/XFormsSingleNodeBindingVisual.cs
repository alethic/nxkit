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

        XFormsEvaluationContext context;
        bool contextCached;

        /// <summary>
        /// Gets the evaluation context contributed to visuals within this visual's scope.
        /// </summary>
        public override XFormsEvaluationContext Context
        {
            get
            {
                if (!contextCached)
                {
                    context = null;

                    if (Binding != null &&
                        Binding.Node != null &&
                        Binding.Node.Document != null)
                    {
                        var model = Binding.Node.Document.Annotation<XFormsModelVisual>();
                        Contract.Assert(model != null);

                        var instance = Binding.Node.Document.Annotation<XFormsInstanceVisual>();
                        Contract.Assert(instance != null);

                        context = new XFormsEvaluationContext(model, instance, Binding.Node, 1, 1);
                    }

                    contextCached = true;
                }

                return context;
            }
        }

        /// <summary>
        /// Gets the type of the bound data.
        /// </summary>
        public XName Type
        {
            get { return Binding != null ? Binding.Type : null; }
        }

        /// <summary>
        /// Gets whether or not this visual is enabled.
        /// </summary>
        public virtual bool Relevant
        {
            get
            {
                if (Binding != null && Binding.Node != null && !Binding.Relevant)
                    return false;

                if (Ascendants().OfType<IRelevancyScope>().Any(i => !i.Relevant))
                    return false;

                return true;
            }
        }

        /// <summary>
        /// Gets whether or not this visual is read-only.
        /// </summary>
        public bool ReadOnly
        {
            get { return Binding != null ? Binding.ReadOnly : true; }
        }

        /// <summary>
        /// Gets whether or not this visual is required.
        /// </summary>
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

            // rebuild cached values
            context = null;
            contextCached = false;

            base.Refresh();
        }

    }

}
