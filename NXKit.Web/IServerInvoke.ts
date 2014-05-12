module NXKit.Web {

    export interface IServerInvoke {
        (commands: any[], cb: ICallback): void;
    }

}
