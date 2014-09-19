using System.IO;

namespace NXKit.IO.Media
{

    /// <summary>
    /// Provides an interface to convert from one <see cref="MediaRange"/> to another.
    /// </summary>
    public interface IMediaConverter
    {

        /// <summary>
        /// Returns whether or not the given <see cref="Stream"/> of <see cref="MediaRange"/> can be converted to the given output.
        /// </summary>
        /// <param name="inputType"></param>
        /// <param name="input"></param>
        /// <param name="outputType"></param>
        /// <returns></returns>
        Priority CanConvert(MediaRange inputType, Stream input, MediaRange outputType);

        /// <summary>
        /// Converts the given <see cref="Stream"/> of <see cref="MediaRange"/> to the given <see cref="MediaRange"/> output.
        /// </summary>
        /// <param name="inputType"></param>
        /// <param name="input"></param>
        /// <param name="outputType"></param>
        /// <returns></returns>
        Stream Convert(MediaRange inputType, Stream input, MediaRange outputType);

    }

}
