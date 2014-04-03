using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Describes an element which is part of the interface hierarchy and supports binding.
    /// </summary>
    public abstract class UIBindingElement :
        BindingElement,
        IModelItemBinding,
        IUIBindingNode
    {

        UIBinding uiBinding;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        protected UIBindingElement(XName name)
            : base(name)
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


        public virtual string Value
        {
            get { return UIBinding.Value; }
            set { UIBinding.Value = value; }
        }

        public XName DataType
        {
            get { return UIBinding.DataType; }
        }

        public bool Relevant
        {
            get { return UIBinding.Relevant; }
        }

        public bool ReadOnly
        {
            get { return UIBinding.ReadOnly; }
        }

        public bool Required
        {
            get { return UIBinding.Required; }
        }

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
