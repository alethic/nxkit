/// <reference path="Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="Scripts/typings/knockout/knockout.d.ts" />

module NXKit.Web {

    export interface IVisualPropertyValueChangedEvent extends IEvent {
        add(listener: (visual: Visual, property: Property) => void): void;
        remove(listener: (visual: Visual, property: Property) => void): void;
        trigger(visual: Visual, property: Property): void;
    }

    export interface IPropertyValueChangedEvent extends IEvent {
        add(listener: (property: Property) => void): void;
        remove(listener: (property: Property) => void): void;
        trigger(property: Property): void;
    }

    export interface ICallbackRequestEvent extends IEvent {
        add(listener: (data: any) => void): void;
        remove(listener: (data: any) => void): void;
        trigger(data: any): void;
    }

    export class VisualViewModel {

        private _context: KnockoutBindingContext;
        private _visual: Visual;

        constructor(context: KnockoutBindingContext, visual: Visual) {
            var self = this;
            self._context = context;
            self._visual = visual;
        }

        get Context(): KnockoutBindingContext {
            return this._context;
        }

        get Visual(): Visual {
            return this._visual;
        }

    }

    export interface PropertyList {
        [name: string]: Property;
    }

    export class Property {

        private _value: KnockoutObservable<any>;
        private _version: KnockoutObservable<number>;

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

    export class Visual {

        _type: string;
        _baseTypes: string[];
        _properties: Property[];
        _visuals: KnockoutObservableArray<Visual>;

        /**
         * Raised when the Visual has changes to be pushed to the server.
         */
        public ValueChanged: IVisualPropertyValueChangedEvent = new TypedEvent();

        /**
         * Initializes a new instance from the given initial data.
         */
        constructor(source: any) {
            this._type = null;
            this._baseTypes = new Array<string>();
            this._properties = new Array<Property>();
            this._visuals = ko.observableArray<Visual>();

            // update from source data
            if (source != null)
                this.Update(source);
        }

        /**
         * Gets the type of this visual.
         */
        public get Type(): string {
            return this._type;
        }

        /**
         * Gets the inheritence hierarchy of this visual.
         */
        public get BaseTypes(): string[] {
            return this._baseTypes;
        }

        /**
         * Gets the interactive properties of this visual.
         */
        public get Properties(): any {
            return this._properties;
        }

        /**
         * Gets the content of this visual.
         */
        public get Visuals(): KnockoutObservableArray<Visual> {
            return this._visuals;
        }

        /**
         * Integrates the data given by the visual parameter into this Visual.
         */
        public Update(source: any) {
            this.UpdateType(source.Type);
            this.UpdateBaseTypes(source.BaseTypes);
            this.UpdateProperties(source.Properties);
            this.UpdateVisuals(source.Visuals);
        }

        /**
         * Updates the type of this Visual with the new value.
         */
        UpdateType(type: string) {
            this._type = type;
        }

        /**
         * Updates the base types of this Visual with the new set of values.
         */
        UpdateBaseTypes(baseTypes: string[]) {
            this._baseTypes = baseTypes;
        }

        /**
         * Integrates the set of properties given with this Visual.
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
         * Integrates the set of content Visuals with the given object values.
         */
        UpdateVisuals(sources: Array<any>) {
            var self = this;

            // clear visuals if none
            if (sources == null) {
                self._visuals.removeAll();
                return;
            }

            // update or insert new values
            for (var i = 0; i < sources.length; i++) {
                if (self._visuals().length < i + 1) {
                    var v = new Visual(sources[i]);
                    v.ValueChanged.add((_, __) => {
                        self.OnValueChanged(_, __);
                    });
                    self._visuals.push(v);
                } else {
                    self._visuals()[i].Update(sources[i]);
                }
            }

            // delete trailing values
            if (self._visuals().length > sources.length)
                self._visuals.splice(sources.length);
        }

        public ToData(): any {
            return {
                Type: this._type,
                BaseTypes: this._baseTypes,
                Properties: this.PropertiesToData(),
                Visuals: this.VisualsToData(),
            }
        }

        /**
         * Transforms the given Property array into a list of data to push.
         */
        PropertiesToData(): any {
            var l = {};
            for (var p in this._properties) {
                l[p] = this._properties[p].ToData();
            }
            return l;
        }

        /**
         * Transforms the given Property array into a list of data to push.
         */
        VisualsToData(): any[] {
            return ko.utils.arrayMap(this._visuals(), v => {
                return v.ToData();
            });
        }

        /**
         * Initiates a push of new values to the server.
         */
        OnValueChanged(visual: Visual, property: Property) {
            this.ValueChanged.trigger(visual, property);
        }

        /**
         * Gets the template that should be used to render this Visual.
         */
        public get Template(): any {
            // result standard template
            var node = document.getElementById(this._type);

            // attempt to resolve fall-back templates
            for (var i in this._baseTypes)
                if (node == null)
                    node = document.getElementById(this._baseTypes[i]);

            // no template found, invent an error
            if (node == null)
                node = $('<script />', {
                    'type': 'text/html',
                    'id': this._type,
                    'text': '<p>no template for ' + this._type + '</p>',
                }).appendTo('body')[0];

            return node.id;
        }

    }

    export class View {

        _body: HTMLElement;
        _data: any;
        _root: Visual;
        _bind: boolean;

        /**
         * Raised when the Visual has changes to be pushed to the server.
         */
        public CallbackRequest: ICallbackRequestEvent = new TypedEvent();

        constructor(body: HTMLElement) {
            this._body = body;
            this._data = null;
            this._root = null;
            this._bind = true;
        }

        get Body(): HTMLElement {
            return this._body;
        }

        set Body(value: HTMLElement) {
            this._body = value;
        }

        get Data(): any {
            return this._data;
        }

        set Data(value: any) {
            var self = this;

            if (typeof (value) === 'string')
                self._data = JSON.parse(<string>value);
            else
                self._data = value;

            // raise the value changed event
            self.Update();
        }

        /**
         * Initiates a refresh of the view model.
         */
        Update() {
            var self = this;

            if (self._root == null)
                // generate new visual tree
                self._root = new Visual(self._data);
            else
                // update existing visual tree
                self._root.Update(self._data);

            self._root.ValueChanged.add((_, __) => self.OnCallbackRequest());
            self.ApplyBindings();
        }

        /**
         * Invoked when the view model initiates a request to push updates.
         */
        OnCallbackRequest() {
            var self = this;

            self.CallbackRequest.trigger(self._root.ToData());
        }

        /**
         * Applies the bindings to the view if possible.
         */
        ApplyBindings() {
            // apply bindings to our element and our view model
            if (this._bind &&
                this._body != null &&
                this._root != null) {

                // clear existing bindings
                ko.cleanNode(
                    this._body);

                // apply knockout to view node
                ko.applyBindings(
                    this._root,
                    this._body);

                this._bind = false;
            }

        }

    }

}
