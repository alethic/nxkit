using System;
using System.Xml.Linq;

namespace ISIS.Forms
{

    public interface IFormProcessor
    {

        /// <summary>
        /// Gets a reference to the loaded module of <typeparam name="T" />
        /// </summary>
        T GetModule<T>()
            where T : Module;

        /// <summary>
        /// Gets a reference to the DOM of the form.
        /// </summary>
        XDocument Document { get; }

        /// <summary>
        /// Gets a reference to the resolver used to interact with external named resources.
        /// </summary>
        IResourceResolver Resolver { get; }

        /// <summary>
        /// Gets the 'id' attribute of the given <see cref="Element"/>.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        string GetElementId(XElement element);

        /// <summary>
        /// Gets a reference to the root visual of the form.
        /// </summary>
        StructuralVisual RootVisual { get; }

        /// <summary>
        /// Gets the <see cref="VisualStateCollection"/>.
        /// </summary>
        VisualStateCollection VisualState { get; }

        /// <summary>
        /// Queries registered <see cref="Module"/>s to generate a <see cref="Visual"/> for the given <see cref="XNode"/>.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        Visual CreateVisual(StructuralVisual parent, XNode node);

        /// <summary>
        /// Handles any outstanding actions in the form.
        /// </summary>
        void Run();

        /// <summary>
        /// Raised when the form wants to submit.
        /// </summary>
        event EventHandler ProcessSubmit;

    }

}
