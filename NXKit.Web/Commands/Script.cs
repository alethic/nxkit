using NXKit.IO.Media;

namespace NXKit.Web.Commands
{

    /// <summary>
    /// Commands the client to execute a script.
    /// </summary>
    public class Script :
        Command
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public Script()
        {
            Language = "application/javascript";
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="code"></param>
        public Script(string code)
        {
            Language = "application/javascript";
            Code = code;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="language"></param>
        /// <param name="code"></param>
        public Script(MediaRange language, string code)
        {
            Language = language;
            Code = code;
        }

        /// <summary>
        /// Gets or sets the type of script to execute. Usually 'application/javascript'.
        /// </summary>
        public MediaRange Language { get; set; }

        /// <summary>
        /// Gets or sets the script text to be executed.
        /// </summary>
        public string Code { get; set; }

    }

}
