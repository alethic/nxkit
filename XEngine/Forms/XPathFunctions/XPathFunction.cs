using System.Xml.XPath;
using System.Xml.Xsl;

namespace XEngine.Forms.XForms.XPathFunctions
{

    internal abstract class XPathFunction : IXsltContextFunction
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

    }

}
