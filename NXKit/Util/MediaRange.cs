using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace NXKit.Util
{

    /// <summary>
    /// Represents a mime type media range, possibly including wildcards.
    /// </summary>
    public class MediaRange
    {

        /// <summary>
        /// Parses a new instance of <see cref="MediaRange"/> from a 'type/subtype' string.
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static MediaRange Parse(string contentType)
        {
            Contract.Requires<ArgumentNullException>(contentType != null);
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(contentType));

            if (contentType.Equals("*"))
                contentType = "*/*";

            var parts = contentType.Split('/', ';');
            if (parts.Length < 2)
                throw new ArgumentException("Content type not in correct 'type/subType' format.", contentType);

            return new MediaRange(
                parts[0],
                parts[1].TrimEnd(),
                parts.Length > 2 ? MediaRangeParameters.Parse(contentType.Substring(contentType.IndexOf(';'))) : new MediaRangeParameters());
        }

        public static implicit operator MediaRange(string contentType)
        {
            return contentType != null ? MediaRange.Parse(contentType) : null;
        }

        public static implicit operator string(MediaRange mediaRange)
        {
            return mediaRange != null ? mediaRange.ToString() : null;
        }

        public static bool operator ==(MediaRange a1, MediaRange a2)
        {
            return object.Equals(a1, a2);
        }

        public static bool operator !=(MediaRange a1, MediaRange a2)
        {
            return !object.Equals(a1, a2);
        }


        readonly MediaType type;
        readonly MediaType subtype;
        readonly MediaRangeParameters parameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaRange"/> class.
        /// </summary>
        MediaRange(MediaType type, MediaType subtype, MediaRangeParameters parameters)
        {
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Requires<ArgumentNullException>(subtype != null);
            Contract.Requires<ArgumentNullException>(parameters != null);

            this.type = type;
            this.subtype = subtype;
            this.parameters = parameters;
        }

        /// <summary>
        /// Media range type.
        /// </summary>
        public MediaType Type
        {
            get { return type; }
        }

        /// <summary>
        /// Media range subtype.
        /// </summary>
        public MediaType Subtype
        {
            get { return subtype; }
        }

        /// <summary>
        /// Media range parameters
        /// </summary>
        public MediaRangeParameters Parameters { get; set; }

        /// <summary>
        /// Gets a value indicating if the media range is the */* wildcard.
        /// </summary>
        public bool IsWildcard
        {
            get { return type.IsWildcard && subtype.IsWildcard; }
        }

        /// <summary>
        /// Whether or not a media range matches another, taking into account wildcards.
        /// </summary>
        /// <param name="other">Other media range.</param>
        /// <returns>True if matching, false if not.</returns>
        public bool Matches(MediaRange other)
        {
            return type.Matches(other.type) && subtype.Matches(other.subtype);
        }

        /// <summary>
        /// Whether or not a media range matches another, taking into account wildcards and parameters.
        /// </summary>
        /// <param name="other">Other media range.</param>
        /// <returns>True if matching, false if not.</returns>
        public bool MatchesWithParameters(MediaRange other)
        {
            return Matches(other) && parameters.Matches(other.parameters);
        }

        public override string ToString()
        {
            if (parameters.Any())
                return string.Format("{0}/{1};{2}", type, subtype, parameters);
            else
                return string.Format("{0}/{1}", type, subtype);
        }

        public override bool Equals(object obj)
        {
            var other = obj as MediaRange;
            if (other == null)
                return false;

            return MatchesWithParameters(other);
        }

        public override int GetHashCode()
        {
            return type.GetHashCode() ^ subtype.GetHashCode() ^ parameters.GetHashCode();
        }

    }

}