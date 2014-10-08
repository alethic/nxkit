using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms
{

    /// <summary>
    /// Resolves various <see cref="EvaluationContext"/> instances with regards to the specified <see cref="XAttribute"/>.
    /// </summary>
    [Extension(ExtensionObjectType.Attribute)]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class AttributeEvaluationContextResolver :
        EvaluationContextResolver,
        IExtension
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="attribute"></param>
        [ImportingConstructor]
        public AttributeEvaluationContextResolver(
            XAttribute attribute)
            : base(attribute)
        {
            Contract.Requires<ArgumentNullException>(attribute != null);
        }

        /// <summary>
        /// Gets the <see cref="XAttribute"/> this instance is resolving <see cref="EvaluationContext"/>s for.
        /// </summary>
        public XAttribute Attribute
        {
            get { return (XAttribute)base.Object; }
        }

        /// <summary>
        /// Gets the <see cref="EvaluationContext"/> to be used by this attribute.
        /// </summary>
        /// <returns></returns>
        internal EvaluationContext GetContextForAttribute()
        {
            return base.GetContext();
        }

        protected override EvaluationContext GetContext()
        {
            return GetContextForAttribute();
        }

    }

}
