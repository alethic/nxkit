using System.Xml.Linq;

namespace XEngine
{

    /// <summary>
    /// Descripts a <see cref="Visual"/> type and provides factory methods to initialize instances.
    /// </summary>
    public abstract class VisualTypeDescriptor
    {

        public abstract Visual CreateVisual(IFormProcessor form, StructuralVisual parent, XNode node);

    }

}
