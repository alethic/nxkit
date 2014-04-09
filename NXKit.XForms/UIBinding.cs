using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

using NXKit.DOMEvents;
using NXKit.Util;

namespace NXKit.XForms
{

    /// <summary>
    /// Encapsulates the binding information for a UI element.
    /// </summary>
    public class UIBinding :
        IUIBinding
    {

        readonly XElement element;
        readonly Binding binding;
        readonly Lazy<UIBindingState> state;
        ModelItem modelItem;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="node"></param>
        public UIBinding(XElement node)
        {
            Contract.Requires<ArgumentNullException>(node != null);

            this.element = node;
            this.state = new Lazy<UIBindingState>(() => element.AnnotationOrCreate<UIBindingState>());
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="node"></param>
        internal UIBinding(XElement node, ModelItem modelItem)
            : this(node)
        {
            Contract.Requires<ArgumentNullException>(node != null);

            this.modelItem = modelItem;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="binding"></param>
        internal UIBinding(XElement element, Binding binding)
            : this(element, binding.ModelItem)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(binding != null);

            this.binding = binding;
        }

        /// <summary>
        /// Gets the model item associated with this binding.
        /// </summary>
        public ModelItem ModelItem
        {
            get { return modelItem; }
        }

        /// <summary>
        /// Gets the state of the binding.
        /// </summary>
        UIBindingState State
        {
            get { return state.Value; }
        }

        /// <summary>
        /// Gets the current data type of the interface.
        /// </summary>
        public XName DataType
        {
            get { return State.DataType; }
        }

        /// <summary>
        /// Gets whether or not the interface is considered relevant.
        /// </summary>
        public bool Relevant
        {
            get { return GetRelevant(); }
        }

        bool GetRelevant()
        {
            if (State.Relevant == false)
                return false;

            var next = element.Ancestors()
                .Select(i => i.InterfaceOrDefault<IUIBindingNode>())
                .Where(i => i != null)
                .FirstOrDefault();
            if (next != null &&
                next.UIBinding != null)
                return next.UIBinding.Relevant;

            return true;
        }

        /// <summary>
        /// Gets whether or not the interface is considered read-only.
        /// </summary>
        public bool ReadOnly
        {
            get { return State.ReadOnly; }
        }

        /// <summary>
        /// Gets whether or not the interface is considered required.
        /// </summary>
        public bool Required
        {
            get { return State.Required; }
        }

        /// <summary>
        /// Gets whether or not the interface is considered valid.
        /// </summary>
        public bool Valid
        {
            get { return State.Valid; }
        }

        /// <summary>
        /// Gets the current value of the interface.
        /// </summary>
        public string Value
        {
            get { return State.Value; }
            set { SetValue(value); }
        }

        /// <summary>
        /// Implements the setter for Value.
        /// </summary>
        /// <param name="value"></param>
        void SetValue(string value)
        {
            if (modelItem != null)
                modelItem.Value = value;
        }

        /// <summary>
        /// Refreshes all properties.
        /// </summary>
        public void Refresh()
        {
            var oldItemType = DataType;
            var oldRelevant = Relevant;
            var oldReadOnly = ReadOnly;
            var oldRequired = Required;
            var oldValid = Valid;
            var oldValue = Value;

            if (binding != null)
            {
                binding.Refresh();

                if (modelItem != binding.ModelItem)
                    modelItem = binding.ModelItem;
            }

            if (modelItem != null)
            {
                State.DataType = modelItem.ItemType;
                State.Relevant = modelItem.Relevant;
                State.ReadOnly = modelItem.ReadOnly;
                State.Required = modelItem.Required;
                State.Valid = modelItem.Valid;
                State.Value = modelItem.Value;
            }
            else
            {
                // default values
                State.DataType = null;
                State.Relevant = true;
                State.ReadOnly = false;
                State.Required = false;
                State.Valid = true;
                State.Value = null;
            }

            // mark all required events
            var valueChanged = Value != oldValue;
            if (valueChanged)
            {
                Debug.WriteLine("{0}: Value changed: {1}", element, Value);
                State.DispatchValueChanged = true;
            }

            if (Relevant != oldRelevant || valueChanged)
            {
                Debug.WriteLine("{0}: Relevant changed: {1}", element, Relevant);

                if (Relevant)
                    State.DispatchEnabled = true;
                else
                    State.DispatchDisabled = true;
            }

            if (ReadOnly != oldReadOnly || valueChanged)
            {
                Debug.WriteLine("{0}: ReadOnly changed: {1}", element, Relevant);

                if (ReadOnly)
                    State.DispatchReadOnly = true;
                else
                    State.DispatchReadWrite = true;
            }

            if (Required != oldRequired || valueChanged)
            {
                Debug.WriteLine("{0}: Required changed: {1}", element, Required);

                if (Required)
                    State.DispatchRequired = true;
                else
                    State.DispatchOptional = true;
            }

            if (Valid != oldValid || valueChanged)
            {
                Debug.WriteLine("{0}: Valid changed: {1}", element, Valid);

                if (Valid)
                    State.DispatchValid = true;
                else
                    State.DispatchInvalid = true;
            }
        }

        /// <summary>
        /// Dispatches all pending events.
        /// </summary>
        public void DispatchEvents()
        {
            var target = element.Interface<INXEventTarget>();
            if (target == null)
                return;

            if (State.DispatchValueChanged)
            {
                State.DispatchValueChanged = false;
                target.DispatchEvent(Events.ValueChanged);
            }

            if (State.DispatchValid)
            {
                State.DispatchValid = false;
                target.DispatchEvent(Events.Valid);
            }

            if (State.DispatchInvalid)
            {
                State.DispatchInvalid = false;
                target.DispatchEvent(Events.Invalid);
            }

            if (State.DispatchEnabled)
            {
                State.DispatchEnabled = false;
                target.DispatchEvent(Events.Enabled);
            }

            if (State.DispatchDisabled)
            {
                State.DispatchDisabled = false;
                target.DispatchEvent(Events.Disabled);
            }

            if (State.DispatchOptional)
            {
                State.DispatchOptional = false;
                target.DispatchEvent(Events.Optional);
            }

            if (State.DispatchRequired)
            {
                State.DispatchRequired = false;
                target.DispatchEvent(Events.Required);
            }

            if (State.DispatchReadOnly)
            {
                State.DispatchReadOnly = false;
                target.DispatchEvent(Events.ReadOnly);
            }

            if (State.DispatchReadWrite)
            {
                State.DispatchReadWrite = false;
                target.DispatchEvent(Events.ReadWrite);
            }
        }

        /// <summary>
        /// Clears all pending events.
        /// </summary>
        public void DiscardEvents()
        {
            State.DispatchValueChanged = false;
            State.DispatchValid = false;
            State.DispatchInvalid = false;
            State.DispatchEnabled = false;
            State.DispatchDisabled = false;
            State.DispatchOptional = false;
            State.DispatchRequired = false;
            State.DispatchReadOnly = false;
            State.DispatchReadWrite = false;
        }

    }

}
