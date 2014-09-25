/// <reference path="TypedEvent.ts" />

module NXKit.View {

    export class Property {

        private _intf: Interface;
        private _name: string;
        private _suspend: boolean = false;

        private _value: KnockoutObservable<any>;
        private _valueAsString: KnockoutComputed<string>;
        private _valueAsBoolean: KnockoutComputed<boolean>;
        private _valueAsNumber: KnockoutComputed<number>;
        private _valueAsDate: KnockoutComputed<Date>;

        constructor(intf: Interface, name: string, source: any) {
            var self = this;

            self._intf = intf;
            self._name = name;

            self._value = ko.observable<any>(null);
            self._value.subscribe(_ => {
                if (!self._suspend) {
                    self.OnUpdate();
                }
            });

            self._valueAsString = Util.Computed({
                read: () => {
                    var s = self._value() != null ? String(self._value()).trim() : null;
                    return s ? s : null;
                },
                write: (value: string) => {
                    var s = value != null ? value.trim() : null;
                    return self._value(s ? s : null);
                },
            });

            self._valueAsBoolean = Util.Computed({
                read: () => {
                    return self._value() === true || self._value() === 'true' || self._value() === 'True';
                },
                write: (value: boolean) => {
                    self._value(value ? 'true' : 'false');
                },
            });

            self._valueAsNumber = Util.Computed({
                read: () => {
                    return self._value() != '' ? parseFloat(self._value()) : null;
                },
                write: (value: number) => {
                    self._value(value != null ? value.toString() : null);
                },
            });

            self._valueAsDate = Util.Computed({
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

        public get Interface(): Interface {
            return this._intf;
        }

        public get Node(): Node {
            return this._intf.Node;
        }

        public get View(): View {
            return this._intf.View;
        }

        public get Name(): string {
            return this._name;
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
                        (<Node>self._value()).Apply(source);
                        Log.Debug(self.Name + ': ' + 'Node' + '=>' + 'Node');
                    } else {
                        var node = new Node(self._intf.View, source);
                        self._value(node);
                        Log.Debug(self.Name + ': ' + 'Node' + '+>' + 'Node');
                    }
                    self._suspend = false;

                    return;
                }

                var old = self._value();
                if (old !== source) {
                    self._suspend = true;
                    self._value(source);
                    Log.Debug(self.Name + ': ' + old + '=>' + source);
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

        public OnUpdate() {
            this.View.PushUpdate(this.Node, this.Interface, this, this._value());
        }

    }

}