using System.Collections.Generic;

namespace ISIS.Forms.Web.UI
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
            // handle opaque children that express that they should be handled as content

            foreach (var visual in Visual.OpaqueChildren())
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
