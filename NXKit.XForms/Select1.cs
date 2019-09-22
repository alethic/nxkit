using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.Xml;

namespace NXKit.XForms
{

    [Extension("{http://www.w3.org/2002/xforms}select1")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    [Remote]
    public class Select1 :
        ElementExtension
    {

        readonly Select1Attributes attributes;
        readonly Extension<IBindingNode> bindingNode;
        readonly Extension<IUIBindingNode> uiBindingNode;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public Select1(
            XElement element,
            Select1Attributes attributes,
            Extension<IBindingNode> bindingNode,
            Extension<IUIBindingNode> uiBindingNode)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
            this.bindingNode = bindingNode ?? throw new ArgumentNullException(nameof(bindingNode));
            this.uiBindingNode = uiBindingNode ?? throw new ArgumentNullException(nameof(uiBindingNode));
        }

        /// <summary>
        /// Gets whether the selection is open or closed.
        /// </summary>
        [Remote]
        public bool Open
        {
            get { return attributes.Selection != "closed"; }
        }

        /// <summary>
        /// Gets the binding of the element.
        /// </summary>
        public Binding Binding
        {
            get { return bindingNode.Value.Binding; }
        }

        /// <summary>
        /// Gets the UI binding of the element.
        /// </summary>
        public UIBinding UIBinding
        {
            get { return uiBindingNode.Value.UIBinding; }
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
        /// <param name="selected"></param>
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
        public int? SelectedId
        {
            get { return Selected != null ? (int?)Selected.Id : null; }
            set { SetSelectedId(value); }
        }

        /// <summary>
        /// Implements the setter for SelectedItemVisualId.
        /// </summary>
        /// <param name="id"></param>
        void SetSelectedId(int? id)
        {
            Selected = id != null ? Element.Descendants()
                .SelectMany(i => i.Interfaces<ISelectable>())
                .FirstOrDefault(i => i.Id == (int)id) : null;
        }

    }

}
