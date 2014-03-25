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
        /// <param name="xml"></param>
        public XFormsBindVisual(XElement xml)
            : base(xml)
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
