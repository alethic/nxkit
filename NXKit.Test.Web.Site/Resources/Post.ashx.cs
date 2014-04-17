using System;
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

            var input = XDocument.Load(context.Request.InputStream);
            Console.WriteLine(input.Element("data").Element("value").Value);

            new XDocument(
                new XElement("data",
                    new XElement("value", "New Value From Server")))
                .Save(context.Response.Output);
        }

        public bool IsReusable
        {
            get { return false; }
        }

    }

}