using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;

using NXKit.Util;

namespace NXKit.Composition
{

    class ScopeImportDefinition :
        ImportDefinition
    {

        readonly ImportDefinition parent;
        readonly Scope scope;

        static IDictionary<string, object> GetMetadata(IDictionary<string, object> metadata, Scope scope)
        {
            metadata = new Dictionary<string, object>(metadata)
            {
                { "Scope", scope }
            };
            return metadata;
        }

        static Expression<Func<ExportDefinition, bool>> GetConstraint(Expression<Func<ExportDefinition, bool>> parent, Scope scope)
        {
            var par = parent.Parameters[0];
            var exp = Expression.Lambda<Func<ExportDefinition, bool>>(
                Expression.AndAlso(
                    parent.Body,
                    Expression.Call(
                        typeof(ScopeImportDefinition),
                        "IsConstraintApplied",
                        null,
                        par,
                        Expression.Constant(scope))),
                par);

            return exp;
        }

        static bool IsConstraintApplied(ExportDefinition exportDefinition, Scope scope)
        {
            var data = exportDefinition.Metadata.GetOrDefault("Scope");
            if (data == null)
                return scope == Scope.Global;

            if (data is Scope)
                return (Scope)data == scope;

            var array = data as IEnumerable<Scope>;
            if (array != null)
                return array.Any(i => i == scope);

            return false;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="scope"></param>
        public ScopeImportDefinition(ImportDefinition parent, Scope scope)
            : base(
                GetConstraint(parent.Constraint, scope),
                parent.ContractName,
                parent.Cardinality,
                parent.IsRecomposable,
                parent.IsPrerequisite,
                parent.Metadata)
        {
            Contract.Requires<ArgumentNullException>(parent != null);

            this.parent = parent;
            this.scope = scope;
        }

        public override bool IsConstraintSatisfiedBy(ExportDefinition exportDefinition)
        {
            return base.IsConstraintSatisfiedBy(exportDefinition);
        }

    }

}
