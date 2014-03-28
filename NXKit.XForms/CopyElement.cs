using System.Linq;
using System.Xml.Linq;
using NXKit.DOMEvents;

namespace NXKit.XForms
{

    [Element("copy")]
    public class CopyElement :
        SingleNodeUIBindingElement,
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
        public void Select(SingleNodeUIBindingElement visual)
        {
            if (Binding == null ||
                Binding.ModelItem == null ||
                visual.Binding == null ||
                visual.Binding.ModelItem == null ||
                !(Binding.ModelItem.Xml is XElement) ||
                !(visual.Binding.ModelItem.Xml is XElement))
            {
                this.Interface<INXEventTarget>().DispatchEvent(Events.BindingException);
                return;
            }

            visual.Binding.ModelItem.Contents = Binding.ModelItem.Contents;
        }

        /// <summary>
        /// Implements the actions when unselecting the item from a list control.
        /// </summary>
        /// <param name="visual"></param>
        public void Deselect(SingleNodeUIBindingElement visual)
        {
            if (Binding == null ||
                Binding.ModelItem == null ||
                visual.Binding == null ||
                visual.Binding.ModelItem == null ||
                !(Binding.ModelItem.Xml is XElement) ||
                !(visual.Binding.ModelItem.Xml is XElement))
            {
                this.Interface<INXEventTarget>().DispatchEvent(Events.BindingException);
                return;
            }

            visual.Binding.ModelItem.Clear();
        }

        /// <summary>
        /// Returns <c>true</c> if the <see cref="SingleNodeUIBindingElement"/> currently has this item selected.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        public bool Selected(SingleNodeUIBindingElement visual)
        {
            if (visual.Binding == null ||
                visual.Binding.ModelItem == null)
                return false;

            if (Binding == null ||
                Binding.ModelItem == null)
                return false;

            if (!(Binding.ModelItem.Xml is XElement))
                return false;

            // our value matches the current value?
            var currentNode = ((XElement)visual.Binding.ModelItem.Xml).Elements().FirstOrDefault();
            var proposeNode = (XElement)Binding.ModelItem.Xml;

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
                Binding.ModelItem == null)
                return 0;

            if (!(Binding.ModelItem.Xml is XElement))
                return 0;

            // get deep hashcode of entire element tree
            return new XNodeEqualityComparer().GetHashCode((XElement)Binding.ModelItem.Xml);
        }

    }

}
