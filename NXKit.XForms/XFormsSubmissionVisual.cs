using System;
using System.IO;
using System.Text;
using System.Xml.Linq;

using NXKit.DOMEvents;
using NXKit.Util;

namespace NXKit.XForms
{

    [Visual("submission")]
    public class XFormsSubmissionVisual :
        XFormsSingleNodeBindingVisual,
        IEventDefaultActionHandler<XFormsSubmitEvent>
    {

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
                var t = d.ToString(SaveOptions.DisableFormatting);

                // normalize uri with base
                var u = new Uri(action);
                if (Element.BaseUri != null && !u.IsAbsoluteUri)
                    u = new Uri(new Uri(Element.BaseUri), u);

                // put data
                var resource = Document.Resolver.Put(u, new MemoryStream(Encoding.UTF8.GetBytes(t)));

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
