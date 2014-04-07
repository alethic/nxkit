using System;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Abstract implementation for all visuals that support binding expressions.
    /// </summary>
    public abstract class BindingElement :
        NXElement
    {

        readonly Lazy<Binding> binding;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        protected BindingElement(XName name)
            : base(name)
        {
            this.binding = new Lazy<Binding>(() => this.Interface<IBindingNode>().Binding);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        protected BindingElement(XElement xml)
            : base(xml)
        {
            this.binding = new Lazy<Binding>(() => this.Interface<IBindingNode>().Binding);
        }

        /// <summary>
        /// Gets a reference to the XForms module.
        /// </summary>
        public XFormsModule Module
        {
            get { return Document.Module<XFormsModule>(); }
        }

        /// <summary>
        /// Gets a reference to the default binding expressed on this node.
        /// </summary>
        public Binding Binding
        {
            get { return binding.Value; }
        }

    }

}
