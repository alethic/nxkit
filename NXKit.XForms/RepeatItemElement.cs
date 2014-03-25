using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Serves as a generated item within a repeat.
    /// </summary>
    public class RepeatItemElement :
        XFormsElement,
        IEvaluationContextScope,
        INamingScope,
        IRelevancyScope
    {

        EvaluationContext context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public RepeatItemElement(XElement xml)
            : base(xml)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public override string Id
        {
            get { return "NODE" + Module.GetModelItemId(Context, Context.Node); }
        }

        /// <summary>
        /// Obtains the evaluation context for this visual.
        /// </summary>
        public EvaluationContext Context
        {
            get { return context; }
        }

        /// <summary>
        /// Sets the context to a new value, should only be used by the repeat container.
        /// </summary>
        /// <param name="ec"></param>
        internal void SetContext(EvaluationContext ec)
        {
            context = ec;
        }

        protected override void OnAdded(NXObjectEventArgs args)
        {
            base.OnAdded(args);

            // ensure module item id is initialized
            Module.GetModelItemId(Context, Context.Node);
        }

        public bool Relevant
        {
            get { return Module.GetModelItemRelevant(Context.Node); }
        }

    }

}
