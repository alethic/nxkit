using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

using NXKit.Util;

namespace NXKit.Web.UI
{

    public class VisualControlCollection : Control
    {

        /// <summary>
        /// Maintains <see cref="VisualControl"/> instances for children visuals.
        /// </summary>
        private Dictionary<Visual, VisualControl> cache = new Dictionary<Visual, VisualControl>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        public VisualControlCollection(View view, StructuralVisual visual)
        {
            View = view;
            Visual = visual;
        }

        /// <summary>
        /// Gets a reference to the <see cref="View"/>.
        /// </summary>
        public View View { get; private set; }

        /// <summary>
        /// Gets a reference to the <see cref="Visual"/> associated with this manager.
        /// </summary>
        public StructuralVisual Visual { get; private set; }

        /// <summary>
        /// Invoked for the Init phase.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnInit(EventArgs args)
        {
            base.OnInit(args);

            EnsureChildControls();
        }

        /// <summary>
        /// Gets the set of visuals contained by this manager.
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<Visual> GetVisuals()
        {
            yield break;
        }

        /// <summary>
        /// Creates a new <see cref="VisualControl"/> instance for the specified visual.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        protected VisualControl CreateVisualControl(Visual visual)
        {
            return View.CreateVisualControl(visual);
        }

        /// <summary>
        /// Returns the <see cref="Control"/> instance that implements the specified <see cref="Visual"/>. Does not set 
        /// the control's ID properly. Rely on View.SetVisualControlID to calculate this based on the position of the
        /// control in the visual hierarchy.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        public virtual VisualControl GetOrCreateControl(Visual visual)
        {
            if (visual == null)
                throw new ArgumentNullException("visual");

            // return existing control
            var control = cache.GetOrDefault(visual);
            if (control == null)
            {
                // create visual control, if available
                control = CreateVisualControl(visual);
                if (control != null)
                    // prevents reentry
                    if (!cache.ContainsKey(visual))
                        cache[visual] = control;
            }

            return control;
        }

        /// <summary>
        /// Updates the collection of visual controls.
        /// </summary>
        public virtual void Update()
        {
            var controls = new LinkedList<VisualControl>();

            int i = 0;

            foreach (var visual in GetVisuals())
            {
                // acquire the control
                var ctl = GetOrCreateControl(visual);
                if (ctl == null)
                    continue;

                // add control to content container
                Controls.AddAt(i++, ctl);

                // control has been added to container, preserve later
                controls.AddLast(ctl);

                // set the id of the control
                View.SetVisualControlId(ctl);
            }

            // remove any controls that were not just added
            foreach (var ctl in Controls.Cast<VisualControl>().ToList())
                if (!controls.Contains(ctl))
                    Controls.Remove(ctl);
        }

        protected override void CreateChildControls()
        {
            Update();
        }

        protected override void OnPreRender(EventArgs args)
        {
            base.OnPreRender(args);

            Update();
        }

    }

}
