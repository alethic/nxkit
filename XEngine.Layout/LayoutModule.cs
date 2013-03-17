using System.ComponentModel.Composition;
using System.Xml.Linq;

namespace NXKit.Layout
{

    [Module]
    public class LayoutModule : Module
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        [ImportingConstructor]
        public LayoutModule([Import(typeof(FormProcessor))] FormProcessor form)
            : base(form)
        {

        }

        /// <summary>
        /// Resolves the XForms node for attribute <paramref name="name"/> on <paramref name="element"/>.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        internal XAttribute ResolveAttribute(XElement element, string name)
        {
            if (element.Name.Namespace == Constants.Layout_1_0)
                // only layout native elements support default-ns attributes
                return element.Attribute(Constants.Layout_1_0 + name) ?? element.Attribute(name);
            else
                // non-layout native elements must be prefixed
                return element.Attribute(Constants.Layout_1_0 + name);
        }

        /// <summary>
        /// Gets the XForms attribute value <paramref name="name"/> on <paramref name="element"/>.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        internal string GetAttributeValue(XElement element, string name)
        {
            var attr = ResolveAttribute(element, name);
            return attr != null ? (string)attr : null;
        }

        public override void AnnotateVisual(Visual visual)
        {
            base.AnnotateVisual(visual);

            // set importance annotation if marked
            if (visual is StructuralVisual)
            {
                var attr = GetAttributeValue(((StructuralVisual)visual).Element, "importance");
                if (attr == "high")
                    visual.Annotations.Set<ImportanceAnnotation>(new ImportanceAnnotation(Importance.High));
                else if (attr == "low")
                    visual.Annotations.Set<ImportanceAnnotation>(new ImportanceAnnotation(Importance.Low));
            }
        }

        public override bool Run()
        {
            return false;
        }

    }

}