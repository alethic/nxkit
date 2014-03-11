using System;

namespace NXKit.Wpf
{

    /// <summary>
    /// 
    /// </summary>
    public interface IDocumentSource
    {

        /// <summary>
        /// Gets a reference to the current document.
        /// </summary>
        NXDocument Document { get; }

        /// <summary>
        /// Raised when the document is changed.
        /// </summary>
        event EventHandler DocumentChanged;

    }

}
