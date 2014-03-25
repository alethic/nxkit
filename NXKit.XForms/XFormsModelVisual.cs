using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using NXKit.DOMEvents;

namespace NXKit.XForms
{

    [Visual("model")]
    public class XFormsModelVisual :
        XFormsVisual,
        IEvaluationContextScope,
        IEventDefaultActionHandler
    {

        XFormsModelVisualState state;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public XFormsModelVisual(XElement xml)
            : base(xml)
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
        /// Gets the set of <see cref="XFormsInstanceVisual"/>s.
        /// </summary>
        public IEnumerable<XFormsInstanceVisual> Instances
        {
            get { return Elements.OfType<XFormsInstanceVisual>(); }
        }

        /// <summary>
        /// Gets a new <see cref="XFormsEvaluationContext"/> that should be the default used for the model when no
        /// instance is known.
        /// </summary>
        public XFormsEvaluationContext DefaultEvaluationContext
        {
            get
            {
                // default context is only available once instances have been instantiated 
                if (State != null && Instances.Any())
                    return new XFormsEvaluationContext(this, Instances.First(), Instances.First().State.Document.Root, 1, 1);
                else
                    return null;
            }
        }

        void ModelConstructEventDefaultAction(Event evt)
        {
            // mark step as complete, regardless of outcome
            State.Construct = true;

            // validate model version, we only support 1.0
            var versions = Module.GetAttributeValue(Xml, "version");
            if (versions != null)
                foreach (var version in versions.Split(' ').Select(i => i.Trim()).Where(i => !string.IsNullOrEmpty(i)))
                    if (version != "1.0")
                    {
                        this.Interface<IEventTarget>().DispatchEvent(new XFormsVersionExceptionEvent(this).Event);
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

            Module.RebuildModel(this);
            Module.RecalculateModel(this);
            Module.RevalidateModel(this);
        }


        void ModelConstructDoneDefaultAction(Event evt)
        {
            State.ConstructDone = true;
            State.ConstructDoneOnce = true;
        }

        void ReadyDefaultAction(Event evt)
        {
            Module.ReadyDefaultAction(evt);
        }

        void RebuildDefaultAction(Event evt)
        {
            Module.RebuildDefaultAction(evt);
        }

        void RecalculateDefaultAction(Event evt)
        {
            Module.RecalculateDefaultAction(evt);
        }

        void RevalidateDefaultAction(Event evt)
        {
            Module.RevalidateDefaultAction(evt);
        }

        void RefreshDefaultAction(Event evt)
        {
            Module.RefreshDefaultAction(evt);
        }

        void ResetDefaultAction(Event evt)
        {
            Module.ResetDefaultAction(evt);
        }

        void IEventDefaultActionHandler.DefaultAction(Event evt)
        {
            switch (evt.Type)
            {
                case XFormsEvents.ModelConstruct:
                    ModelConstructEventDefaultAction(evt);
                    break;
                case XFormsEvents.ModelConstructDone:
                    ModelConstructDoneDefaultAction(evt);
                    break;
                case XFormsEvents.Ready:
                    ReadyDefaultAction(evt);
                    break;
                case XFormsEvents.Rebuild:
                    RebuildDefaultAction(evt);
                    break;
                case XFormsEvents.Recalculate:
                    RecalculateDefaultAction(evt);
                    break;
                case XFormsEvents.Revalidate:
                    RevalidateDefaultAction(evt);
                    break;
                case XFormsEvents.Refresh:
                    RefreshDefaultAction(evt);
                    break;
                case XFormsEvents.Reset:
                    ResetDefaultAction(evt);
                    break;
            }
        }

    }

}
