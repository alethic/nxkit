using System.Xml.Linq;

namespace NXKit.XForms
{

    [Remote]
    public interface IUIAppearance
    {

        [Remote]
        XName Appearance { get; }

    }

}
