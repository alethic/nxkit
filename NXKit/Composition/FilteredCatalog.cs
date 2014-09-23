using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;

namespace NXKit.Composition
{

    public class FilteredCatalog :
        ComposablePartCatalog
    {

        readonly ComposablePartCatalog parent;
        readonly Expression<Func<ComposablePartDefinition, bool>> filter;
        readonly Lazy<IEnumerable<ComposablePartDefinition>> parts;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="filter"></param>
        public FilteredCatalog(ComposablePartCatalog parent, Expression<Func<ComposablePartDefinition, bool>> filter)
        {
            Contract.Requires<ArgumentNullException>(parent != null);
            Contract.Requires<ArgumentNullException>(filter != null);

            this.parent = parent;
            this.filter = filter;
            this.parts = new Lazy<IEnumerable<ComposablePartDefinition>>(() => parent.Parts.Where(filter).ToList());
        }

        public override IEnumerator<ComposablePartDefinition> GetEnumerator()
        {
            return parts.Value.GetEnumerator();
        }

    }

}
