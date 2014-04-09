using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

using NXKit.DOMEvents;

namespace NXKit.XForms
{

    [Interface(XmlNodeType.Document)]
    public class DocumentInvoke :
        IOnInvoke
    {

        readonly XDocument document;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="document"></param>
        public DocumentInvoke(XDocument document)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            this.document = document;
        }

        /// <summary>
        /// Invokes any outstanding deferred actions on the models.
        /// </summary>
        /// <returns></returns>
        public bool Invoke()
        {
            var work = false;

            // obtain all models
            var models = document
                .Descendants(Constants.XForms_1_0 + "model")
                .Select(i => i.Interface<Model>())
                .ToList();

            // raise construct event on all non-constructed models
            foreach (var model in models)
                if (!model.State.Construct)
                {
                    model.Element.Interface<INXEventTarget>().DispatchEvent(Events.ModelConstruct);
                    work = true;
                }

            // if all models have passed construct, raise construct done event
            if (models.All(i => i.State.Construct))
                foreach (var model in models)
                    if (!model.State.ConstructDone)
                    {
                        model.Element.Interface<INXEventTarget>().DispatchEvent(Events.ModelConstructDone);
                        work = true;
                    }

            // if all models have passed construct-done, raise ready event
            if (models.All(i => i.State.ConstructDone))
                foreach (var model in models)
                    if (!model.State.Ready)
                    {
                        model.Element.Interface<INXEventTarget>().DispatchEvent(Events.Ready);
                        work = true;
                    }

            // only process main events if all models are ready
            if (models.All(i => i.State.Ready))
            {
                foreach (var model in models.Where(i => i.State.Rebuild))
                {
                    work = true;
                    model.Element.Interface<INXEventTarget>().DispatchEvent(Events.Rebuild);
                }

                foreach (var model in models.Where(i => i.State.Recalculate))
                {
                    work = true;
                    model.Element.Interface<INXEventTarget>().DispatchEvent(Events.Recalculate);
                }

                foreach (var model in models.Where(i => i.State.Revalidate))
                {
                    work = true;
                    model.Element.Interface<INXEventTarget>().DispatchEvent(Events.Revalidate);
                }

                foreach (var model in models.Where(i => i.State.Refresh))
                {
                    work = true;
                    model.Element.Interface<INXEventTarget>().DispatchEvent(Events.Refresh);
                }
            }

            return work;
        }

    }

}
