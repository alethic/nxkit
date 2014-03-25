module NXKit.Web {

    export interface INodePropertyValueChangedEvent extends IEvent {
        add(listener: () => void): void;
        remove(listener: () => void): void;
        trigger(...a: any[]): void;
        add(listener: (node: Node, property: Property) => void): void;
        remove(listener: (node: Node, property: Property) => void): void;
        trigger(node: Node, property: Property): void;
    }

}
