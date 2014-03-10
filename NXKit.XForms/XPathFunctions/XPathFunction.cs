using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace NXKit.XForms.XPathFunctions
{

    /// <summary>
    /// Base XPath function for XForms.
    /// </summary>
    internal abstract class XPathFunction :
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
        protected abstract object Invoke(VisualXmlNamespaceContext context, XPathNavigator navigator, params object[] args);

        /// <summary>
        /// Implements <see cref="IXsltContextFunction"/>.Invoke.
        /// </summary>
        /// <param name="xsltContext"></param>
        /// <param name="args"></param>
        /// <param name="docContext"></param>
        /// <returns></returns>
        object IXsltContextFunction.Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
        {
            return Invoke((VisualXmlNamespaceContext)xsltContext, docContext, args);
        }

        /// <summary>
        /// Gets the current model of the navigator.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="navigator"></param>
        /// <returns></returns>
        protected XFormsModelVisual GetModel(VisualXmlNamespaceContext context, XPathNavigator navigator)
        {
            Contract.Requires<ArgumentNullException>(context != null);
            Contract.Requires<ArgumentNullException>(navigator != null);
            Contract.Requires<ArgumentNullException>(navigator.UnderlyingObject != null);
            Contract.Requires<ArgumentNullException>(navigator.UnderlyingObject is XObject);

            return GetModel(context.Visual.Engine.GetModule<XFormsModule>(), navigator);
        }

        /// <summary>
        /// Gets the current instance of the navigator.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="navigator"></param>
        /// <returns></returns>
        protected XFormsInstanceVisual GetInstance(VisualXmlNamespaceContext context, XPathNavigator navigator)
        {
            Contract.Requires<ArgumentNullException>(context != null);
            Contract.Requires<ArgumentNullException>(navigator != null);
            Contract.Requires<ArgumentNullException>(navigator.UnderlyingObject != null);
            Contract.Requires<ArgumentNullException>(navigator.UnderlyingObject is XObject);

            return GetInstance(context.Visual.Engine.GetModule<XFormsModule>(), navigator);
        }

        /// <summary>
        /// Gets the current model of the navigator.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="navigator"></param>
        /// <returns></returns>
        XFormsModelVisual GetModel(XFormsModule module, XPathNavigator navigator)
        {
            Contract.Requires<ArgumentNullException>(module != null);
            Contract.Requires<ArgumentNullException>(navigator != null);
            Contract.Requires<ArgumentNullException>(navigator.UnderlyingObject != null);
            Contract.Requires<ArgumentNullException>(navigator.UnderlyingObject is XObject);

            return module.GetModelItemModel((XObject)navigator.UnderlyingObject);
        }

        /// <summary>
        /// Gets the current instance of the navigator.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="navigator"></param>
        /// <returns></returns>
        XFormsInstanceVisual GetInstance(XFormsModule module, XPathNavigator navigator)
        {
            Contract.Requires<ArgumentNullException>(navigator != null);
            Contract.Requires<ArgumentNullException>(navigator.UnderlyingObject != null);
            Contract.Requires<ArgumentNullException>(navigator.UnderlyingObject is XObject);

            return module.GetModelItemInstance((XObject)navigator.UnderlyingObject);
        }

    }

}
