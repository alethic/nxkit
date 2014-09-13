declare module NXKit.Web {
    interface ICallback {
        (result: any): void;
    }
}
declare module NXKit.Web {
    interface IEvent {
        add(listener: () => void): void;
        remove(listener: () => void): void;
        trigger(...a: any[]): void;
    }
}
declare module NXKit.Web {
    interface IInterfaceMap {
        [name: string]: Interface;
    }
}
declare module NXKit.Web {
    interface IPropertyMap {
        [name: string]: Property;
    }
}
declare module NXKit.Web {
    interface IServerInvoke {
        (args: any, cb: ICallback): void;
    }
}
declare module NXKit.Web.Knockout {
    class InputBindingHandler implements KnockoutBindingHandler {
        static _init(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext): void;
        static _update(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext): void;
        public init(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext): void;
        public update(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext): void;
    }
}
declare module NXKit.Web.Util {
    function Observable<T>(value?: T): KnockoutObservable<T>;
    function ObservableArray<T>(value?: T[]): KnockoutObservableArray<T>;
    function Computed<T>(def: KnockoutComputedDefine<T>): KnockoutComputed<T>;
    /**
    * Tests two objects for equality.
    */
    function DeepEquals(a: any, b: any, f?: (a: any, b: any) => boolean): boolean;
    /**
    * Generates a unique identifier.
    */
    function GenerateGuid(): string;
    /**
    * Returns the entire context item chain from the specified context upwards.
    */
    function GetContextItems(context: KnockoutBindingContext): any[];
    /**
    * Gets the layout manager in scope of the given binding context.
    */
    function GetLayoutManager(context: KnockoutBindingContext): LayoutManager;
}
declare module NXKit.Web.Knockout {
    class OptionsBindingHandler implements KnockoutBindingHandler {
        public init(element: HTMLElement, valueAccessor: () => any, allBindingsAccessor: () => any, viewModel: any, bindingContext: KnockoutBindingContext): any;
    }
}
declare module NXKit.Web.Knockout {
    class HorizontalVisibleBindingHandler implements KnockoutBindingHandler {
        public init(element: HTMLElement, valueAccessor: () => any): void;
        public update(element: HTMLElement, valueAccessor: () => any): void;
    }
}
declare module NXKit.Web.Knockout {
    class LayoutManagerExportBindingHandler implements KnockoutBindingHandler {
        static _init(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext): {
            controlsDescendantBindings: boolean;
        };
        public init(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext): {
            controlsDescendantBindings: boolean;
        };
    }
}
declare module NXKit.Web {
    class LayoutOptions {
        /**
        * Gets the full set of currently applied layout option args for the given context.
        */
        static GetArgs(bindingContext: KnockoutBindingContext): any;
        private _args;
        constructor(args: any);
        public Args : any;
    }
}
declare module NXKit.Web {
}
declare module NXKit.Web {
    class Property {
        private _intf;
        private _name;
        private _suspend;
        private _value;
        private _valueAsString;
        private _valueAsBoolean;
        private _valueAsNumber;
        private _valueAsDate;
        constructor(intf: Interface, name: string, source: any);
        public Interface : Interface;
        public Node : Node;
        public View : View;
        public Name : string;
        public ValueAsString : KnockoutComputed<string>;
        public ValueAsBoolean : KnockoutComputed<boolean>;
        public ValueAsNumber : KnockoutComputed<number>;
        public ValueAsDate : KnockoutComputed<Date>;
        public Update(source: any): void;
        public ToData(): any;
        public OnUpdate(): void;
    }
}
declare module NXKit.Web {
    class Node {
        public _view: View;
        public _id: number;
        public _data: any;
        public _type: NodeType;
        public _name: string;
        public _value: KnockoutObservable<string>;
        public _intfs: IInterfaceMap;
        public _nodes: KnockoutObservableArray<Node>;
        /**
        * Initializes a new instance from the given initial data.
        */
        constructor(view: View, source: any);
        public View : View;
        public IsNode : boolean;
        /**
        * Gets the data of this node.
        */
        public Data : any;
        /**
        * Gets the unique ID of this node.
        */
        public Id : number;
        /**
        * Gets the type of this node.
        */
        public Type : NodeType;
        /**
        * Gets the name of this node.
        */
        public Name : string;
        /**
        * Gets the value of this node.
        */
        public Value : KnockoutObservable<string>;
        /**
        * Gets the exposed interfaces of this node.
        */
        public Interfaces : IInterfaceMap;
        /**
        * Gets the content of this node.
        */
        public Nodes : KnockoutObservableArray<Node>;
        /**
        * Gets the named property on the named interface.
        */
        public Property(interfaceName: string, propertyName: string): Property;
        /**
        * Invokes a named method on the specified interface.
        */
        public Invoke(interfaceName: string, methodName: string, params?: any): void;
        /**
        * Integrates the data given by the node parameter into this node.
        */
        public Apply(source: any): void;
        /**
        * Updates the type of this node with the new value.
        */
        public ApplyId(id: number): void;
        /**
        * Updates the type of this node with the new value.
        */
        public ApplyType(type: string): void;
        /**
        * Updates the name of this node with the new value.
        */
        public ApplyName(name: string): void;
        /**
        * Updates the value of this node with the new value.
        */
        public ApplyValue(value: string): void;
        /**
        * Integrates the set of interfaces given with this node.
        */
        public ApplyInterfaces(source: any): void;
        /**
        * Updates the property given by the specified name with the specified value.
        */
        public UpdateInterface(name: string, source: any): void;
        /**
        * Integrates the set of content nodes with the given object values.
        */
        public ApplyNodes(sources: any[]): void;
        /**
        * Transforms the node and its hierarchy into JSON data.
        */
        public ToData(): any;
        /**
        * Transforms the given Property array into a list of data to push.
        */
        public NodesToData(): any[];
    }
}
declare module NXKit.Web {
    class LayoutManager {
        private _context;
        private _parent;
        constructor(context: KnockoutBindingContext);
        /**
        * Gets the context inside which this layout manager was created.
        */
        public Context : KnockoutBindingContext;
        /**
        * Gets the parent layout manager.
        */
        public Parent : LayoutManager;
        /**
        * Parses the given template binding information for a data structure to pass to the template lookup procedures.
        */
        public GetTemplateOptions_(valueAccessor: () => any, viewModel: any, bindingContext: KnockoutBindingContext, options: any): any;
        /**
        * Parses the given template binding information for a data structure to pass to the template lookup procedures.
        */
        public GetTemplateOptions(valueAccessor: () => any, viewModel: any, bindingContext: KnockoutBindingContext, options: any): any;
        /**
        * Gets the templates provided by this layout manager for the given data.
        */
        public GetLocalTemplates(): HTMLElement[];
        /**
        * Gets the set of available templates for the given data.
        */
        public GetTemplates(): HTMLElement[];
        /**
        * Helper method for resolving a node given a Knockout context.
        */
        public GetNode(valueAccessor: KnockoutObservable<any>, viewModel: any, bindingContext: KnockoutBindingContext): Node;
        /**
        * Gets the fallback template for the given data.
        */
        public GetUnknownTemplate(data: any): HTMLElement;
        /**
        * Extracts a JSON representation of a template node's data-nxkit bindings.
        */
        static GetTemplateNodeData(node: HTMLElement): any;
        /**
        * Tests whether a template node matches the given data.
        */
        static TemplatePredicate(node: HTMLElement, opts: any): boolean;
        /**
        * Tests each given node against the predicate function.
        */
        private static TemplateFilter(nodes, data);
        /**
        * Gets the appropriate template for the given data.
        */
        public GetTemplate(data: any): HTMLElement;
        /**
        * Gets the template that applies for the given data.
        */
        public GetTemplateName(data: any): string;
    }
}
declare module NXKit.Web {
    class DefaultLayoutManager extends LayoutManager {
        constructor(context: KnockoutBindingContext);
        /**
        * Parses the given template binding information for a data structure to pass to the template lookup procedures.
        */
        public GetTemplateOptions(valueAccessor: KnockoutObservable<any>, viewModel: any, bindingContext: KnockoutBindingContext, options: any): any;
        /**
        * Gets all available templates currently in the document.
        */
        public GetLocalTemplates(): HTMLElement[];
    }
}
declare module NXKit.Web.Knockout {
    class ModalBindingHandler implements KnockoutBindingHandler {
        public init(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext): void;
    }
}
declare module NXKit.Web.Knockout {
    class CheckboxBindingHandler implements KnockoutBindingHandler {
        static _init(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext): void;
        static _update(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext): void;
        public init(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext): void;
        public update(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext): void;
    }
}
declare module NXKit.Web {
    class Interface {
        private _node;
        private _name;
        private _properties;
        constructor(node: Node, name: string, source: any);
        public Node : Node;
        public View : View;
        public Name : string;
        public Properties : IPropertyMap;
        public Apply(source: any): void;
        public ToData(): {
            [name: string]: any;
        };
    }
}
declare module NXKit.Web {
    class Log {
        static Verbose: boolean;
        static Group<T>(title?: string, func?: () => T): any;
        static Debug(message: string, ...args: any[]): void;
        static Object(object: any, ...args: any[]): void;
    }
}
declare module NXKit.Web {
    class Message {
        private _severity;
        private _text;
        constructor(severity: Severity, text: string);
        public Severity : Severity;
        public Text : string;
    }
}
declare module NXKit.Web {
    class InterfaceMap implements IInterfaceMap {
        [name: string]: Interface;
    }
}
declare module NXKit.Web {
    class NodeType {
        static Object: NodeType;
        static Text: NodeType;
        static Element: NodeType;
        private _value;
        static Parse(value: string): NodeType;
        constructor(value: string);
        public ToString(): string;
    }
}
declare module NXKit.Web {
    class PropertyMap implements IPropertyMap {
        [name: string]: Property;
    }
}
declare module NXKit.Web {
    enum Severity {
        Verbose = 1,
        Information = 2,
        Warning = 3,
        Error = 4,
    }
}
declare module NXKit.Web.ViewModelUtil {
    /**
    * Set of functions to inject layout managers at the top of the hierarchy.
    */
    var LayoutManagers: {
        (context: KnockoutBindingContext): LayoutManager;
    }[];
    /**
    * Nodes which represent a grouping element.
    */
    var GroupNodes: string[];
    /**
    * Nodes which are considered to be control elements.
    */
    var ControlNodes: string[];
    /**
    * Nodes which are considered to be metadata elements for their parents.
    */
    var MetadataNodes: string[];
    /**
    * Nodes which are considered to be transparent, and ignored when calculating content membership.
    */
    var TransparentNodes: string[];
    /**
    * Nodes which are considered to be transparent, and ignored when calculating content membership.
    */
    var TransparentNodePredicates: {
        (n: Node): boolean;
    }[];
    /**
    * Returns true of the given node is an empty text node.
    */
    function IsEmptyTextNode(node: Node): boolean;
    /**
    * Returns true if the current node is one that should be completely ignored.
    */
    function IsIgnoredNode(node: Node): boolean;
    /**
    * Returns true if the given node is a control node.
    */
    function IsGroupNode(node: Node): boolean;
    /**
    * Returns true if the given node set contains a control node.
    */
    function HasGroupNode(nodes: Node[]): boolean;
    /**
    * Filters out the given node set for control nodes.
    */
    function GetGroupNodes(nodes: Node[]): Node[];
    /**
    * Returns true if the given node is a control node.
    */
    function IsControlNode(node: Node): boolean;
    /**
    * Returns true if the given node set contains a control node.
    */
    function HasControlNode(nodes: Node[]): boolean;
    /**
    * Filters out the given node set for control nodes.
    */
    function GetControlNodes(nodes: Node[]): Node[];
    /**
    * Returns true if the given node is a transparent node.
    */
    function IsMetadataNode(node: Node): boolean;
    /**
    * Returns true if the given node set contains a metadata node.
    */
    function HasMetadataNode(nodes: Node[]): boolean;
    /**
    * Filters out the given node set for control nodes.
    */
    function GetMetadataNodes(nodes: Node[]): Node[];
    /**
    * Returns true if the given node is a transparent node.
    */
    function IsTransparentNode(node: Node): boolean;
    /**
    * Returns true if the given node set contains a transparent node.
    */
    function HasTransparentNode(nodes: Node[]): boolean;
    /**
    * Filters out the given node set for transparent nodes.
    */
    function GetTransparentNodes(nodes: Node[]): Node[];
    /**
    * Filters out the given node set for content nodes. This descends through transparent nodes.
    */
    function GetContentNodes(nodes: Node[]): Node[];
    /**
    * Gets the content nodes of the given node. This descends through transparent nodes.
    */
    function GetContents(node: Node): Node[];
}
declare module NXKit.Web {
    /**
    * Main NXKit client-side view class. Injects the view interface into a set of HTML elements.
    */
    class View {
        private _body;
        private _server;
        private _save;
        private _hash;
        private _root;
        private _bind;
        private _messages;
        private _threshold;
        private _queue;
        private _queueRunning;
        private _busy;
        constructor(body: HTMLElement, server: IServerInvoke);
        public Busy : KnockoutObservable<boolean>;
        public Body : HTMLElement;
        public Data : any;
        public Root : Node;
        public Messages : KnockoutObservableArray<Message>;
        public Threshold : Severity;
        /**
        * Updates the view in response to a received message.
        */
        public Receive(args: any): void;
        /**
        * Updates the view in response to a received data package.
        */
        public ReceiveData(data: any): void;
        /**
        * Updates the messages of the view with the specified items.
        */
        private AppendMessages(messages);
        /**
        * Executes the given scripts.
        */
        private ExecuteScripts(scripts);
        /**
        * Removes the current message from the set of messages.
        */
        public RemoveMessage(message: Message): void;
        /**
        * Initiates a refresh of the view model.
        */
        private Apply(data);
        /**
        * Invoked when the view model initiates a request to push an update to a node.
        */
        public PushUpdate(node: Node, $interface: Interface, property: Property, value: any): void;
        public PushInvoke(node: Node, interfaceName: string, methodName: string, params: any): void;
        /**
        * Queues the given data to be sent to the server.
        */
        public Queue(command: any): void;
        /**
        * Applies the bindings to the view if possible.
        */
        public ApplyBindings(): void;
    }
}
declare module NXKit.Web {
    /**
    * Base view model class for wrapping a node.
    */
    class NodeViewModel {
        private _context;
        private _node;
        constructor(context: KnockoutBindingContext, node: Node);
        /**
        * Gets the binding context available at the time the view model was created.
        */
        public Context : KnockoutBindingContext;
        /**
        * Gets the node that is wrapped by this view model.
        */
        public Node : Node;
        /**
        * Gets the content nodes of the current node.
        */
        public Contents : Node[];
        public GetContents(): Node[];
        public ContentsCount : number;
    }
}
declare module NXKit.Web.Knockout {
    class VisibleBindingHandler implements KnockoutBindingHandler {
        public init(element: HTMLElement, valueAccessor: () => any): void;
        public update(element: HTMLElement, valueAccessor: () => any): void;
    }
}
declare module NXKit.Web.Knockout {
    class TemplateBindingHandler implements KnockoutBindingHandler {
        public init(element: HTMLElement, valueAccessor: () => any, allBindingsAccessor: () => any, viewModel: any, bindingContext: KnockoutBindingContext): void;
        public update(element: HTMLElement, valueAccessor: () => any, allBindingsAccessor: () => any, viewModel: any, bindingContext: KnockoutBindingContext): void;
        /**
        * Converts the given value accessor into a value accessor compatible with the default template implementation.
        */
        static ConvertValueAccessor(valueAccessor: () => any, viewModel: any, bindingContext: KnockoutBindingContext): () => any;
        /**
        * Gets the recommended view model for the given binding information.
        */
        static GetTemplateViewModel(valueAccessor: () => any, viewModel: any, bindingContext: KnockoutBindingContext): any;
        /**
        * Extracts template index data from the given binding information.
        */
        static GetTemplateOptions(valueAccessor: () => any, viewModel: any, bindingContext: KnockoutBindingContext): any;
        /**
        * Determines the named template from the given extracted data and context.
        */
        static GetTemplateName(bindingContext: KnockoutBindingContext, data: any): string;
    }
}
