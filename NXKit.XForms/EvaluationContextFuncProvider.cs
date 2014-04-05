using System;
using System.Diagnostics.Contracts;

namespace NXKit.XForms
{

    /// <summary>
    /// Describes an evaluation context specified explicitly on the element.
    /// </summary>
    public class EvaluationContextFuncProvider :
        IEvaluationContextProvider
    {

        readonly Func<EvaluationContext> func;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="func"></param>
        public EvaluationContextFuncProvider(Func<EvaluationContext> func)
        {
            Contract.Requires<ArgumentNullException>(func != null);

            this.func = func;
        }

        /// <summary>
        /// Gets the evaluation context.
        /// </summary>
        public EvaluationContext Context
        {
            get { return func(); }
        }

    }

}
