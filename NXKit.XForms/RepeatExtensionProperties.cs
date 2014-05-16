using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using System.Xml.XPath;

using NXKit.Xml;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms repeat extension properties.
    /// </summary>
    public class RepeatExtensionProperties
    {

        readonly XElement element;
        readonly RepeatExtensionAttributes attributes;
        readonly EvaluationContext context;
        readonly Lazy<XPathExpression> ref_;
        readonly Lazy<XPathExpression> nodeSet;
        readonly Lazy<XPathExpression> indexRef;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="context"></param>
        public RepeatExtensionProperties(XElement element, EvaluationContext context)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(context != null);

            this.attributes = element.AnnotationOrCreate(() => new RepeatExtensionAttributes(element));
            this.context = context;

            this.ref_ = new Lazy<XPathExpression>(() =>
                !string.IsNullOrEmpty(attributes.Ref) ?
                context.CompileXPath(element, attributes.Ref) :
                null);

            this.nodeSet = new Lazy<XPathExpression>(() =>
                !string.IsNullOrEmpty(attributes.NodeSet) ?
                context.CompileXPath(element, attributes.NodeSet) :
                null);

            this.indexRef = new Lazy<XPathExpression>(() =>
                !string.IsNullOrEmpty(attributes.IndexRef) ?
                context.CompileXPath(element, attributes.IndexRef) :
                null);
        }

        public IdRef Model
        {
            get { return attributes.Model; }
        }

        public IdRef Bind
        {
            get { return attributes.Bind; }
        }

        public XPathExpression Ref
        {
            get { return ref_.Value; }
        }

        public XPathExpression NodeSet
        {
            get { return nodeSet.Value; }
        }

        public int? StartIndex
        {
            get { return attributes.StartIndex; }
        }

        public int? Number
        {
            get { return attributes.Number; }
        }

        public XPathExpression IndexRef
        {
            get { return indexRef.Value; }
        }

    }

}