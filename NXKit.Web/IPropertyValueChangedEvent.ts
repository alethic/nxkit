module NXKit.Web {

    export interface IPropertyValueChangedEvent extends IEvent {
        add(listener: () => void): void;
        remove(listener: () => void): void;
        trigger(...a: any[]): void;
        add(listener: (property: Property) => void): void;
        remove(listener: (property: Property) => void): void;
        trigger(property: Property): void;
    }

}
