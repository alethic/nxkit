module NXKit.Web {

    export interface IMethodInvokedEvent extends IEvent {
        add(listener: () => void): void;
        remove(listener: () => void): void;
        trigger(...a: any[]): void;
        add(listener: (method: Method, params: any) => void): void;
        remove(listener: (method: Method, params: any) => void): void;
        trigger(method: Method, params: any): void;
    }

}
