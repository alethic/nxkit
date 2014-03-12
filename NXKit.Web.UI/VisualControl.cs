using System;
using System.Diagnostics.Contracts;
using System.Web.UI;

namespace NXKit.Web.UI
{

    /// <summary>
    /// Base control to handle interfaces for <see cref="Visual"/> instances.
    /// </summary>
    public abstract class VisualControl : 
        Control, 
        INamingContainer
    {

        readonly View view;
        readonly Visual visual;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        protected VisualControl(View view, Visual visual)
        {
            Contract.Requires<ArgumentNullException>(view != null);
            Contract.Requires<ArgumentNullException>(visual != null);

            this.view = view;
            this.visual = visual;
        }

        /// <summary>
        /// Gets a reference to the containing <see cref="View"/>.
        /// </summary>
        public View View
        {
            get { return view; }
        }

        /// <summary>
        /// Gets a reference to the <see cref="Visual"/> to be handled.
        /// </summary>
        public Visual Visual
        {
            get { return visual; }
        }

        /// <summary>
        /// Called after a child control is added to the controls collection.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="index"></param>
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
        /// Invoked for the Init phase.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnInit(EventArgs args)
        {
            Page.LoadComplete += Page_LoadComplete;
            base.OnInit(args);
            EnsureChildControls();
        }

        /// <summary>
        /// Invoked when the Page.LoadComplete event is raised.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void Page_LoadComplete(object sender, EventArgs args)
        {
            OnLoadComplete(args);
        }

        /// <summary>
        /// Occurs at the end of the load stage of the page's lifecycle.
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnLoadComplete(EventArgs args)
        {

        }

    }

    /// <summary>
    /// Generic <see cref="VisualControl"/> type. Extend this class to handle specific <see cref="Visual"/> types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class VisualControl<T> : 
        VisualControl
        where T : Visual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        protected VisualControl(View view, T visual)
            : base(view, visual)
        {
            Contract.Requires<ArgumentNullException>(view != null);
            Contract.Requires<ArgumentNullException>(visual != null);
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
