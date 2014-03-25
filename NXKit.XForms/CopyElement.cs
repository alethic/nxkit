using System.Linq;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [Element("copy")]
    public class CopyElement :
        SingleNodeBindingElement,
        ISelectableNode
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public CopyElement(XElement element)
            : base(element)
        {

        }

        /// <summary>
        /// Implements the actions when selecting the item from a list control.
        /// </summary>
        /// <param name="visual"></param>
        public void Select(SingleNodeBindingElement visual)
        {
            if (Binding == null ||
                Binding.Node == null ||
                visual.Binding == null ||
                visual.Binding.Node == null ||
                !(Binding.Node is XElement) ||
                !(visual.Binding.Node is XElement))
            {
                DispatchEvent<BindingExceptionEvent>();
                return;
            }

            visual.Binding.SetValue((XElement)Binding.Node);
        }

        /// <summary>
        /// Implements the actions when unselecting the item from a list control.
        /// </summary>
        /// <param name="visual"></param>
        public void Deselect(SingleNodeBindingElement visual)
        {
            if (Binding == null ||
                Binding.Node == null ||
                visual.Binding == null ||
                visual.Binding.Node == null ||
                !(Binding.Node is XElement) ||
                !(visual.Binding.Node is XElement))
            {
                DispatchEvent<BindingExceptionEvent>();
                return;
            }

            visual.Binding.ClearNode();
        }

        /// <summary>
        /// Returns <c>true</c> if the <see cref="SingleNodeBindingElement"/> currently has this item selected.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        public bool Selected(SingleNodeBindingElement visual)
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
