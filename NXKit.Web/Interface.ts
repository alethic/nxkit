/// <reference path="TypedEvent.ts" />

module NXKit.Web {

    export class Interface {

        private _node: Node;
        private _name: string;
        private _properties: IPropertyMap;
        private _methods: IMethodMap;

        public PropertyChanged: INodePropertyChangedEvent = new TypedEvent();
        public MethodInvoked: INodeMethodInvokedEvent = new TypedEvent();

        constructor(node: Node, name: string, source: any) {
            var self = this;

            self._node = node;
            self._name = name;
            self._properties = new PropertyMap();
            self._methods = new MethodMap();

            if (source != null)
                self.Update(source);
        }

        public get Node(): Node {
            return this._node;
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
            try {
                var self = this;

                var removeM: Method[] = [];
                for (var i in self._methods)
                    removeM.push(self._methods[i]);

                var removeP: Property[] = [];
                for (var i in self._properties)
                    removeP.push(self._properties[i]);

                for (var i in source) {
                    var s = <string>i;
                    if (s.indexOf('@') === 0) {
                        var n = s.substring(1, s.length);
                        var m = self._methods[n];
                        if (m == null) {
                            self._methods[n] = new Method(self, n, source[s]);
                            self._methods[n].MethodInvoked.add((node, intf, method, params) => {
                                self.MethodInvoked.trigger(node, intf, method, params);
                            });
                        } else {
                            m.Update(source[s]);
                        }

                        var index = removeM.indexOf(m);
                        if (index != -1) {
                            removeM[index] = null;
                        }
                    } else {
                        var n = s;
                        var p = self._properties[n];
                        if (p == null) {
                            self._properties[n] = new Property(self, n, source[s]);
                            self._properties[n].PropertyChanged.add((node, intf, property, value) => {
                                self.PropertyChanged.trigger(node, intf, property, value);
                            });
                            self._properties[n].MethodInvoked.add((node, intf, method, params) => {
                                self.MethodInvoked.trigger(node, intf, method, params);
                            });
                        } else {
                            p.Update(source[s]);
                        }

                        var index = removeP.indexOf(p);
                        if (index != -1) {
                            removeP[index] = null;
                        }
                    }
                }

                // remove stale methods
                for (var j = 0; j < removeM.length; j++) {
                    var m = removeM[j];
                    if (m != null) {
                        delete self._methods[m.Name];
                    }
                }

                // remove stale properties
                for (var j = 0; j < removeP.length; j++) {
                    var p = removeP[j];
                    if (p != null) {
                        delete self._properties[p.Name];
                    }
                }

            } catch (ex) {
                ex.message = "Interface.Update()" + '\nMessage: ' + ex.message;
                throw ex;
            }
        }

        public ToData(): { [name: string]: any } {
            var self = this;
            var r: { [name: string]: any } = {};

            // add properties to the data
            for (var i in self._properties) {
                var s = <string>i;
                var p = self._properties[s];
                r[self._properties[s].Name] = p.ToData();
            }

            // add methods to the data
            for (var i in self._methods) {
                var s = <string>i;
                var m = self._methods[s];
                r['@' + m.Name] = m.ToData();
            }

            return r;
        }

    }

}