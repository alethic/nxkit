/// <reference path="Scripts/typings/knockout/knockout.d.ts" />

module NXKit.Web {

    export interface IVisualPropertyValueChangedEvent extends IEvent {
        add(listener: (visual: Visual, property: Property) => void): void;
        remove(listener: (visual: Visual, property: Property) => void): void;
        trigger(visual: Visual, property: Property): void;
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

}