using System.Collections.Generic;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// Describes a table of available <see cref="EventInfo"/> instances.
    /// </summary>
    public interface IEventInfoTable
    {

        /// <summary>
        /// Gets all the items from the table.
        /// </summary>
        /// <returns></returns>
        IEnumerable<EventInfo> GetEventInfos();

    }

}
