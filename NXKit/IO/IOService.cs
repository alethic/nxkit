using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;

namespace NXKit.IO
{
    
    [Export(typeof(IIOService))]
    public class IOService :
        IIOService
    {

        readonly IEnumerable<IIOTransport> transports;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="transports"></param>
        [ImportingConstructor]
        public IOService(
            [ImportMany] IEnumerable<IIOTransport> transports)
        {
            Contract.Requires<ArgumentNullException>(transports != null);

            this.transports = transports;
        }

        /// <summary>
        /// Gets the <see cref="IIOTransport"/> to handle the given request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        IIOTransport GetTransport(IORequest request)
        {
            Contract.Requires<ArgumentNullException>(request != null);

            return transports
                .Select(i => new { Priority = i.CanSend(request), Processor = i })
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
        public IOResponse Send(IORequest request)
        {
            var transport = GetTransport(request);
            if (transport == null)
                return new IOResponse(request, IOStatus.NoTransport);

            return transport.Submit(request);
        }

    }

}
