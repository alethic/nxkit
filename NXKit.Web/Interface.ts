/// <reference path="TypedEvent.ts" />

module NXKit.Web {

    export class Interface {

        private _name: string;

        private _properties: IPropertyMap;
        private _methods: IMethodMap;

        public PropertyChanged: IInterfacePropertyChangedEvent = new TypedEvent();
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

        public get Properties(): IPropertyMap {
            return this._properties;
        }

        public get Methods(): IMethodMap {
            return this._methods;
        }

        public Update(source: any) {
            var self = this;

            for (var i in source) {
                var s = <string>i;
                if (s.indexOf('@') === 0) {
                    var m = self._methods[s.substring(1, s.length - 1)];
                    if (m == null) {
                        self._methods[s] = new Method(s, source[s]);
                        self._methods[s].MethodInvoked.add((_, params) => {
                            self.OnMethodInvoked(_, params);
                        });
                    } else {
                        m.Update(source[s]);
                    }
                } else {
                    var p = self._properties[s];
                    if (p == null) {
                        self._properties[s] = new Property(s, source[s]);
                        self._properties[s].PropertyChanged.add((_, value) => {
                            self.OnPropertyChanged(_, value);
                        });
                    } else {
                        p.Update(source[s]);
                    }
                }
            }
        }

        public ToData(): { [name: string]: any } {
            var self = this;
            var r: { [name: string]: any } = {};

            // add properties to the data
            for (var i in self._properties) {
                var s = <string>i;
                var p = self._properties[s].ToData();
                if (p != null) {
                    r[self._properties[s].Name] = self._properties[s].ToData();
                }
            }

            // add methods to the data
            for (var i in self._methods) {
                var s = <string>i;
                var m = self._methods[s].ToData();
                if (m != null) {
                    r['@' + self._methods[s].Name] = self._methods[s].ToData();
                }
            }

            return r;
        }

        OnPropertyChanged(property: Property, value: any) {
            this.PropertyChanged.trigger(this, property, value);
        }

        OnMethodInvoked(method: Method, params: any) {
            this.MethodInvoked.trigger(this, method, params);
        }

    }

}