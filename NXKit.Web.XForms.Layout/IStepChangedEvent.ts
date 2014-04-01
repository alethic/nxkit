module NXKit.Web {

    export interface IStepChangedEvent extends IEvent {
        add(listener: () => void): void;
        remove(listener: () => void): void;
        trigger(...a: any[]): void;
    }

}