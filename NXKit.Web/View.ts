/// <reference path="Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="Scripts/typings/knockout/knockout.d.ts" />

module NXKit.Web {

    export interface ICallbackRequestEvent extends IEvent {
        add(listener: (data: any) => void): void;
        remove(listener: (data: any) => void): void;
        trigger(data: any): void;
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
