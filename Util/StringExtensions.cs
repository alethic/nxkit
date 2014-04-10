namespace NXKit.Util
{

    /// <summary>
    /// Provides various extensions methods for working with strings.
    /// </summary>
    public static class StringExtensions
    {

        /// <summary>
        /// Returns the string, or <c>null</c> if the string is empty or <c>null</c>.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string TrimToNull(this string source)
        {
            var s = source != null ? source.Trim() : null;
            return s != "" ? s : null;
        }

    }

}
