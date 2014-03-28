module NXKit.Web {

    export interface IInterfacePropertyChangedEvent extends IEvent {
        add(listener: () => void): void;
        remove(listener: () => void): void;
        trigger(...a: any[]): void;
        add(listener: ($interface: Interface, property: Property, value: any) => void): void;
        remove(listener: ($interface: Interface, property: Property, value: any) => void): void;
        trigger($interface: Interface, property: Property, value: any): void;
    }

}
