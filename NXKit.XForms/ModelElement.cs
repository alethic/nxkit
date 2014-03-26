using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using NXKit.DOMEvents;
using NXKit.Util;

namespace NXKit.XForms
{

    [Element("model")]
    public class ModelElement :
        XFormsElement,
        IEvaluationContextScope,
        IEventDefaultActionHandler
    {

        ModelElementState state;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public ModelElement(XElement xml)
            : base(xml)
        {

        }

        /// <summary>
        /// Gets a reference to the model visual's state.
        /// </summary>
        public ModelElementState State
        {
            get { return state ?? (state = GetState<ModelElementState>()); }
        }

        /// <summary>
        /// Provides the model default evaluation context.
        /// </summary>
        public EvaluationContext Context
        {
            get { return DefaultEvaluationContext; }
        }

        /// <summary>
        /// Gets the set of <see cref="InstanceElement"/>s.
        /// </summary>
        public IEnumerable<InstanceElement> Instances
        {
            get { return Elements().OfType<InstanceElement>(); }
        }

        /// <summary>
        /// Gets a new <see cref="EvaluationContext"/> that should be the default used for the model when no
        /// instance is known.
        /// </summary>
        public EvaluationContext DefaultEvaluationContext
        {
            get
            {
                // default context is only available once instances have been instantiated 
                if (State != null && Instances.Any())
                    return new EvaluationContext(this, Instances.First(), Instances.First().State.Document.Root, 1, 1);
                else
                    return null;
            }
        }

        void OnConstructModel()
        {
            // mark step as complete, regardless of outcome
            State.Construct = true;

            // validate model version, we only support 1.0
            var versions = Module.GetAttributeValue(Xml, "version");
            if (versions != null)
                foreach (var version in versions.Split(' ').Select(i => i.Trim()).Where(i => !string.IsNullOrEmpty(i)))
                    if (version != "1.0")
                    {
                        this.Interface<IEventTarget>().DispatchEvent(new VersionExceptionEvent(this).Event);
                        return;
                    }

            var schema = Module.GetAttributeValue(Xml, "schema");
            if (schema != null)
                foreach (var item in schema.Split(' ').Select(i => i.Trim()).Where(i => !string.IsNullOrEmpty(i)))
                    continue; // TODO

            // attempt to load model instance data, if possible; if no instance loaded, exit
            Module.ProcessModelInstance(this);
            if (!Instances.Any(i => i.State.Document != null))
                return;

            OnRebuild();
            OnRecalculate();
            OnRevalidate();
        }

        void OnConstructDone()
        {
            State.ConstructDone = true;

            if (Document.Root.GetState<ModuleState>().ConstructDoneOnce)
                return;

            var elements = Document.Root
                .Descendants(true)
                .OfType<BindingElement>();

            // refresh each bound control
            foreach (var element in elements)
                element.Refresh();

            Document.Root.GetState<ModuleState>().ConstructDoneOnce = true;
        }

        void IEventDefaultActionHandler.DefaultAction(Event evt)
        {
            switch (evt.Type)
            {
                case XFormsEvents.ModelConstruct:
                    OnConstructModel();
                    break;
                case XFormsEvents.ModelConstructDone:
                    OnConstructDone();
                    break;
                case XFormsEvents.Ready:
                    OnReady();
                    break;
                case XFormsEvents.Rebuild:
                    OnRebuild();
                    break;
                case XFormsEvents.Recalculate:
                    OnRecalculate();
                    break;
                case XFormsEvents.Revalidate:
                    OnRevalidate();
                    break;
                case XFormsEvents.Refresh:
                    OnRefresh();
                    break;
                case XFormsEvents.Reset:
                    OnReset();
                    break;
            }
        }

        /// <summary>
        /// Marks the model as ready.
        /// </summary>
        internal void OnReady()
        {
            State.Ready = true;
        }

        /// <summary>
        /// Rebuilds the model.
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
        /// Recalculates the model.
        /// </summary>
        internal void OnRecalculate()
        {
            do
            {
                State.RecalculateFlag = false;
                State.RevalidateFlag = true;

                // for each each instance underneath the model
                foreach (var instance in Instances)
                {
                    var instanceModelItems = instance.State.Document.Root
                        .DescendantsAndSelf()
                        .SelectMany(i => i.Attributes().Cast<XObject>().Prepend(i))
                        .Where(i => i is XElement || i is XAttribute)
                        .Select(i => new { Node = i, ModelItem = Module.GetModelItem(i) });

                    // for each model item underneath the instance
                    foreach (var i in instanceModelItems)
                    {
                        var node = i.Node;
                        var modelItem = i.ModelItem;

                        if (modelItem.Remove)
                        {
                            if (node is XElement)
                                ((XElement)node).RemoveNodes();
                            else if (node is XAttribute)
                                ((XAttribute)node).SetValue("");
                            else
                                throw new Exception();

                            modelItem.Remove = false;
                            modelItem.DispatchValueChanged = true;

                            // prompt model to act
                            State.RevalidateFlag = true;
                            State.RefreshFlag = true;
                        }

                        // model item contains a new value
                        if (modelItem.NewElement != null)
                        {
                            if (node is XElement)
                                ((XElement)node).ReplaceAll(modelItem.NewElement);
                            else if (node is XAttribute)
                                ((XAttribute)node).SetValue(modelItem.NewElement.Value);
                            else
                                throw new Exception();

                            modelItem.NewElement = null;
                            modelItem.DispatchValueChanged = true;

                            // prompt model to act
                            State.RevalidateFlag = true;
                            State.RefreshFlag = true;
                        }

                        if (modelItem.NewValue != null)
                        {
                            if (node is XElement)
                                ((XElement)node).SetValue(modelItem.NewValue);
                            else if (node is XAttribute)
                                ((XAttribute)node).SetValue(modelItem.NewValue);
                            else
                                throw new Exception();

                            modelItem.NewValue = null;
                            modelItem.DispatchValueChanged = true;

                            // prompt model to act
                            State.RevalidateFlag = true;
                            State.RefreshFlag = true;
                        }
                    }
                }

                // apply binding expressions
                foreach (var bind in this.Descendants(false).OfType<BindElement>())
                {
                    bind.Refresh();

                    if (bind.Binding == null ||
                        bind.Binding.ModelItems == null ||
                        bind.Binding.ModelItems.Length == 0)
                        continue;

                    var typeAttr = Module.GetAttributeValue(bind.Xml, "type");
                    var calculateAttr = Module.GetAttributeValue(bind.Xml, "calculate");
                    var readonlyAttr = Module.GetAttributeValue(bind.Xml, "readonly");
                    var requiredAttr = Module.GetAttributeValue(bind.Xml, "required");
                    var relevantAttr = Module.GetAttributeValue(bind.Xml, "relevant");
                    var constraintAttr = Module.GetAttributeValue(bind.Xml, "constraint");

                    for (int i = 0; i < bind.Binding.ModelItems.Length; i++)
                    {
                        var node = bind.Binding.ModelItems[i];
                        var modelItem = Module.GetModelItem(node);

                        var ec = new EvaluationContext(bind.Binding.Context.Model, bind.Binding.Context.Instance, node, i + 1, bind.Binding.ModelItems.Length);
                        var nc = new XFormsXsltContext(bind, ec);

                        // get existing values
                        var oldReadOnly = Module.GetModelItemReadOnly(node);
                        var oldRequired = Module.GetModelItemRequired(node);
                        var oldRelevant = Module.GetModelItemRelevant(node);

                        if (typeAttr != null)
                        {
                            // lookup namespace of type specifier
                            var st = typeAttr.Split(':');
                            var ns = st.Length == 2 ? nc.LookupNamespace(st[0]) : null;
                            var lp = st.Length == 2 ? st[1] : st[0];
                            modelItem.Type = XName.Get(lp, ns);
                        }

                        // calculate before setting read-only, so read-only can be overridden
                        if (calculateAttr != null)
                        {
                            // calculated nodes are readonly
                            modelItem.ReadOnly = true;

                            var calculateBinding = new Binding(bind, ec, calculateAttr);
                            if (calculateBinding.Value != null)
                            {
                                var oldValue = Module.GetModelItemValue(node);
                                if (oldValue != calculateBinding.Value)
                                {
                                    modelItem.NewValue = calculateBinding.Value;
                                    modelItem.DispatchValueChanged = true;
                                    State.RecalculateFlag = true;
                                }
                            }
                        }

                        // recalculate read-only value
                        if (readonlyAttr != null)
                        {
                            var obj = Module.EvaluateXPath(bind, ec, readonlyAttr, XPathResultType.Any);
                            if (obj is bool)
                                modelItem.ReadOnly = (bool)obj;
                            else if (obj is string && !string.IsNullOrWhiteSpace((string)obj))
                                modelItem.ReadOnly = bool.Parse((string)obj);
                            else
                                throw new Exception();
                        }

                        // recalculate required value
                        if (requiredAttr != null)
                        {
                            var obj = Module.EvaluateXPath(bind, ec, requiredAttr, XPathResultType.Any);
                            if (obj is bool)
                                modelItem.Required = (bool)obj;
                            else if (obj is string && !string.IsNullOrWhiteSpace((string)obj))
                                modelItem.Required = bool.Parse((string)obj);
                        }

                        // recalculate relevant value
                        if (relevantAttr != null)
                        {
                            var obj = Module.EvaluateXPath(bind, ec, relevantAttr, XPathResultType.Any);
                            if (obj is bool)
                                modelItem.Relevant = (bool)obj;
                            else if (obj is string && !string.IsNullOrWhiteSpace((string)obj))
                                modelItem.Relevant = bool.Parse((string)obj);
                        }

                        if (constraintAttr != null)
                        {
                            var obj = Module.EvaluateXPath(bind, ec, constraintAttr, XPathResultType.Any);
                            if (obj is bool)
                                modelItem.Constraint = (bool)obj;
                            else if (obj is string && !string.IsNullOrWhiteSpace((string)obj))
                                modelItem.Constraint = bool.Parse((string)obj);
                        }

                        // get new read-only value; raise event on change
                        var readOnly = Module.GetModelItemReadOnly(node);
                        if (readOnly != oldReadOnly)
                            if (readOnly)
                                modelItem.DispatchReadOnly = true;
                            else
                                modelItem.DispatchReadWrite = true;

                        // get new required value; raise event on change
                        var required = Module.GetModelItemRequired(node);
                        if (required != oldRequired)
                            if (required)
                                modelItem.DispatchRequired = true;
                            else
                                modelItem.DispatchOptional = true;

                        // get new relevant value; raise event on change
                        var relevant = Module.GetModelItemRelevant(node);
                        if (relevant != oldRelevant)
                            if (relevant)
                                modelItem.DispatchEnabled = true;
                            else
                                modelItem.DispatchDisabled = true;
                    }
                }
            }
            while (State.RecalculateFlag);
        }

        /// <summary>
        /// Revalidates the model.
        /// </summary>
        internal void OnRevalidate()
        {
            do
            {
                State.RevalidateFlag = false;

                foreach (var instance in Instances)
                {
                    // all model items
                    var items = instance.State.Document.Root.DescendantNodesAndSelf()
                        .OfType<XElement>()
                        .SelectMany(i => i.Attributes().Cast<XObject>().Prepend(i));

                    foreach (var item in items)
                    {
                        var modelItem = Module.GetModelItem(item);

                        // get new relevant value; raise event on change
                        var required = Module.GetModelItemRequired(item);
                        var constraint = Module.GetModelItemConstraint(item);
                        var value = Module.GetModelItemValue(item);
                        var oldValid = Module.GetModelItemValid(item);

                        // get new validity, raise on change
                        var valid = (required ? value.TrimToNull() != null : true) && constraint;
                        if (valid != oldValid)
                        {
                            modelItem.Valid = valid;

                            if (valid)
                                modelItem.DispatchValid = true;
                            else
                                modelItem.DispatchInvalid = true;
                        }
                    }
                }
            }
            while (State.RevalidateFlag);
        }

        /// <summary>
        /// Refreshes the model.
        /// </summary>
        internal void OnRefresh()
        {
            do
            {
                State.RefreshFlag = false;

                // resolve visuals whom are dependent on this model
                var elements = Document.Root
                    .Descendants(true)
                    .OfType<BindingElement>();

                // for each visual, dispatch required events
                foreach (var element in elements)
                {
                    var target = element.Interface<IEventTarget>();
                    if (target == null)
                        throw new NullReferenceException();

                    // refresh underlying data
                    element.Refresh();

                    var singleNodeBindingVisual = element as SingleNodeBindingElement;
                    if (singleNodeBindingVisual != null)
                    {
                        if (singleNodeBindingVisual.Binding == null)
                            continue;

                        var node = singleNodeBindingVisual.Binding.ModelItem;
                        if (node == null)
                            continue;

                        var modelItem = Module.GetModelItem(node);

                        // dispatch required events
                        if (modelItem.DispatchValueChanged)
                            target.DispatchEvent(new ValueChangedEvent(element).Event);
                        if (modelItem.DispatchValid)
                            target.DispatchEvent(new ValidEvent(element).Event);
                        if (modelItem.DispatchInvalid)
                            target.DispatchEvent(new InvalidEvent(element).Event);
                        if (modelItem.DispatchEnabled)
                            target.DispatchEvent(new EnabledEvent(element).Event);
                        if (modelItem.DispatchDisabled)
                            target.DispatchEvent(new DisabledEvent(element).Event);
                        if (modelItem.DispatchOptional)
                            target.DispatchEvent(new OptionalEvent(element).Event);
                        if (modelItem.DispatchRequired)
                            target.DispatchEvent(new RequiredEvent(element).Event);
                        if (modelItem.DispatchReadOnly)
                            target.DispatchEvent(new ReadOnlyEvent(element).Event);
                        if (modelItem.DispatchReadWrite)
                            target.DispatchEvent(new ReadWriteEvent(element).Event);

                        // clear events
                        modelItem.DispatchValueChanged = false;
                        modelItem.DispatchValid = false;
                        modelItem.DispatchInvalid = false;
                        modelItem.DispatchEnabled = false;
                        modelItem.DispatchDisabled = false;
                        modelItem.DispatchOptional = false;
                        modelItem.DispatchRequired = false;
                        modelItem.DispatchReadOnly = false;
                        modelItem.DispatchReadWrite = false;

                        continue;
                    }
                }
            }
            while (State.RefreshFlag);
        }

        /// <summary>
        /// Resets the model.
        /// </summary>
        internal void OnReset()
        {

        }

    }

}
