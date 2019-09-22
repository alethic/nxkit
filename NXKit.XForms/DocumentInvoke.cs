using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.DOMEvents;
using NXKit.Xml;

namespace NXKit.XForms
{

    [Extension(ExtensionObjectType.Document)]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class DocumentInvoke :
        DocumentExtension,
        IOnInvoke
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="document"></param>
        [ImportingConstructor]
        public DocumentInvoke(
            XDocument document)
            : base(document)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));
        }

        /// <summary>
        /// Invokes any outstanding deferred actions on the models.
        /// </summary>
        /// <returns></returns>
        public bool Invoke()
        {
            var work = false;

            // obtain all models
            var models = Document
                .Descendants(Constants.XForms_1_0 + "model")
                .Select(i => i.Interface<Model>())
                .ToList();

            // raise construct event on all non-constructed models
            foreach (var model in models)
                if (!model.State.Construct)
                {
                    model.Element.Interface<EventTarget>().Dispatch(Events.ModelConstruct);
                    work = true;
                }

            // if all models have passed construct, raise construct done event
            if (models.All(i => i.State.Construct))
                foreach (var model in models)
                    if (!model.State.ConstructDone)
                    {
                        model.Element.Interface<EventTarget>().Dispatch(Events.ModelConstructDone);
                        work = true;
                    }

            // if all models have passed construct-done, raise ready event
            if (models.All(i => i.State.ConstructDone))
                foreach (var model in models)
                    if (!model.State.Ready)
                    {
                        model.Element.Interface<EventTarget>().Dispatch(Events.Ready);
                        work = true;
                    }

            // only process main events if all models are ready
            if (models.All(i => i.State.Ready))
            {
                foreach (var model in models.Where(i => i.State.Rebuild))
                {
                    work = true;
                    model.Element.Interface<EventTarget>().Dispatch(Events.Rebuild);
                }

                foreach (var model in models.Where(i => i.State.Recalculate))
                {
                    work = true;
                    model.Element.Interface<EventTarget>().Dispatch(Events.Recalculate);
                }

                foreach (var model in models.Where(i => i.State.Revalidate))
                {
                    work = true;
                    model.Element.Interface<EventTarget>().Dispatch(Events.Revalidate);
                }

                foreach (var model in models.Where(i => i.State.Refresh))
                {
                    work = true;
                    model.Element.Interface<EventTarget>().Dispatch(Events.Refresh);
                }
            }

            return work;
        }

    }

}
