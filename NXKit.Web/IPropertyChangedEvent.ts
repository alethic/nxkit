module NXKit.Web {

    export interface IPropertyChangedEvent extends IEvent {
        add(listener: () => void): void;
        remove(listener: () => void): void;
        trigger(...a: any[]): void;
        add(listener: (property: Property, value: any) => void): void;
        remove(listener: (property: Property, value: any) => void): void;
        trigger(property: Property, value: any): void;
    }

}
