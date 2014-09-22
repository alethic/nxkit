using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.DOMEvents;
using NXKit.Xml;

namespace NXKit.XForms
{

    [Extension("{http://www.w3.org/2002/xforms}submit")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class Submit :
        ElementExtension,
        IEventDefaultAction
    {

        readonly SubmitAttributes attributes;
        readonly Extension<EvaluationContextResolver> context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        /// <param name="context"></param>
        [ImportingConstructor]
        public Submit(
            XElement element,
            SubmitAttributes attributes,
            Extension<EvaluationContextResolver> context)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(attributes != null);
            Contract.Requires<ArgumentNullException>(context != null);

            this.attributes = attributes;
            this.context = context;
        }

        void OnDOMActivate()
        {
            var submission = GetSubmission();
            if (submission == null)
                return;

            submission.DispatchEvent(Events.Submit);
        }

        /// <summary>
        /// Gets the targetted 'submission' interface.
        /// </summary>
        /// <returns></returns>
        XElement GetSubmission()
        {
            // Author-optional attribute containing a reference to element submission. If this attribute is given but
            // does not identify a submission element, then activating the submit does not result in the dispatch of
            // an xforms-submit event.
            if (attributes.Submission != null)
                return Element.ResolveId(attributes.Submission);

            // If this attribute is omitted, then the first submission in document order from the model associated with
            // the in-scope evaluation context is used.
            return context.Value.Context.Model.Element
                .Elements(Constants.XForms_1_0 + "submission")
                .FirstOrDefault();
        }

        void IEventDefaultAction.DefaultAction(Event evt)
        {
            switch (evt.Type)
            {
                case DOMEvents.Events.DOMActivate:
                    OnDOMActivate();
                    break;
            }
        }

    }

}
