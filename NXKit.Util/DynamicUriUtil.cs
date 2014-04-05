using System.IO;
using System.Text;

namespace NXKit.Util
{

    public static class DynamicUriUtil
    {

        public static DisposableUri GetUriFor(string file)
        {
            return new AuthorityUri(new DynamicUriRootFunc(() => new MemoryStream(Encoding.UTF8.GetBytes(file))));
        }

    }

}
