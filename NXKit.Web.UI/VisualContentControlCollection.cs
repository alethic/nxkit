using System.Collections.Generic;

namespace NXKit.Web.UI
{

    public class VisualContentControlCollection : VisualControlCollection
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        public VisualContentControlCollection(FormView view, StructuralVisual visual, bool includeTextAsContent = false)
            : base(view, visual)
        {
            IncludeTextAsContent = includeTextAsContent;
        }

        /// <summary>
        /// Gets whether or not textual content will be returned as content.
        /// </summary>
        public bool IncludeTextAsContent { get; private set; }

        protected override IEnumerable<Visual> GetVisuals()
        {
            foreach (var visual in View.OpaqueChildren(Visual))
            {
                // skip text content?
                if (!IncludeTextAsContent && visual is TextVisual)
                    continue;

                var desc = View.ResolveVisualControlDescriptor(visual);
                if (desc == null)
                    continue;

                // filter out non-content visuals
                if (!desc.IsContent(visual))
                    continue;

                yield return visual;
            }
        }

    }

}
