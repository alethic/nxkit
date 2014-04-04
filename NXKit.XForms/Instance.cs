using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace NXKit.XForms
{

    [NXElement("{http://www.w3.org/2002/xforms}instance")]
    public class Instance :
        IInitialize,
        INodeContents
    {

        readonly NXElement element;
        InstanceState state;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Instance(NXElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
        }

        /// <summary>
        /// Gets the associated element.
        /// </summary>
        public NXElement Element
        {
            get { return element; }
        }

        /// <summary>
        /// Gets the model of the instance.
        /// </summary>
        NXElement Model
        {
            get { return element.Ancestors().OfType<NXElement>().First(i => i.Name == Constants.XForms_1_0 + "model"); }
        }

        /// <summary>
        /// Gets the instance state associated with this instance visual.
        /// </summary>
        public InstanceState State
        {
            get { return state ?? (state = GetState()); }
        }

        /// <summary>
        /// Implements the getter for State.
        /// </summary>
        /// <returns></returns>
        InstanceState GetState()
        {
            var state = element.Storage.OfType<InstanceState>().FirstOrDefault();
            if (state == null)
                element.Storage.AddLast(state = CreateState());

            return state;
        }

        /// <summary>
        /// Creates a new state instance.
        /// </summary>
        /// <returns></returns>
        InstanceState CreateState()
        {
            var state = new InstanceState();
            state.Initialize(Model, element);
            return state;
        }

        void IInitialize.Init()
        {
            State.Initialize(Model, element);
        }

        IEnumerable<NXNode> INodeContents.GetContents()
        {
            yield break;
        }

    }

}
