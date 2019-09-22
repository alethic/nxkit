using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using NXKit.IO.Media;
using NXKit.XForms.Serialization;

namespace NXKit.XForms.IO
{

    /// <summary>
    /// Base implementation of the <see cref="IModelRequestHandler"/> interface.
    /// </summary>
    public abstract class ModelRequestHandler :
        IModelRequestHandler
    {

        readonly IEnumerable<IModelSerializer> serializers;
        readonly IEnumerable<IModelDeserializer> deserializers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="serializers"></param>
        /// <param name="deserializers"></param>
        [ImportingConstructor]
        public ModelRequestHandler(
            [ImportMany] IEnumerable<IModelSerializer> serializers,
            [ImportMany] IEnumerable<IModelDeserializer> deserializers)
        {
            this.serializers = serializers ?? throw new ArgumentNullException(nameof(serializers));
            this.deserializers = deserializers ?? throw new ArgumentNullException(nameof(deserializers));
        }

        /// <summary>
        /// Return <c>true</c> if your <see cref="IModelRequestHandler"/> supports the given <see cref="ModelRequest"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public abstract Priority CanSubmit(ModelRequest request);

        /// <summary>
        /// Gets the <see cref="MediaRange"/> to determine the format of the outgoing data.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected abstract MediaRange GetMediaType(ModelRequest request);

        /// <summary>
        /// Gets the serializer which supports the given <see cref="MediaRange"/>.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        protected IModelSerializer GetSerializer(XNode node, MediaRange mediaType)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            if (mediaType == null)
                throw new ArgumentNullException(nameof(mediaType));

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
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            if (mediaType == null)
                throw new ArgumentNullException(nameof(mediaType));

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
        protected IModelDeserializer GetDeserializer(MediaRange mediaType)
        {
            if (mediaType == null)
                throw new ArgumentNullException(nameof(mediaType));

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
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            if (mediaType == null)
                throw new ArgumentNullException(nameof(mediaType));

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
        public abstract ModelResponse Submit(ModelRequest request);

    }

}
