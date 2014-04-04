using System;
using System.Diagnostics.Contracts;

namespace NXKit.XForms
{

    [NXElementInterface("{http://www.w3.org/2002/xforms}bind")]
    public class Bind :
        IEvaluationContextScope
    {

        readonly NXElement element;
        INodeBinding nodeBinding;
        BindAttributes attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Bind(NXElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
        }

        /// <summary>
        /// Gets the 'bind' element.
        /// </summary>
        public NXElement Element
        {
            get { return element; }
        }

        /// <summary>
        /// Provides the binding specified on the element.
        /// </summary>
        public INodeBinding NodeBinding
        {
            get { return nodeBinding ?? (nodeBinding = element.Interface<INodeBinding>()); }
        }

        /// <summary>
        /// Gets the attributes of the bind element.
        /// </summary>
        public BindAttributes Attributes
        {
            get { return attributes ?? (attributes = new BindAttributes(element)); }
        }

        /// <summary>
        /// Gets the binding of the element.
        /// </summary>
        public Binding Binding
        {
            get { return NodeBinding != null ? NodeBinding.Binding : null; }
        }

        /// <summary>
        /// Gets the <see cref="EvaluationContext"/> provided to further children elements.
        /// </summary>
        public EvaluationContext Context
        {
            get { return Binding != null ? new EvaluationContext(Binding.ModelItem.Model, Binding.ModelItem.Instance, Binding.ModelItem, 1, 1) : null; }
        }

    }

}
