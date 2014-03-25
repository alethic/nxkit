using System.Xml.Linq;
namespace NXKit.XForms
{

    public class ItemSetItemElement :
        ItemElement,
        IEvaluationContextScope,
        INamingScope
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public ItemSetItemElement(XElement xml)
            : base(xml)
        {

        }

        public override string Id
        {
            get { return "NODE" + Module.GetModelItemId(Context, Context.Node); }
        }

        /// <summary>
        /// Obtains the evaluation context for this visual.
        /// </summary>
        public EvaluationContext Context { get; private set; }

        /// <summary>
        /// Sets the context to a new value, should only be used by the repeat container.
        /// </summary>
        /// <param name="ec"></param>
        internal void SetContext(EvaluationContext ec)
        {
            Context = ec;

            // ensure module item id is initialized
            Module.GetModelItemId(Context, Context.Node);
        }

    }

}
