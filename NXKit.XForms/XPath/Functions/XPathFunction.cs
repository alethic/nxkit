using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;

using NXKit.Xml;

namespace NXKit.XForms.XPath.Functions
{

    /// <summary>
    /// Base XPath function for XForms.
    /// </summary>
    public abstract class XPathFunction :
        IXsltContextFunction
    {

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
            Contract.Requires<ArgumentNullException>(context != null);
            Contract.Requires<ArgumentNullException>(navigator != null);
            Contract.Requires<ArgumentNullException>(navigator.UnderlyingObject != null);
            Contract.Requires<ArgumentNullException>(navigator.UnderlyingObject is XObject);

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
            Contract.Requires<ArgumentNullException>(context != null);
            Contract.Requires<ArgumentNullException>(navigator != null);
            Contract.Requires<ArgumentNullException>(navigator.UnderlyingObject != null);
            Contract.Requires<ArgumentNullException>(navigator.UnderlyingObject is XObject);

            return GetInstance(navigator);
        }

        /// <summary>
        /// Gets the current model of the navigator.
        /// </summary>
        /// <param name="navigator"></param>
        /// <returns></returns>
        Model GetModel(XPathNavigator navigator)
        {
            Contract.Requires<ArgumentNullException>(navigator != null);
            Contract.Requires<ArgumentNullException>(navigator.UnderlyingObject != null);
            Contract.Requires<ArgumentNullException>(navigator.UnderlyingObject is XObject);
            Contract.Ensures(Contract.Result<Model>() != null);

            return ((XObject)navigator.UnderlyingObject).AnnotationOrCreate<ModelItem>(() =>
                new ModelItem((XObject)navigator.UnderlyingObject)).Model;
        }

        /// <summary>
        /// Gets the current instance of the navigator.
        /// </summary>
        /// <param name="navigator"></param>
        /// <returns></returns>
        Instance GetInstance(XPathNavigator navigator)
        {
            Contract.Requires<ArgumentNullException>(navigator != null);
            Contract.Requires<ArgumentNullException>(navigator.UnderlyingObject != null);
            Contract.Requires<ArgumentNullException>(navigator.UnderlyingObject is XObject);
            Contract.Ensures(Contract.Result<Instance>() != null);

            return ((XObject)navigator.UnderlyingObject).AnnotationOrCreate<ModelItem>(() =>
                new ModelItem((XObject)navigator.UnderlyingObject)).Instance;
        }

    }

}
