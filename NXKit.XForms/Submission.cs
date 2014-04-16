using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

using NXKit.DOMEvents;
using NXKit.Xml;

namespace NXKit.XForms
{

    [Interface("{http://www.w3.org/2002/xforms}submission")]
    public class Submission :
        ElementExtension,
        IEventDefaultActionHandler
    {

        /// <summary>
        /// Stores the validity on the transformed element tree.
        /// </summary>
        class ValidityAnnotation
        {

            readonly bool valid;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="valid"></param>
            public ValidityAnnotation(bool valid)
            {
                this.valid = valid;
            }

            public bool Valid
            {
                get { return valid; }
            }

        }

        class ValidityVisitor :
            XVisitor
        {

            bool isValid = true;

            /// <summary>
            /// Visits the node until an invalid node is found.
            /// </summary>
            /// <param name="node"></param>
            /// <returns></returns>
            public new bool Visit(XNode node)
            {
                base.Visit(node);

                return isValid;
            }

            public override void Visit(XObject obj)
            {
                isValid &= obj.Annotation<ValidityAnnotation>().Valid;
                if (!isValid)
                    return;

                base.Visit(obj);
            }

        }

        /// <summary>
        /// Visits each node in a document and removes those which are not relevant if specified.
        /// </summary>
        class RelevantTransformer :
            XTransformer
        {

            readonly bool exclude;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="exclude"></param>
            public RelevantTransformer(bool exclude)
            {
                this.exclude = exclude;
            }

            public override XObject Visit(XObject obj)
            {
                var modelItem = obj.AnnotationOrCreate<ModelItem>(() => new ModelItem(obj));
                if (modelItem == null ||
                    modelItem.Relevant ||
                    exclude == false /* exclude overrides */)
                {
                    // visit node
                    var o = base.Visit(obj);

                    // attach validity to annotation for usage later
                    if (modelItem != null)
                        o.AddAnnotation(new ValidityAnnotation(modelItem.Valid));

                    // return new object
                    return o;
                }
                else
                    return null;
            }

        }

        readonly SubmissionProperties properties;
        readonly Lazy<EvaluationContextResolver> context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Submission(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.properties = new SubmissionProperties(element);
            this.context = new Lazy<EvaluationContextResolver>(() => element.Interface<EvaluationContextResolver>());
        }

        void IEventDefaultActionHandler.DefaultAction(Event evt)
        {
            switch (evt.Type)
            {
                case Events.Submit:
                    OnSubmit();
                    break;
            }
        }

        void OnSubmit()
        {
            // The data model is updated based on some of the flags defined for deferred updates. Specifically, if the
            // deferred update rebuild flag is set for the model containing this submission, then the rebuild operation 
            // is performed without dispatching an event to invoke the operation. Then, if the deferred update 
            // recalculate flag is set for the model containing this submission, then the recalculate operation is
            // performed without dispatching an event to invoke the operation. This sequence of operations affects the 
            // deferred update behavior by clearing the deferred update flags associated with the operations performed.
            var model = Element.Ancestors(Constants.XForms_1_0 + "model").First().Interface<Model>();
            if (model.State.Rebuild)
                model.OnRebuild();
            if (model.State.Recalculate)
                model.OnRecalculate();

            // If the binding attributes of submission indicate an empty sequence or an item other than an element or 
            // an instance document root node, then submission fails with no-data. Otherwise, the binding attributes of 
            // submission indicate a node of instance data.
            var modelItems = new Binding(Element, context.Value.Context, properties.Ref).ModelItems;
            if (modelItems == null ||
                modelItems.Length != 1 ||
                modelItems.Any(i => i.Xml.NodeType != XmlNodeType.Document && i.Xml.NodeType != XmlNodeType.Element))
                throw new DOMTargetEventException(Element, Events.SubmitError, new SubmitErrorContextInfo(
                    SubmitErrorErrorType.NoData
                ));

            // The indicated node and all nodes for which it is an ancestor are selected. If the attribute relevant is
            // true, whether by default or declaration, then any selected node which is not relevant as defined in The
            // relevant Property is deselected (pruned). If all instance nodes are deselected, then submission fails
            // with no-data.
            var node = (XNode)new RelevantTransformer(properties.Relevant)
                .Visit((XNode)modelItems[0].Xml);
            if (node == null)
                throw new DOMTargetEventException(Element, Events.SubmitError, new SubmitErrorContextInfo(
                    SubmitErrorErrorType.NoData
                ));

            // If the attribute validate is true, whether by default or declaration, then all selected instance data
            // nodes are checked for validity according to the definition in The xforms-revalidate Event (no
            // notification events are marked for dispatching due to this operation). If any selected instance data
            // node is found to be invalid, submission fails with validation-error.
            if (properties.Validate &&
                new ValidityVisitor().Visit(node) == false)
                throw new DOMTargetEventException(Element, Events.SubmitError, new SubmitErrorContextInfo(
                    SubmitErrorErrorType.ValidationError
                ));

            // The submission method is determined.
            // The submission method may be specified by the method attribute. The submission element can have a child
            // element named method, which overrides the submission method setting obtained from the method attribute
            // if both are specified. If more than one method element is given, the first occurrence in document order 
            // must be selected for use. Individually, the method element and the method attribute are not required.
            // However, one of the two is mandatory as there is no default submission method.
            var method = GetMethod();
            if (string.IsNullOrEmpty(method))
                throw new DOMTargetEventException(Element, Events.SubmitError);

            // The resource element provides the submission URI, overriding the resource attribute and the action 
            // attribute. If a submission has more than one resource child element, the first resource element child 
            // must be selected for use. Individually, the resource element, the resource attribute and the action 
            // attribute are not required. However, one of the three is mandatory as there is no default submission 
            // resource.
            var resource = GetResource();
            if (resource == null)
                throw new DOMTargetEventException(Element, Events.SubmitError, new SubmitErrorContextInfo(
                    SubmitErrorErrorType.ResourceError));

            // If the serialization attribute value is "none", then the submission data serialization is the empty
            // string. Otherwise, the event xforms-submit-serialize is dispatched; if the submission-body property
            // of the event is changed from the initial value of empty string, then the content of the submission-body
            // property string is used as the submission data serialization. Otherwise, the submission data
            // serialization consists of a serialization of the selected instance data according to the rules stated
            // in Serialization.
            if (properties.Serialization.None)
                node = null;
            else
            {
                var evt = Element.Interface<INXEventTarget>().DispatchEvent(Events.SubmitSerialize, new SubmitSerializeContextInfo());
                var ctx = evt.Context as SubmitSerializeContextInfo;
                if (ctx != null &&
                    ctx.SubmissionBody != "")
                    // wrap in XText
                    node = new XText(ctx.SubmissionBody);
            }

            // The submission is performed based on the submission headers, submission method, submission resource, and
            // submission data serialization. The exact rules of submission are based on the URI scheme and the 
            // submission method, as defined in Submission Options.
            var request = new SubmissionRequest(
                resource,
                method,
                properties.Serialization,
                properties.MediaType,
                node,
                properties.Encoding,
                GetHeaders());

            // obtain the handler capable of dealing with the submission
            var handler = GetHandlers()
                .FirstOrDefault(i => i.CanSubmit(request));
            if (handler == null)
                throw new DOMTargetEventException(Element, Events.SubmitError, new SubmitErrorContextInfo(
                    SubmitErrorErrorType.ResourceError));

            // submit and check for response
            var response = handler.Submit(request);
            if (response == null)
                throw new DOMTargetEventException(Element, Events.SubmitError, new SubmitErrorContextInfo(
                    SubmitErrorErrorType.ResourceError));

            // For error responses, processing depends on the value of the replace attribute on element submission:
            // all: either the document is replaced with an implementation-specific indication of an error or submission fails with resource-error.
            // any other value: nothing in the document is replaced, and submission fails with resource-error.
            if (response.Status == SubmissionStatus.Error)
                throw new DOMTargetEventException(Element, Events.SubmitError, new SubmitErrorContextInfo(
                    SubmitErrorErrorType.ResourceError));

            // handle result based on 'replace' property
            switch (properties.Replace)
            {
                // none: submission succeeds.
                case SubmissionReplace.None:
                    Element.DispatchEvent(Events.SubmitDone);
                    break;

                // all: the event xforms-submit-done may be dispatched with appropriate context information, and submit
                // processing concludes with the entire containing document being replaced with the returned body.
                case SubmissionReplace.All:
                    throw new NotImplementedException();

                // instance: If the body is not of type accepted by the processor, as specified in Creating instance data
                // from external resources, nothing in the document is replaced and submission fails with resource-error. 
                // Otherwise the body is parsed to give an XPath Data Model according to Creating instance data from
                // external resources. If the parse fails, then submission fails with parse-error. If the parse succeeds,
                // then instance data replacement is performed according to Replacing Data with the Submission Response.
                // If this operation fails, submission fails with target-error. Otherwise, submission succeeds.
                case SubmissionReplace.Instance:
                    throw new NotImplementedException();

                // text: If the body is neither an XML media type (i.e. with a content type not matching any of the
                // specifiers in [RFC 3023]) nor a text type (i.e. with a content type not matching text/*), nothing in the
                // document is replaced and submission fails with resource-error. Otherwise the content replacement is
                // performed according to Replacing Data with the Submission Response. If this operation fails, then the
                // submission fails with target-error. Otherwise, submission succeeds.
                case SubmissionReplace.Text:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// The submission method may be specified by the method attribute. The submission element can have a child
        /// element named method, which overrides the submission method setting obtained from the method attribute if 
        /// both are specified. If more than one method element is given, the first occurrence in document order must 
        /// be selected for use. Individually, the method element and the method attribute are not required. However, 
        /// one of the two is mandatory as there is no default submission method.
        /// </summary>
        /// <returns></returns>
        string GetMethod()
        {
            var method = Element.Element(Constants.XForms_1_0 + "method");
            if (method != null)
                return method.Interface<Method>().GetValue();

            if (properties.Method != null)
                return properties.Method;

            return null;
        }

        /// <summary>
        /// The submission resource is the URI for the submission. It is of type xsd:anyURI.
        /// 
        /// In XForms 1.0, the URI for submission was provided by the action attribute. For consistency, form authors
        /// should now use the attribute resource of type xsd:anyURI, which deprecates the action attribute. If both
        /// action and resource are present, then the resource attribute takes precedence.
        /// 
        /// The resource element provides the submission URI, overriding the resource attribute and the action
        /// attribute. If a submission has more than one resource child element, the first resource element child must
        /// be selected for use. Individually, the resource element, the resource attribute and the action attribute
        /// are not required. However, one of the three is mandatory as there is no default submission resource.
        /// </summary>
        /// <returns></returns>
        Uri GetResource()
        {
            var uri = GetResourceUris()
                .FirstOrDefault();
            if (uri == null)
                throw new DOMTargetEventException(Element, Events.SubmitError,
                    new SubmitErrorContextInfo(SubmitErrorErrorType.ResourceError));

            return uri;
        }

        /// <summary>
        /// Returns the resource IDs to be used in order of priority.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Uri> GetResourceUris()
        {
            var resource = Element.Element(Constants.XForms_1_0 + "resource");
            if (resource != null)
            {
                var uri = resource.Interface<Resource>().Uri;
                if (uri != null)
                    yield return uri;
            }

            if (properties.Resource != null)
                yield return properties.Resource;

            if (properties.Action != null)
                yield return properties.Action;
        }

        /// <summary>
        /// The submission headers are determined using the header entries produced by the header element(s) in the
        /// submission and the mediatype attribute or its default.
        /// </summary>
        /// <returns></returns>
        SubmissionHeaders GetHeaders()
        {
            return new SubmissionHeaders();
        }


        /// <summary>
        /// Gets the set of available submission handlers.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ISubmissionHandler> GetHandlers()
        {
            return Element.Host().Container.GetExportedValues<ISubmissionHandler>();
        }

    }

}
