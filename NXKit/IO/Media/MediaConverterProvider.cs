using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using NXKit.Composition;

namespace NXKit.IO.Media
{

    [Export(typeof(MediaConverterProvider))]
    public class MediaConverterProvider
    {

        readonly IEnumerable<IMediaConverter> converters;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="converters"></param>
        public MediaConverterProvider(IEnumerable<IMediaConverter> converters)
        {
            this.converters = converters ?? throw new ArgumentNullException(nameof(converters));
        }

        /// <summary>
        /// Gets the first <see cref="IMediaConverter"/> capable of handling the given input type, data and output type.
        /// </summary>
        /// <param name="inputType"></param>
        /// <param name="input"></param>
        /// <param name="outputType"></param>
        /// <returns></returns>
        IMediaConverter GetConverter(MediaRange inputType, Stream input, MediaRange outputType)
        {
            return converters
                .Select(i => new
                {
                    Converter = i,
                    Priority = i.CanConvert(inputType, input, outputType),
                })
                .Where(i => i.Priority != Priority.Ignore)
                .OrderByDescending(i => i.Priority)
                .Select(i => i.Converter)
                .FirstOrDefault();
        }

        /// <summary>
        /// Returns <c>true</c> if the given input data can be converted into the given output type.
        /// </summary>
        /// <param name="inputType"></param>
        /// <param name="input"></param>
        /// <param name="outputType"></param>
        /// <returns></returns>
        public bool CanConvert(MediaRange inputType, Stream input, MediaRange outputType)
        {
            return GetConverter(inputType, input, outputType) != null;
        }

        /// <summary>
        /// Returns the given input data converted into the given output type.
        /// </summary>
        /// <param name="inputType"></param>
        /// <param name="input"></param>
        /// <param name="outputType"></param>
        /// <returns></returns>
        public Stream Convert(MediaRange inputType, Stream input, MediaRange outputType)
        {
            var converter = GetConverter(inputType, input, outputType);
            if (converter == null)
                throw new NullReferenceException();

            return converter.Convert(inputType, input, outputType);
        }

    }

}
