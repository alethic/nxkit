using System;
using System.IO;
using System.Text;
using System.Xml.Linq;

using NXKit.DOMEvents;
using NXKit.Util;

namespace NXKit.XForms
{

    [Element("submission")]
    public class SubmissionElement :
        SingleNodeUIBindingElement,
        IEventDefaultActionHandler
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public SubmissionElement(XElement element)
            : base(element)
        {

        }

        void IEventDefaultActionHandler.DefaultAction(Event evt)
        {
            if (evt.Type != XFormsEvents.Submit)
                return;

            var ec = Module.ResolveBindingEvaluationContext(this);

            // single node binding, fall back to evaluation context
            var modelItem = Binding != null ? Binding.ModelItem : ec.ModelItem;
            var node = Binding != null ? Binding.ModelItem.Xml : ec.ModelItem.Xml;
            if (node is XElement)
            {
                if (!modelItem.Relevant)
                {
                    DispatchEvent<SubmitErrorEvent>();
                    return;
                }

                var action = Module.GetAttributeValue(Xml, "action").TrimToNull();
                var method = Module.GetAttributeValue(Xml, "method").TrimToNull();

                if (method != "put")
                    throw new NotSupportedException("Unsupported submission method.");

                // copy data into new document
                var d = new XDocument(node);

                // transform DOM into string
                var t = d.ToString(SaveOptions.DisableFormatting);

                // normalize uri with base
                var u = new Uri(action);
                if (Xml.BaseUri != null && !u.IsAbsoluteUri)
                    u = new Uri(new Uri(Xml.BaseUri), u);

                // put data
                var resource = Document
                    .Container.GetExportedValue<IResolver>()
                    .Put(u, new MemoryStream(Encoding.UTF8.GetBytes(t)));

                if (resource != null)
                    throw new NotSupportedException("Cannot return new data from a Put.");

                DispatchEvent<SubmitDoneEvent>();
                return;
            }
            else
            {
                DispatchEvent<SubmitErrorEvent>();
                return;
            }
        }

    }

}
