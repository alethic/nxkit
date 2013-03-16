using System;
using System.Web.UI;

namespace XEngine.Forms.Web.UI
{

    /// <summary>
    /// Base control to handle interfaces for <see cref="Visual"/> instances.
    /// </summary>
    public abstract class VisualControl : Control, INamingContainer
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        protected VisualControl(FormView view, Visual visual)
        {
            View = view;
            Visual = visual;
        }

        protected override void AddedControl(Control control, int index)
        {
            base.AddedControl(control, index);

            var ctl = control as VisualControl;
            if (ctl == null)
                return;

            // raise added event
            View.OnVisualControlAdded(new VisualControlAddedEventArgs(ctl));
        }

        /// <summary>
        /// Gets a reference to the containing <see cref="FormView"/>.
        /// </summary>
        public FormView View { get; private set; }

        /// <summary>
        /// Gets a reference to the <see cref="Visual"/> to be handled.
        /// </summary>
        public Visual Visual { get; private set; }

        /// <summary>
        /// Invoked for the Init phase.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnInit(EventArgs args)
        {
            base.OnInit(args);

            EnsureChildControls();
        }

    }

    /// <summary>
    /// Generic <see cref="VisualControl"/> type. Extend this class to handle specific <see cref="Visual"/> types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class VisualControl<T> : VisualControl
        where T : Visual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        protected VisualControl(FormView view, T visual)
            : base(view, visual)
        {

        }

        /// <summary>
        /// Gets a reference to the <see cref="Visual"/> to be handled.
        /// </summary>
        public new T Visual
        {
            get { return (T)base.Visual; }
        }

    }

}
