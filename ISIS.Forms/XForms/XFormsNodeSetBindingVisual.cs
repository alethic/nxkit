using System.Xml.Linq;

namespace ISIS.Forms.XForms
{

    public abstract class XFormsNodeSetBindingVisual : XFormsBindingVisual
    {

        private bool contextCached;
        private XFormsEvaluationContext context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public XFormsNodeSetBindingVisual(StructuralVisual parent, XElement element)
            : base(parent, element)
        {

        }

        /// <summary>
        /// Provides the default evaluation context for child elements, the first element.
        /// </summary>
        public override XFormsEvaluationContext Context
        {
            get
            {
                if (!contextCached)
                {
                    context = null;

                    if (Binding != null &&
                        Binding.Nodes != null &&
                        Binding.Nodes.Length >= 1)
                        context = new XFormsEvaluationContext(Binding.Context.Model, Binding.Context.Instance, Binding.Nodes[0], 1, Binding.Nodes.Length);

                    contextCached = true;
                }

                return context;
            }
        }

        /// <summary>
        /// Gets a reference to the node-set binding.
        /// </summary>
        public XFormsBinding Binding { get; private set; }

        public override void Refresh()
        {
            // rebuild binding
            Binding = Module.ResolveNodeSetBinding(this);

            // rebuild cached values
            context = null;
            contextCached = false;

            base.Refresh();

            // rebuild children
            base.InvalidateChildren();
        }

    }

}
