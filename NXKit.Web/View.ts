/// <reference path="Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="Scripts/typings/knockout/knockout.d.ts" />

module NXKit.Web {

    export interface IPushRequestEvent extends IEvent {
        add(listener: (visual: Visual, name: string, value: any) => void): void;
        remove(listener: (visual: Visual, name: string, value: any) => void): void;
        trigger(visual: Visual, name: string, value: any): void;
    }

    export class Visual {

        _type: string;
        _baseTypes: string[];
        _properties: any;
        _visuals: KnockoutObservableArray<any>;

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
            this._properties = {};
            this._visuals = ko.observableArray();

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
        updateProperty(name: string, value: any) {
            if (this._properties[name] == undefined) {
                // create new observable and subscribe to changes
                var o = ko.observable(value);
                o.subscribe((v) => {
                    this.pushRequest(this, name, this._properties[name]());
                });
                this._properties[name] = o;
            }
            else
                this._properties[name](value);
        }

        /**
         * Integrates the set of content Visuals with the given object values.
         */
        updateVisuals(sources: Array<any>) {
            // insert new visuals
            // TODO merge into list
            for (var source in sources) {
                var v = new Visual(sources[source]);
                v.onPushRequest.add((visual, name, value) => {
                    this.pushRequest(visual, name, value);
                });
                this._visuals.push(v);
            }
        }

        /**
         * Initiates a push of new values to the server.
         */
        pushRequest(visual: Visual, name: string, value: any) {
            this.onPushRequest.trigger(visual, name, value);
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
            if (typeof (value) === 'string')
                this._data = JSON.parse(<string>value);
            else
                this._data = value;

            // raise the value changed event
            this._refresh();
        }

        /**
         * Initiates a refresh of the view model.
         */
        _refresh() {
            console.debug('_onModelChangedHandler');

            var self = this;
            self._root = new Visual(self._data);
            self._root.onPushRequest.add((visual: Visual, name: string, value: any) => self._onPushRequest(visual, name, value));
            self._applyBindings();
        }

        /**
         * Invoked when the view model initiates a request to push updates.
         */
        _onPushRequest(visual: Visual, name: string, value: any) {
            this.onPushRequest.trigger(visual, name, value);
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
