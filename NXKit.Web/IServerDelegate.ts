module NXKit.Web {

    export interface IServerDelegate {
        (data: any, cb: ICallbackComplete): void;
    }

}
