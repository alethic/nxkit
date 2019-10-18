using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.Diagnostics;
using NXKit.Serialization;
using NXKit.Util;
using NXKit.Xml;

namespace NXKit
{

    /// <summary>
    /// Hosts an NXKit document. Provides access to the visual tree for a renderer or other processor.
    /// </summary>
    public class Document :
        IDisposable
    {

        readonly ICompositionContext context;
        readonly IInvoker invoker;
        readonly ITraceService trace;
        readonly XDocument xml;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="context"></param>
        internal Document(
            Func<Document, XDocument> xml,
            ICompositionContext context)
        {
            if (xml is null)
                throw new ArgumentNullException(nameof(xml));
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            // configure composition
            this.context = context.BeginContext(CompositionScope.Host);
            this.context.Resolve<DocumentEnvironment>().SetHost(this);

            // required services
            invoker = this.context.Resolve<IInvoker>();
            trace = this.context.Resolve<ITraceService>();

            // initialize xml
            this.xml = xml(this);
            this.xml.AddAnnotation(this);

            // parallel initialization of common interfaces
            Parallel.ForEach(this.xml.DescendantNodesAndSelf(), i =>
            {
                Enumerable.Empty<object>()
                    .Concat(i.Interfaces<IOnInit>())
                    .Concat(i.Interfaces<IOnLoad>())
                    .ToLinkedList();
            });

            // initial invocation entry
            invoker.Invoke(() => { });
        }

        /// <summary>
        /// Gets the host configured <see cref="ICompositionContext"/>.
        /// </summary>
        public ICompositionContext Context => context;

        /// <summary>
        /// Gets a reference to the current <see cref="Xml"/> being handled.
        /// </summary>
        public XDocument Xml => xml;

        /// <summary>
        /// Gets a reference to the root <see cref="XElement"/> instance for navigating the visual tree.
        /// </summary>
        public XElement Root => xml.Root;

        /// <summary>
        /// Saves the current state of the <see cref="Document"/> to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public void Save(XmlWriter writer)
        {
            if (writer is null)
                throw new ArgumentNullException(nameof(writer));

            // instruct any interfaces to save their state
            var saves = Xml.DescendantsAndSelf()
                .SelectMany(i => i.Interfaces<IOnSave>())
                .ToLinkedList();
            foreach (var save in saves)
                save.Save();

            // serialize document to writer
            context.Resolve<AnnotationSerializer>().Serialize(xml).Save(writer);
        }

        /// <summary>
        /// Saves the current state of the <see cref="Document"/> to the specified <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="writer"></param>
        public void Save(TextWriter writer)
        {
            if (writer is null)
                throw new ArgumentNullException(nameof(writer));

            var settings = new XmlWriterSettings()
            {
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = true,
                NamespaceHandling = NamespaceHandling.Default,
            };

            using (var wrt = XmlWriter.Create(writer, settings))
                Save(wrt);
        }

        /// <summary>
        /// Saves the current state of the <see cref="Document"/> to the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream"></param>
        public void Save(Stream stream)
        {
            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            using (var wrt = new StreamWriter(stream, Encoding.UTF8))
                Save(wrt);
        }

        /// <summary>
        /// Disposes of the <see cref="Document"/>.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);

            if (xml != null)
            {
                // dispose of any annotations that support it
                var disposable = xml
                    .DescendantNodesAndSelf()
                    .SelectMany(i => i.Annotations<IDisposable>());

                foreach (var dispose in disposable)
                    if (dispose != this)
                        dispose.Dispose();
            }

            // dispose of host container
            if (context != null)
                context.Dispose();
        }

        /// <summary>
        /// Finalizes the instance.
        /// </summary>
        ~Document()
        {
            Dispose();
        }

    }

}
