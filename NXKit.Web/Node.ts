/// <reference path="TypedEvent.ts" />
/// <reference path="Property.ts" />

module NXKit.Web {

    export class Node {

        _id: number;
        _data: any;
        _type: NodeType;
        _name: string;
        _value: KnockoutObservable<string>;
        _interfaces: IInterfaceMap;
        _nodes: KnockoutObservableArray<Node>;

        /**
         * Raised when the node has changes to be pushed to the server.
         */
        public PropertyChanged: INodePropertyChangedEvent = new TypedEvent();

        /**
         * Raised when the node has methods to be invoked on the server.
         */
        public MethodInvoked: INodeMethodInvokedEvent = new TypedEvent();

        /**
         * Initializes a new instance from the given initial data.
         */
        constructor(source: any) {
            this._id = -1;
            this._data = null;
            this._type = null;
            this._name = null;
            this._value = ko.observable<string>(null);
            this._interfaces = new InterfaceMap();
            this._nodes = ko.observableArray<Node>();

            // update from source data
            if (source != null)
                this.Update(source);
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
            return this._interfaces;
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
                var i = this._interfaces[interfaceName];
                if (i == null)
                    throw new Error('Unknown interface [' + interfaceName + ':' + propertyName + ']');

                var p = i.Properties[propertyName];
                if (p == null)
                    throw new Error('Unknown property [' + interfaceName + ':' + propertyName + ']');

                return p;
            } catch (ex) {
                ex.message = "Node.Property()" + '\nMessage: ' + ex.message;
                throw ex;
            }
        }

        /**
         * Gets the named method on the named interface.
         */
        public Method(interfaceName: string, methodName: string): Method {
            try {
                var i = this._interfaces[interfaceName];
                if (i == null)
                    throw new Error('Unknown interface');

                var m = i.Methods[methodName];
                if (m == null)
                    throw new Error('Unknown method');

                return m;
            } catch (ex) {
                ex.message = "Node.Method()" + '\nMessage: ' + ex.message;
                throw ex;
            }
        }

        /**
         * Invokes a named method on the specified interface.
         */
        public Invoke(interfaceName: string, methodName: string, params?: any): void {
            this.Method(interfaceName, methodName).Invoke(params);
        }

        /**
         * Integrates the data given by the node parameter into this node.
         */
        public Update(source: any) {
            try {
                this._data = source;
                this.UpdateId(source.Id);
                this.UpdateType(source.Type);
                this.UpdateName(source.Name);
                this.UpdateValue(source.Value);
                this.UpdateInterfaces(source);
                this.UpdateNodes(source.Nodes);
            } catch (ex) {
                ex.message = "Node.Update()" + '\nMessage: ' + ex.message;
                throw ex;
            }
        }

        /**
         * Updates the type of this node with the new value.
         */
        UpdateId(id: number) {
            this._id = id;
        }

        /**
         * Updates the type of this node with the new value.
         */
        UpdateType(type: string) {
            this._type = NodeType.Parse(type);
        }

        /**
         * Updates the name of this node with the new value.
         */
        UpdateName(name: string) {
            this._name = name;
        }

        /**
         * Updates the value of this node with the new value.
         */
        UpdateValue(value: string) {
            this._value(value);
        }

        /**
         * Integrates the set of interfaces given with this node.
         */
        UpdateInterfaces(source: any) {
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
                var intf: Interface = self._interfaces[name];
                if (intf == null) {
                    intf = self._interfaces[name] = new Interface(self, name, source);
                    intf.PropertyChanged.add((node, intf, property, value) => {
                        self.PropertyChanged.trigger(node, intf, property, value);
                    });
                    intf.MethodInvoked.add((node, intf, method, params) => {
                        self.MethodInvoked.trigger(node, intf, method, params);
                    });
                } else {
                    intf.Update(source);
                }
            } catch (ex) {
                ex.message = "Node.UpdateInterface()" + '\nMessage: ' + ex.message;
                throw ex;
            }
        }

        /**
         * Integrates the set of content nodes with the given object values.
         */
        UpdateNodes(sources: Array<any>) {
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
                        var v = new Node(sources[i]);
                        v.PropertyChanged.add((n, intf, property, value) => {
                            self.PropertyChanged.trigger(n, intf, property, value);
                        });
                        v.MethodInvoked.add((n, intf, method, params) => {
                            self.MethodInvoked.trigger(n, intf, method, params);
                        });
                        self._nodes.push(v);
                    } else {
                        self._nodes()[i].Update(sources[i]);
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

            for (var i in self._interfaces)
                r[<string>i] = self._interfaces[<string>i].ToData();

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