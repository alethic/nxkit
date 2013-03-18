using System.ComponentModel.Composition;

using NXKit.Web.UI;
using NXKit.XForms.Web.UI;

namespace NXKit.XForms.Layout.Web.UI
{

    public class LayoutModule : FormModule
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        [ImportingConstructor]
        public LayoutModule([Import(FormModule.ViewParameter)] FormView view)
            : base(view)
        {
            View.VisualControlAdded += View_VisualControlAdded;
        }

        /// <summary>
        /// Invoked when a new <see cref="VisualControl"/> is added.
        /// </summary>
        /// <param name="args"></param>
        private void View_VisualControlAdded(VisualControlAddedEventArgs args)
        {
            var groupCtl = args.Control as GroupControl;
            if (groupCtl != null)
                groupCtl.BeginRender += group_BeginRender;
        }

        private void group_BeginRender(object sender, BeginRenderEventArgs args)
        {
            var groupCtl = sender as GroupControl;
            if (groupCtl == null)
                return;

            var annotation = groupCtl.Visual.Annotations.Get<ImportanceAnnotation>();
            if (annotation == null)
                return;

            switch (annotation.Importance)
            {
                case Importance.High:
                    args.CssClasses.Add("Layout__Importance_High");
                    break;
                case Importance.Low:
                    args.CssClasses.Add("Layout__Importance_Low");
                    break;
            }
        }

    }

}
