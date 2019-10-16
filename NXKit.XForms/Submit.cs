using System;
using System.Linq;
using System.Xml.Linq;

using NXKit.DOMEvents;
using NXKit.Xml;

namespace NXKit.XForms
{

    [Extension("{http://www.w3.org/2002/xforms}submit")]
    public class Submit :
        ElementExtension,
        IEventDefaultAction
    {

        readonly SubmitAttributes attributes;
        readonly Lazy<EvaluationContextResolver> context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        /// <param name="context"></param>
        public Submit(
            XElement element,
            SubmitAttributes attributes,
            Lazy<EvaluationContextResolver> context)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
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
