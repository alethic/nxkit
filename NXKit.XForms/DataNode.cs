using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms
{

    /// <summary>
    /// Encapsulates the binding information for a UI element.
    /// </summary>
    [Extension(PredicateType = typeof(DataNodePredicate))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class DataNode :
        ElementExtension,
        IDataNode
    {

        public class DataNodePredicate :
            ExtensionPredicateBase
        {

            public override bool IsMatch(XObject obj)
            {
                var element = obj as XElement;
                if (element != null)
                    return element.Name != Constants.XForms_1_0 + "bind";

                return true;
            }

        }

        readonly Extension<IUIBindingNode> uiBindingNode;
        readonly Lazy<UIBinding> uiBinding;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="uiBindingNode"></param>
        [ImportingConstructor]
        public DataNode(
            XElement element,
            Extension<IUIBindingNode> uiBindingNode)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.uiBindingNode = uiBindingNode ?? throw new ArgumentNullException(nameof(uiBindingNode));
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
