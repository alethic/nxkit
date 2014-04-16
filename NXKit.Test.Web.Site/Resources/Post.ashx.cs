using System.Web;
using System.Xml.Linq;

namespace NXKit.Test.Web.Site.Resources
{

    public class Post : 
        IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/xml";

            new XDocument(new XElement("value", "New Value")).Save(context.Response.Output);
        }

        public bool IsReusable
        {
            get { return false; }
        }

    }

}