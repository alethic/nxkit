using System.Collections.Generic;
using System.Linq;

using ISIS.Forms.XForms;
using System.Web.UI;

namespace ISIS.Forms.Web.UI.XForms
{

    /// <summary>
    /// Manages the common XForms visuals.
    /// </summary>
    public class CommonControlCollection : VisualControlCollection
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        public CommonControlCollection(FormView view, StructuralVisual visual)
            : base(view, visual)
        {

        }

        protected override IEnumerable<Visual> GetVisuals()
        {
            // our visuals
            if (LabelVisual != null)
                yield return LabelVisual;
            if (HelpVisual != null)
                yield return HelpVisual;
            if (HintVisual != null)
                yield return HintVisual;
            if (AlertVisual != null)
                yield return AlertVisual;
        }

        /// <summary>
        /// Gets a reference to the nested <see cref="XFormsLabelVisual"/>.
        /// </summary>
        public XFormsLabelVisual LabelVisual { get; private set; }

        /// <summary>
        /// Gets a reference to the label control.
        /// </summary>
        public LabelControl LabelControl
        {
            get { return LabelVisual != null ? (LabelControl)GetOrCreateControl(LabelVisual) : null; }
        }

        /// <summary>
        /// Gets a reference to the nested <see cref="XFormsHelpVisual"/>.
        /// </summary>
        public XFormsHelpVisual HelpVisual { get; private set; }

        /// <summary>
        /// Gets a reference to the help control.
        /// </summary>
        public Control HelpControl
        {
            get { return HelpVisual != null ? (Control)GetOrCreateControl(HelpVisual) : null; }
        }

        /// <summary>
        /// Gets a reference to the nested <see cref="XFormsHintVisual"/>.
        /// </summary>
        public XFormsHintVisual HintVisual { get; private set; }

        /// <summary>
        /// Gets a reference to the hint control.
        /// </summary>
        public Control HintControl
        {
            get { return HintVisual != null ? (Control)GetOrCreateControl(HintVisual) : null; }
        }

        /// <summary>
        /// Gets a reference to the nested <see cref="XFormsAlertVisual"/>.
        /// </summary>
        public XFormsAlertVisual AlertVisual { get; private set; }

        /// <summary>
        /// Gets a reference to the alert control.
        /// </summary>
        public Control AlertControl
        {
            get { return AlertVisual != null ? (Control)GetOrCreateControl(AlertVisual) : null; }
        }

        public override void Update()
        {
            // find visuals
            LabelVisual = Visual.Children.OfType<XFormsLabelVisual>().FirstOrDefault();
            HelpVisual = Visual.Children.OfType<XFormsHelpVisual>().FirstOrDefault();
            HintVisual = Visual.Children.OfType<XFormsHintVisual>().FirstOrDefault();
            AlertVisual = Visual.Children.OfType<XFormsAlertVisual>().FirstOrDefault();

            base.Update();
        }

    }


}
