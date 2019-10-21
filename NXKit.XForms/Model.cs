using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

using NXKit.Diagnostics;
using NXKit.DOMEvents;
using NXKit.IO;
using NXKit.Xml;
using NXKit.Xml.Schema;

namespace NXKit.XForms
{

    [Extension("{http://www.w3.org/2002/xforms}model")]
    public class Model :
        ElementExtension,
        IEventDefaultAction
    {

        readonly IIOService io;
        readonly ModelAttributes attributes;
        readonly IEnumerable<IDefaultXmlSchemaProvider> defaultSchemaProvider;
        readonly IEnumerable<IModelXmlSchemaProvider> modelSchemaProvider;
        readonly ITraceService trace;
        readonly Lazy<ModelState> state;
        readonly Lazy<DocumentAnnotation> documentAnnotation;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        /// <param name="io"></param>
        /// <param name="defaultSchemaProvider"></param>
        /// <param name="modelSchemaProvider"></param>
        /// <param name="trace"></param>
        public Model(
            XElement element,
            ModelAttributes attributes,
            IIOService io,
            IEnumerable<IDefaultXmlSchemaProvider> defaultSchemaProvider,
            IEnumerable<IModelXmlSchemaProvider> modelSchemaProvider,
            ITraceService trace)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
            this.io = io ?? throw new ArgumentNullException(nameof(io));
            this.defaultSchemaProvider = defaultSchemaProvider ?? throw new ArgumentNullException(nameof(defaultSchemaProvider));
            this.modelSchemaProvider = modelSchemaProvider ?? throw new ArgumentNullException(nameof(modelSchemaProvider));
            this.trace = trace ?? throw new ArgumentNullException(nameof(trace));

            state = new Lazy<ModelState>(() => Element.AnnotationOrCreate<ModelState>());
            documentAnnotation = new Lazy<DocumentAnnotation>(() => Element.Document.AnnotationOrCreate<DocumentAnnotation>());
        }

        /// <summary>
        /// Gets the model state associated with this model interface.
        /// </summary>
        public ModelState State => state.Value;

        /// <summary>
        /// Gets the known XML schemas.
        /// </summary>
        public XmlSchemaSet XmlSchemas => State.XmlSchemas;

        /// <summary>
        /// Gets the current document annotation.
        /// </summary>
        DocumentAnnotation DocumentAnnotation => documentAnnotation.Value;

        /// <summary>
        /// Provides the model default evaluation context.
        /// </summary>
        public EvaluationContext Context => DefaultEvaluationContext;

        /// <summary>
        /// Gets the set of <see cref="Instance"/>s.
        /// </summary>
        public IEnumerable<Instance> Instances
        {
            get { return Element.Elements(Constants.XForms + "instance").SelectMany(i => i.Interfaces<Instance>()); }
        }

        /// <summary>
        /// Gets a new <see cref="EvaluationContext"/> that should be the default used for the model when no
        /// instance is known.
        /// </summary>
        public EvaluationContext DefaultEvaluationContext
        {
            get
            {
                if (State == null)
                    return null;

                var defaultInstance = Instances.FirstOrDefault();
                if (defaultInstance == null)
                    return null;

                if (defaultInstance.State.Document == null)
                    return null;

                return new EvaluationContext(this, defaultInstance, ModelItem.Get(defaultInstance.State.Document.Root, trace), 1, 1);
            }
        }

        /// <summary>
        /// Gets all implementations of the given extension type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetAllExtensions<T>()
            where T : class, IExtension
        {
            return Element.Document.Root
                .DescendantsAndSelf().Cast<XObject>()
                .Prepend(Element.Document)
                .Where(i => i.Document != null)
                .SelectMany(i => i.Interfaces<T>());
        }

