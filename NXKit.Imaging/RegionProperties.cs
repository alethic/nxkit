using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.Imaging
{

    /// <summary>
    /// 
    /// </summary>
    [Extension(typeof(RegionProperties))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class RegionProperties :
        ElementExtension
    {

        readonly RegionAttributes attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        [ImportingConstructor]
        public RegionProperties(
            XElement element,
            RegionAttributes attributes)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.attributes = attributes;
        }

        int? TryParseInt32(string value)
        {
            int i;
            return int.TryParse(value, out i) ? (int?)i : null;
        }

        Unit? TryParseUnit(string value)
        {
            return null;
        }

        public int? Page
        {
            get { return TryParseInt32(attributes.Page); }
        }

        public Unit? Left
        {
            get { return TryParseUnit(attributes.Left); }
        }

        public Unit? Top
        {
            get { return TryParseUnit(attributes.Top); }
        }

        public Unit? Right
        {
            get { return TryParseUnit(attributes.Right); }
        }

        public Unit? Bottom
        {
            get { return TryParseUnit(attributes.Bottom); }
        }

        public Unit? Width
        {
            get { return TryParseUnit(attributes.Width); }
        }

        public Unit? Height
        {
            get { return TryParseUnit(attributes.Height); }
        }

    }

}