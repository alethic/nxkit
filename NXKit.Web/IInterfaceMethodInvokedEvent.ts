module NXKit.Web {

    export interface IInterfaceMethodInvokedEvent extends IEvent {
        add(listener: () => void): void;
        remove(listener: () => void): void;
        trigger(...a: any[]): void;
        add(listener: ($interface: Interface, method: Method, params: any) => void): void;
        remove(listener: ($interface: Interface, method: Method, params: any) => void): void;
        trigger($interface: Interface, method: Method, params: any): void;
    }

}
