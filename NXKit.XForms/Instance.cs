using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [NXElementInterface("{http://www.w3.org/2002/xforms}instance")]
    public class Instance :
        IOnInitialize
    {

        readonly XElement element;
        InstanceState state;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Instance(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
        }

        /// <summary>
        /// Gets the associated element.
        /// </summary>
        public XElement Element
        {
            get { return element; }
        }

        /// <summary>
        /// Gets the model of the instance.
        /// </summary>
        XElement Model
        {
            get { return element.Ancestors(Constants.XForms_1_0 + "model").First(); }
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
            var state = element.Annotation<InstanceState>();
            if (state == null)
                element.AddAnnotation(state = CreateState());

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

        void IOnInitialize.Init()
        {
            State.Initialize(Model, element);
        }

    }

}
