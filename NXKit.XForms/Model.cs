using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

using NXKit.Composition;
using NXKit.DOMEvents;
using NXKit.IO;
using NXKit.Util;
using NXKit.Xml;

namespace NXKit.XForms
{

    [Extension("{http://www.w3.org/2002/xforms}model")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class Model :
        ElementExtension,
        IEventDefaultAction
    {

        readonly IIOService io;
        readonly ModelAttributes attributes;
        readonly Lazy<ModelState> state;
        readonly Lazy<DocumentAnnotation> documentAnnotation;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="io"></param>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        [ImportingConstructor]
        public Model(
            IIOService io,
            XElement element,
            ModelAttributes attributes)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.io = io ?? throw new ArgumentNullException(nameof(io));
            this.attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
            this.state = new Lazy<ModelState>(() => Element.AnnotationOrCreate<ModelState>());
            this.documentAnnotation = new Lazy<DocumentAnnotation>(() => Element.Document.AnnotationOrCreate<DocumentAnnotation>());
        }

        /// <summary>
        /// Gets the model state associated with this model interface.
        /// </summary>
        public ModelState State
        {
            get { return state.Value; }
        }

        DocumentAnnotation DocumentAnnotation
        {
            get { return documentAnnotation.Value; }
        }

        /// <summary>
        /// Provides the model default evaluation context.
        /// </summary>
        public EvaluationContext Context
        {
            get { return DefaultEvaluationContext; }
        }

        /// <summary>
        /// Gets the set of <see cref="Instance"/>s.
        /// </summary>
        public IEnumerable<Instance> Instances
        {
            get { return Element.Elements(Constants.XForms_1_0 + "instance").SelectMany(i => i.Interfaces<Instance>()); }
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

                return new EvaluationContext(this, defaultInstance, ModelItem.Get(defaultInstance.State.Document.Root), 1, 1);
            }
        }

        /// <summary>
        /// Gets all implementations of the given extension type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetAllExtensions<T>()
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

            // attempt to load XML schemas refered to by the Schema attribute
            if (attributes.Schema != null)
                foreach (var item in attributes.Schema.Split(' ').Select(i => i.Trim()).Where(i => !string.IsNullOrEmpty(i)))
                    LoadSchema(new Uri(item, UriKind.RelativeOrAbsolute));

            // attempt to load XML schemas listed directly under the model
            foreach (var element in Element.Elements("{http://www.w3.org/2001/XMLSchema}schema"))
            {
                LoadSchema(element);

                // remove node from tree
                element.Remove();
            }

            // attempt to load model instance data
            foreach (var instance in Instances)
                instance.Load();

            OnRebuild();
            OnRecalculate();
            OnRevalidate();
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
        void LoadSchema(Uri uri)
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
                throw new DOMTargetEventException(Element, Events.LinkException,
                    string.Format("Error retrieving schema '{0}'.", uri));

            // load the retrieved schema
            using (var rdr = XmlReader.Create(response.Content))
                LoadSchema(rdr);
        }

        /// <summary>
        /// Loads the schema given by the specified <see cref="XElement"/>.
        /// </summary>
        /// <param name="element"></param>
        void LoadSchema(XElement element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            if (element.Name != XName.Get("{http://www.w3.org/2001/XMLSchema}schema"))
                throw new ArgumentException("", nameof(element));

            using (var rdr = element.CreateReader())
                LoadSchema(rdr);
        }

        /// <summary>
        /// Loads the schema given the specified <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="reader"></param>
        void LoadSchema(XmlReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            // load instance
            var schema = XmlSchema.Read(reader, LoadSchema_ValidationEvent);
            if (schema != null)
                State.XmlSchemas.Add(schema);
        }

        void LoadSchema_ValidationEvent(object sender, ValidationEventArgs args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));
        }

    }

}
