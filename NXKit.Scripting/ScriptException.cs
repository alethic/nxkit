using System;

namespace NXKit.Scripting
{

    public class ScriptException :
        Exception
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ScriptException()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        public ScriptException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public ScriptException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

    }

}
