module NXKit.Web {

    export interface ICallbackRequestEvent extends IEvent {
        add(listener: () => void): void;
        remove(listener: () => void): void;
        trigger(...a: any[]): void;
        add(listener: (data: any, wh: ICallbackComplete) => void): void;
        remove(listener: (data: any, wh: ICallbackComplete) => void): void;
        trigger(data: any, wh: ICallbackComplete): void;
    }

}