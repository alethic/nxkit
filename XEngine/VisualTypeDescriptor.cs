using System.Xml.Linq;

namespace ISIS.Forms
{

    /// <summary>
    /// Descripts a <see cref="Visual"/> type and provides factory methods to initialize instances.
    /// </summary>
    public abstract class VisualTypeDescriptor
    {

        public abstract Visual CreateVisual(IFormProcessor form, StructuralVisual parent, XNode node);

    }

}
