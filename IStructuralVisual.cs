using System;
using System.Collections.Generic;

namespace NXKit
{

    public interface IStructuralVisual : IVisual
    {

        /// <summary>
        /// Gets an enumeration of children for this visual.
        /// </summary>
        IEnumerable<Visual> Children { get; }

        /// <summary>
        /// Raised when the children of this <see cref="IVisual"/> have been invalidated.
        /// </summary>
        event EventHandler<EventArgs> ChildrenInvalidated;

    }

}
