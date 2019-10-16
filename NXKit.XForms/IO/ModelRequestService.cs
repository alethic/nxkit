using System;
using System.Collections.Generic;
using System.Linq;
using NXKit.Composition;

namespace NXKit.XForms.IO
{

    [Export(typeof(IModelRequestService))]
    public class ModelRequestService :
        IModelRequestService
    {

        readonly IEnumerable<IModelRequestHandler> handlers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="handlers"></param>
        public ModelRequestService(
            IEnumerable<IModelRequestHandler> handlers)
        {
            this.handlers = handlers ?? throw new ArgumentNullException(nameof(handlers));
        }

        /// <summary>
        /// Gets the <see cref="IModelRequestHandler"/> to handle the given request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        IModelRequestHandler GetHandler(ModelRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

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
        public ModelResponse Submit(ModelRequest request)
        {
            var handler = GetHandler(request);
            if (handler == null)
                return new ModelResponse(request, ModelResponseStatus.Error, null);

            return handler.Submit(request);
        }

    }

}
