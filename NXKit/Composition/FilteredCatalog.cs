using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;
using System.Linq;

using NXKit.Util;

namespace NXKit.Composition
{

    public class FilteredCatalog :
        ComposablePartCatalog
    {

        readonly ComposablePartCatalog parent;
        readonly Func<ComposablePartDefinition, bool> filter;
        readonly IEnumerable<ComposablePartDefinition> parts;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="filter"></param>
        public FilteredCatalog(ComposablePartCatalog parent, Func<ComposablePartDefinition, bool> filter)
        {
            Contract.Requires<ArgumentNullException>(parent != null);
            Contract.Requires<ArgumentNullException>(filter != null);

            this.parent = parent;
            this.filter = filter;
            this.parts = parent.Parts.Where(i => filter(i)).Buffer();
        }

        public override IEnumerator<ComposablePartDefinition> GetEnumerator()
        {
            return parts.GetEnumerator();
        }

    }

}
