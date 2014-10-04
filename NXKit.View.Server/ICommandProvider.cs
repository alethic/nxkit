using System.Collections.Generic;

using NXKit.View.Server.Commands;

namespace NXKit.View.Server
{

    /// <summary>
    /// Provides a source of commands to be sent to the client when saving a <see cref="Document"/>.
    /// </summary>
    public interface ICommandProvider
    {

        /// <summary>
        /// Gets the set of commands to be sent to the client.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Command> Commands { get; }

    }

}
