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
    function DeepEquals(a: any, b: any, f?: (a: any, b: any) => boolean): boolean;
    function GenerateGuid(): string;
    function GetContextItems(context: KnockoutBindingContext): any[];
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
        constructor(view: View, source: any);
        public View : View;
        public IsNode : boolean;
        public Data : any;
        public Id : number;
        public Type : NodeType;
        public Name : string;
        public Value : KnockoutObservable<string>;
        public Interfaces : IInterfaceMap;
        public Nodes : KnockoutObservableArray<Node>;
        public Property(interfaceName: string, propertyName: string): Property;
        public Invoke(interfaceName: string, methodName: string, params?: any): void;
        public Apply(source: any): void;
        public ApplyId(id: number): void;
        public ApplyType(type: string): void;
        public ApplyName(name: string): void;
        public ApplyValue(value: string): void;
        public ApplyInterfaces(source: any): void;
        public UpdateInterface(name: string, source: any): void;
        public ApplyNodes(sources: any[]): void;
        public ToData(): any;
        public NodesToData(): any[];
    }
}
declare module NXKit.Web {
    class LayoutManager {
        private _context;
        private _parent;
        constructor(context: KnockoutBindingContext);
        public Context : KnockoutBindingContext;
        public Parent : LayoutManager;
        public GetTemplateOptions_(valueAccessor: () => any, viewModel: any, bindingContext: KnockoutBindingContext, options: any): any;
        public GetTemplateOptions(valueAccessor: () => any, viewModel: any, bindingContext: KnockoutBindingContext, options: any): any;
        public GetLocalTemplates(): HTMLElement[];
        public GetTemplates(): HTMLElement[];
        public GetNode(valueAccessor: KnockoutObservable<any>, viewModel: any, bindingContext: KnockoutBindingContext): Node;
        public GetUnknownTemplate(data: any): HTMLElement;
        static GetTemplateNodeData(node: HTMLElement): any;
        static TemplatePredicate(node: HTMLElement, opts: any): boolean;
        private static TemplateFilter(nodes, data);
        public GetTemplate(data: any): HTMLElement;
        public GetTemplateName(data: any): KnockoutObservable<string>;
    }
}
declare module NXKit.Web {
    class DefaultLayoutManager extends LayoutManager {
        private _templates;
        constructor(context: KnockoutBindingContext);
        public GetTemplateOptions(valueAccessor: KnockoutObservable<any>, viewModel: any, bindingContext: KnockoutBindingContext, options: any): any;
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
declare module NXKit.Web {
    class DeferredExecutor {
        private _queue;
        public Register(cb: (promise: JQueryDeferred<void>) => void): void;
        public Wait(cb: () => void): void;
    }
}
declare module NXKit.Web {
    class TemplateManager {
        static Default: TemplateManager;
        private _executor;
        private _baseUrl;
        constructor(baseUrl: string);
        public Register(name: string): void;
        public Wait(cb: () => void): void;
    }
}
declare module NXKit.Web.ViewModelUtil {
    var LayoutManagers: {
        (context: KnockoutBindingContext): LayoutManager;
    }[];
    var GroupNodes: string[];
    var ControlNodes: string[];
    var MetadataNodes: string[];
    var TransparentNodes: string[];
    var TransparentNodePredicates: {
        (n: Node): boolean;
    }[];
    function IsEmptyTextNode(node: Node): boolean;
    function IsIgnoredNode(node: Node): boolean;
    function IsGroupNode(node: Node): boolean;
    function HasGroupNode(nodes: Node[]): boolean;
    function GetGroupNodes(nodes: Node[]): Node[];
    function IsControlNode(node: Node): boolean;
    function HasControlNode(nodes: Node[]): boolean;
    function GetControlNodes(nodes: Node[]): Node[];
    function IsMetadataNode(node: Node): boolean;
    function HasMetadataNode(nodes: Node[]): boolean;
    function GetMetadataNodes(nodes: Node[]): Node[];
    function IsTransparentNode(node: Node): boolean;
    function HasTransparentNode(nodes: Node[]): boolean;
    function GetTransparentNodes(nodes: Node[]): Node[];
    function GetContentNodes(nodes: Node[]): Node[];
    function GetContents(node: Node): Node[];
}
declare module NXKit.Web {
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
        public Receive(args: any): void;
        public ReceiveData(data: any): void;
        private AppendMessages(messages);
        private ExecuteScripts(scripts);
        public RemoveMessage(message: Message): void;
        private Apply(data);
        public PushUpdate(node: Node, $interface: Interface, property: Property, value: any): void;
        public PushInvoke(node: Node, interfaceName: string, methodName: string, params: any): void;
        public Queue(command: any): void;
        public ApplyBindings(): void;
    }
}
declare module NXKit.Web {
    class NodeViewModel {
        private _context;
        private _node;
        constructor(context: KnockoutBindingContext, node: Node);
        public Context : KnockoutBindingContext;
        public Node : Node;
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
        static ConvertValueAccessor(valueAccessor: () => any, viewModel: any, bindingContext: KnockoutBindingContext): () => any;
        static GetTemplateViewModel(valueAccessor: () => any, viewModel: any, bindingContext: KnockoutBindingContext): any;
        static GetTemplateOptions(valueAccessor: () => any, viewModel: any, bindingContext: KnockoutBindingContext): any;
        static GetTemplateName(bindingContext: KnockoutBindingContext, data: any): KnockoutObservable<string>;
    }
}
