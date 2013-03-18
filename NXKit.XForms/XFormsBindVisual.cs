using System.Xml.Linq;

namespace NXKit.XForms
{

    [Visual("bind")]
    public class XFormsBindVisual : XFormsBindingVisual
    {

        public override void Refresh()
        {
            // rebuild binding
            Binding = Module.ResolveNodeSetBinding(this);

            base.Refresh();

            // rebuild children
            base.InvalidateChildren();
        }

        /// <summary>
        /// Provides the default evaluation context for child elements, the first element.
        /// </summary>
        public override XFormsEvaluationContext Context
        {
            get { return null; }
        }

    }

}
