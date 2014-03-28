module NXKit.Web {

    export interface INodePropertyChangedEvent extends IEvent {
        add(listener: () => void): void;
        remove(listener: () => void): void;
        trigger(...a: any[]): void;
        add(listener: (node: Node, $interface: Interface, property: Property, value: any) => void): void;
        remove(listener: (node: Node, $interface: Interface, property: Property, value: any) => void): void;
        trigger(node: Node, $interface: Interface, property: Property, value: any): void;
    }

}
