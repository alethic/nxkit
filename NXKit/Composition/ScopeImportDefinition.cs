using System;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;

namespace NXKit.Composition
{

    /// <summary>
    /// Modifies the given <see cref="ImportDefinition"/> to only import exports that are marked with the provided
    /// <see cref="Scope"/>.
    /// </summary>
    class ScopeImportDefinition :
        ImportDefinition
    {

        static readonly MethodInfo IsConstraintAppliedMethodInfo = typeof(ScopeImportDefinition)
            .GetMethod("IsConstraintApplied", BindingFlags.Static | BindingFlags.NonPublic);

        readonly ImportDefinition parent;
        readonly Scope scope;

        /// <summary>
        /// Appends an invocation to the IsConstraintApplied method.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        static Expression<Func<ExportDefinition, bool>> GetConstraint(Expression<Func<ExportDefinition, bool>> parent, Scope scope)
        {
            var par = parent.Parameters[0];
            var exp = Expression.Lambda<Func<ExportDefinition, bool>>(
                Expression.AndAlso(
                    parent.Body,
                    Expression.Call(
                        IsConstraintAppliedMethodInfo,
                        par,
                        Expression.Constant(scope))),
                par);

            return exp;
        }

        /// <summary>
        /// Returns <c>true</c> if the constraint is applied.
        /// </summary>
        /// <param name="exportDefinition"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        static bool IsConstraintApplied(ExportDefinition exportDefinition, Scope scope)
        {
            return ScopeHelper.IsScoped(exportDefinition.Metadata, scope);
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

    }

}
