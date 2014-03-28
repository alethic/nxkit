/// <reference path="TypedEvent.ts" />

module NXKit.Web {

    export class Method {

        private _name: string;
        private _data: any[];

        public MethodInvoked: IMethodInvokedEvent = new TypedEvent();

        constructor(name: string, source: any) {
            var self = this;

            this._name = name;
            this._data = [];
        }


        public get Name(): string {
            return this._name;
        }

        public Invoke(params: any) {
            this._data.push(params);
            this.OnMethodInvoked(params);
        }

        public Update(source: any[]) {
            this._data = source;
        }

        public ToData(): any[] {
            return this._data.length > 0 ? this._data : null;
        }

        OnMethodInvoked(params: any) {
            this.MethodInvoked.trigger(this, params);
        }

    }

}