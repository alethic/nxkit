using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Describes an element which is part of the interface hierarchy and supports binding.
    /// </summary>
    [Public]
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
            get { return uiBinding ?? (uiBinding = CreateUIBinding()); }
        }

        [Public]
        public XName DataType
        {
            get { return UIBinding.DataType; }
        }

        [Public]
        public bool Relevant
        {
            get { return UIBinding.Relevant; }
        }

        [Public]
        public bool ReadOnly
        {
            get { return UIBinding.ReadOnly; }
        }

        [Public]
        public bool Required
        {
            get { return UIBinding.Required; }
        }

        [Public]
        public bool Valid
        {
            get { return UIBinding.Valid; }
        }

        protected virtual UIBinding CreateUIBinding()
        {
            return Binding != null ? new UIBinding(this, Binding) : new UIBinding(this);
        }

    }

}
