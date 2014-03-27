using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Describes an element which is part of the interface hierarchy and supports binding.
    /// </summary>
    public abstract class UIBindingElement :
        BindingElement,
        IUIBindingNode
    {

        UIBinding uiBinding;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        protected UIBindingElement()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        protected UIBindingElement(XElement xml)
            : base(xml)
        {

        }

        /// <summary>
        /// Gets a reference to the default binding expressed on this node.
        /// </summary>
        public UIBinding UIBinding
        {
            get { return uiBinding; }
        }

        [Interactive]
        public XName ItemType
        {
            get { return UIBinding.DataType; }
        }

        [Interactive]
        public bool Relevant
        {
            get { return UIBinding.Relevant; }
        }

        [Interactive]
        public bool ReadOnly
        {
            get { return UIBinding.ReadOnly; }
        }

        [Interactive]
        public bool Required
        {
            get { return UIBinding.Required; }
        }

        [Interactive]
        public bool Valid
        {
            get { return UIBinding.Valid; }
        }

        protected virtual UIBinding CreateUIBinding()
        {
            return Binding != null ? new UIBinding(this, Binding) : new UIBinding(this);
        }

        internal void UIBind()
        {
            if (uiBinding == null)
                uiBinding = CreateUIBinding();
        }

        public override void Refresh()
        {
            base.Refresh();

            UIBind();

            if (uiBinding != null)
                uiBinding.Refresh();
        }

    }

}
