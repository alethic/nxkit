/// <reference path="Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="Scripts/typings/knockout/knockout.d.ts" />

module NXKit.Web {

    export interface IPushRequestEvent extends IEvent {
        add(listener: (visual: Visual) => void): void;
        remove(listener: (visual: Visual) => void): void;
        trigger(data: any): void;
    }

    export interface IPropertyValueChangedEvent extends IEvent {
        add(listener: (property: Property) => void): void;
        remove(listener: (property: Property) => void): void;
        trigger(property: Property): void;
    }

    export interface PropertyList {
        [name: string]: Property;
    }

    export class Property {

        _value: KnockoutObservable<any>;
        _version: KnockoutObservable<number>;

        /**
         * Raised when the Property's value has changed.
         */
        public onValueChanged: IPropertyValueChangedEvent = new TypedEvent();

        constructor(source: any) {
            var self = this;

            self._value = ko.observable<any>();
            self._value.subscribe(_ => {
                // version is set below zero when integrating changes
                if (self._version() >= 0) {
                    self._version(self._version() + 1);
                    self.onValueChanged.trigger(self);
                }
            });

            self._version = ko.observable<number>();
            self._version.subscribe(_ => {
                console.debug('version+1');
            });

            if (source != null)
                self.update(source);
        }

        get value(): KnockoutObservable<any> {
            return this._value;
        }

        get version(): KnockoutObservable<number> {
            return this._version
        }

        public update(source: any) {
            var self = this;
            if (self._value() !== source.Value) {
                self._version(-1);
                self._value(source.Value);
                self._version(0);
            }
        }

        public toData(): any {
            return {
                Value: this.value(),
                Version: this.version(),
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
        public onPushRequest: IPushRequestEvent = new TypedEvent();

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
                this.update(source);
        }

        /**
         * Gets the type of this visual.
         */
        get type(): string {
            return this._type;
        }

        /**
         * Gets the inheritence hierarchy of this visual.
         */
        get baseTypes(): string[] {
            return this._baseTypes;
        }

        /**
         * Gets the interactive properties of this visual.
         */
        get properties(): any {
            return this._properties;
        }

        /**
         * Gets the content of this visual.
         */
        get visuals(): KnockoutObservableArray<Visual> {
            return this._visuals;
        }

        /**
         * Integrates the data given by the visual parameter into this Visual.
         */
        update(visual: any) {
            this.updateType(visual.Type);
            this.updateBaseTypes(visual.BaseTypes);
            this.updateProperties(visual.Properties);
            this.updateVisuals(visual.Visuals);
        }

        /**
         * Updates the type of this Visual with the new value.
         */
        updateType(type: string) {
            this._type = type;
        }

        /**
         * Updates the base types of this Visual with the new set of values.
         */
        updateBaseTypes(baseTypes: string[]) {
            this._baseTypes = baseTypes;
        }

        /**
         * Integrates the set of properties given with this Visual.
         */
        updateProperties(source: any) {
            for (var i in source) {
                this.updateProperty(<string>i, source[<string>i]);
            }
        }

        /**
         * Updates the property given by the specified name with the specified value.
         */
        updateProperty(name: string, source: any) {
            var self = this;
            var prop: Property = self._properties[name];
            if (prop == null) {
                prop = self._properties[name] = new Property(source);
                prop.onValueChanged.add(_ => {
                    self.pushRequest(self);
                });
            } else {
                prop.update(source);
            }
        }

        /**
         * Integrates the set of content Visuals with the given object values.
         */
        updateVisuals(sources: Array<any>) {
            // insert new visuals
            // TODO merge into list
            for (var source in sources) {
                var v = new Visual(sources[source]);
                v.onPushRequest.add(_ => {
                    this.pushRequest(_);
                });
                this._visuals.push(v);
            }
        }

        public toData(): any {
            return {
                Type: this._type,
                BaseTypes: this._baseTypes,
                Properties: this.propertiesToData(),
                Visuals: this.visualsToData(),
            }
        }

        /**
         * Transforms the given Property array into a list of data to push.
         */
        propertiesToData(): any {
            var l = {};
            for (var p in this._properties) {
                l[p] = this._properties[p].toData();
            }
            return l;
        }

        /**
         * Transforms the given Property array into a list of data to push.
         */
        visualsToData(): any[] {
            return ko.utils.arrayMap(this._visuals(), v => {
                return v.toData();
            });
        }

        /**
         * Initiates a push of new values to the server.
         */
        pushRequest(visual: Visual) {
            this.onPushRequest.trigger(visual);
        }

        /**
         * Gets the template that should be used to render this Visual.
         */
        get template(): any {
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
        public onPushRequest: IPushRequestEvent = new TypedEvent();

        constructor(body: HTMLElement) {
            this._body = body;
            this._data = null;
            this._root = null;
            this._bind = true;
        }

        get body(): HTMLElement {
            return this._body;
        }

        set body(value: HTMLElement) {
            this._body = value;
        }

        get data(): any {
            return this._data;
        }

        set data(value: any) {
            var self = this;

            if (typeof (value) === 'string')
                self._data = JSON.parse(<string>value);
            else
                self._data = value;

            // raise the value changed event
            self._refresh();
        }

        /**
         * Initiates a refresh of the view model.
         */
        _refresh() {
            console.debug('_onModelChangedHandler');

            var self = this;
            self._root = new Visual(self._data);
            self._root.onPushRequest.add(_ => self._onPushRequest(_));
            self._applyBindings();
        }

        /**
         * Invoked when the view model initiates a request to push updates.
         */
        _onPushRequest(visual: Visual) {
            var self = this;

            self.onPushRequest.trigger(self._root.toData());
        }

        /**
         * Applies the bindings to the view if possible.
         */
        _applyBindings() {
            // apply bindings to our element and our view model
            if (this._body != null &&
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
