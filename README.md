NXKit
==========

NXKit forms the basis of an XML processing engine that can be used to implement other XML based models, such as XForms, or theoritically a full web browser user agent.

NXKit is simple to use. Just create a project, ensure extension assemblies are referenced, and load some XML into the `Document` class.

    public static void Main(string[] args)
    {
        var nx = Document.Load(@"<root />");
    }

The `Document` instance loads your document into an internal XLinq instance and provides extensions on elements. These extensions can come from various plugin assemblies, such as `NXKit.XForms`.
Extensions implement various interfaces. For example, `Input` from the NXKit.XForms namespace.


NXKit.DOMEvents
----------

Implements W3C DOM Events. Elements can have events dispatched to them using the `IEventTarget` interface.

    document.Xml.Element("my-element").Interface<IEventTarget>().DispatchEvent(UIEvents.DOMActivate);


NXKit.XMLEvents
----------

Implements W3C XML Events. This provides a number of elements and attributes for listening to DOM Events.


NXKit.XForms
----------

NXKit.XForms is an implementation of XForms in the NXKit processing model.

The implementation currently covers most features from XForms 1.1, and some features from XForms 2.0.


NXKit.XForms.Layout
----------

NXKit.XForms.Layout contains a basic layout module supporting pages and sections.


NXKit.View.Server
----------

Provides a server side object model for assisting in running a NXKit Document on a server that communicates with a remote (client-based) user interface.


NXKit.View.Js
----------

Base implementation of the NXKit View in JavaScript using Knockout.


NXKit.View.Web.UI
----------

ASP.Net WebForms starting point for NXKit. Contains a View control that you can add to the page, which imports all the neccessary client side objects and script.


NXKit.Scripting.EcmaScript
----------

Support for ECMAScript using the Jurassic JavaScript implementation.
