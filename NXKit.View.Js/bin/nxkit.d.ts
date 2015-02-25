declare module NXKit.View {
    class DeferredExecutor {
        private _queue;
        Register(cb: (promise: JQueryDeferred<void>) => void): void;
        Wait(cb: () => void): void;
    }
}
declare module NXKit.View {
    interface ICallback {
        (result: any): void;
    }
}
declare module NXKit.View {
    interface IEvent {
        add(listener: () => void): void;
        remove(listener: () => void): void;
        trigger(...a: any[]): void;
    }
}
declare module NXKit.View {
    interface IInterfaceMap {
        [name: string]: Interface;
    }
}
declare module NXKit.View {
}
declare module NXKit.View {
    class Interface {
        private _node;
        private _name;
        private _properties;
        constructor(node: Node, name: string, source: any);
        Node: Node;
        View: View;
        Name: string;
        Properties: IPropertyMap;
        Apply(source: any): void;
        ToData(): {
            [name: string]: any;
        };
    }
}
declare module NXKit.View {
    class InterfaceMap implements IInterfaceMap {
        [name: string]: Interface;
    }
}
declare module NXKit.View {
    interface IPropertyMap {
        [name: string]: Property;
    }
}
declare module NXKit.View {
    interface IServerInvoke {
        (args: any, cb: ICallback): void;
    }
}
declare module NXKit.View.Knockout {
    class CheckboxBindingHandler implements KnockoutBindingHandler {
        static _init(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext): void;
        static _update(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext): void;
        init(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext): void;
        update(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext): void;
    }
}
declare module NXKit.View.Knockout {
    class HorizontalVisibleBindingHandler implements KnockoutBindingHandler {
        init(element: HTMLElement, valueAccessor: () => any): void;
        update(element: HTMLElement, valueAccessor: () => any): void;
    }
}
declare module NXKit.View.Knockout {
    class InputBindingHandler implements KnockoutBindingHandler {
        static _init(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext): void;
        static _update(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext): void;
        init(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext): void;
        update(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext): void;
    }
}
declare module NXKit.View.Knockout {
    class ModalBindingHandler implements KnockoutBindingHandler {
        init(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext): void;
    }
}
declare module NXKit.View.Util {
    function Observable<T>(value?: T): KnockoutObservable<T>;
    function ObservableArray<T>(value?: T[]): KnockoutObservableArray<T>;
    function Computed<T>(def: KnockoutComputedDefine<T>): KnockoutComputed<T>;
    function DeepEquals(a: any, b: any, f?: (a: any, b: any) => boolean): boolean;
    function GenerateGuid(): string;
    function GetContextItems(context: KnockoutBindingContext): any[];
}
declare module NXKit.View.Knockout {
    class NodeBindingHandler implements KnockoutBindingHandler {
        init(element: HTMLElement, valueAccessor: () => any, allBindingsAccessor: KnockoutAllBindingsAccessor, viewModel: any, bindingContext: KnockoutBindingContext): void;
        update(element: HTMLElement, valueAccessor: () => any, allBindingsAccessor: KnockoutAllBindingsAccessor, viewModel: any, bindingContext: KnockoutBindingContext): void;
        static ConvertValueAccessor(element: HTMLElement, valueAccessor: () => any, viewModel: any, bindingContext: KnockoutBindingContext): any;
        static Match(node: Node, data: any): boolean;
    }
}
declare module NXKit.View.Knockout {
    class VisibleBindingHandler implements KnockoutBindingHandler {
        init(element: HTMLElement, valueAccessor: () => any): void;
        update(element: HTMLElement, valueAccessor: () => any): void;
    }
}
declare module NXKit.View {
    class LayoutOptions {
        static GetArgs(bindingContext: KnockoutBindingContext): any;
        private _args;
        constructor(args: any);
        Args: any;
    }
}
declare module NXKit.View {
    class Log {
        static Verbose: boolean;
        static Group<T>(title?: string, func?: () => T): any;
        static InvokeLogMethod(func: (m: string, ...args: any[]) => void, message: string, args: any[]): void;
        static Debug(message: string, ...args: any[]): void;
        static Information(message: string, ...args: any[]): void;
        static Warning(message: string, ...args: any[]): void;
        static Error(message: string, ...args: any[]): void;
        static Object(object: any, ...args: any[]): void;
    }
}
declare module NXKit.View {
    class Message {
        private _severity;
        private _text;
        constructor(severity: Severity, text: string);
        Severity: Severity;
        Text: string;
    }
}
declare module NXKit.View {
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
        Interface: Interface;
        Node: Node;
        View: View;
        Name: string;
        Value: KnockoutObservable<any>;
        ValueAsString: KnockoutObservable<string>;
        ValueAsBoolean: KnockoutObservable<boolean>;
        ValueAsNumber: KnockoutObservable<number>;
        ValueAsDate: KnockoutObservable<Date>;
        Update(source: any): void;
        ToData(): any;
        OnUpdate(): void;
    }
}
declare module NXKit.View {
    class Node {
        _view: View;
        _id: number;
        _data: any;
        _type: NodeType;
        _name: string;
        _value: KnockoutObservable<string>;
        _intfs: IInterfaceMap;
        _nodes: KnockoutObservableArray<Node>;
        constructor(view: View, source: any);
        View: View;
        IsNode: boolean;
        Data: any;
        Id: number;
        Type: NodeType;
        Name: string;
        Value: KnockoutObservable<string>;
        Interfaces: IInterfaceMap;
        Nodes: KnockoutObservableArray<Node>;
        Property(interfaceName: string, propertyName: string): Property;
        Invoke(interfaceName: string, methodName: string, params?: any): void;
        Apply(source: any): void;
        ApplyId(id: number): void;
        ApplyType(type: string): void;
        ApplyName(name: string): void;
        ApplyValue(value: string): void;
        ApplyInterfaces(source: any): void;
        UpdateInterface(name: string, source: any): void;
        ApplyNodes(sources: Array<any>): void;
        ToData(): any;
        NodesToData(): any[];
    }
}
declare module NXKit.View {
    class NodeType {
        static Object: NodeType;
        static Text: NodeType;
        static Element: NodeType;
        private _value;
        static Parse(value: string): NodeType;
        constructor(value: string);
        ToString(): string;
    }
}
declare module NXKit.View {
    class NodeViewModel {
        private _context;
        private _node;
        constructor(context: KnockoutBindingContext, node: Node);
        Context: KnockoutBindingContext;
        Node: Node;
        Contents: Node[];
        GetContents(): Node[];
        ContentsCount: number;
    }
}
declare module NXKit.View {
    class PropertyMap implements IPropertyMap {
        [name: string]: Property;
    }
}
declare module NXKit.View {
    enum Severity {
        Verbose = 1,
        Information = 2,
        Warning = 3,
        Error = 4,
    }
}
declare module NXKit.View {
    class View {
        private _body;
        private _server;
        private _save;
        private _hash;
        private _root;
        private _bind;
        private _threshold;
        private _queue;
        private _queueRunning;
        private _busy;
        constructor(body: HTMLElement, server: IServerInvoke);
        Busy: KnockoutObservable<boolean>;
        Body: HTMLElement;
        Data: any;
        Root: Node;
        Threshold: Severity;
        Receive(args: any): void;
        ReceiveNode(node: any): void;
        ReceiveCommands(commands: any[]): void;
        private Apply(data);
        PushUpdate(node: Node, $interface: Interface, property: Property, value: any): void;
        PushInvoke(node: Node, interfaceName: string, methodName: string, parameters: any): void;
        Queue(command: any): void;
        ApplyBindings(): void;
    }
}
declare module NXKit.View.ViewModelUtil {
    var GroupNodes: string[];
    var ControlNodes: string[];
    var MetadataNodes: string[];
    var TransparentNodes: string[];
    var TransparentNodePredicates: Array<(n: Node) => boolean>;
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
