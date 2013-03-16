using System.Linq;
using System.Xml.Linq;

namespace ISIS.Forms.XForms
{

    [VisualTypeDescriptor(Constants.XForms_1_0_NS, "copy")]
    public class XFormsCopyVisualTypeDescriptor : VisualTypeDescriptor
    {

        public override Visual CreateVisual(IFormProcessor form, StructuralVisual parent, XNode node)
        {
            return new XFormsCopyVisual(parent, (XElement)node);
        }

    }

    public class XFormsCopyVisual : XFormsSingleNodeBindingVisual, ISelectableVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public XFormsCopyVisual(StructuralVisual parent, XElement element)
            : base(parent, element)
        {

        }

        /// <summary>
        /// Implements the actions when selecting the item from a list control.
        /// </summary>
        /// <param name="visual"></param>
        public void Select(XFormsSingleNodeBindingVisual visual)
        {
            if (Binding == null ||
                Binding.Node == null ||
                visual.Binding == null ||
                visual.Binding.Node == null ||
                !(Binding.Node is XElement) ||
                !(visual.Binding.Node is XElement))
            {
                DispatchEvent<XFormsBindingExceptionEvent>();
                return;
            }

            visual.Binding.SetValue((XElement)Binding.Node);
        }

        /// <summary>
        /// Implements the actions when unselecting the item from a list control.
        /// </summary>
        /// <param name="visual"></param>
        public void Deselect(XFormsSingleNodeBindingVisual visual)
        {
            if (Binding == null ||
                Binding.Node == null ||
                visual.Binding == null ||
                visual.Binding.Node == null ||
                !(Binding.Node is XElement) ||
                !(visual.Binding.Node is XElement))
            {
                DispatchEvent<XFormsBindingExceptionEvent>();
                return;
            }

            visual.Binding.ClearNode();
        }

        /// <summary>
        /// Returns <c>true</c> if the <see cref="XFormsSingleNodeBindingVisual"/> currently has this item selected.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        public bool Selected(XFormsSingleNodeBindingVisual visual)
        {
            if (visual.Binding == null ||
                visual.Binding.Node == null)
                return false;

            if (Binding == null ||
                Binding.Node == null)
                return false;

            if (!(Binding.Node is XElement))
                return false;

            // our value matches the current value?
            var currentNode = ((XElement)visual.Binding.Node).Elements().FirstOrDefault();
            var proposeNode = (XElement)Binding.Node;

            if (!XNode.DeepEquals(currentNode, proposeNode))
                return false;

            return true;
        }

        /// <summary>
        /// Returns an integer hash code that should identify the bound node.
        /// </summary>
        /// <returns></returns>
        public int GetValueHashCode()
        {
            if (Binding == null ||
                Binding.Node == null)
                return 0;

            if (!(Binding.Node is XElement))
                return 0;

            // get deep hashcode of entire element tree
            return new XNodeEqualityComparer().GetHashCode((XElement)Binding.Node);
        }

    }

}
