using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.Xml;

namespace NXKit.XForms
{

    /// <summary>
    /// Encapsulates the binding information for a UI element.
    /// </summary>
    [Extension]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class DataNode :
        ElementExtension,
        IDataNode
    {

        readonly Lazy<IUIBindingNode> uiBindingNode;
        readonly Lazy<UIBinding> uiBinding;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public DataNode(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.uiBindingNode = new Lazy<IUIBindingNode>(() => Element.Interface<IUIBindingNode>());
            this.uiBinding = new Lazy<UIBinding>(() => uiBindingNode.Value.UIBinding);
        }

        /// <summary>
        /// Gets the <see cref="UIBinding"/> applied to this node.
        /// </summary>
        /// <returns></returns>
        UIBinding SelfUIBinding()
        {
            return uiBinding.Value;
        }

        /// <summary>
        /// Gets the data type of the node.
        /// </summary>
        public XName DataType
        {
            get { return GetDataType(); }
        }

        /// <summary>
        /// Implements the getter for Type.
        /// </summary>
        /// <returns></returns>
        XName GetDataType()
        {
            var self = SelfUIBinding();
            if (self != null)
                return self.DataType;

            return null;
        }

        /// <summary>
        /// Gets or sets the value of the node.
        /// </summary>
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
            var self = SelfUIBinding();
            if (self != null)
                return self.Value;

            return null;
        }

        /// <summary>
        /// Implements the setter for Value.
        /// </summary>
        /// <param name="value"></param>
        void SetValue(string value)
        {
            var self = SelfUIBinding();
            if (self != null)
                self.Value = value;
        }

        /// <summary>
        /// Gets whether or not the element is considered valid.
        /// </summary>
        public bool Valid
        {
            get { return GetValid(); }
        }

        /// <summary>
        /// Implements the getter for Valid.
        /// </summary>
        /// <returns></returns>
        bool GetValid()
        {
            var self = SelfUIBinding();
            if (self != null)
                return self.Valid;

            return true;
        }

    }

}
