using System.IO;

namespace NXKit
{

    /// <summary>
    /// Describes a <see cref="Visual"/> that can be rendered to a simple string.
    /// </summary>
    public interface ITextVisual : IVisual
    {

        /// <summary>
        /// Gets the text rendered from the visual.
        /// </summary>
        void WriteText(TextWriter w);

    }

}
