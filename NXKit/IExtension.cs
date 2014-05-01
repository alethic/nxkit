using System.Xml.Linq;

namespace NXKit
{

    public interface IExtension<T>
        where T : XObject
    {

        T Object { get; }

    }

}
