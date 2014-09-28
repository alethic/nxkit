/// <reference path="TypedEvent.ts" />
/// <reference path="Property.ts" />

module NXKit.View {

    export class Node {

        _view: View;
        _id: number;
        _data: any;
        _type: NodeType;
        _name: string;
        _value: KnockoutObservable<string>;
        _intfs: IInterfaceMap;
        _nodes: KnockoutObservableArray<Node>;

        /**
         * Initializes a new instance from the given initial data.
         */
        constructor(view: View, source: any) {
            this._view = view;
            this._id = -1;
            this._data = null;
            this._type = null;
            this._name = null;
            this._value = ko.observable<string>();
            this._intfs = new InterfaceMap();
            this._nodes = ko.observableArray<Node>();

            // update from source data
            if (source != null)
                this.Apply(source);
        }

        public get View(): View {
            return this._view;
        }

        public get IsNode(): boolean {
            return true;
        }

        /**
         * Gets the data of this node.
         */
        public get Data(): any {
            return this._data;
        }

        /**
         * Gets the unique ID of this node.
         */
        public get Id(): number {
            return this._id;
        }

        /**
         * Gets the type of this node.
         */
        public get Type(): NodeType {
            return this._type;
        }

        /**
         * Gets the name of this node.
         */
        public get Name(): string {
            return this._name;
        }

        /**
         * Gets the value of this node.
         */
        public get Value(): KnockoutObservable<string> {
            return this._value;
        }

        /**
         * Gets the exposed interfaces of this node.
         */
        public get Interfaces(): IInterfaceMap {
            return this._intfs;
        }

        /**
         * Gets the content of this node.
         */
        public get Nodes(): KnockoutObservableArray<Node> {
            return this._nodes;
        }

        /**
         * Gets the named property on the named interface.
         */
        public Property(interfaceName: string, propertyName: string): Property {
            try {
                var i = this._intfs[interfaceName];
                if (i == null)
                    return null;

                var p = i.Properties[propertyName];
                if (p == null)
                    return null;

                return p;
            } catch (ex) {
                ex.message = "Node.Property()" + '\nMessage: ' + ex.message;
                throw ex;
            }
        }

        /**
         * Invokes a named method on the specified interface.
         */
        public Invoke(interfaceName: string, methodName: string, params?: any): void {
            this.View.PushInvoke(this, interfaceName, methodName, params);
        }

        /**
         * Integrates the data given by the node parameter into this node.
         */
        public Apply(source: any) {
            var self = this;

            var next = function () {
                try {
                    self._data = source;
                    self.ApplyId(source.Id);
                    self.ApplyType(source.Type);
                    self.ApplyName(source.Name);
                    self.ApplyValue(source.Value);
                    self.ApplyInterfaces(source);
                    self.ApplyNodes(source.Nodes);
                } catch (ex) {
                    ex.message = "Node.Apply()" + '\nMessage: ' + ex.message;
                    throw ex;
                }
            };

            // check if any modules are required, if so dispatch to require framework
            var vmod = source['NXKit.View.Js.ViewModule']
            if (vmod != null) {
                var deps = vmod['Require'];
                if (deps != null) {
                    NXKit.require(deps, next);
                    return;
                }
            }

            // no requirements, continue
            next();
        }

        /**
         * Updates the type of this node with the new value.
         */
        ApplyId(id: number) {
            this._id = id;
        }

        /**
         * Updates the type of this node with the new value.
         */
        ApplyType(type: string) {
            this._type = NodeType.Parse(type);
        }

        /**
         * Updates the name of this node with the new value.
         */
        ApplyName(name: string) {
            this._name = name;
        }

        /**
         * Updates the value of this node with the new value.
         */
        ApplyValue(value: string) {
            this._value(value);
        }

        /**
         * Integrates the set of interfaces given with this node.
         */
        ApplyInterfaces(source: any) {
            try {
                var self = this;
                for (var i in source) {
                    if (i.indexOf('.') > -1)
                        self.UpdateInterface(<string>i, source[<string>i]);
                }
            } catch (ex) {
                ex.message = "Node.UpdateInterfaces()" + '\nMessage: ' + ex.message;
                throw ex;
            }
        }

        /**
         * Updates the property given by the specified name with the specified value.
         */
        UpdateInterface(name: string, source: any) {
            try {
                var self = this;
                var intf: Interface = self._intfs[name];
                if (intf == null) {
                    intf = self._intfs[name] = new Interface(self, name, source);
                } else {
                    intf.Apply(source);
                }
            } catch (ex) {
                ex.message = "Node.UpdateInterface()" + '\nMessage: ' + ex.message;
                throw ex;
            }
        }

        /**
         * Integrates the set of content nodes with the given object values.
         */
        ApplyNodes(sources: Array<any>) {
            try {
                var self = this;

                // clear nodes if none
                if (sources == null) {
                    self._nodes.removeAll();
                    return;
                }

                // update or insert new values
                for (var i = 0; i < sources.length; i++) {
                    if (self._nodes().length < i + 1) {
                        var v = new Node(self._view, sources[i]);
                        self._nodes.push(v);
                    } else {
                        self._nodes()[i].Apply(sources[i]);
                    }
                }

                // delete trailing values
                if (self._nodes().length > sources.length)
                    self._nodes.splice(sources.length);
            } catch (ex) {
                ex.message = "Node.UpdateNodes()" + '\nMessage: ' + ex.message;
                throw ex;
            }
        }

        /**
         * Transforms the node and its hierarchy into JSON data.
         */
        public ToData(): any {
            var self = this;

            var r: any = {
                Id: self._id,
            };

            for (var i in self._intfs)
                r[<string>i] = self._intfs[<string>i].ToData();

            return r;
        }

        /**
         * Transforms the given Property array into a list of data to push.
         */
        NodesToData(): any[] {
            return ko.utils.arrayMap(this._nodes(), v => {
                return v.ToData();
            });
        }

    }

}