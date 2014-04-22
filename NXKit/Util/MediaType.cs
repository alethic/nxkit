using System;
using System.Diagnostics.Contracts;

namespace NXKit.Util
{

    /// <summary>
    /// Represents a media type or subtype in a <see cref="MediaRange"/>.
    /// </summary>
    public struct MediaType
    {

        public static implicit operator MediaType(string value)
        {
            return value != null ? new MediaType(value) : null;
        }

        public static implicit operator string(MediaType type)
        {
            return type.ToString();
        }


        readonly string value;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaType"/> class for the media type part.
        /// </summary>
        /// <param name="value"></param>
        MediaType(string value)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(value));

            this.value = value;
        }

        /// <summary>
        /// Gets a value indicating whether the media type is a wildcard or not.
        /// </summary>
        /// <value><see langword="true" /> if the media type is a wildcard, otherwise <see langword="false" />.</value>
        public bool IsWildcard
        {
            get { return value != null && string.Equals(value, "*", StringComparison.Ordinal); }
        }

        /// <summary>
        /// Matched the media type with another media type.
        /// </summary>
        /// <param name="other">The media type that should be matched against.</param>
        /// <returns><see langword="true" /> if the media types match, otherwise <see langword="false" />.</returns>
        public bool Matches(MediaType other)
        {
            return IsWildcard || other.IsWildcard || value.Equals(other.value, StringComparison.InvariantCultureIgnoreCase);
        }

        public override string ToString()
        {
            return value;
        }

        public override bool Equals(object other)
        {
            return other is MediaType ? Matches((MediaType)other) : false;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

    }

}