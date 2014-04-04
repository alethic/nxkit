using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Obtains the in-scope evaluation context for a UI node.
    /// </summary>
    public class NodeUIBindingEvaluationContextInScope
    {

        readonly NXElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public NodeUIBindingEvaluationContextInScope(NXElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
        }

        public EvaluationContext Context
        {
            get { return GetContext(); }
        }

        /// <summary>
        /// Implements the getter for Context.
        /// </summary>
        /// <returns></returns>
        EvaluationContext GetContext()
        {
            return element.Ancestors()
                .Select(i => i.InterfaceOrDefault<IEvaluationContextScope>())
                .Where(i => i != null)
                .Select(i => i.Context)
                .FirstOrDefault();
        }

    }

}
