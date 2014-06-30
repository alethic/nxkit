using System;
using System.Collections.Generic;
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
        /// <param name="value"></param>
        /// <returns></returns>
        public static MediaRange Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (value.Equals("*"))
                value = "*/*";

            var parts = value.Split('/', ';');
            if (parts.Length < 2)
                throw new ArgumentException("Content type not in correct 'type/subType' format.", value);

            return new MediaRange(
                parts[0],
                parts[1].TrimEnd(),
                parts.Length > 2 ? MediaRangeParameters.Parse(value.Substring(value.IndexOf(';'))) : new MediaRangeParameters());
        }

        public static implicit operator MediaRange(string value)
        {
            return value != null ? MediaRange.Parse(value) : null;
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
        public MediaRangeParameters Parameters
        {
            get { return parameters; }
        }

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
            Contract.Requires<ArgumentNullException>(other != null);

            return type.Matches(other.type) && subtype.Matches(other.subtype);
        }

        /// <summary>
        /// Whether or not a media range matches another, taking into account wildcards and parameters.
        /// </summary>
        /// <param name="other">Other media range.</param>
        /// <returns>True if matching, false if not.</returns>
        public bool MatchesWithParameters(MediaRange other)
        {
            Contract.Requires<ArgumentNullException>(other != null);

            return Matches(other) && parameters.Matches(other.parameters);
        }

        /// <summary>
        /// Whether or not a media range matches any other media ranges, taking into account wildcards.
        /// </summary>
        /// <param name="others"></param>
        /// <returns></returns>
        public bool Matches(IEnumerable<MediaRange> others)
        {
            Contract.Requires<ArgumentNullException>(others != null);

            var self = this;
            return others.Any(i => self.Matches(i));
        }

        /// <summary>
        /// Whether or not a media range matches any other media ranges, taking into account wildcards and parameters.
        /// </summary>
        /// <param name="others"></param>
        /// <returns></returns>
        public bool MatchesWithParameters(IEnumerable<MediaRange> others)
        {
            Contract.Requires<ArgumentNullException>(others != null);

            var self = this;
            return others.Any(i => self.MatchesWithParameters(i));
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