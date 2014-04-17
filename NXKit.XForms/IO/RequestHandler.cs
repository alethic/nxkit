using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using NXKit.Util;
using NXKit.XForms.Serialization;

namespace NXKit.XForms.IO
{

    /// <summary>
    /// Base implementation of the <see cref="IRequestHandler"/> interface.
    /// </summary>
    public abstract class RequestHandler :
        IRequestHandler
    {

        readonly IEnumerable<INodeSerializer> serializers;
        readonly IEnumerable<INodeDeserializer> deserializers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="serializers"></param>
        /// <param name="deserializers"></param>
        [ImportingConstructor]
        public RequestHandler(
            [ImportMany] IEnumerable<INodeSerializer> serializers,
            [ImportMany] IEnumerable<INodeDeserializer> deserializers)
        {
            Contract.Requires<ArgumentNullException>(serializers != null);
            Contract.Requires<ArgumentNullException>(deserializers != null);

            this.serializers = serializers;
            this.deserializers = deserializers;
        }

        /// <summary>
        /// Return <c>true</c> if your <see cref="IRequestHandler"/> supports the given <see cref="Request"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public abstract Priority CanSubmit(Request request);

        /// <summary>
        /// Gets the <see cref="MediaRange"/> to determine the format of the outgoing data.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected abstract MediaRange GetMediaType(Request request);

        /// <summary>
        /// Gets the serializer which supports the given <see cref="MediaRange"/>.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        protected INodeSerializer GetSerializer(XNode node, MediaRange mediaType)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(mediaType != null);

            return serializers
                .Select(i => new { Priority = i.CanSerialize(node, mediaType), Serializer = i })
                .Where(i => i.Priority != Priority.Ignore)
                .OrderByDescending(i => i.Priority)
                .Select(i => i.Serializer)
                .FirstOrDefault();
        }

        /// <summary>
        /// Serializes the <see cref="XNode"/> to a <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="node"></param>
        /// <param name="mediaType"></param>
        protected void Serialize(TextWriter writer, XNode node, MediaRange mediaType)
        {
            Contract.Requires<ArgumentNullException>(writer != null);
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(mediaType != null);

            // obtain serializer
            var serializer = GetSerializer(node, mediaType);
            if (serializer == null)
                throw new UnsupportedMediaTypeException();

            serializer.Serialize(writer, node, mediaType);
        }

        /// <summary>
        /// Gets the deserializer which supports the given <see cref="MediaRange"/>.
        /// </summary>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        protected INodeDeserializer GetDeserializer(MediaRange mediaType)
        {
            Contract.Requires<ArgumentNullException>(mediaType != null);

            return deserializers
                .Select(i => new { Priority = i.CanDeserialize(mediaType), Serializer = i })
                .Where(i => i.Priority != Priority.Ignore)
                .OrderByDescending(i => i.Priority)
                .Select(i => i.Serializer)
                .FirstOrDefault();
        }

        /// <summary>
        /// Deserializes the <see cref="TextReader"/> to a <see cref="XNode"/>.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        protected XNode Deserialize(TextReader reader, MediaRange mediaType)
        {
            Contract.Requires<ArgumentNullException>(reader != null);
            Contract.Requires<ArgumentNullException>(mediaType != null);

            // obtain serializer
            var deserializer = GetDeserializer(mediaType);
            if (deserializer == null)
                throw new InvalidOperationException();

            return deserializer.Deserialize(reader, mediaType);
        }

        /// <summary>
        /// Handles a submitted request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public abstract Response Submit(Request request);

    }

}
