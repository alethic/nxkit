using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.XPath;

using NXKit.Diagnostics;
using NXKit.DOMEvents;
using NXKit.Util;
using NXKit.XForms.Xml;
using NXKit.Xml;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides a wrapper for a model item that controls access to the underlying model item properties.
    /// </summary>
    public class ModelItem
    {

        static readonly XmlQualifiedName XS_ANYTYPE = new XmlQualifiedName("anyType", XmlSchemaConstants.XMLSchema_NS);

        /// <summary>
        /// Gets the model item for the given <see cref="XObject"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ModelItem Get(XObject obj, ITraceService trace)
        {
            return obj.AnnotationOrCreate(() => new ModelItem(obj, trace));
        }


        readonly XObject xml;
        readonly ITraceService trace;
        readonly Lazy<Model> model;
        readonly Lazy<Instance> instance;
        readonly Lazy<ModelItemState> state;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="trace"></param>
        public ModelItem(XObject xml, ITraceService trace)
        {
            this.xml = xml ?? throw new ArgumentNullException(nameof(xml));
            this.trace = trace ?? throw new ArgumentNullException(nameof(trace));

            model = new Lazy<Model>(() => xml.Document.Annotation<Model>());
            instance = new Lazy<Instance>(() => xml.Document.Annotation<Instance>());
            state = new Lazy<ModelItemState>(() => xml.AnnotationOrCreate<ModelItemState>());
        }

        /// <summary>
        /// Returns a string representation of the instance.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Xml is XElement)
                return ((XElement)xml).Name.ToString();
            if (Xml is XDocument)
                return "Document";
            if (Xml is XAttribute)
                return ((XAttribute)xml).Name.ToString();
            if (Xml is XText)
                return Get(Xml.Parent, trace).ToString();

            return Xml.GetObjectId().ToString();
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
            internal set { SetItemType(value); }
        }

        /// <summary>
        /// Getter for the ItemType property.
        /// </summary>
        /// <returns></returns>
        XName GetItemType()
        {
            // apply model item state if not empty element
            var type = Xml is XElement e && e.HasElements ? null : State.Type;

            // derive item type from XSI attribute or associated schema
            if (type == null)
                type = GetXsiType() ?? GetXsdItemType();

            // do not fallback to string for element with content
            if (type == null && (Xml as XElement)?.HasElements != true)
                type = NXKit.XmlSchemaConstants.XMLSchema + "string";

            return type;
        }

        /// <summary>
        /// Setter for the ItemType property.
        /// </summary>
        /// <param name="type"></param>
        void SetItemType(XName type)
        {
            State.Type = type;
            trace.Debug("ModelItem item type set: {0}: {1}", this, type);
        }

        /// <summary>
        /// Gets whether the given model item is required.
        /// </summary>
        /// <returns></returns>
        public bool Required
        {
            get { return GetRequired(); }
            internal set { SetRequired(value); }
        }

        /// <summary>
        /// Getter for the Required property.
        /// </summary>
        /// <returns></returns>
        bool GetRequired()
        {
            return State.Required ?? false;
        }

        /// <summary>
        /// Setter for the Required property.
        /// </summary>
        /// <param name="value"></param>
        void SetRequired(bool value)
        {
            State.Required = value;
            trace.Debug("ModelItem required set: {0}: {1}", this, value);
        }

        /// <summary>
        /// Gets whether the given model item is read-only.
        /// </summary>
        /// <returns></returns>
        public bool ReadOnly
        {
            get { return GetReadOnly(); }
            internal set { SetReadOnly(value); }
        }

        /// <summary>
        /// Getter for the ReadOnly property.
        /// </summary>
        /// <returns></returns>
        bool GetReadOnly()
        {
            return xml.AncestorsAndSelf().Any(i => i.AnnotationOrCreate<ModelItemState>().ReadOnly ?? false);
        }

        /// <summary>
        /// Setter for the ReadOnly property.
        /// </summary>
        /// <param name="value"></param>
        void SetReadOnly(bool value)
        {
            State.ReadOnly = value;
            trace.Debug("ModelItem readonly set: {0}: {1}", this, value);
        }

        /// <summary>
        /// Gets whether the given model item is relevant.
        /// </summary>
        /// <returns></returns>
        public bool Relevant
        {
            get { return GetRelevant(); }
            internal set { State.Relevant = value; }
        }

        /// <summary>
        /// Getter for the Relevant property.
        /// </summary>
        /// <returns></returns>
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
            internal set { State.Constraint = value; }
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
            internal set { SetValid(value); }
        }

        /// <summary>
        /// Getter for the Valid property.
        /// </summary>
        /// <returns></returns>
        bool GetValid()
        {
            return State.Valid ?? true;
        }

        /// <summary>
        /// Setter for the Valid property.
        /// </summary>
        /// <param name="value"></param>
        void SetValid(bool value)
        {
            State.Valid = value;
        }

        /// <summary>
        /// Gets or sets the value of the simple node.
        /// </summary>
        public string Value
        {
            get { return GetValue(); }
            set { if (value == null) throw new ArgumentNullException(nameof(value)); SetValue(value); }
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
            if (xsdType == null)
                throw new ArgumentNullException(nameof(xsdType));
            if (value == null)
                throw new ArgumentNullException(nameof(value));

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
            if (newValue == null)
                throw new ArgumentNullException(nameof(newValue));

            // convert value to destination item type if possible
            if (ItemType != null)
            {
                if (GetXsdSchemaType() is XmlSchemaSimpleType schemaType)
                {
                    try
                    {
                        var value = schemaType.Datatype.ParseValue(newValue, Model.State.XmlSchemas.NameTable, null);
                        if (value != null)
                            newValue = (string)schemaType.Datatype.ChangeType(value, typeof(string));
                    }
                    catch (XmlSchemaException)
                    {
                        // ignore
                    }
                }
                else
                    newValue = ConvertTo(ItemType, newValue);
            }

            // some sort of conversion error occurred
            if (newValue == null)
                return;

            // nothing changed
            if (newValue == GetValue())
                return;
            // An xforms-binding-exception occurs if the Single Item Binding indicates a node whose content is not
            // simpleContent (i.e., a node that has element children).

            if (Xml is XElement element)
            {
                if (element.HasElements)
                    throw new DOMTargetEventException(element, Events.BindingException);

                // find existing text node or create
                // preserves any existing annotations
                var text = element.Nodes().OfType<XText>().FirstOrDefault();
                if (text == null)
                {
                    text = new XText(newValue);
                    element.AddFirst(text);
                }

                // set new value
                text.Value = newValue;

                trace.Debug("ModelItem simple content changed: {0}: '{1}'", this, text.Value);
            }
            else if (Xml is XAttribute attribute)
            {
                attribute.Value = newValue;

                trace.Debug("ModelItem attribute value changed: {0}: '{1}'", this, attribute.Value);
            }
            else if (Xml is XText text)
            {
                text.Value = newValue;

                trace.Debug("ModelItem text value changed: {0}: '{1}'", this, text.Value);
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
                if (newObject is XDocument document)
                {
                    Instance.Load(document);
                    Model.State.Rebuild = true;
                    Model.State.Recalculate = true;
                    Model.State.Revalidate = true;
                    Model.State.Refresh = true;
                    return;
                }

                // new object is an element, replace entire instance with derived document
                if (newObject is XElement element)
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
                if (newObject is XDocument document)
                {
                    Replace(document.Root);
                    return;
                }

                // new object is an element
                if (newObject is XElement element)
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
            if (element == null)
                throw new ArgumentNullException(nameof(element));

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
            if (item == null)
                throw new ArgumentNullException(nameof(item));

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
            if (xml is XAttribute attribute)
            {
                // navigator needs to be created on parent, and navigated to attribute
                var nav = attribute.Parent.CreateNavigator();
                nav.MoveToAttribute(attribute.Name.LocalName, attribute.Name.NamespaceName);
                return nav;
            }

            if (xml is XElement element)
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
            var itemType = ItemType;
            if (itemType != null)
                if (Model.State.XmlSchemas.GetSchemaType(itemType) is XmlSchemaType schemaType)
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
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (nt == null)
                throw new ArgumentNullException(nameof(nt));

            try
            {
                type.Datatype.ParseValue(value, nt, resolver);
                return true;
            }
            catch (XmlSchemaValidationException)
            {
                // ignore
            }
            catch (XmlSchemaException)
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
            if (xml is XElement element)
            {
                var value = (string)element.Attribute("{http://www.w3.org/2001/XMLSchema-instance}type");
                if (value != null)
                    return element.ResolvePrefixedName(value);
            }

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
