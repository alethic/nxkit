module NXKit.Web {

    export interface ICommandDelegate {
        (commands: any[], cb: ICallbackComplete): void;
    }

}
