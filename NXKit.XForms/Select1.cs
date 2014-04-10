using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

using NXKit.Xml;

namespace NXKit.XForms
{

    [Interface("{http://www.w3.org/2002/xforms}select1")]
    [Remote]
    public class Select1 :
        UIBindingNode
    {

        readonly Select1Attributes attributes;
        readonly Lazy<IBindingNode> nodeBinding;
        readonly Lazy<UIBinding> uiBinding;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Select1(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.attributes = new Select1Attributes(element);
            this.nodeBinding = new Lazy<IBindingNode>(() => element.Interface<IBindingNode>());
            this.uiBinding = new Lazy<UIBinding>(() => new UIBinding(element, nodeBinding.Value.Binding));
        }

        /// <summary>
        /// Gets the property collection.
        /// </summary>
        public Select1Attributes Attributes
        {
            get { return attributes; }
        }

        /// <summary>
        /// Gets whether the selection is open or closed.
        /// </summary>
        public bool Open
        {
            get { return Attributes.Selection != "closed"; }
        }

        /// <summary>
        /// Gets the binding of the element.
        /// </summary>
        public Binding Binding
        {
            get { return nodeBinding.Value != null ? nodeBinding.Value.Binding : null; }
        }

        /// <summary>
        /// Gets the UI binding of the element.
        /// </summary>
        public UIBinding UIBinding
        {
            get { return uiBinding.Value; }
        }

        /// <summary>
        /// Gets the <see cref="EvaluationContext"/> provided to further children elements.
        /// </summary>
        public EvaluationContext Context
        {
            get { return Binding != null ? new EvaluationContext(Binding.ModelItem.Model, Binding.ModelItem.Instance, Binding.ModelItem, 1, 1) : null; }
        }

        /// <summary>
        /// Gets or sets the text value of an open selection.
        /// </summary>
        [Remote]
        public string Value
        {
            get { return GetValue(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Implements the getter for Value.
        /// </summary>
        /// <returns></returns>
        string GetValue()
        {
            if (UIBinding != null)
                return UIBinding.Value;

            return null;
        }

        /// <summary>
        /// Implements the setter for value.
        /// </summary>
        /// <param name="value"></param>
        void SetValue(string value)
        {
            if (UIBinding != null)
                UIBinding.Value = value;
        }

        /// <summary>
        /// Gets the currently selected item element.
        /// </summary>
        public ISelectable Selected
        {
            get { return GetSelected(); }
            set { SetSelected(value); }
        }

        /// <summary>
        /// Implements the getter for SelectedItemVisual.
        /// </summary>
        /// <returns></returns>
        ISelectable GetSelected()
        {
            return Element.Descendants()
                .SelectMany(i => i.Interfaces<ISelectable>())
                .Where(i => i.IsSelected(UIBinding))
                .FirstOrDefault();
        }

        /// <summary>
        /// Implements the setter for SelectedItemVisual.
        /// </summary>
        /// <param name="node"></param>
        void SetSelected(ISelectable selected)
        {
            // deselect existing item
            if (Selected != null &&
                selected == null)
                Selected.Deselect(UIBinding);

            // select new item
            if (selected != null &&
                selected != Selected)
                selected.Select(UIBinding);
        }

        /// <summary>
        /// Gets or sets the unique identifier of the selected item.
        /// </summary>
        [Remote]
        public string SelectedId
        {
            get { return Selected != null ? Selected.Id : null; }
            set { SetSelectedId(value); }
        }

        /// <summary>
        /// Implements the setter for SelectedItemVisualId.
        /// </summary>
        /// <param name="id"></param>
        void SetSelectedId(string id)
        {
            Selected = Element.Descendants()
                .SelectMany(i => i.Interfaces<ISelectable>())
                .FirstOrDefault(i => i.Id == id);
        }

    }

}
