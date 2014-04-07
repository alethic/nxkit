//using System;
//using System.IO;
//using System.Net;
//using System.Text;
//using System.Xml.Linq;
//using NXKit.DOMEvents;
//using NXKit.Util;

//namespace NXKit.XForms
//{

//    [NXElementInterface("{http://www.w3.org/2002/xforms}submission")]
//    public class Submission:
//        IEventDefaultActionHandler
//    {

//        /// <summary>
//        /// Initializes a new instance.
//        /// </summary>
//        /// <param name="element"></param>
//        public Submission(NXElement element)
//        {

//        }

//        void IEventDefaultActionHandler.DefaultAction(Event evt)
//        {
//            if (evt.Type != Events.Submit)
//                return;

//            var ec = Module.ResolveBindingEvaluationContext(this);

//            // single node binding, fall back to evaluation context
//            var modelItem = Binding != null ? Binding.ModelItem : ec.ModelItem;
//            var node = Binding != null ? Binding.ModelItem.Xml : ec.ModelItem.Xml;
//            if (node is XElement)
//            {
//                if (!modelItem.Relevant)
//                {
//                    this.Interface<INXEventTarget>().DispatchEvent(Events.SubmitError);
//                    return;
//                }

//                var action = Module.GetAttributeValue(Xml, "action").TrimToNull();
//                var method = Module.GetAttributeValue(Xml, "method").TrimToNull();

//                if (method != "put")
//                    throw new NotSupportedException("Unsupported submission method.");

//                // copy data into new document
//                var d = new XDocument(node);

//                // transform DOM into string
//                var t = d.ToString(SaveOptions.DisableFormatting);

//                // normalize uri with base
//                var u = new Uri(action);
//                if (Xml.BaseUri != null && !u.IsAbsoluteUri)
//                    u = new Uri(new Uri(Xml.BaseUri), u);

//                // put data
//                var request = WebRequest.Create(u);
//                request.Method = "PUT";
//                new MemoryStream(Encoding.UTF8.GetBytes(t)).CopyTo(request.GetRequestStream());
//                var response = request.GetResponse().GetResponseStream();

//                if (response != null)
//                    throw new NotSupportedException("Cannot return new data from a Put.");

//                this.Interface<INXEventTarget>().DispatchEvent(Events.SubmitDone);
//                return;
//            }
//            else
//            {
//                this.Interface<INXEventTarget>().DispatchEvent(Events.SubmitError);
//                return;
//            }
//        }

//    }

//}
