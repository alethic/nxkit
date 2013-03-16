namespace XForms.Util
{

    /// <summary>
    /// Provides various extensions methods for working with strings.
    /// </summary>
    public static class StringExtensions
    {

        public static string TrimToNull(this string source)
        {
            var s = source != null ? source.Trim() : null;
            return s != "" ? s : null;
        }

    }

}
