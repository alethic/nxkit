using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Encapsulates the binding information for a UI element.
    /// </summary>
    public class UIBinding
    {

        readonly NXNode node;
        readonly Binding binding;
        UIBindingState state;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="node"></param>
        internal UIBinding(NXNode node)
        {
            Contract.Requires<ArgumentNullException>(node != null);

            this.node = node;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="binding"></param>
        internal UIBinding(NXNode node, Binding binding)
            : this(node)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(binding != null);

            this.binding = binding;
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

        public XName ItemType
        {
            get { return State.ItemType; }
        }

        public bool Relevant
        {
            get { return State.Relevant; }
        }

        public bool ReadOnly
        {
            get { return State.ReadOnly; }
        }

        public bool Required
        {
            get { return State.Required; }
        }

        public bool Valid
        {
            get { return State.Valid; }
        }

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
                binding.Refresh();

            if (binding != null &&
                binding.ModelItem != null)
            {
                State.ItemType = binding.ModelItem.ItemType;
                State.Relevant = binding.ModelItem.Relevant;
                State.ReadOnly = binding.ModelItem.ReadOnly;
                State.Required = binding.ModelItem.Required;
                State.Valid = binding.ModelItem.Valid;
                State.Value = binding.ModelItem.Value;
            }
            else
            {
                // default values
                State.ItemType = null;
                State.Relevant = true;
                State.ReadOnly = false;
                State.Required = false;
                State.Valid = true;
                State.Value = null;
            }
        }

    }

}
