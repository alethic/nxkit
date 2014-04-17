using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;

namespace NXKit.XForms.IO
{
    
    [Export(typeof(IRequestService))]
    public class RequestService :
        IRequestService
    {

        readonly IEnumerable<IRequestHandler> handlers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="handlers"></param>
        [ImportingConstructor]
        public RequestService(
            [ImportMany] IEnumerable<IRequestHandler> handlers)
        {
            Contract.Requires<ArgumentNullException>(handlers != null);

            this.handlers = handlers;
        }

        /// <summary>
        /// Gets the <see cref="IRequestHandler"/> to handle the given request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        IRequestHandler GetHandler(Request request)
        {
            Contract.Requires<ArgumentNullException>(request != null);

            return handlers
                .Select(i => new { Priority = i.CanSubmit(request), Processor = i })
                .Where(i => i.Priority != Priority.Ignore)
                .OrderByDescending(i => i.Priority)
                .Select(i => i.Processor)
                .FirstOrDefault();
        }

        /// <summary>
        /// Submits the given request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response Submit(Request request)
        {
            Contract.Requires<ArgumentNullException>(request != null);

            var handler = GetHandler(request);
            if (handler == null)
                return new Response(request, ResponseStatus.Error, null);

            return handler.Submit(request);
        }

    }

}
