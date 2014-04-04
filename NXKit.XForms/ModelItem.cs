using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

using NXKit.Util;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides a wrapper for a model item that controls access to the underlying model item properties.
    /// </summary>
    public class ModelItem
    {

        static ModelItemState GetState(XFormsModule module, XObject item)
        {
            var state = item.Annotation<ModelItemState>();
            if (state == null)
                item.AddAnnotation(state = new ModelItemState());

            return state;
        }

        readonly XFormsModule module;
        readonly XObject xml;

        ModelItemState state;
        Model model;
        Instance instance;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="item"></param>
        public ModelItem(XFormsModule module, XObject item)
        {
            Contract.Requires<ArgumentNullException>(module != null);
            Contract.Requires<ArgumentNullException>(item != null);

            this.module = module;
            this.xml = item;
        }

        /// <summary>
        /// Gets a reference to the module.
        /// </summary>
        public XFormsModule Module
        {
            get { return module; }
        }

        /// <summary>
        /// Gets a reference to the underlying XML object.
        /// </summary>
        public XObject Xml
        {
            get { return xml; }
        }

        /// <summary>
        /// Gets the state structure for the given model item.
        /// </summary>
        /// <returns></returns>
        internal ModelItemState State
        {
            get { return state ?? (state = GetState()); }
        }

        /// <summary>
        /// Gets the state structure for the given model item.
        /// </summary>
        /// <returns></returns>
        ModelItemState GetState()
        {
            Contract.Ensures(Contract.Result<ModelItemState>() != null);

            return GetState(module, xml);
        }

        /// <summary>
        /// Gets the model element of the specified model item.
        /// </summary>
        /// <returns></returns>
        public Model Model
        {
            get { return model ?? (model = GetModel()); }
        }

        Model GetModel()
        {
            Contract.Ensures(Contract.Result<Model>() != null);

            return xml.Document.Annotation<Model>();
        }

        /// <summary>
        /// Gets the instance element of the specified model item.
        /// </summary>
        /// <returns></returns>
        public Instance Instance
        {
            get { return instance ?? (instance = GetInstance()); }
        }

        Instance GetInstance()
        {
            Contract.Ensures(Contract.Result<Instance>() != null);

            return xml.Document.Annotation<Instance>();
        }

        /// <summary>
        /// Gets the identifier of the model item.
        /// </summary>
        /// <returns></returns>
        public int Id
        {
            get { return GetId(); }
        }

        int GetId()
        {
            if (State.Id == null)
                State.Id = Instance.State.AllocateItemId();

            return (int)State.Id;
        }

        /// <summary>
        /// Gets the unique identifier for the given instance data node.
        /// </summary>
        /// <returns></returns>
        public string UniqueId
        {
            get { return GetUniqueId(); }
        }

        string GetUniqueId()
        {
            return module.GetAttributeValue(GetInstance().Element.Xml, "id") + "$" + GetId();
        }

        /// <summary>
        /// Gets or sets the type of the given model item.
        /// </summary>
        /// <returns></returns>
        public XName ItemType
        {
            get { return GetItemType(); }
            set { SetItemType(value); }
        }

        XName GetItemType()
        {
            return State.Type ?? NXKit.XmlSchemaConstants.XMLSchema + "string";
        }

        /// <summary>
        /// Sets the type of the given model item.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="item"></param>
        /// <param name="type"></param>
        public void SetItemType(XName type)
        {
            State.Type = type;
        }

        /// <summary>
        /// Gets whether the given model item is required.
        /// </summary>
        /// <returns></returns>
        public bool Required
        {
            get { return GetRequired(); }
        }

        bool GetRequired()
        {
            return State.Required ?? false;
        }

        /// <summary>
        /// Gets whether the given model item is read-only.
        /// </summary>
        /// <returns></returns>
        public bool ReadOnly
        {
            get { return GetReadOnly(); }
        }

        bool GetReadOnly()
        {
            return xml.AncestorsAndSelf().Any(i => GetState(module, i).ReadOnly ?? false);
        }

        /// <summary>
        /// Gets whether the given model item is relevant.
        /// </summary>
        /// <returns></returns>
        public bool Relevant
        {
            get { return GetRelevant(); }
        }

        bool GetRelevant()
        {
            return xml.AncestorsAndSelf().All(i => GetState(module, i).Relevant ?? true);
        }

        /// <summary>
        /// Gets whether the given model item's constraint value is currently valid.
        /// </summary>
        /// <returns></returns>
        public bool Constraint
        {
            get { return GetConstraint(); }
        }

        bool GetConstraint()
        {
            return State.Constraint ?? true;
        }

        /// <summary>
        /// Gets whether the given model item is currently valid.
        /// </summary>
        /// <returns></returns>
        public bool Valid
        {
            get { return GetValid(); }
        }

        bool GetValid()
        {
            return State.Valid ?? true;
        }

        public string Value
        {
            get { return GetValue(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets the value of the given instance data node.
        /// </summary>
        /// <returns></returns>
        string GetValue()
        {
            // obtain any scheduled new value
            if (State.Clear)
                return "";
            if (State.NewValue != null)
                return State.NewValue;
            if (State.NewContents != null)
                throw new InvalidOperationException();

            if (xml is XElement)
                return !((XElement)xml).HasElements ? ((XElement)xml).Value : null;
            else if (xml is XAttribute)
                return ((XAttribute)xml).Value;
            else
                throw new InvalidOperationException();
        }

        /// <summary>
        /// Sets the value of the given model item.
        /// </summary>
        /// <param name="newValue"></param>
        void SetValue(string newValue)
        {
            var lastValue = GetValue();
            if (lastValue == newValue)
                return;

            // register new value with model item
            State.Clear = false;
            State.NewContents = null;
            State.NewValue = newValue ?? "";

            // trigger recalculate event to collect new value
            Model.State.RecalculateFlag = true;
        }

        /// <summary>
        /// Gets the element value of the given model item.
        /// </summary>
        /// <param name="newElement"></param>
        public XElement Contents
        {
            get { return GetContents(); }
            set { SetContents(value); }
        }

        XElement GetContents()
        {
            Contract.Requires<ArgumentException>(Xml is XElement);

            if (State.Clear)
                return null;
            if (State.NewContents != null)
                return State.NewContents;
            if (state.NewValue != null)
                throw new InvalidOperationException();

            if (xml is XElement)
                return ((XElement)xml).HasElements ? (XElement)((XElement)xml).FirstNode : null;
            else
                throw new InvalidOperationException();
        }

        void SetContents(XElement newContents)
        {
            Contract.Requires<ArgumentException>(Xml is XElement);

            State.Clear = false;
            State.NewValue = null;
            State.NewContents = newContents;

            // trigger recalculate event to collect new value
            Model.State.RecalculateFlag = true;
        }

        /// <summary>
        /// Clears the given model item.
        /// </summary>
        public void Clear()
        {
            // register new value with model item
            State.Clear = true;
            State.NewValue = null;
            State.NewContents = null;

            // trigger recalculate event to collect new value
            Model.State.RecalculateFlag = true;
        }

        /// <summary>
        /// Applies all of the known values from the given model item to the current one.
        /// </summary>
        /// <param name="item"></param>
        public void Apply(ModelItem item)
        {
            State.Type = item.State.Type;
            State.Relevant = item.State.Relevant;
            State.ReadOnly = item.state.ReadOnly;
            state.Required = item.state.Required;
            state.Clear = item.State.Clear;
            state.NewValue = item.state.NewValue;
            state.NewContents = item.State.NewContents;
        }

    }

}
