using System;

namespace NXKit.Scripting
{

    public class ScriptingException :
        Exception
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ScriptingException()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        public ScriptingException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public ScriptingException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

    }

}
