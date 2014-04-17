namespace NXKit
{

    /// <summary>
    /// Describes the priority of a negotiation.
    /// </summary>
    public enum Priority :
        int
    {

        /// <summary>
        /// Exclude this response.
        /// </summary>
        Ignore = int.MinValue,

        /// <summary>
        /// Lowest priority.
        /// </summary>
        Low = -1,

        /// <summary>
        /// Default priority.
        /// </summary>
        Default = 0,
        
        /// <summary>
        /// Highest priority.
        /// </summary>
        High = 1,

    }

}
