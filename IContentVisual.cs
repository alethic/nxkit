using System;
using System.Collections.Generic;

namespace NXKit
{

    /// <summary>
    /// Describes a <see cref="IVisual"/> implementation that supports content.
    /// </summary>
    public interface IContentVisual : 
        IVisual
    {

        /// <summary>
        /// Gets an enumeration of children for this visual.
        /// </summary>
        IEnumerable<Visual> Visuals { get; }

        /// <summary>
        /// Raised when the children of this <see cref="IContentVisual"/> have been invalidated.
        /// </summary>
        event EventHandler<EventArgs> VisualsInvalidated;

    }

}
