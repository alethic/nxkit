using System;
using System.Linq;
using System.Xml.Linq;

using NXKit.Xml;

namespace NXKit.XForms
{

    /// <summary>
    /// Resolves various <see cref="EvaluationContext"/> instances with regards to the specified <see cref="XObject"/>.
    /// </summary>
    public abstract class EvaluationContextResolver
    {

        readonly XObject obj;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="obj"></param>
        public EvaluationContextResolver(XObject obj)
        {
            this.obj = obj ?? throw new ArgumentNullException(nameof(obj));
        }

        /// <summary>
        /// Gets the <see cref="XObject"/> this instance is resolving <see cref="EvaluationContext"/>s for.
        /// </summary>
        public XObject Object
        {
            get { return obj; }
        }

        /// <summary>
        /// Returns the <see cref="EvaluationContext"/> described by the element, if the element is itself a model.
        /// </summary>
        /// <returns></returns>
        internal EvaluationContext GetSelfModelEvaluationContext()
        {
            return obj.Interfaces<Model>()
                .Select(i => i.DefaultEvaluationContext)
                .FirstOrDefault(i => i != null);
        }

        /// <summary>
        /// Returns the <see cref="EvaluationContext"/> from any parent 'model' element.
        /// </summary>
        /// <returns></returns>
        internal EvaluationContext GetAncestorModelEvaluationContext()
        {
            return obj.Ancestors(Constants.XForms_1_0 + "model")
                .SelectMany(i => i.Interfaces<Model>())
                .Select(i => i.DefaultEvaluationContext)
                .FirstOrDefault(i => i != null);
        }

        /// <summary>
        /// Gets the <see cref="EvaluationContext"/> to be used by default when no other is available by other means.
        /// </summary>
        /// <returns></returns>
        internal EvaluationContext GetDefaultEvaluationContext()
        {
            return obj.Document
                .Descendants(Constants.XForms_1_0 + "model")
                .SelectMany(i => i.Interfaces<Model>())
                .Select(i => i.DefaultEvaluationContext)
                .FirstOrDefault(i => i != null);
        }

        /// <summary>
        /// Returns the in-scope evaluation context. This is the evalutation context contributed to this <see
        /// cref="XObject"/> by its parents.
        /// </summary>
        /// <returns></returns>
        public EvaluationContext GetInScopeEvaluationContext()
        {
            return obj.Ancestors()
                .SelectMany(i => i.Interfaces<IEvaluationContextScope>())
                .Select(i => i.Context)
                .FirstOrDefault(i => i != null);
        }

        /// <summary>
        /// Returns the provided evaluation context. This is the evaluation context set directly on the this <see
        /// cref="XObject"/>.
        /// </summary>
        /// <returns></returns>
        public EvaluationContext GetSelfEvaluationContext()
        {
            return obj.Interfaces<IEvaluationContextProvider>()
                .Select(i => i.Context)
                .FirstOrDefault(i => i != null);
        }

        /// <summary>
        /// Returns the <see cref="EvaluationContext"/> to be used by this <see cref="XObject"/>
        /// </summary>
        /// <returns></returns>
        protected virtual EvaluationContext GetContext()
        {
            return
                GetSelfEvaluationContext() ??
                GetSelfModelEvaluationContext() ??
                GetInScopeEvaluationContext() ??
                GetAncestorModelEvaluationContext() ??
                GetDefaultEvaluationContext();
        }

        /// <summary>
        /// Returns the <see cref="EvaluationContext"/> to be used by this <see cref="XObject"/>.
        /// </summary>
        public EvaluationContext Context
        {
            get { return GetContext(); }
        }

    }

}