        void IEventDefaultAction.DefaultAction(Event evt)
        {
            switch (evt.Type)
            {
                case Events.ModelConstruct:
                    OnModelConstruct();
                    break;
                case Events.ModelConstructDone:
                    OnModelConstructDone();
                    break;
                case Events.Ready:
                    OnReady();
                    break;
                case Events.Rebuild:
                    OnRebuild();
                    break;
                case Events.Recalculate:
                    OnRecalculate();
                    break;
                case Events.Revalidate:
                    OnRevalidate();
                    break;
                case Events.Refresh:
                    OnRefresh();
                    break;
                case Events.Reset:
                    OnReset();
                    break;
            }
        }

        /// <summary>
        /// Default action for the xforms-model-construct event.
        /// </summary>
        void OnModelConstruct()
        {
            // mark step as complete, regardless of outcome
            State.Construct = true;

            // validate model version, we only support 1.0
            if (attributes.Version != null)
                foreach (var version in attributes.Version.Split(' ').Select(i => i.Trim()).Where(i => !string.IsNullOrEmpty(i)))
                    if (version != "1.0")
                        throw new DOMTargetEventException(Element, Events.VersionException);
            
            // begin assemblying schemas for the model
            var builder = new XmlSchemaSetBuilder();

            // add default schemas
            foreach (var schema in defaultSchemaProvider.SelectMany(i => i.GetSchemas()).Distinct())
                builder.Add(schema);

            // add default schemas
            foreach (var schema in modelSchemaProvider.SelectMany(i => i.GetSchemas(this)).Distinct())
                builder.Add(schema);

            // override with existing schema
            foreach (var schema in State.XmlSchemas.Schemas().OfType<XmlSchema>())
                builder.Add(schema);

            // attempt to load XML schemas referred to by the Schema attribute
            if (attributes.Schema != null)
                foreach (var item in attributes.Schema.Split(' ').Select(i => i.Trim()).Where(i => !string.IsNullOrEmpty(i)))
                    builder.Add(LoadSchema(new Uri(item, UriKind.RelativeOrAbsolute)));

            // attempt to load XML schemas listed directly under the model
            foreach (var element in Element.Elements("{http://www.w3.org/2001/XMLSchema}schema"))
            {
                builder.Add(LoadSchema(element));
                element.Remove();
            }

            // build, compile, and check
            State.XmlSchemas = builder.Build(ValidationEventHandler);
            if (State.XmlSchemas.IsCompiled == false)
                throw new InvalidOperationException("Unable to compile schema set for model.");

            // attempt to load model instance data
            foreach (var instance in Instances)
                instance.Load();

            OnRebuild();
            OnRecalculate();
            OnRevalidate();
        }

        /// <summary>
        /// Handles any validation errors during compilation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void ValidationEventHandler(object sender, ValidationEventArgs args)
        {
            XmlSchemaCompilationEvent(args.Exception, args.Message, args.Severity);
        }

        /// <summary>
        /// Invoked when an exception occurs during validation.
        /// </summary>
        /// <param name="exception"></param>
        void XmlSchemaCompilationEvent(XmlSchemaException exception, string message, XmlSeverityType severity)
        {
            // default message to exception output
            if (message == null && exception != null)
                message = exception.Message;

            // log based on severity
            switch (severity)
            {
                case XmlSeverityType.Error:
                    trace.Error(message);
                    break;
                case XmlSeverityType.Warning:
                    trace.Warning(message);
                    break;
            }
        }

        /// <summary>
        /// Default action for the xforms-model-construct-done event.
        /// </summary>
        void OnModelConstructDone()
        {
            State.ConstructDone = true;

            if (DocumentAnnotation.ConstructDoneOnce)
                return;

            // refresh bindings
            foreach (var i in GetAllExtensions<IOnRefresh>())
                i.RefreshBinding();

            // discard refresh events
            foreach (var i in GetAllExtensions<IOnRefresh>())
                i.DiscardEvents();

            // final refresh
            foreach (var i in GetAllExtensions<IOnRefresh>())
                i.Refresh();

            DocumentAnnotation.ConstructDoneOnce = true;
        }

