using System;

namespace NXKit
{

    /// <summary>
    /// Provides an interface that will capture and handle exceptions raised by various components of the system.
    /// </summary>
    public interface IExceptionHandler
    {

        /// <summary>
        /// Attempts to handle the given exception. Returns <c>true</c> if the exception was handled and should not be rethrown.
        /// </summary>
        /// <param name="exception"></param>
        bool HandleException(Exception exception);

    }

}
