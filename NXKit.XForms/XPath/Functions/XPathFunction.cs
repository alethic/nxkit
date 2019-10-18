using System;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;

using NXKit.Diagnostics;
using NXKit.Xml;

namespace NXKit.XForms.XPath.Functions
{

    /// <summary>
    /// Base XPath function for XForms.
    /// </summary>
    public abstract class XPathFunction : IXsltContextFunction
    {

        readonly ITraceService trace;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="trace"></param>
        public XPathFunction(ITraceService trace)
        {
            this.trace = trace ?? throw new ArgumentNullException(nameof(trace));
        }

        public abstract int Maxargs { get; }

        public abstract int Minargs { get; }

        public abstract XPathResultType[] ArgTypes { get; }

        public abstract XPathResultType ReturnType { get; }

        /// <summary>
        /// Implement this method to provide function implementation.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected abstract object Invoke(EvaluationXsltContext context, XPathNavigator navigator, params object[] args);

        /// <summary>
        /// Implements <see cref="IXsltContextFunction"/>.Invoke.
        /// </summary>
        /// <param name="xsltContext"></param>
        /// <param name="args"></param>
        /// <param name="docContext"></param>
        /// <returns></returns>
        object IXsltContextFunction.Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
        {
            return Invoke((EvaluationXsltContext)xsltContext, docContext, args);
        }

        /// <summary>
        /// Gets the current model of the navigator.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="navigator"></param>
        /// <returns></returns>
        protected Model GetModel(EvaluationXsltContext context, XPathNavigator navigator)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (navigator == null)
                throw new ArgumentNullException(nameof(navigator));
            if (navigator.UnderlyingObject == null)
                throw new ArgumentException(nameof(navigator));
            if (navigator.UnderlyingObject is XObject == false)
                throw new ArgumentException(nameof(navigator));

            var result = GetModel(navigator);
            if (result == null)
                throw new NullReferenceException();

            return result;
        }

        /// <summary>
        /// Gets the current instance of the navigator.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="navigator"></param>
        /// <returns></returns>
        protected Instance GetInstance(EvaluationXsltContext context, XPathNavigator navigator)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (navigator == null)
                throw new ArgumentNullException(nameof(navigator));
            if (navigator.UnderlyingObject == null)
                throw new ArgumentException(nameof(navigator));
            if (navigator.UnderlyingObject is XObject == false)
                throw new ArgumentException(nameof(navigator));

            return GetInstance(navigator);
        }

        /// <summary>
        /// Gets the current model of the navigator.
        /// </summary>
        /// <param name="navigator"></param>
        /// <returns></returns>
        Model GetModel(XPathNavigator navigator)
        {
            if (navigator == null)
                throw new ArgumentNullException(nameof(navigator));
            if (navigator.UnderlyingObject == null)
                throw new ArgumentException(nameof(navigator));
            if (navigator.UnderlyingObject is XObject == false)
                throw new ArgumentException(nameof(navigator));

            return ((XObject)navigator.UnderlyingObject).AnnotationOrCreate(() => new ModelItem((XObject)navigator.UnderlyingObject, trace)).Model;
        }

        /// <summary>
        /// Gets the current instance of the navigator.
        /// </summary>
        /// <param name="navigator"></param>
        /// <returns></returns>
        Instance GetInstance(XPathNavigator navigator)
        {
            if (navigator == null)
                throw new ArgumentNullException(nameof(navigator));
            if (navigator.UnderlyingObject == null)
                throw new ArgumentException(nameof(navigator));
            if (navigator.UnderlyingObject is XObject == false)
                throw new ArgumentException(nameof(navigator));

            return ModelItem.Get((XObject)navigator.UnderlyingObject, trace).Instance;
        }

    }

}
