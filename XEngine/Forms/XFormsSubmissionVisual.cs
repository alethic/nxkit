using System;
using System.IO;
using System.Text;
using System.Xml.Linq;

using XEngine.Util;

namespace XEngine.Forms.XForms
{

    [VisualTypeDescriptor(Constants.XForms_1_0_NS, "submission")]
    public class XFormsSubmissionVisualTypeDescriptor : VisualTypeDescriptor
    {

        public override Visual CreateVisual(IFormProcessor form, StructuralVisual parent, XNode node)
        {
            return new XFormsSubmissionVisual(parent, (XElement)node);
        }

    }

    public class XFormsSubmissionVisual : XFormsSingleNodeBindingVisual,
        IEventDefaultActionHandler<XFormsSubmitEvent>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public XFormsSubmissionVisual(StructuralVisual parent, XElement element)
            : base(parent, element)
        {

        }

        void IEventDefaultActionHandler<XFormsSubmitEvent>.DefaultAction(XFormsSubmitEvent evt)
        {
            var ec = Module.ResolveBindingEvaluationContext(this);

            // single node binding, fall back to evaluation context
            var node = Binding != null ? Binding.Node : ec.Node;

            if (node is XElement)
            {
                if (!Module.GetModelItemRelevant(node))
                {
                    DispatchEvent<XFormsSubmitErrorEvent>();
                    return;
                }

                var action = Module.GetAttributeValue(Element, "action").TrimToNull();
                var method = Module.GetAttributeValue(Element, "method").TrimToNull();

                if (method != "put")
                    throw new NotSupportedException("Unsupported submission method.");

                // copy data into new document
                var d = new XDocument(node);

                // transform DOM into string
                var t = FormProcessor.XDocumentToString(d);

                // put data
                var resource = Module.Resolver.Put(action, Element.BaseUri, new MemoryStream(Encoding.UTF8.GetBytes(t)));

                if (resource != null)
                    throw new NotSupportedException("Cannot return new data from a Put.");

                DispatchEvent<XFormsSubmitDoneEvent>();
                return;
            }
            else
            {
                DispatchEvent<XFormsSubmitErrorEvent>();
                return;
            }
        }

    }

}
