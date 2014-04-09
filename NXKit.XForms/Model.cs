using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

using NXKit.DOMEvents;
using NXKit.Util;

namespace NXKit.XForms
{

    [Interface("{http://www.w3.org/2002/xforms}model")]
    public class Model :
        IEvaluationContextScope,
        IEventDefaultActionHandler
    {

        readonly XElement element;
        readonly ModelAttributes attributes;
        readonly Lazy<ModelState> state;
        readonly Lazy<DocumentAnnotation> documentAnnotation;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Model(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
            this.attributes = new ModelAttributes(element);
            this.state = new Lazy<ModelState>(() => element.AnnotationOrCreate<ModelState>());
            this.documentAnnotation = new Lazy<DocumentAnnotation>(() => element.Document.AnnotationOrCreate<DocumentAnnotation>());
        }

        /// <summary>
        /// Gets a reference to the encapsulated 'model' element.
        /// </summary>
        public XElement Element
        {
            get { return element; }
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
            get { return element.Elements(Constants.XForms_1_0 + "instance").SelectMany(i => i.Interfaces<Instance>()); }
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

                var modelItem = defaultInstance.State.Document.Root.AnnotationOrCreate<ModelItem>(() =>
                    new ModelItem(defaultInstance.State.Document.Root));

                return new EvaluationContext(this, defaultInstance, modelItem, 1, 1);
            }
        }

        /// <summary>
        /// Gets all 'bind' elements for this model.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Bind> GetBindNodes()
        {
            return element.Descendants()
                .SelectMany(i => i.Interfaces<Bind>());
        }

        /// <summary>
        /// Gets all of the bound nodes for this model.
        /// </summary>
        /// <returns></returns>
        IEnumerable<UIBinding> GetUIBindingNodes()
        {
            // all available ui bindings
            return element.Document.Root
                .DescendantsAndSelf()
                .SelectMany(i => i.Interfaces<IUIBindingNode>())
                .Select(i => i.UIBinding);
        }

        /// <summary>
        /// Gets all of teh UI nodes for this model.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IUINode> GetUINodes()
        {
            return element.Document.Root
                .DescendantsAndSelf()
                .SelectMany(i => i.Interfaces<IUINode>());
        }

        void IEventDefaultActionHandler.DefaultAction(Event evt)
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
                    {
                        element.Interface<INXEventTarget>().DispatchEvent(Events.VersionException);
                        return;
                    }

            if (attributes.Schema != null)
                foreach (var item in attributes.Schema.Split(' ').Select(i => i.Trim()).Where(i => !string.IsNullOrEmpty(i)))
                    continue; // TODO

            // attempt to load model instance data, if possible; if no instance loaded, exit
            if (Instances.Select(i => i.Load()).ToList().Any())
            {
                OnRebuild();
                OnRecalculate();
                OnRevalidate();
            }
        }

        /// <summary>
        /// Default action for the xforms-model-construct-done event.
        /// </summary>
        void OnModelConstructDone()
        {
            State.ConstructDone = true;

            if (DocumentAnnotation.ConstructDoneOnce)
                return;

            // refresh interface bindings
            foreach (var ui in GetUIBindingNodes())
                ui.Refresh();

            // discard interface events
            foreach (var ui in GetUIBindingNodes())
                ui.DiscardEvents();

            // refresh interfaces
            foreach (var ui in GetUINodes())
                ui.Refresh();

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
                State.RebuildFlag = false;
                State.RecalculateFlag = true;
            }
            while (State.RebuildFlag);
        }

        /// <summary>
        /// Default action for the xforms-recalculate event.
        /// </summary>
        internal void OnRecalculate()
        {
            do
            {
                State.RecalculateFlag = false;
                State.RevalidateFlag = true;

                var binds = element
                    .Descendants()
                    .SelectMany(i => i.Interfaces<Bind>())
                    .ToList();

                foreach (var bind in GetBindNodes())
                {
                    if (bind.Binding != null)
                        bind.Binding.Refresh();

                    if (bind.ModelItems == null)
                        continue;

                    foreach (var modelItem in bind.ModelItems)
                    {
                        var modelItemState = modelItem.State;

                        if (bind.Type != null)
                            if (modelItemState.Type != bind.Type)
                                modelItemState.Type = bind.Type;

                        if (bind.ReadOnly != null)
                            if (modelItemState.ReadOnly != bind.ReadOnly)
                                modelItemState.ReadOnly = bind.ReadOnly;

                        if (bind.Required != null)
                            if (modelItemState.Required != bind.Required)
                                modelItemState.Required = bind.Required;

                        if (bind.Relevant != null)
                            if (modelItemState.Relevant != bind.Relevant)
                                modelItemState.Relevant = bind.Relevant;

                        if (bind.Constraint != null)
                            if (modelItemState.Constraint != bind.Constraint)
                                modelItemState.Constraint = bind.Constraint;

                        if (bind.Calculate != null)
                        {
                            if (modelItemState.ReadOnly == false)
                                modelItemState.ReadOnly = true;
                            if (modelItem.Value != bind.Calculate)
                                modelItem.Value = bind.Calculate;
                        }
                    }
                }
            }
            while (State.RecalculateFlag);
        }

        /// <summary>
        /// Default action for the xforms-revalidate event.
        /// </summary>
        internal void OnRevalidate()
        {
            do
            {
                State.RevalidateFlag = false;

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
            while (State.RevalidateFlag);
        }

        /// <summary>
        /// Default action for the xforms-refresh event.
        /// </summary>
        internal void OnRefresh()
        {
            do
            {
                State.RefreshFlag = false;

                // refresh interface bindings
                foreach (var item in GetUIBindingNodes())
                    item.Refresh();

                // refresh interfaces
                foreach (var ui in GetUINodes())
                    ui.Refresh();

                // dispatch interface events
                foreach (var item in GetUIBindingNodes())
                    item.DispatchEvents();
            }
            while (State.RefreshFlag);
        }

        /// <summary>
        /// Default action for the xforms-reset event.
        /// </summary>
        internal void OnReset()
        {

        }

    }

}
