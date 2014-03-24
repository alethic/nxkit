using System.Xml.Linq;
namespace NXKit.XForms
{

    public class XFormsItemSetItemVisual :
        XFormsItemVisual, 
        IEvaluationContextScope,
        INamingScope
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public XFormsItemSetItemVisual(NXElement parent, XElement element)
            : base(parent, element)
        {

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

    }

}
