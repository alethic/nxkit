using System.Linq;
using System.Web.UI;

using XEngine.Forms;

namespace XEngine.Forms.Web.UI.XForms
{

    [VisualControlTypeDescriptor]
    public class TriggerVisualDescriptor : VisualControlTypeDescriptor
    {

        public override bool CanHandleVisual(Visual visual)
        {
            return visual is XFormsTriggerVisual;
        }

        public override bool IsContent(Visual visual)
        {
            return true;
        }

        public override VisualControl CreateControl(FormView view, Visual visual)
        {
            return new TriggerControl(view, (XFormsTriggerVisual)visual);
        }

    }

    /// <summary>
    /// Provides rendering for a <see cref="XFormsTriggerVisual"/>.
    /// </summary>
    public class TriggerControl : VisualControl<XFormsTriggerVisual>, IPostBackEventHandler
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        public TriggerControl(FormView view, XFormsTriggerVisual visual)
            : base(view, visual)
        {

        }

        public CommonControlCollection Common { get; private set; }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            Controls.Add(Common = new CommonControlCollection(View, Visual));
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, Page.ClientScript.GetPostBackClientHyperlink(this, "DOMActivate"));
            writer.RenderBeginTag(HtmlTextWriterTag.Button);

            if (Common.LabelControl != null)
                Common.LabelControl.RenderControl(writer);

            writer.RenderEndTag();
        }

        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            var args = eventArgument.Split(':');

            switch (args[0])
            {
                case "DOMActivate":
                    Visual.DispatchEvent<DOMActivateEvent>();
                    View.Form.Run();
                    break;
            }
        }

    }

}
