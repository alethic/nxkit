using NXKit.IO.Media;

namespace NXKit.XForms
{

    /// <summary>
    /// Defines the serialization property for the 'submission' element.
    /// </summary>
    public class SubmissionSerialization
    {

        readonly bool none;
        readonly MediaRange mediaRange;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="none"></param>
        public SubmissionSerialization(bool none)
        {
            this.none = none;
            this.mediaRange = null;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="mediaRange"></param>
        public SubmissionSerialization(MediaRange mediaRange)
        {
            this.none = false;
            this.mediaRange = mediaRange;
        }

        /// <summary>
        /// If <c>true</c>, submission should proceed with no serialized data.
        /// </summary>
        public bool None
        {
            get { return none; }
        }

        /// <summary>
        /// Gets the specified media type to serialize as.
        /// </summary>
        public MediaRange MediaRange
        {
            get { return mediaRange; }
        }

    }

}
