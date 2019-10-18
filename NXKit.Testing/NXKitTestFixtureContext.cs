using System;

using NXKit.Composition;

namespace NXKit.Testing
{

    public class NXKitTestFixtureContext
    {

        readonly Func<ICompositionContext> getCompositionContext;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="getCompositionContext"></param>
        public NXKitTestFixtureContext(string contextName, Func<ICompositionContext> getCompositionContext)
        {
            ContextName = contextName ?? throw new ArgumentNullException(nameof(contextName));
            this.getCompositionContext = getCompositionContext ?? throw new ArgumentNullException(nameof(getCompositionContext));
        }

        /// <summary>
        /// Name of the context.
        /// </summary>
        public string ContextName { get; }

        /// <summary>
        /// Gets the composition context.
        /// </summary>
        public ICompositionContext CompositionContext => getCompositionContext();

        /// <summary>
        /// Gets a reference to the NXEngine.
        /// </summary>
        public NXEngine Engine => CompositionContext.Resolve<NXEngine>();

        /// <summary>
        /// Gets display names suitable for rendering arguments.
        /// </summary>
        /// <returns></returns>
        public string[] GetDisplayNames()
        {
            return new[]
            {
                ContextName,
            };
        }

    }

}