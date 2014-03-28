using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

using NXKit.DOMEvents;

namespace NXKit.XForms
{

    /// <summary>
    /// Encapsulates the binding information for a UI element.
    /// </summary>
    public class UIBinding
    {

        readonly NXElement node;
        readonly Binding binding;
        ModelItem modelItem;
        UIBindingState state;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="node"></param>
        internal UIBinding(NXElement node)
        {
            Contract.Requires<ArgumentNullException>(node != null);

            this.node = node;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="node"></param>
        internal UIBinding(NXElement node, ModelItem modelItem)
            : this(node)
        {
            Contract.Requires<ArgumentNullException>(node != null);

            this.modelItem = modelItem;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="binding"></param>
        internal UIBinding(NXElement node, Binding binding)
            : this(node, binding.ModelItem)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(binding != null);

            this.binding = binding;
        }

        /// <summary>
        /// Gets the module.
        /// </summary>
        public XFormsModule Module
        {
            get { return node.Document.Module<XFormsModule>(); }
        }

        /// <summary>
        /// Gets the model item associated with this binding.
        /// </summary>
        public ModelItem ModelItem
        {
            get { return modelItem; }
        }

        UIBindingState State
        {
            get { return state ?? (state = GetState()); }
        }

        UIBindingState GetState()
        {
            var state = node.Storage.OfType<UIBindingState>().FirstOrDefault();
            if (state == null)
                node.Storage.AddLast(state = new UIBindingState());

            return state;
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

            var next = node.Ancestors()
                .Select(i => i.InterfaceOrDefault<IUIBindingNode>())
                .Where(i => i != null)
                .FirstOrDefault();
            if (next != null)
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
        }

        /// <summary>
        /// Refreshes all properties.
        /// </summary>
        public void Refresh()
        {
            if (binding != null)
            {
                binding.Refresh();

                if (modelItem != binding.ModelItem)
                    modelItem = binding.ModelItem;
            }

            var oldItemType = State.DataType;
            var oldRelevant = State.Relevant;
            var oldReadOnly = State.ReadOnly;
            var oldRequired = State.Required;
            var oldValid = State.Valid;
            var oldValue = State.Value;

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
                State.DispatchValueChanged = true;

            if (Relevant != oldRelevant || valueChanged)
                if (Relevant)
                    State.DispatchEnabled = true;
                else
                    State.DispatchDisabled = true;

            if (ReadOnly != oldReadOnly || valueChanged)
                if (ReadOnly)
                    State.DispatchReadOnly = true;
                else
                    State.DispatchReadWrite = true;

            if (Required != oldRequired || valueChanged)
                if (Required)
                    State.DispatchRequired = true;
                else
                    State.DispatchOptional = true;

            if (Valid != oldValid || valueChanged)
                if (Valid)
                    state.DispatchValid = true;
                else
                    state.DispatchInvalid = true;
        }

        /// <summary>
        /// Dispatches all pending events.
        /// </summary>
        public void DispatchEvents()
        {
            var target = node.Interface<INXEventTarget>();
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
        public void ClearEvents()
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
