/// <reference path="TypedEvent.ts" />

module NXKit.Web {

    export interface IPropertyValueChangedEvent extends IEvent {
        add(listener: (property: Property) => void): void;
        remove(listener: (property: Property) => void): void;
        trigger(property: Property): void;
    }

    export class Property {

        private _value: KnockoutObservable<any>;
        private _version: KnockoutObservable<number>;

        private _valueAsString: KnockoutComputed<string>;
        private _valueAsBoolean: KnockoutComputed<boolean>;
        private _valueAsNumber: KnockoutComputed<number>;
        private _valueAsDate: KnockoutComputed<Date>;

        /**
         * Raised when the Property's value has changed.
         */
        public ValueChanged: IPropertyValueChangedEvent = new TypedEvent();

        constructor(source: any) {
            var self = this;

            self._value = ko.observable<any>();
            self._value.subscribe(_ => {
                // version is set below zero when integrating changes
                if (self._version() >= 0) {
                    self._version(self._version() + 1);
                    self.ValueChanged.trigger(self);
                }
            });

            self._version = ko.observable<number>();
            self._version.subscribe(_ => {

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
                read: function () {
                    return self._value() === true || self._value() == 'true' || self._value() == 'True';
                },
                write: function (value: boolean) {
                    self._value(value === true ? "true" : "false");
                },
            });

            self._valueAsBoolean = ko.computed({
                read: function () {
                    return self._value() === true || self._value() == 'true' || self._value() == 'True';
                },
                write: function (value: boolean) {
                    self._value(value === true ? "true" : "false");
                },
            });

            self._valueAsNumber = ko.computed({
                read: function () {
                    return self._value() != '' ? parseFloat(self._value()) : null;
                },
                write: function (value: number) {
                    self._value(value != null ? value.toString() : null);
                },
            });

            self._valueAsDate = ko.computed({
                read: function () {
                    return self._value() != null ? new Date(self._value()) : null;
                },
                write: function (value: Date) {
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

        public get Version(): KnockoutObservable<number> {
            return this._version
        }

        public Update(source: any) {
            var self = this;
            if (self._value() !== source.Value) {
                self._version(-1);
                self._value(source.Value);
                self._version(0);
            }
        }

        public ToData(): any {
            return {
                Value: this.Value(),
                Version: this.Version(),
            }
        }

    }

}