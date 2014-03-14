/// <reference path="Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="Scripts/typings/knockout/knockout.d.ts" />

module NXKit.Web {

    export class Visual {

        _type: string;
        _baseTypes: string[];
        _properties: any;
        _visuals: KnockoutObservableArray<any>;

        constructor(visual: any) {
            this._type = null;
            this._baseTypes = new Array<string>();
            this._properties = {};
            this._visuals = ko.observableArray();

            // update from source data
            if (visual != null)
                this.update(visual);
        }

        get type(): string {
            return this._type;
        }

        get baseTypes(): string[] {
            return this._baseTypes;
        }

        get properties(): any {
            return this._properties;
        }

        get visuals(): KnockoutObservableArray<Visual> {
            return this._visuals;
        }

        update(visual: any) {
            console.debug('update');

            this.updateType(visual.Type);
            this.updateBaseTypes(visual.BaseTypes);
            this.updateProperties(visual.Properties);
            this.updateVisuals(visual.Visuals);
        }

        private updateType(type: string) {
            console.debug('updateType');
            this._type = type;
        }

        private updateBaseTypes(baseTypes: string[]) {
            console.debug('updateBaseTypes');
            this._baseTypes = baseTypes;
        }

        private updateProperties(properties: any) {
            console.debug('updateProperties');

            // update each property with new observable
            // TODO update existing observable
            for (var i in properties) {
                if (this._properties[i] == undefined)
                    this._properties[i] = ko.observable(properties[i]);
                else
                    this._properties[i](properties[i]);
            }
        }

        private updateVisuals(visuals: Array<any>) {
            console.debug('updateVisuals');

            // insert new visuals
            // TODO merge into list
            for (var i in visuals) {
                this._visuals.push(new Visual(visuals[i]));
            }
        }

        get template(): any {
            return this._getTemplate();
        }

        _getTemplate() {

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

            return this._type;
        }

    }

    export class View {

        _element: HTMLElement;
        _model: any;
        _viewModel: Visual;

        public onVisualChanged: IEvent = new TypedEvent();

        constructor(element: HTMLElement) {
            this._element = element;
            this._model = null;
            this._viewModel = null;

            this.onVisualChanged.add(() => this._onVisualChangedHandler());
        }

        get element(): HTMLElement {
            return this._element;
        }

        set element(value: HTMLElement) {
            this._element = value;
        }

        get model(): any {
            return this._model;
        }

        set model(value: any) {
            if (typeof (value) === 'string')
                this._model = JSON.parse(<string>value);
            else
                this._model = value;

            // raise the value changed event
            this.onVisualChanged.trigger();
        }

        _onVisualChangedHandler() {
            console.debug('_onVisualChangedHandler');

            this._viewModel = new Visual(this._model);
            this._applyBindings();
        }

        _applyBindings() {
            // apply bindings to our element and our view model
            if (this._element != null &&
                this._viewModel != null) {

                // apply knockout to view node
                ko.applyBindings(
                    this._viewModel,
                    this._element);
            }
        }

    }

}
