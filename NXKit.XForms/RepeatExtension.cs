using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.DOMEvents;
using NXKit.Xml;

namespace NXKit.XForms
{

    [Extension(typeof(Predicate))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    [Remote]
    public class RepeatExtension :
        ElementExtension
    //IOnInit,
    //IOnRefresh
    {

        public class Predicate :
            IExtensionPredicate
        {

            public bool IsMatch(XObject obj)
            {
                var element = obj as XElement;
                if (element != null &&
                    element.Name.Namespace != Constants.XForms_1_0 &&
                    element.AnnotationOrCreate(() => new RepeatExtensionAttributes(element)).HasAttributes())
                    return true;

                return false;
            }

        }

        readonly Lazy<RepeatExtensionProperties> properties;
        readonly RepeatState state;

        IEventListener listener;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="context"></param>
        [ImportingConstructor]
        public RepeatExtension(XElement element, Lazy<EvaluationContextResolver> context)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(context != null);

            this.properties = new Lazy<RepeatExtensionProperties>(() => 
                element.AnnotationOrCreate(() => 
                    new RepeatExtensionProperties(element, context.Value.Context)));
            this.state = element.AnnotationOrCreate<RepeatState>();
        }

        /// <summary>
        /// Gets or sets the current repeat index.
        /// </summary>
        [Remote]
        public int Index
        {
            get { return state.Index; }
            set { state.Index = value; }
        }

        public void Init()
        {
            // acquire template
            state.Template = new XElement(
                Constants.XForms_1_0 + "template",
                Element.GetNamespacePrefixAttributes(),
                Element.Nodes());
            Element.RemoveNodes();
        }

        //Binding GetRepeatBinding()
        //{

        //}

        ///// <summary>
        ///// Dynamically generate repeat items, reusing existing instances if available.
        ///// </summary>
        ///// <returns></returns>
        //public void Update()
        //{
        //        if (listener == null)
        //        {
        //            var target = Binding.Context.Instance.Element.Parent.Interface<EventTarget>();

        //            // find existing listener
        //            listener =
        //                InterfaceEventListener.GetListener(target, Events.Insert, true, Update) ??
        //                InterfaceEventListener.GetListener(target, Events.Delete, true, Update);

        //            // register listener
        //            if (listener == null)
        //            {
        //                listener = InterfaceEventListener.Create(Update);
        //                target.Register(Events.Insert, listener, true);
        //                target.Register(Events.Delete, listener, true);
        //            }
        //        }

        //    // store current index item
        //    var indexPrev = Index;
        //    var indexItem = Element
        //        .Elements()
        //        .FirstOrDefault(i => i.AnnotationOrCreate<RepeatItemState>().Index == Index);

        //    // build new list of properly ordered nodes
        //    var items = Binding != null ? Binding.ModelItems.Select(i => i.Xml).ToArray() : new XObject[0];
        //    var nodes = Element.Elements().ToArray();
        //    var sorts = new XElement[items.Length];
        //    for (int index = 0; index < items.Length; index++)
        //    {
        //        // model item at current index
        //        var item = items[index];

        //        // get existing item or create new
        //        var indx = Array.FindIndex(nodes, i => i.AnnotationOrCreate<RepeatItemState>().ModelObjectId == item.GetObjectId());
        //        var node = indx >= 0 ? nodes[indx] : null;
        //        if (node == null)
        //            node = new XElement(
        //                Constants.XForms_1_0 + "group",
        //                Template.GetNamespacePrefixAttributes(),
        //                Template.Nodes());

        //        // set node into output list
        //        sorts[index] = node;

        //        // configure item state
        //        var anno = node.AnnotationOrCreate<RepeatItemState>();
        //        anno.ModelObjectId = item.GetObjectId();
        //        anno.Index = index + 1;
        //        anno.Size = items.Length;
        //    }

        //    // new sequence is different from old sequence
        //    if (sorts.Length != nodes.Length ||
        //        sorts.SequenceEqual(nodes) == false)
        //    {
        //        // replace all children
        //        Element.RemoveNodes();
        //        Element.Add(sorts);

        //        // set of elements that were added
        //        var added = sorts
        //            .Except(nodes)
        //            .ToArray();

        //        // model-construct-done sequence applied to new children
        //        foreach (var node in added)
        //            foreach (var i in GetAllExtensions<IOnRefresh>(node))
        //                i.RefreshBinding();

        //        // discard refresh events
        //        foreach (var node in added)
        //            foreach (var i in GetAllExtensions<IOnRefresh>(node))
        //                i.DiscardEvents();

        //        // final refresh
        //        foreach (var node in added)
        //            foreach (var i in GetAllExtensions<IOnRefresh>(node))
        //                i.Refresh();
        //    }

        //    // restore or reset index
        //    var length = Element.Elements().Count();
        //    if (indexItem != null &&
        //        indexItem.Parent != null)
        //        Index = indexItem.AnnotationOrCreate<RepeatItemState>().Index;
        //    else if (indexPrev > 0)
        //        Index = indexPrev <= length ? indexPrev : length;
        //    else if (length > 0)
        //        Index = 1;
        //    else
        //        Index = 0;
        //}

        ///// <summary>
        ///// Gets all implementations of the given extension type.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <returns></returns>
        //IEnumerable<T> GetAllExtensions<T>(XElement root)
        //{
        //    Contract.Requires<ArgumentNullException>(root != null);

        //    return root
        //        .DescendantNodesAndSelf()
        //        .SelectMany(i => i.Interfaces<T>());
        //}

        ///// <summary>
        ///// Gets the <see cref="EvaluationContext"/> for a specific item.
        ///// </summary>
        ///// <param name="element"></param>
        ///// <returns></returns>
        //internal EvaluationContext GetItemContext(XElement element)
        //{
        //    var item = element.Annotation<RepeatItemState>();
        //    if (item == null)
        //        throw new InvalidOperationException();

        //    if (Binding == null ||
        //        Binding.ModelItems.Length == 0)
        //        return null;

        //    var xml = Binding.ModelItem.Instance.State.Document.ResolveObjectId(item.ModelObjectId);
        //    if (xml == null)
        //    {
        //        var d = NXKit.Serialization.XNodeAnnotationSerializer.Serialize(Binding.ModelItem.Instance.State.Document);
        //        var s = d.ToString();
        //        var l = Binding.ModelItem.Instance.State.Document.ToString();
        //        throw new InvalidOperationException();
        //    }

        //    return new EvaluationContext(
        //        ModelItem.Get(xml),
        //        item.Index,
        //        item.Size);
        //}

        //void IOnRefresh.RefreshBinding()
        //{

        //}

        //void IOnRefresh.Refresh()
        //{
        //    Update();
        //}

        //void IOnRefresh.DispatchEvents()
        //{

        //}

        //void IOnRefresh.DiscardEvents()
        //{

        //}


    }

}
