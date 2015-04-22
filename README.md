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

NXKit `Document` instances fully support serializing and deserializing a running instance through the usage of the `Save` method. The output of the serialization process is a copy of the current running `Document` instance with all the included runtime state.

        var host = Document.Load(new Uri("nx-example:///form.xml"));
        var save = new MemoryStream();
        host.Save(save);


NXKit.DOMEvents
----------

Implements W3C DOM Events. Elements can have events dispatched to them using the `IEventTarget` interface.

For instance to synthesize a UI client or activate (defined as emitting a `DOMActivate` event):

    document.Xml
        .Element("my-element")
        .Interface<IEventTarget>()
        .DispatchEvent(UIEvents.DOMActivate);


NXKit.XMLEvents
----------

Implements W3C XML Events. This provides a number of elements and attributes for listening to DOM Events.


NXKit.XForms
----------

NXKit.XForms is an implementation of XForms in the NXKit processing model.

The implementation currently covers most features from XForms 1.1, and some features from XForms 2.0.


NXKit.XForms.Layout
----------

NXKit.XForms.Layout contains a basic layout module supporting sections, tables, and a number of text formatting elements. Can serve as a semantic-only approach to building a form without getting involed with HTML.

    <table>
        <table-row>
            <table-cell><strong>Header 1</strong></table-cell>
            <table-cell><strong>Header 2</strong></table-cell>
            <table-cell><strong>Header 3</strong></table-cell>
        </table-row>
        <table-row>
            <table-cell>Header 1</table-cell>
            <table-cell>Header 2</table-cell>
            <table-cell>Header 3</table-cell>
        </table-row>
    </table>

NXKit.View.Server
----------

Provides a server side object model for assisting in running a NXKit Document on a server that communicates with a remote (client-based) user interface.

This class is used by the Web UI library to host a `Document` instance on the server to which the HTML interface can connect to.

NXKit.View.Js
----------

Base implementation of the NXKit View in JavaScript using Knockout.


NXKit.View.Web.UI
----------

ASP.Net WebForms starting point for NXKit. Contains a View control that you can add to the page, which imports all the neccessary client side objects and script.

    <xforms:View ID="View" runat="server"
        CssClass="FormView"
        OnLoad="View_Load" />

    protected void View_Load(object sender, EventArgs args)
    {
        if (!IsPostBack)
        {
            View.Open(new Uri(Request.Url, "nx-example:///aship-form.xml"));
        }
    }

NXKit.Scripting.EcmaScript
----------

Support for ECMAScript using the Jurassic JavaScript implementation.
