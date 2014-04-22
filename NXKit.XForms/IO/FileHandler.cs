using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;

using NXKit.IO;
using NXKit.XForms.Serialization;

namespace NXKit.XForms.IO
{

    /// <summary>
    /// Handles submissions of the 'file' scheme.
    /// </summary>
    [Export(typeof(IModelRequestHandler))]
    public class FileHandler :
        DefaultHandler
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="ioService"></param>
        /// <param name="serializers"></param>
        /// <param name="deserializers"></param>
        [ImportingConstructor]
        public FileHandler(
            IIOService ioService,
            [ImportMany] IEnumerable<IModelSerializer> serializers,
            [ImportMany] IEnumerable<IModelDeserializer> deserializers)
            : base(ioService, serializers, deserializers)
        {
            Contract.Requires<ArgumentNullException>(ioService != null);
            Contract.Requires<ArgumentNullException>(serializers != null);
            Contract.Requires<ArgumentNullException>(deserializers != null);
        }

        /// <summary>
        /// Returns <c>true</c> if the processor can handle this request.
        /// </summary>
        /// <param name="submit"></param>
        /// <returns></returns>
        public override Priority CanSubmit(ModelRequest submit)
        {
            if (submit.ResourceUri.Scheme == Uri.UriSchemeFile)
                return Priority.Default;
            else
                return Priority.Ignore;
        }

        protected override IOMethod GetMethod(ModelRequest request)
        {
            if (request.Method == ModelMethod.Get)
                return IOMethod.Get;
            if (request.Method == ModelMethod.Put)
                return IOMethod.Put;
            if (request.Method == ModelMethod.Post)
                return IOMethod.Put;

            return IOMethod.None;
        }

        protected override bool IsQuery(ModelRequest request)
        {
            return false;
        }

    }

}
