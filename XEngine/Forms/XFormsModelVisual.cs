using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace XEngine.Forms
{

    [VisualTypeDescriptor(Constants.XForms_1_0_NS, "model")]
    public class XFormsModelVisualTypeDescriptor : VisualTypeDescriptor
    {

        public override Visual CreateVisual(IFormProcessor form, StructuralVisual parent, XNode node)
        {
            return new XFormsModelVisual(parent, (XElement)node);
        }

    }

    public class XFormsModelVisual : XFormsVisual, IEvaluationContextScope,
        IEventDefaultActionHandler<XFormsModelConstructEvent>,
        IEventDefaultActionHandler<XFormsModelConstructDoneEvent>,
        IEventDefaultActionHandler<XFormsReadyEvent>,
        IEventDefaultActionHandler<XFormsRefreshEvent>,
        IEventDefaultActionHandler<XFormsRevalidateEvent>,
        IEventDefaultActionHandler<XFormsRecalculateEvent>,
        IEventDefaultActionHandler<XFormsRebuildEvent>,
        IEventDefaultActionHandler<XFormsResetEvent>
    {

        private XFormsModelVisualState state;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public XFormsModelVisual(StructuralVisual parent, XElement element)
            : base(parent, element)
        {

        }

        /// <summary>
        /// Gets a reference to the model visual's state.
        /// </summary>
        public XFormsModelVisualState State
        {
            get { return state ?? (state = GetState<XFormsModelVisualState>()); }
        }

        /// <summary>
        /// Provides the model default evaluation context.
        /// </summary>
        public XFormsEvaluationContext Context
        {
            get { return DefaultEvaluationContext; }
        }

        /// <summary>
        /// Creates the children in a defined order.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<Visual> CreateChildren()
        {
            var childNodeList = Element.Elements().ToArray();

            // emit instance children first
            foreach (var element in Element.Elements(Constants.XForms_1_0 + "instance"))
                yield return Form.CreateVisual(this, element);

            // emit instance children first
            foreach (var element in Element.Elements(Constants.XForms_1_0 + "bind"))
                yield return Form.CreateVisual(this, element);

            // emit instance children first
            foreach (var element in Element.Elements(Constants.XForms_1_0 + "submission"))
                yield return Form.CreateVisual(this, element);
        }

        /// <summary>
        /// Gets the set of <see cref="XFormsInstanceVisual"/>s.
        /// </summary>
        public IEnumerable<XFormsInstanceVisual> Instances
        {
            get { return Children.OfType<XFormsInstanceVisual>(); }
        }

        public XFormsEvaluationContext DefaultEvaluationContext
        {
            get
            {
                // default context is only available once instances have been instantiated 
                if (State != null && Instances.Any())
                    return new XFormsEvaluationContext(this, Instances.First(), Instances.First().State.InstanceElement, 1, 1);
                else
                    return null;
            }
        }

        void IEventDefaultActionHandler<XFormsModelConstructEvent>.DefaultAction(XFormsModelConstructEvent evt)
        {
            // mark step as complete, regardless of outcome
            State.Construct = true;

            // validate model version, we only support 1.0
            var versions = Module.GetAttributeValue(Element, "version");
            if (versions != null)
                foreach (var version in versions.Split(' ').Select(i => i.Trim()).Where(i => !string.IsNullOrEmpty(i)))
                    if (version != "1.0")
                    {
                        DispatchEvent<XFormsVersionExceptionEvent>();
                        return;
                    }

            var schema = Module.GetAttributeValue(Element, "schema");
            if (schema != null)
                foreach (var item in schema.Split(' ').Select(i => i.Trim()).Where(i => !string.IsNullOrEmpty(i)))
                    continue; // TODO

            // attempt to load model instance data, if possible; if no instance loaded, exit
            Module.ProcessModelInstance(this);
            if (!Instances.Any(i => i.State.InstanceDocument != null))
                return;

            Module.RebuildModel(this);
            Module.RecalculateModel(this);
            Module.RevalidateModel(this);
        }


        void IEventDefaultActionHandler<XFormsModelConstructDoneEvent>.DefaultAction(XFormsModelConstructDoneEvent evt)
        {
            State.ConstructDone = true;
            State.ConstructDoneOnce = true;
        }

        void IEventDefaultActionHandler<XFormsReadyEvent>.DefaultAction(XFormsReadyEvent evt)
        {
            Module.ReadyDefaultAction(evt);
        }

        void IEventDefaultActionHandler<XFormsRebuildEvent>.DefaultAction(XFormsRebuildEvent evt)
        {
            Module.RebuildDefaultAction(evt);
        }

        void IEventDefaultActionHandler<XFormsRecalculateEvent>.DefaultAction(XFormsRecalculateEvent evt)
        {
            Module.RecalculateDefaultAction(evt);
        }

        void IEventDefaultActionHandler<XFormsRevalidateEvent>.DefaultAction(XFormsRevalidateEvent evt)
        {
            Module.RevalidateDefaultAction(evt);
        }

        void IEventDefaultActionHandler<XFormsRefreshEvent>.DefaultAction(XFormsRefreshEvent evt)
        {
            Module.RefreshDefaultAction(evt);
        }

        void IEventDefaultActionHandler<XFormsResetEvent>.DefaultAction(XFormsResetEvent evt)
        {
            Module.ResetDefaultAction(evt);
        }

    }

}
