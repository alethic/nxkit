/// <reference path="TypedEvent.ts" />
/// <reference path="Property.ts" />

module NXKit.Web {

    export class Node {

        _type: string;
        _baseTypes: string[];
        _properties: IPropertyMap;
        _interfaces: IInterfaceMap;
        _nodes: KnockoutObservableArray<Node>;

        /**
         * Raised when the node has changes to be pushed to the server.
         */
        public ValueChanged: INodePropertyValueChangedEvent = new TypedEvent();

        /**
         * Raised when the node has methods to be invoked on the server.
         */
        public MethodInvoked: INodeMethodInvokedEvent = new TypedEvent();

        /**
         * Initializes a new instance from the given initial data.
         */
        constructor(source: any) {
            this._type = null;
            this._baseTypes = new Array<string>();
            this._properties = new PropertyMap();
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
         * Gets the type of this node.
         */
        public get Type(): string {
            return this._type;
        }

        /**
         * Gets the inheritence hierarchy of this node.
         */
        public get BaseTypes(): string[] {
            return this._baseTypes;
        }

        /**
         * Gets the interactive properties of this node.
         */
        public get Properties(): IPropertyMap {
            return this._properties;
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
         * Invokes a named method.
         */
        public Invoke(interfaceName: string, methodName: string, params: any): void {
            var i = this._interfaces[interfaceName];
            if (i == null)
                throw new Error('Unknown interface');

            var m = i.Methods[methodName];
            if (m == null)
                throw new Error('Unknown method.');

            m.Invoke(params);
        }

        /**
         * Integrates the data given by the node parameter into this node.
         */
        public Update(source: any) {
            this.UpdateType(source.Type);
            this.UpdateBaseTypes(source.BaseTypes);
            this.UpdateProperties(source.Properties);
            this.UpdateInterfaces(source);
            this.UpdateNodes(source.Nodes);
        }

        /**
         * Updates the type of this node with the new value.
         */
        UpdateType(type: string) {
            this._type = type;
        }

        /**
         * Updates the base types of this node with the new set of values.
         */
        UpdateBaseTypes(baseTypes: string[]) {
            this._baseTypes = baseTypes;
        }

        /**
         * Integrates the set of properties given with this node.
         */
        UpdateProperties(source: any) {
            for (var i in source) {
                this.UpdateProperty(<string>i, source[<string>i]);
            }
        }

        /**
         * Updates the property given by the specified name with the specified value.
         */
        UpdateProperty(name: string, source: any) {
            var self = this;
            var prop: Property = self._properties[name];
            if (prop == null) {
                prop = self._properties[name] = new Property(source);
                prop.ValueChanged.add(_ => {
                    self.OnValueChanged(self, _);
                });
            } else {
                prop.Update(source);
            }
        }

        /**
         * Integrates the set of interfaces given with this node.
         */
        UpdateInterfaces(source: any) {
            for (var i in source) {
                if (i.indexOf('.') > -1)
                    this.UpdateInterface(<string>i, source[<string>i]);
            }
        }

        /**
         * Updates the property given by the specified name with the specified value.
         */
        UpdateInterface(name: string, source: any) {
            var self = this;
            var intf: Interface = self._interfaces[name];
            if (intf == null) {
                intf = self._interfaces[name] = new Interface(name, source);
                intf.PropertyChanged.add(_ => {
                    self.OnValueChanged(self, _);
                });
                intf.MethodInvoked.add((_, method, params) => {
                    self.OnMethodInvoked(_, method, params);
                });
            } else {
                intf.Update(source);
            }
        }

        /**
         * Integrates the set of content nodes with the given object values.
         */
        UpdateNodes(sources: Array<any>) {
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
                    v.ValueChanged.add((_, __) => {
                        self.OnValueChanged(_, __);
                    });
                    self._nodes.push(v);
                } else {
                    self._nodes()[i].Update(sources[i]);
                }
            }

            // delete trailing values
            if (self._nodes().length > sources.length)
                self._nodes.splice(sources.length);
        }

        public ToData(): any {
            var r: any = {
                Type: this._type,
                BaseTypes: this._baseTypes,
                Properties: this.PropertiesToData(),
                Nodes: this.NodesToData(),
            };

            for (var i in this._interfaces)
                r[<string>i] = this._interfaces[<string>i].ToData();

            return r;
        }

        /**
         * Transforms the given Property array into a list of data to push.
         */
        PropertiesToData(): any {
            var l: any = {};
            for (var p in this._properties) {
                l[<string>p] = this._properties[<string>p].ToData();
            }
            return l;
        }

        /**
         * Transforms the given Property array into a list of data to push.
         */
        NodesToData(): any[] {
            return ko.utils.arrayMap(this._nodes(), v => {
                return v.ToData();
            });
        }

        /**
         * Initiates a push of new values to the server.
         */
        OnValueChanged(node: Node, property: Property) {
            this.ValueChanged.trigger(node, property);
        }

        /**
         * Initiates a push of method invocations to the server.
         */
        OnMethodInvoked($interface: Interface, method: Method, params: any) {
            this.MethodInvoked.trigger(this, $interface, method, params);
        }

    }

}