declare module NXKit.Web {
    interface ICallbackRequestEvent extends IEvent {
        add(listener: () => void): void;
        remove(listener: () => void): void;
        trigger(...a: any[]): void;
        add(listener: (data: any, wh: ICallbackComplete) => void): void;
        remove(listener: (data: any, wh: ICallbackComplete) => void): void;
        trigger(data: any, wh: ICallbackComplete): void;
    }
}
declare module NXKit.Web {
    interface ICallbackComplete {
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
    interface IPropertyChangedEvent extends IEvent {
        add(listener: () => void): void;
        remove(listener: () => void): void;
        trigger(...a: any[]): void;
        add(listener: (property: Property, value: any) => void): void;
        remove(listener: (property: Property, value: any) => void): void;
        trigger(property: Property, value: any): void;
    }
}
declare module NXKit.Web {
    interface INodeMethodInvokedEvent extends IEvent {
        add(listener: () => void): void;
        remove(listener: () => void): void;
        trigger(...a: any[]): void;
        add(listener: (node: Node, $interface: Interface, method: Method, params: any) => void): void;
        remove(listener: (node: Node, $interface: Interface, method: Method, params: any) => void): void;
        trigger(node: Node, $interface: Interface, method: Method, params: any): void;
    }
}
declare module NXKit.Web {
    interface IInterfaceMethodInvokedEvent extends IEvent {
        add(listener: () => void): void;
        remove(listener: () => void): void;
        trigger(...a: any[]): void;
        add(listener: ($interface: Interface, method: Method, params: any) => void): void;
        remove(listener: ($interface: Interface, method: Method, params: any) => void): void;
        trigger($interface: Interface, method: Method, params: any): void;
    }
}
declare module NXKit.Web {
    interface IMethodMap {
        [name: string]: Method;
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
    interface INodePropertyChangedEvent extends IEvent {
        add(listener: () => void): void;
        remove(listener: () => void): void;
        trigger(...a: any[]): void;
        add(listener: (node: Node, $interface: Interface, property: Property, value: any) => void): void;
        remove(listener: (node: Node, $interface: Interface, property: Property, value: any) => void): void;
        trigger(node: Node, $interface: Interface, property: Property, value: any): void;
    }
}
declare module NXKit.Web {
    interface IMethodInvokedEvent extends IEvent {
        add(listener: () => void): void;
        remove(listener: () => void): void;
        trigger(...a: any[]): void;
        add(listener: (method: Method, params: any) => void): void;
        remove(listener: (method: Method, params: any) => void): void;
        trigger(method: Method, params: any): void;
    }
}
declare module NXKit.Web {
    interface IInterfacePropertyChangedEvent extends IEvent {
        add(listener: () => void): void;
        remove(listener: () => void): void;
        trigger(...a: any[]): void;
        add(listener: ($interface: Interface, property: Property, value: any) => void): void;
        remove(listener: ($interface: Interface, property: Property, value: any) => void): void;
        trigger($interface: Interface, property: Property, value: any): void;
    }
}
declare module NXKit.Web.Knockout {
}
declare module NXKit.Web.Util {
    /**
    * Tests two objects for equality.
    */
    function DeepEquals(a: any, b: any): boolean;
    /**
    * Generates a unique identifier.
    */
    function GenerateGuid(): string;
    /**
    * Gets the unique document ID of the given node.
    */
    function GetUniqueId(node: Node): string;
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
}
declare module NXKit.Web.Knockout {
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
    class TypedEvent implements IEvent {
        public _listeners: any[];
        public add(listener: () => void): void;
        public remove(listener?: () => void): void;
        public trigger(...a: any[]): void;
    }
}
declare module NXKit.Web {
    class Property {
        private _name;
        private _value;
        private _suspend;
        private _valueAsString;
        private _valueAsBoolean;
        private _valueAsNumber;
        private _valueAsDate;
        /**
        * Raised when the Property's value has changed.
        */
        public PropertyChanged: IPropertyChangedEvent;
        constructor(name: string, source: any);
        public Name : string;
        public Value : KnockoutObservable<any>;
        public ValueAsString : KnockoutComputed<string>;
        public ValueAsBoolean : KnockoutComputed<boolean>;
        public ValueAsNumber : KnockoutComputed<number>;
        public ValueAsDate : KnockoutComputed<Date>;
        public Update(source: any): void;
        public ToData(): any;
    }
}
declare module NXKit.Web {
    class Node {
        public _type: string;
        public _baseTypes: string[];
        public _interfaces: IInterfaceMap;
        public _nodes: KnockoutObservableArray<Node>;
        /**
        * Raised when the node has changes to be pushed to the server.
        */
        public PropertyChanged: INodePropertyChangedEvent;
        /**
        * Raised when the node has methods to be invoked on the server.
        */
        public MethodInvoked: INodeMethodInvokedEvent;
        /**
        * Initializes a new instance from the given initial data.
        */
        constructor(source: any);
        public IsNode : boolean;
        /**
        * Gets the type of this node.
        */
        public Type : string;
        /**
        * Gets the inheritence hierarchy of this node.
        */
        public BaseTypes : string[];
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
        * Gets the property value accessor for the named property on the specified interface.
        */
        public Value(interfaceName: string, propertyName: string): KnockoutObservable<any>;
        /**
        * Gets the property value accessor for the named property on the specified interface as a string.
        */
        public ValueAsString(interfaceName: string, propertyName: string): KnockoutObservable<string>;
        /**
        * Gets the property value accessor for the named property on the specified interface as a boolean.
        */
        public ValueAsBoolean(interfaceName: string, propertyName: string): KnockoutObservable<boolean>;
        /**
        * Gets the property value accessor for the named property on the specified interface as a number.
        */
        public ValueAsNumber(interfaceName: string, propertyName: string): KnockoutObservable<number>;
        /**
        * Gets the property value accessor for the named property on the specified interface as a date.
        */
        public ValueAsDate(interfaceName: string, propertyName: string): KnockoutObservable<Date>;
        /**
        * Gets the named method on the named interface.
        */
        public Method(interfaceName: string, methodName: string): Method;
        /**
        * Invokes a named method on the specified interface.
        */
        public Invoke(interfaceName: string, methodName: string, params?: any): void;
        /**
        * Integrates the data given by the node parameter into this node.
        */
        public Update(source: any): void;
        /**
        * Updates the type of this node with the new value.
        */
        public UpdateType(type: string): void;
        /**
        * Updates the base types of this node with the new set of values.
        */
        public UpdateBaseTypes(baseTypes: string[]): void;
        /**
        * Integrates the set of interfaces given with this node.
        */
        public UpdateInterfaces(source: any): void;
        /**
        * Updates the property given by the specified name with the specified value.
        */
        public UpdateInterface(name: string, source: any): void;
        /**
        * Integrates the set of content nodes with the given object values.
        */
        public UpdateNodes(sources: any[]): void;
        public ToData(): any;
        /**
        * Transforms the given Property array into a list of data to push.
        */
        public NodesToData(): any[];
        /**
        * Initiates a push of new values to the server.
        */
        public OnPropertyChanged($interface: Interface, property: Property, value: any): void;
        /**
        * Initiates a push of method invocations to the server.
        */
        public OnMethodInvoked($interface: Interface, method: Method, params: any): void;
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
        public ParseTemplateBinding(valueAccessor: () => any, viewModel: any, bindingContext: KnockoutBindingContext, data: any): any;
        /**
        * Gets the templates provided by this layout manager for the given data.
        */
        public GetLocalTemplates(): HTMLElement[];
        /**
        * Gets the set of available templates for the given data.
        */
        public GetTemplates(): HTMLElement[];
        /**
        * Gets the fallback template for the given data.
        */
        public GetUnknownTemplate(data: any): HTMLElement;
        /**
        * Extracts a JSON representation of a template node's data-nxkit bindings.
        */
        public GetTemplateNodeData(node: HTMLElement): any;
        /**
        * Tests whether a template node matches the given data.
        */
        public TemplatePredicate(node: HTMLElement, data: any): boolean;
        /**
        * Tests each given node against the predicate function.
        */
        private TemplateFilter(nodes, data);
        /**
        * Gets the appropriate template for the given data.
        */
        public GetTemplate(data: any): KnockoutObservable<HTMLElement>;
        /**
        * Gets the template that applies for the given data.
        */
        public GetTemplateName(data: any): KnockoutObservable<string>;
    }
}
declare module NXKit.Web {
    class DefaultLayoutManager extends LayoutManager {
        constructor(context: KnockoutBindingContext);
        /**
        * Parses the given template binding information for a data structure to pass to the template lookup procedures.
        */
        public ParseTemplateBinding(valueAccessor: KnockoutObservable<any>, viewModel: any, bindingContext: KnockoutBindingContext, data: any): any;
        /**
        * Gets all available templates currently in the document.
        */
        public GetLocalTemplates(): HTMLElement[];
    }
}
declare module NXKit.Web.Knockout {
}
declare module NXKit.Web.Knockout {
}
declare module NXKit.Web {
    class Interface {
        private _name;
        private _properties;
        private _methods;
        public PropertyChanged: IInterfacePropertyChangedEvent;
        public MethodInvoked: IInterfaceMethodInvokedEvent;
        constructor(name: string, source: any);
        public Name : string;
        public Properties : IPropertyMap;
        public Methods : IMethodMap;
        public Update(source: any): void;
        public ToData(): {
            [name: string]: any;
        };
        public OnPropertyChanged(property: Property, value: any): void;
        public OnMethodInvoked(method: Method, params: any): void;
    }
}
declare module NXKit.Web {
    class Method {
        private _name;
        private _data;
        public MethodInvoked: IMethodInvokedEvent;
        constructor(name: string, source: any);
        public Name : string;
        public Invoke(params?: any): void;
        public Update(source: any[]): void;
        public ToData(): any[];
        public OnMethodInvoked(params: any): void;
    }
}
declare module NXKit.Web {
    class InterfaceMap implements IInterfaceMap {
        [name: string]: Interface;
    }
}
declare module NXKit.Web {
    class MethodMap implements IMethodMap {
        [name: string]: Method;
    }
}
declare module NXKit.Web {
    class PropertyMap implements IPropertyMap {
        [name: string]: Property;
    }
}
declare module NXKit.Web.ViewModelUtil {
    /**
    * Node types which represent a grouping element.
    */
    var GroupNodeTypes: string[];
    /**
    * Node types which are considered to be control elements.
    */
    var ControlNodeTypes: string[];
    /**
    * Node types which are considered to be metadata elements for their parents.
    */
    var MetadataNodeTypes: string[];
    /**
    * Node types which are considered to be transparent, and ignored when calculating content membership.
    */
    var TransparentNodeTypes: string[];
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
        private _root;
        private _bind;
        private _onNodePropertyChanged;
        private _onNodeMethodInvoked;
        private _queue;
        private _queueRunning;
        /**
        * Raised when the Node has changes to be pushed to the server.
        */
        public CallbackRequest: ICallbackRequestEvent;
        constructor(body: HTMLElement);
        public Body : HTMLElement;
        public Data : any;
        /**
        * Initiates a refresh of the view model.
        */
        private Update(data);
        /**
        * Invoked to handle root node value change events.
        */
        public OnRootNodePropertyChanged(node: Node, $interface: Interface, property: Property, value: any): void;
        /**
        * Invoked to handle root node method invocations.
        */
        public OnRootNodeMethodInvoked(node: Node, $interface: Interface, method: Method, params: any): void;
        /**
        * Invoked when the view model initiates a request to push updates.
        */
        public Push(): void;
        /**
        * Runs any available items in the queue.
        */
        public Queue(func: (cb: ICallbackComplete) => void): void;
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
        * Gets the unique document ID of the wrapped node.
        */
        public UniqueId : string;
        /**
        * Gets the content nodes of the current node.
        */
        public Contents : Node[];
        public GetContents(): Node[];
    }
}
declare module NXKit.Web.Knockout {
}
declare module NXKit.Web.Knockout {
}
declare module NXKit.Web.Knockout {
}
