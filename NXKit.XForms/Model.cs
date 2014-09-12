using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.DOMEvents;
using NXKit.Util;
using NXKit.Xml;

namespace NXKit.XForms
{

    [Extension("{http://www.w3.org/2002/xforms}model")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class Model :
        ElementExtension,
        IEventDefaultAction
    {


        readonly ModelAttributes attributes;
        readonly Lazy<ModelState> state;
        readonly Lazy<DocumentAnnotation> documentAnnotation;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Model(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.attributes = new ModelAttributes(Element);
            this.state = new Lazy<ModelState>(() => Element.AnnotationOrCreate<ModelState>());
            this.documentAnnotation = new Lazy<DocumentAnnotation>(() => Element.Document.AnnotationOrCreate<DocumentAnnotation>());
        }

        /// <summary>
        /// Gets the model state associated with this model interface.
        /// </summary>
        public ModelState State
        {
            get { return state.Value; }
        }

        DocumentAnnotation DocumentAnnotation
        {
            get { return documentAnnotation.Value; }
        }

        /// <summary>
        /// Provides the model default evaluation context.
        /// </summary>
        public EvaluationContext Context
        {
            get { return DefaultEvaluationContext; }
        }

        /// <summary>
        /// Gets the set of <see cref="Instance"/>s.
        /// </summary>
        public IEnumerable<Instance> Instances
        {
            get { return Element.Elements(Constants.XForms_1_0 + "instance").SelectMany(i => i.Interfaces<Instance>()); }
        }

        /// <summary>
        /// Gets a new <see cref="EvaluationContext"/> that should be the default used for the model when no
        /// instance is known.
        /// </summary>
        public EvaluationContext DefaultEvaluationContext
        {
            get
            {
                if (State == null)
                    return null;

                var defaultInstance = Instances.FirstOrDefault();
                if (defaultInstance == null)
                    return null;

                if (defaultInstance.State.Document == null)
                    return null;

                return new EvaluationContext(this, defaultInstance, ModelItem.Get(defaultInstance.State.Document.Root), 1, 1);
            }
        }

        /// <summary>
        /// Gets all implementations of the given extension type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetAllExtensions<T>()
        {
            return Element.Document.Root
                .DescendantNodesAndSelf()
                .Where(i => i.Document != null)
                .SelectMany(i => i.Interfaces<T>());
        }

        void IEventDefaultAction.DefaultAction(Event evt)
        {
            switch (evt.Type)
            {
                case Events.ModelConstruct:
                    OnModelConstruct();
                    break;
                case Events.ModelConstructDone:
                    OnModelConstructDone();
                    break;
                case Events.Ready:
                    OnReady();
                    break;
                case Events.Rebuild:
                    OnRebuild();
                    break;
                case Events.Recalculate:
                    OnRecalculate();
                    break;
                case Events.Revalidate:
                    OnRevalidate();
                    break;
                case Events.Refresh:
                    OnRefresh();
                    break;
                case Events.Reset:
                    OnReset();
                    break;
            }
        }

        /// <summary>
        /// Default action for the xforms-model-construct event.
        /// </summary>
        void OnModelConstruct()
        {
            // mark step as complete, regardless of outcome
            State.Construct = true;

            // validate model version, we only support 1.0
            if (attributes.Version != null)
                foreach (var version in attributes.Version.Split(' ').Select(i => i.Trim()).Where(i => !string.IsNullOrEmpty(i)))
                    if (version != "1.0")
                        throw new DOMTargetEventException(Element, Events.VersionException);

            if (attributes.Schema != null)
                foreach (var item in attributes.Schema.Split(' ').Select(i => i.Trim()).Where(i => !string.IsNullOrEmpty(i)))
                    continue; // TODO

            // attempt to load model instance data
            foreach (var instance in Instances)
                instance.Load();

            OnRebuild();
            OnRecalculate();
            OnRevalidate();
        }

        /// <summary>
        /// Default action for the xforms-model-construct-done event.
        /// </summary>
        void OnModelConstructDone()
        {
            State.ConstructDone = true;

            if (DocumentAnnotation.ConstructDoneOnce)
                return;

            // refresh bindings
            foreach (var i in GetAllExtensions<IOnRefresh>())
                i.RefreshBinding();

            // discard refresh events
            foreach (var i in GetAllExtensions<IOnRefresh>())
                i.DiscardEvents();

            // final refresh
            foreach (var i in GetAllExtensions<IOnRefresh>())
                i.Refresh();

            DocumentAnnotation.ConstructDoneOnce = true;
        }

        /// <summary>
        /// Default action for the xforms-ready event.
        /// </summary>
        internal void OnReady()
        {
            State.Ready = true;
        }

        /// <summary>
        /// Default action for the xforms-rebuild event.
        /// </summary>
        internal void OnRebuild()
        {
            do
            {
                State.Rebuild = false;
                State.Recalculate = true;
            }
            while (State.Rebuild);
        }

        /// <summary>
        /// Default action for the xforms-recalculate event.
        /// </summary>
        internal void OnRecalculate()
        {
            do
            {
                State.Recalculate = false;
                State.Revalidate = true;

                // apply all of the bind elements
                foreach (var bind in GetAllExtensions<Bind>())
                {
                    bind.Recalculate();
                    bind.Apply();
                }
            }
            while (State.Recalculate);
        }

        /// <summary>
        /// Default action for the xforms-revalidate event.
        /// </summary>
        internal void OnRevalidate()
        {
            do
            {
                State.Revalidate = false;

                foreach (var instance in Instances)
                {
                    // all model items
                    var modelItems = instance.State.Document.Root
                        .DescendantNodesAndSelf()
                        .OfType<XElement>()
                        .SelectMany(i => i.Attributes().Cast<XObject>().Prepend(i))
                        .Select(i => i.AnnotationOrCreate<ModelItem>(() => new ModelItem(i)));

                    foreach (var modelItem in modelItems)
                        modelItem.State.Valid = (modelItem.Required ? modelItem.Value.TrimToNull() != null : true) && modelItem.Constraint;
                }
            }
            while (State.Revalidate);
        }

        /// <summary>
        /// Default action for the xforms-refresh event.
        /// </summary>
        internal void OnRefresh()
        {
            do
            {
                State.Refresh = false;

                // refresh bindings
                foreach (var ui in GetAllExtensions<IOnRefresh>())
                    ui.RefreshBinding();

                // refresh interfaces
                foreach (var ui in GetAllExtensions<IOnRefresh>())
                    ui.Refresh();

                // dispatch events
                foreach (var item in GetAllExtensions<IOnRefresh>())
                    item.DispatchEvents();
            }
            while (State.Refresh);
        }

        /// <summary>
        /// Default action for the xforms-reset event.
        /// </summary>
        internal void OnReset()
        {

        }

        /// <summary>
        /// Invokes any outstanding deferred updates.
        /// </summary>
        internal void InvokeDeferredUpdates()
        {
            if (state.Value.Rebuild)
                OnRebuild();

            if (state.Value.Recalculate)
                OnRecalculate();

            if (state.Value.Revalidate)
                OnRevalidate();

            if (state.Value.Refresh)
                OnRefresh();
        }

    }

}
