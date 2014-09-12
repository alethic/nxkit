using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;

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
        [ImportingConstructor]
        public ModelRequestService(
            [ImportMany] IEnumerable<IModelRequestHandler> handlers)
        {
            Contract.Requires<ArgumentNullException>(handlers != null);

            this.handlers = handlers;
        }

        /// <summary>
        /// Gets the <see cref="IModelRequestHandler"/> to handle the given request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        IModelRequestHandler GetHandler(ModelRequest request)
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
        public ModelResponse Submit(ModelRequest request)
        {
            var handler = GetHandler(request);
            if (handler == null)
                return new ModelResponse(request, ModelResponseStatus.Error, null);

            return handler.Submit(request);
        }

    }

}
