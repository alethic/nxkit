/// <reference path="TypedEvent.ts" />

module NXKit.Web {

    export class Interface {

        private _name: string;

        private _properties: IPropertyMap;
        private _methods: IMethodMap;

        public PropertyChanged: IPropertyValueChangedEvent = new TypedEvent();
        public MethodInvoked: IInterfaceMethodInvokedEvent = new TypedEvent();

        constructor(name: string, source: any) {
            var self = this;

            self._name = name;
            self._properties = new PropertyMap();
            self._methods = new MethodMap();

            if (source != null)
                self.Update(source);
        }

        public get Name(): string {
            return this._name;
        }

        public get Methods(): IMethodMap {
            return this._methods;
        }

        public Update(source: any) {
            var self = this;

            for (var i in source) {
                var m = self._methods[<string>i];
                if (m == null) {
                    self._methods[<string>i] = new Method(<string>i, source[<string>i]);
                    self._methods[<string>i].MethodInvoked.add((_, params) => {
                        self.OnMethodInvoked(_, params);
                    });
                } else {
                    m.Update(source[<string>i]);
                }
            }
        }

        public ToData(): { [name: string]: any } {
            var r: { [name: string]: any } = {};
            for (var i in this._methods) {
                var m = this._methods[<string>i].ToData();
                if (m != null)
                    r[this._methods[<string>i].Name] = this._methods[<string>i].ToData();
            }
            return r;
        }

        OnMethodInvoked(method: Method, params: any) {
            this.MethodInvoked.trigger(this, method, params);
        }

    }

}