using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Linq;

namespace NXKit.Util
{

    /// <summary>
    /// Represents animmutable ordered sequence of <see cref="MediaRange"/>s.
    /// </summary>
    public struct MediaRangeList :
        IEnumerable<MediaRange>
    {

        /// <summary>
        /// Parses a string representation of a media-range list.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static MediaRangeList Parse(string value)
        {
            Contract.Requires<ArgumentNullException>(value != null);

            return new MediaRangeList(value
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(i => (MediaRange)i));
        }

        public static implicit operator MediaRangeList(MediaRange mediaRange)
        {
            return new MediaRangeList(mediaRange);
        }

        public static implicit operator MediaRangeList(string value)
        {
            Contract.Requires<ArgumentNullException>(value != null);

            return Parse(value);
        }

        public static implicit operator MediaRangeList(MediaRange[] mediaRanges)
        {
            return new MediaRangeList(mediaRanges);
        }

        public static implicit operator MediaRangeList(string[] mediaRanges)
        {
            return mediaRanges != null ? new MediaRangeList(mediaRanges.SelectMany(i => (MediaRangeList)i)) : new MediaRangeList();
        }

        public static MediaRangeList operator +(MediaRangeList target, MediaRange value)
        {
            return target.AddLast(value);
        }

        public static MediaRangeList operator -(MediaRangeList target, MediaRange value)
        {
            return target.Remove(value);
        }

        public static MediaRangeList operator +(MediaRangeList target, IEnumerable<MediaRange> source)
        {
            return target.AddLast(source);
        }

        public static MediaRangeList operator -(MediaRangeList target, IEnumerable<MediaRange> source)
        {
            return target.Remove(source);
        }

        ImmutableList<MediaRange> items;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        MediaRangeList(MediaRange first)
        {
            items = first != null ? ImmutableList<MediaRange>.Empty.Add(first) : null;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        MediaRangeList(IEnumerable<MediaRange> source)
        {
            items = source != null ? ImmutableList<MediaRange>.Empty.AddRange(source) : null;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="source"></param>
        MediaRangeList(params MediaRange[] source)
            : this((IEnumerable<MediaRange>)source)
        {

        }

        /// <summary>
        /// Adds a new <see cref="MediaRange"/> to the beginning of the <see cref="MediaRangeList"/>.
        /// </summary>
        /// <param name="mediaRange"></param>
        public MediaRangeList AddFirst(MediaRange mediaRange)
        {
            return mediaRange != null ? new MediaRangeList((items ?? ImmutableList<MediaRange>.Empty).Insert(0, mediaRange)) : this;
        }

        /// <summary>
        /// Adds a set of <see cref="MediaRange"/>s to the beginning of the <see cref="MediaRangeList"/>.
        /// </summary>
        /// <param name="mediaRanges"></param>
        /// <returns></returns>
        public MediaRangeList AddFirst(IEnumerable<MediaRange> mediaRanges)
        {
            return mediaRanges != null ? new MediaRangeList((items ?? ImmutableList<MediaRange>.Empty).InsertRange(0, mediaRanges)) : this;
        }

        /// <summary>
        /// Adds a new <see cref="MediaRange"/> to the end of the <see cref="MediaRangeList"/>.
        /// </summary>
        /// <param name="mediaRange"></param>
        public MediaRangeList AddLast(MediaRange mediaRange)
        {
            return mediaRange != null ? new MediaRangeList((items ?? ImmutableList<MediaRange>.Empty).Add(mediaRange)) : this;
        }

        /// <summary>
        /// Adds a set of <see cref="MediaRange"/>s to the end of the <see cref="MediaRangeList"/>.
        /// </summary>
        /// <param name="mediaRanges"></param>
        /// <returns></returns>
        public MediaRangeList AddLast(IEnumerable<MediaRange> mediaRanges)
        {
            return mediaRanges != null ? new MediaRangeList((items ?? ImmutableList<MediaRange>.Empty).AddRange(mediaRanges)) : this;
        }

        /// <summary>
        /// Removes the specified <see cref="MediaRange"/> from the <see cref="MediaRangeList"/>.
        /// </summary>
        /// <param name="mediaRange"></param>
        /// <returns></returns>
        public MediaRangeList Remove(MediaRange mediaRange)
        {
            return items != null ? new MediaRangeList(mediaRange != null ? items.Remove(mediaRange) : items) : this;
        }

        /// <summary>
        /// Removes a set of <see cref="MediaRange"/>s from the <see cref="MediaRangeList"/>.
        /// </summary>
        /// <param name="mediaRanges"></param>
        /// <returns></returns>
        public MediaRangeList Remove(IEnumerable<MediaRange> mediaRanges)
        {
            return mediaRanges != null ? new MediaRangeList((items ?? ImmutableList<MediaRange>.Empty).RemoveRange(mediaRanges)) : this;
        }

        public IEnumerator<MediaRange> GetEnumerator()
        {
            return items != null ? items.GetEnumerator() : Enumerable.Empty<MediaRange>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return string.Join(", ", this);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MediaRangeList))
                return false;

            return this.SequenceEqual((MediaRangeList)obj);
        }

        public override int GetHashCode()
        {
            return this.Aggregate(0, (i, j) => i ^ j.GetHashCode());
        }

    }

}
