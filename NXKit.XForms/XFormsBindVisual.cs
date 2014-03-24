using System.Xml.Linq;

namespace NXKit.XForms
{

    [Visual("bind")]
    public class XFormsBindVisual :
        XFormsBindingVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public XFormsBindVisual(NXElement parent, XElement element)
            : base(parent, element)
        {

        }

        public override void Refresh()
        {
            // rebuild binding
            Binding = Module.ResolveNodeSetBinding(this);

            base.Refresh();
            base.CreateNodes();
        }

        /// <summary>
        /// TODO should provide a context to nested bind elements.
        /// </summary>
        /// <returns></returns>
        protected override XFormsEvaluationContext CreateEvaluationContext()
        {
            return null;
        }

    }

}
