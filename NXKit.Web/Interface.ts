/// <reference path="TypedEvent.ts" />

module NXKit.Web {

    export class Interface {

        private _node: Node;
        private _name: string;
        private _properties: IPropertyMap;

        constructor(node: Node, name: string, source: any) {
            var self = this;

            self._node = node;
            self._name = name;
            self._properties = new PropertyMap();

            if (source != null)
                self.Apply(source);
        }

        public get Node(): Node {
            return this._node;
        }

        public get View(): View {
            return this._node.View;
        }

        public get Name(): string {
            return this._name;
        }

        public get Properties(): IPropertyMap {
            return this._properties;
        }

        public Apply(source: any) {
            try {
                var self = this;

                var removeP: Property[] = [];
                for (var i in self._properties)
                    removeP.push(self._properties[i]);

                for (var i in source) {
                    var s = <string>i;
                    var n = s;
                    var p = self._properties[n];
                    if (p == null) {
                        self._properties[n] = new Property(self, n, source[s]);
                    } else {
                        p.Update(source[s]);
                    }

                    var index = removeP.indexOf(p);
                    if (index != -1) {
                        removeP[index] = null;
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
                ex.message = "Interface.Apply()" + '\nMessage: ' + ex.message;
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

            return r;
        }

    }

}