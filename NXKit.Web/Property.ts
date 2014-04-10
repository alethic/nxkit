/// <reference path="TypedEvent.ts" />

module NXKit.Web {

    export class Property {

        private _interface: Interface;
        private _name: string;
        private _value: KnockoutObservable<any>;
        private _suspend: boolean = false;

        private _valueAsString: KnockoutComputed<string>;
        private _valueAsBoolean: KnockoutComputed<boolean>;
        private _valueAsNumber: KnockoutComputed<number>;
        private _valueAsDate: KnockoutComputed<Date>;

        /**
         * Raised when this property, or a nested property's value changes.
         */
        public PropertyChanged: INodePropertyChangedEvent = new TypedEvent();

        /**
         * Raised when a nested node method is invoked.
         */
        public MethodInvoked: INodeMethodInvokedEvent = new TypedEvent();

        constructor($interface: Interface, name: string, source: any) {
            var self = this;

            self._interface = $interface;
            self._name = name;

            self._value = ko.observable<any>(null);
            self._value.subscribe(_ => {
                if (!self._suspend) {
                    self.PropertyChanged.trigger(self._interface.Node, self._interface, self, self._value());
                }
            });

            self._valueAsString = ko.computed({
                read: () => {
                    var s = self._value() != null ? String(self._value()).trim() : null;
                    return s ? s : null;
                },
                write: (value: string) => {
                    var s = value != null ? value.trim() : null;
                    return self._value(s ? s : null);
                },
            });

            self._valueAsBoolean = ko.computed({
                read: () => {
                    return self._value() === true || self._value() === 'true' || self._value() === 'True';
                },
                write: (value: boolean) => {
                    self._value(value ? 'true' : 'false');
                },
            });

            self._valueAsNumber = ko.computed({
                read: () => {
                    return self._value() != '' ? parseFloat(self._value()) : null;
                },
                write: (value: number) => {
                    self._value(value != null ? value.toString() : null);
                },
            });

            self._valueAsDate = ko.computed({
                read: () => {
                    return self._value() != null ? new Date(self._value()) : null;
                },
                write: (value: Date) => {
                    if (value instanceof Date)
                        self._value(value.toDateString());
                    else if (typeof (value) === 'string')
                        self._value(value != null ? new Date(<string><any>value) : null);
                    else
                        self._value(null);
                },
            });

            if (source != null)
                self.Update(source);
        }

        public get Name(): string {
            return this._name;
        }

        public get Value(): KnockoutObservable<any> {
            return this._value;
        }

        public get ValueAsString(): KnockoutComputed<string> {
            return this._valueAsString;
        }

        public get ValueAsBoolean(): KnockoutComputed<boolean> {
            return this._valueAsBoolean;
        }

        public get ValueAsNumber(): KnockoutComputed<number> {
            return this._valueAsNumber;
        }

        public get ValueAsDate(): KnockoutComputed<Date> {
            return this._valueAsDate;
        }

        public Update(source: any) {
            try {
                var self = this;

                if (source != null &&
                    source.Type != null &&
                    source.Type === 'Object') {
                    self._suspend = true;
                    if (self._value() != null &&
                        self._value() instanceof Node) {
                        (<Node>self._value()).Update(source);
                        console.log(self.Name + ': ' + 'Node' + '=>' + 'Node');
                    } else {
                        var node = new Node(source);
                        node.PropertyChanged.add((n, intf, property, value) => {
                            self.PropertyChanged.trigger(n, intf, property, value);
                        });
                        node.MethodInvoked.add((n, intf, method, params) => {
                            self.MethodInvoked.trigger(n, intf, method, params);
                        });
                        self._value(node);
                        console.log(self.Name + ': ' + 'Node' + '+>' + 'Node');
                    }
                    self._suspend = false;

                    return;
                }

                var old = self._value();
                if (old !== source) {
                    self._suspend = true;
                    self._value(source);
                    console.log(self.Name + ': ' + old + '=>' + source);
                    self._suspend = false;
                }
            } catch (ex) {
                ex.message = "Property.Update()" + '\nMessage: ' + ex.message;
                throw ex;
            }
        }

        public ToData(): any {
            return this._value();
        }

    }

}