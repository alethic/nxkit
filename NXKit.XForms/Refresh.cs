using System;
using System.Xml.Linq;
using NXKit.Composition;
using NXKit.DOMEvents;
using NXKit.XMLEvents;

namespace NXKit.XForms
{

    [Extension("{http://www.w3.org/2002/xforms}refresh")]
    public class Refresh :
        ElementExtension,
        IEventHandler
    {

        readonly IExport<EvaluationContextResolver> context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Refresh(
            XElement element,
            IExport<EvaluationContextResolver> context)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void HandleEvent(Event ev)
        {
            Invoke();
        }

        EvaluationContext GetRefreshContext()
        {
            return context.Value.GetInScopeEvaluationContext();
        }

        public void Invoke()
        {
            var refreshContext = GetRefreshContext();
            if (refreshContext == null)
                return;

            refreshContext.Model.State.Refresh = true;
            refreshContext.Model.InvokeDeferredUpdates();
        }

    }

}
