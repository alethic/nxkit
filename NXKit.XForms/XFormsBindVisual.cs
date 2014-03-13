namespace NXKit.XForms
{

    [Visual("bind")]
    public class XFormsBindVisual :
        XFormsBindingVisual
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
        /// TODO should provide a context to nested bind elements.
        /// </summary>
        /// <returns></returns>
        protected override XFormsEvaluationContext CreateEvaluationContext()
        {
            return null;
        }

    }

}
