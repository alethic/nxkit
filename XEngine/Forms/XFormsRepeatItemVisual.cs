using System.Xml.Linq;


namespace XEngine.Forms
{

    public class XFormsRepeatItemVisual : XFormsVisual, IEvaluationContextScope, INamingScope, IRelevancyScope
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        /// <param name="bindingScopeNode"></param>
        internal XFormsRepeatItemVisual(StructuralVisual parent, XElement element, XFormsEvaluationContext context)
            : base(parent, element)
        {
            Context = context;

            // ensure module item id is initialized
            Module.GetModelItemId(Context, Context.Node);
        }

        public override string Id
        {
            get { return "NODE" + Module.GetModelItemId(Context, Context.Node); }
        }

        /// <summary>
        /// Obtains the evaluation context for this visual.
        /// </summary>
        public XFormsEvaluationContext Context { get; private set; }

        /// <summary>
        /// Sets the context to a new value, should only be used by the repeat container.
        /// </summary>
        /// <param name="ec"></param>
        internal void SetContext(XFormsEvaluationContext ec)
        {
            Context = ec;

            // ensure module item id is initialized
            Module.GetModelItemId(Context, Context.Node);
        }

        public bool Relevant
        {
            get { return Module.GetModelItemRelevant(Context.Node); }
        }

    }

}