        /// <summary>
        /// Default action for the xforms-ready event.
        /// </summary>
        internal void OnReady()
        {
            State.Ready = true;
        }

        /// <summary>
        /// Default action for the xforms-rebuild event.
        /// </summary>
        internal void OnRebuild()
        {
            do
            {
                State.Rebuild = false;
                State.Recalculate = true;

                // validate each instance
                foreach (var instance in Instances)
                    instance.Rebuild();
            }
            while (State.Rebuild);
        }

        /// <summary>
        /// Default action for the xforms-recalculate event.
        /// </summary>
        internal void OnRecalculate()
        {
            do
            {
                State.Recalculate = false;
                State.Revalidate = true;

                // validate each instance
                foreach (var instance in Instances)
                    instance.Calculate();

                // apply all of the bind elements
                foreach (var bind in GetAllExtensions<Bind>())
                {
                    bind.Recalculate();
                    bind.Apply();
                }
            }
            while (State.Recalculate);
        }

        /// <summary>
        /// Default action for the xforms-revalidate event.
        /// </summary>
        internal void OnRevalidate()
        {
            do
            {
                State.Revalidate = false;

                // validate each instance
                foreach (var instance in Instances)
                    instance.Validate();
            }
            while (State.Revalidate);
        }

        /// <summary>
        /// Default action for the xforms-refresh event.
        /// </summary>
        internal void OnRefresh()
        {
            do
            {
                State.Refresh = false;

                // refresh bindings
                foreach (var ui in GetAllExtensions<IOnRefresh>())
                    ui.RefreshBinding();

                // refresh interfaces
                foreach (var ui in GetAllExtensions<IOnRefresh>())
                    ui.Refresh();

                // dispatch events
                foreach (var item in GetAllExtensions<IOnRefresh>())
                    item.DispatchEvents();
            }
            while (State.Refresh);
        }

        /// <summary>
        /// Default action for the xforms-reset event.
        /// </summary>
        internal void OnReset()
        {

        }

        /// <summary>
        /// Invokes any outstanding deferred updates.
        /// </summary>
        internal void InvokeDeferredUpdates()
        {
            if (state.Value.Rebuild)
                OnRebuild();

            if (state.Value.Recalculate)
                OnRecalculate();

            if (state.Value.Revalidate)
                OnRevalidate();

            if (state.Value.Refresh)
                OnRefresh();
        }

        /// <summary>
        /// Loads the given schema into the model.
        /// </summary>
        /// <param name="uri"></param>
        XmlSchema LoadSchema(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            try
            {
                // normalize uri with base
                if (Element.GetBaseUri() != null && !uri.IsAbsoluteUri)
                    uri = new Uri(Element.GetBaseUri(), uri);
            }
            catch (UriFormatException e)
            {
                throw new DOMTargetEventException(Element, Events.LinkException, e);
            }

            // return resource as a stream
            var response = io.Send(new IORequest(uri, IOMethod.Get));
            if (response == null ||
                response.Status != IOStatus.Success)
                throw new DOMTargetEventException(Element, Events.LinkException, string.Format("Error retrieving schema '{0}'.", uri));

            // load the retrieved schema
            using (var rdr = XmlReader.Create(response.Content))
                return LoadSchema(rdr);
        }

        /// <summary>
        /// Loads the schema given by the specified <see cref="XElement"/>.
        /// </summary>
        /// <param name="element"></param>
        XmlSchema LoadSchema(XElement element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            if (element.Name != XName.Get("{http://www.w3.org/2001/XMLSchema}schema"))
                throw new ArgumentException("", nameof(element));

            using (var rdr = element.CreateReader())
                return LoadSchema(rdr);
        }

        /// <summary>
        /// Loads the schema given the specified <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="reader"></param>
        XmlSchema LoadSchema(XmlReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            // load instance
            var schema = XmlSchema.Read(reader, LoadSchema_ValidationEvent);
            return schema;
        }

        void LoadSchema_ValidationEvent(object sender, ValidationEventArgs args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));
        }

    }

}
