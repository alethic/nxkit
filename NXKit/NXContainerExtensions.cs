using System.Collections.Generic;

namespace NXKit
{

    public static class NXContainerExtensions
    {

        /// <summary>
        /// Yields each descendant visual.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IEnumerable<NXNode> Descendants(this NXContainer self)
        {
            return self.Descendants(false);
        }

        /// <summary>
        /// Yields each descendant visual.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="includeSelf"></param>
        /// <returns></returns>
        public static IEnumerable<NXNode> Descendants(this NXContainer self, bool includeSelf)
        {
            if (includeSelf)
                yield return self;

            foreach (var container in self.Nodes())
                if (container is NXContainer)
                    foreach (var descendant in ((NXContainer)container).Descendants(true))
                        yield return descendant;
                else
                    yield return container;
        }

    }

}
