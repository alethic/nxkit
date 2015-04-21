using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.XPath;
using NXKit.DOMEvents;
using NXKit.Util;
using NXKit.Xml;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides a wrapper for a model item that controls access to the underlying model item properties.
    /// </summary>
    public class ModelItem
    {

        readonly static XmlQualifiedName XS_ANYTYPE = new XmlQualifiedName("anyType", XmlSchemaConstants.XMLSchema_NS);

        /// <summary>
        /// Gets the model item for the given <see cref="XObject"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ModelItem Get(XObject obj)
        {
            return obj.AnnotationOrCreate<ModelItem>(() => new ModelItem(obj));
        }


        readonly XObject xml;
        readonly Lazy<Model> model;
        readonly Lazy<Instance> instance;
        readonly Lazy<ModelItemState> state;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public ModelItem(XObject xml)
        {
            Contract.Requires<ArgumentNullException>(xml != null);

            this.xml = xml;
            this.model = new Lazy<Model>(() => xml.Document.Annotation<Model>());
            this.instance = new Lazy<Instance>(() => xml.Document.Annotation<Instance>());
            this.state = new Lazy<ModelItemState>(() => xml.AnnotationOrCreate<ModelItemState>());
        }

        /// <summary>
        /// Gets a reference to the underlying XML object.
        /// </summary>
        public XObject Xml
        {
            get { return xml; }
        }

        /// <summary>
        /// Gets the state structure for the given model item.
        /// </summary>
        /// <returns></returns>
        internal ModelItemState State
        {
            get { return state.Value; }
        }

        /// <summary>
        /// Gets the model element of the specified model item.
        /// </summary>
        /// <returns></returns>
        public Model Model
        {
            get { return model.Value; }
        }

        /// <summary>
        /// Gets the instance element of the specified model item.
        /// </summary>
        /// <returns></returns>
        public Instance Instance
        {
            get { return instance.Value; }
        }

        /// <summary>
        /// Gets or sets the type of the given model item.
        /// </summary>
        /// <returns></returns>
        public XName ItemType
        {
            get { return GetItemType(); }
            set { SetItemType(value); }
        }

        XName GetItemType()
        {
            return State.Type ?? GetXsiType() ?? GetXsdItemType() ?? NXKit.XmlSchemaConstants.XMLSchema + "string";
        }

        /// <summary>
        /// Sets the type of the given model item.
        /// </summary>
        /// <param name="type"></param>
        public void SetItemType(XName type)
        {
            State.Type = type;
        }

        /// <summary>
        /// Gets whether the given model item is required.
        /// </summary>
        /// <returns></returns>
        public bool Required
        {
            get { return GetRequired(); }
        }

        bool GetRequired()
        {
            return State.Required ?? false;
        }

        /// <summary>
        /// Gets whether the given model item is read-only.
        /// </summary>
        /// <returns></returns>
        public bool ReadOnly
        {
            get { return GetReadOnly(); }
        }

        bool GetReadOnly()
        {
            return xml.AncestorsAndSelf().Any(i => i.AnnotationOrCreate<ModelItemState>().ReadOnly ?? false);
        }

        /// <summary>
        /// Gets whether the given model item is relevant.
        /// </summary>
        /// <returns></returns>
        public bool Relevant
        {
            get { return GetRelevant(); }
        }

        bool GetRelevant()
        {
            return xml.AncestorsAndSelf().All(i => i.AnnotationOrCreate<ModelItemState>().Relevant ?? true);
        }

        /// <summary>
        /// Gets whether the given model item's constraint value is currently valid.
        /// </summary>
        /// <returns></returns>
        public bool Constraint
        {
            get { return GetConstraint(); }
        }

        bool GetConstraint()
        {
            return State.Constraint ?? true;
        }

        /// <summary>
        /// Gets whether the given model item is currently valid.
        /// </summary>
        /// <returns></returns>
        public bool Valid
        {
            get { return GetValid(); }
        }

        bool GetValid()
        {
            return State.Valid ?? true;
        }

        /// <summary>
        /// Gets or sets the value of the simple node.
        /// </summary>
        public string Value
        {
            get { return GetValue(); }
            set { Contract.Requires<ArgumentNullException>(value != null); SetValue(value); }
        }

        /// <summary>
        /// Implements the getter for Value.
        /// </summary>
        /// <returns></returns>
        string GetValue()
        {
            if (xml is XElement)
                return !((XElement)xml).HasElements ? ((XElement)xml).Value : null;
            else if (xml is XAttribute)
                return ((XAttribute)xml).Value;
            else
                throw new InvalidOperationException();
        }

        /// <summary>
        /// Converts the given value to the specified XSD type.
        /// </summary>
        /// <param name="xsdType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        string ConvertTo(XName xsdType, string value)
        {
            Contract.Requires<ArgumentNullException>(xsdType != null);
            Contract.Requires<ArgumentNullException>(value != null);

            // select compatible converter
            var converter = Instance.XsdTypeConverters
                .Where(i => i.CanConvertTo(xsdType))
                .FirstOrDefault();

            // convert if converter found
            if (converter != null)
                value = converter.ConvertTo(xsdType, value) ?? value;

            // return new or existing value
            return value;
        }

        /// <summary>
        /// Implements the setter for Value.
        /// </summary>
        /// <param name="newValue"></param>
        void SetValue(string newValue)
        {
            Contract.Requires<ArgumentNullException>(newValue != null);

            // convert value to destination item type if possible
            if (ItemType != null)
                newValue = ConvertTo(ItemType, newValue);

            // nothing changed
            if (newValue == GetValue())
                return;

            // This action has no effect if the Single Item Binding does not select an instance data node or if a 
            // readonly instance data node is selected.
            if (ReadOnly)
                return;

            if (Xml is XElement)
            {
                // An xforms-binding-exception occurs if the Single Item Binding indicates a node whose content is not
                // simpleContent (i.e., a node that has element children).
                var target = (XElement)Xml;
                if (target.HasElements)
                    throw new DOMTargetEventException(target, Events.BindingException);

                // find existing text node or create
                // preserves any existing annotations
                var text = target.Nodes().OfType<XText>().FirstOrDefault();
                if (text == null)
                {
                    text = new XText(newValue);
                    target.AddFirst(text);
                }

                // set new value
                text.Value = newValue;

                Debug.WriteLine("ModelItem value changed: {0}", Xml);
            }
            else if (Xml is XAttribute)
            {
                var target = (XAttribute)Xml;
                target.Value = newValue;
            }
            else if (Xml is XText)
            {
                var target = (XText)Xml;
                target.Value = newValue;
            }
            else
                throw new InvalidOperationException();

            // trigger recalculate event to collect new value
            Model.State.Recalculate = true;
            Model.State.Revalidate = true;
            Model.State.Refresh = true;
        }

        /// <summary>
        /// Gets the element value of the given model item.
        /// </summary>
        public XElement Contents
        {
            get { return GetContents(); }
            set { SetContents(value); }
        }

        /// <summary>
        /// Implements the getter for Contents.
        /// </summary>
        /// <returns></returns>
        XElement GetContents()
        {
            Contract.Requires<ArgumentException>(Xml is XElement);

            if (xml is XElement)
                return ((XElement)xml).HasElements ? (XElement)((XElement)xml).FirstNode : null;
            else
                throw new InvalidOperationException();
        }

        /// <summary>
        /// Implements the setter for Contents.
        /// </summary>
        /// <param name="newContents"></param>
        void SetContents(XElement newContents)
        {
            Contract.Requires<ArgumentException>(Xml is XElement);

            throw new NotImplementedException();

            // trigger recalculate event to collect new value
            //Model.State.RecalculateFlag = true;
            //Model.State.RevalidateFlag = true;
            //Model.State.RefreshFlag = true;
        }

        /// <summary>
        /// Replaces the referenced model item with a new object.
        /// </summary>
        /// <param name="newObject"></param>
        public void Replace(XObject newObject)
        {
            // i am either the document or the root node
            if (Xml is XDocument || Xml == Xml.Document.Root)
            {
                // new object is a document, replace the entire instance
                var document = newObject as XDocument;
                if (document != null)
                {
                    Instance.Load(document);
                    Model.State.Rebuild = true;
                    Model.State.Recalculate = true;
                    Model.State.Revalidate = true;
                    Model.State.Refresh = true;
                    return;
                }

                // new object is an element, replace entire instance with derived document
                var element = newObject as XElement;
                if (element != null)
                {
                    Instance.Load(new XDocument(element));
                    Model.State.Rebuild = true;
                    Model.State.Recalculate = true;
                    Model.State.Revalidate = true;
                    Model.State.Refresh = true;
                    return;
                }

                throw new InvalidOperationException();
            }
            else if (Xml is XElement)
            {
                // new object is a document, replace with root element
                var document = newObject as XDocument;
                if (document != null)
                {
                    Replace(document.Root);
                    return;
                }

                // new object is an element
                var element = newObject as XElement;
                if (element != null)
                {
                    Replace(element);
                    return;
                }

                // new object is text
                var text = newObject as XText;
                {
                    Replace(text);
                    return;
                }

                throw new InvalidOperationException();
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Replaces this element with the given element.
        /// </summary>
        /// <param name="element"></param>
        void Replace(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<InvalidOperationException>(Xml is XElement);

            ((XElement)Xml).ReplaceWith(element);
            Model.State.Rebuild = true;
            Model.State.Recalculate = true;
            Model.State.Revalidate = true;
            Model.State.Refresh = true;
        }

        /// <summary>
        /// Applies all of the known values from the given model item to the current one.
        /// </summary>
        /// <param name="item"></param>
        public void Apply(ModelItem item)
        {
            Contract.Requires<ArgumentNullException>(item != null);

            State.Type = item.State.Type;
            State.Relevant = item.State.Relevant;
            State.ReadOnly = item.State.ReadOnly;
            State.Required = item.State.Required;
            State.Constraint = item.State.Constraint;
            State.Valid = item.State.Valid;
        }

        /// <summary>
        /// Creates a <see cref="XPathNavigator"/> for the given model item.
        /// </summary>
        /// <returns></returns>
        internal XPathNavigator CreateNavigator()
        {
            var attr = xml as XAttribute;
            if (attr != null)
            {
                // navigator needs to be created on parent, and navigated to attribute
                var nav = attr.Parent.CreateNavigator();
                nav.MoveToAttribute(attr.Name.LocalName, attr.Name.NamespaceName);
                return nav;
            }

            var element = xml as XElement;
            if (element != null)
                return element.CreateNavigator();

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Validates the <see cref="ModelItem"/>.
        /// </summary>
        /// <returns></returns>
        internal void Validate()
        {
            State.Valid = GetValidate();
        }

        /// <summary>
        /// Executes the validation and returns the result.
        /// </summary>
        /// <returns></returns>
        bool GetValidate()
        {
            // field is required by binding
            if (Required)
                if (Value.TrimToNull() == null)
                    return false;

            // contraint is invalid
            if (!Constraint)
                return false;

            // schema validation failed
            var schemaInfo = GetSchemaInfo();
            if (schemaInfo != null)
                if (schemaInfo.SchemaType != null)
                    if (schemaInfo.Validity == XmlSchemaValidity.Invalid)
                        return false;

            // validate against expressed item type
            var schemaType = Model.State.XmlSchemas.GlobalTypes[new XmlQualifiedName(ItemType.LocalName, ItemType.NamespaceName)] as XmlSchemaType;
            if (schemaType != null)
                if (!ValidateValueAgainstXmlSchemaType(schemaType, Value, Model.State.XmlSchemas.NameTable, null))
                    return false;

            // otherwise true
            return true;
        }

        /// <summary>
        /// Attempts to validate a value against the given schema type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="nt"></param>
        /// <param name="resolver"></param>
        /// <returns></returns>
        bool ValidateValueAgainstXmlSchemaType(XmlSchemaType type, string value, XmlNameTable nt, IXmlNamespaceResolver resolver)
        {
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Requires<ArgumentNullException>(value != null);
            Contract.Requires<ArgumentNullException>(nt != null);

            try
            {
                type.Datatype.ParseValue(value, nt, resolver);
                return true;
            }
            catch (XmlSchemaValidationException)
            {
                // ignore
            }

            return false;
        }

        /// <summary>
        /// Gets the type associated with the model item by 'xsi:type'.
        /// </summary>
        /// <returns></returns>
        XName GetXsiType()
        {
            var element = xml as XElement;
            if (element == null)
                return null;

            var value = (string)element.Attribute("{http://www.w3.org/2001/XMLSchema-instance}type");
            if (value != null)
                return element.ResolvePrefixedName(value);

            return null;
        }

        /// <summary>
        /// Gets the <see cref="XmlSchemaType"/> associated with the model item by XSD.
        /// </summary>
        /// <returns></returns>
        XmlSchemaType GetXsdSchemaType()
        {
            var schemaInfo = GetSchemaInfo();
            if (schemaInfo == null)
                return null;

            return schemaInfo.SchemaType;
        }

        /// <summary>
        /// Gets the first named <see cref="XmlSchemaType"/> associated with the model item by XSD.
        /// </summary>
        /// <returns></returns>
        XmlSchemaType GetXsdNamedSchemaType()
        {
            var schemaType = GetXsdSchemaType();
            if (schemaType == null)
                return null;

            return schemaType.Recurse(i => i.BaseXmlSchemaType).FirstOrDefault(i => !i.QualifiedName.IsEmpty);
        }

        /// <summary>
        /// Gets the applied XML schema type.
        /// </summary>
        /// <returns></returns>
        XName GetXsdType()
        {
            var schemaType = GetXsdNamedSchemaType();
            if (schemaType == null)
                return null;

            return XName.Get(schemaType.QualifiedName.Name, schemaType.QualifiedName.Namespace);
        }

        /// <summary>
        /// Gets the model item type from the XML schema. This is the first base type of the schema type supported by 
        /// the native XForms type system. Ultimately, richer type information should be exposed out of model-items and
        /// be made available to the client.
        /// </summary>
        /// <returns></returns>
        XName GetXsdItemType()
        {
            var schemaType = GetXsdSchemaType();
            if (schemaType == null)
                return null;

            return schemaType
                .Recurse(i => i.BaseXmlSchemaType)
                .Where(i => !i.QualifiedName.IsEmpty)
                .Where(i => i.QualifiedName != XS_ANYTYPE)
                .Where(i => i.QualifiedName.Namespace == XmlSchemaConstants.XMLSchema_NS)
                .Select(i => XName.Get(i.QualifiedName.Name, i.QualifiedName.Namespace))
                .FirstOrDefault();
        }

        /// <summary>
        /// Gets the <see cref="IXmlSchemaInfo"/> associated with the <see cref="ModelItem"/>.
        /// </summary>
        /// <returns></returns>
        IXmlSchemaInfo GetSchemaInfo()
        {
            if (xml is XAttribute)
                return ((XAttribute)xml).GetSchemaInfo();
            if (xml is XElement)
                return ((XElement)xml).GetSchemaInfo();

            return null;
        }

    }

}
