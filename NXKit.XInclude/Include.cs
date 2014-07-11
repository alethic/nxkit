using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.Diagnostics;
using NXKit.IO;
using NXKit.Xml;

namespace NXKit.XInclude
{

    [ElementExtension("{http://www.w3.org/2001/XInclude}include")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class Include :
        ElementExtension,
        IOnInit
    {

        readonly Lazy<ITraceService> trace;
        readonly Lazy<IIOService> io;
        readonly Func<IncludeProperties> properties;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="trace"></param>
        /// <param name="io"></param>
        /// <param name="properties"></param>
        [ImportingConstructor]
        public Include(
            XElement element,
            Lazy<ITraceService> trace,
            Lazy<IIOService> io,
            Func<IncludeProperties> properties)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(trace != null);
            Contract.Requires<ArgumentNullException>(io != null);
            Contract.Requires<ArgumentNullException>(properties != null);

            this.trace = trace;
            this.io = io;
            this.properties = properties;
        }

        public IncludeProperties Properties
        {
            get { return properties(); }
        }

        protected ITraceService Trace
        {
            get { return trace.Value; }
        }

        protected IIOService IO
        {
            get { return io.Value; }
        }

        /// <summary>
        /// Gets the absolute <see cref="Uri"/> to be used to retrieve the include.
        /// </summary>
        /// <returns></returns>
        public Uri GetAbsoluteHref()
        {
            var uri = Properties.Href ?? new Uri("", UriKind.Relative);
            if (Element.GetBaseUri() != null && !uri.IsAbsoluteUri)
                uri = new Uri(Element.GetBaseUri(), uri);

            return uri;
        }

        /// <summary>
        /// Invoked after the newly inserted XML has been added.
        /// </summary>
        /// <param name="xml"></param>
        protected virtual void PostInsertXml(XElement xml)
        {

        }

        /// <summary>
        /// Includes the remote resource as an XML document.
        /// </summary>
        protected virtual void IncludeAsXml()
        {
            var uri = GetAbsoluteHref();
            var xml = XDocument.Load(IOXmlReader.Create(IO, uri, Properties.Accept));
            if (xml != null)
            {
                // annotate element and replace self in graph
                var element = new XElement(xml.Root);
                element.SetBaseUri(xml.BaseUri != "" ? xml.BaseUri : uri.ToString());
                Element.AddBeforeSelf(element);
                Element.Remove();

                PostInsertXml(element);
            }
        }

        /// <summary>
        /// Invoked afte rthe newly inserted text has been added.
        /// </summary>
        /// <param name="text"></param>
        protected virtual void PostInsertText(XText text)
        {

        }

        /// <summary>
        /// Includes the remote resource as a text document.
        /// </summary>
        protected virtual void IncludeAsText()
        {
            var uri = GetAbsoluteHref();
            var txt = IOTextReader.Create(IO, uri, Properties.Encoding).ReadToEnd();
            if (txt != null)
            {
                var text = new XText(txt);
                Element.AddBeforeSelf(txt);
                Element.Remove();

                PostInsertText(text);
            }
        }

        protected virtual void ExecuteInclude()
        {
            switch (Properties.Parse)
            {
                case IncludeParse.Xml:
                    IncludeAsXml();
                    break;
                case IncludeParse.Text:
                    IncludeAsText();
                    break;
            }
        }

        public void Init()
        {
            ExecuteInclude();
        }

    }

}
