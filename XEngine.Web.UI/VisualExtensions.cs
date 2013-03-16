using System.Collections.Generic;
using System.Linq;

using XEngine.Forms.XForms;

namespace XEngine.Forms.Web.UI
{

    public static class VisualExtensions
    {

        /// <summary>
        /// Returns <c>true</c> if <paramref name="self"/> is a renderable visual. 
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool IsOpaque(this Visual self)
        {
            if (self is XFormsRepeatVisual)
                return false;
            else if (self is XFormsRepeatItemVisual)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Returns the first parent visuals which is not-transparent.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Visual OpaqueParent(this Visual self)
        {
            return self.Ascendants().FirstOrDefault(i => IsOpaque(i));
        }

        /// <summary>
        /// Returns the set of 'children' visuals which are children either directly or by virtue of being contained as
        /// children of transparent visuals.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        public static IEnumerable<Visual> OpaqueChildren(this StructuralVisual visual)
        {
            foreach (var child in visual.Children)
                if (!IsOpaque(child) && child is StructuralVisual)
                {
                    // if child is transparent, recurse
                    foreach (var child2 in OpaqueChildren((StructuralVisual)child))
                        yield return child2;
                }
                else
                    // child is not transparent, and is therefor an opaque child
                    yield return child;
        }

        public static XFormsLabelVisual FindLabelVisual(this StructuralVisual visual)
        {
            return visual.Children.OfType<XFormsLabelVisual>().FirstOrDefault();
        }

    }

}
