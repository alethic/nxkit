using System.Xml.Linq;

namespace ISIS.Forms
{

    /// <summary>
    /// Modules provide the implementation of a specification.
    /// </summary>
    public abstract class Module
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public Module(IFormProcessor form)
        {
            Form = form;
        }

        /// <summary>
        /// Gets a reference to the form processor hosting this module.
        /// </summary>
        public IFormProcessor Form { get; private set; }

        /// <summary>
        /// Gets a reference to the underlying form document.
        /// </summary>
        public XDocument Document
        {
            get { return Form.Document; }
        }
        
        /// <summary>
        /// Gets a reference to the <see cref="IResourceResolver"/>.
        /// </summary>
        public IResourceResolver Resolver
        {
            get { return Form.Resolver; }
        }

        /// <summary>
        /// Initializes the module. This is invoked after whatever state is available has been loaded.
        /// </summary>
        public virtual void Initialize()
        {

        }

        /// <summary>
        /// Runs the module.
        /// </summary>
        /// <returns></returns>
        public abstract bool Run();

        /// <summary>
        /// Expands a DOM node into a <see cref="Visual"/>.
        /// </summary>
        /// <param name="form"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public virtual Visual CreateVisual(IFormProcessor form, StructuralVisual parent, XNode node)
        {
            return null;
        }

        /// <summary>
        /// Gives a <see cref="Module"/> a chance to attach additional information to a <see cref="Visual"/>.
        /// </summary>
        /// <param name="visual"></param>
        public virtual void AnnotateVisual(Visual visual)
        {

        }

    }

}
