/// <reference path="TypedEvent.ts" />

module NXKit.Web {

    export class Property {

        private _name: string;
        private _value: KnockoutObservable<any>;
        private _suspend: boolean = false;

        private _valueAsString: KnockoutComputed<string>;
        private _valueAsBoolean: KnockoutComputed<boolean>;
        private _valueAsNumber: KnockoutComputed<number>;
        private _valueAsDate: KnockoutComputed<Date>;

        /**
         * Raised when the Property's value has changed.
         */
        public PropertyChanged: IPropertyChangedEvent = new TypedEvent();

        constructor(name: string, source: any) {
            var self = this;

            self._name = name;

            self._value = ko.observable<any>();
            self._value.subscribe(_ => {
                if (!self._suspend) {
                    self.PropertyChanged.trigger(self, self._value());
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
            var self = this;
            if (self._value() !== source) {
                self._suspend = true;
                self._value(source);
                self._suspend = false;
            }
        }

        public ToData(): any {
            return this._value();
        }

    }

}