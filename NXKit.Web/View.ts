/// <reference path="Visual.ts" />
/// <reference path="TypedEvent.ts" />

module NXKit.Web {

    export class View {

        _body: HTMLElement;
        _data: any;
        _root: Visual;
        _bind: boolean;

        private _onVisualValueChanged: (visual: Visual, property: Property) => void;

        private _queue: any[];
        private _queueRunning: boolean;

        /**
         * Raised when the Visual has changes to be pushed to the server.
         */
        public CallbackRequest: ICallbackRequestEvent = new TypedEvent();

        constructor(body: HTMLElement) {
            var self = this;

            self._body = body;
            self._data = null;
            self._root = null;
            self._bind = true;

            self._queue = new Array<any>();
            self._queueRunning = false;

            self._onVisualValueChanged = (visual: Visual, property: Property) => {
                self.OnRootVisualValueChanged(visual, property);
            };
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

            if (self._root == null) {
                // generate new visual tree
                self._root = new Visual(self._data);
                self._root.ValueChanged.add(self._onVisualValueChanged);
            }
            else {
                // update existing visual tree
                self._root.ValueChanged.remove(self._onVisualValueChanged);
                self._root.Update(self._data);
                self._root.ValueChanged.add(self._onVisualValueChanged);
            }

            self.ApplyBindings();
        }

        /**
         * Invoked to handle root visual value change events.
         */
        OnRootVisualValueChanged(visual: Visual, property: Property) {
            this.Push();
        }

        /**
         * Invoked when the view model initiates a request to push updates.
         */
        Push() {
            var self = this;

            this.Queue(function (cb: ICallbackComplete) {
                self.CallbackRequest.trigger(self._root.ToData(), cb);
            });
        }

        /**
         * Runs any available items in the queue.
         */
        Queue(func: (cb: ICallbackComplete) => void) {
            var self = this;

            // pushes a new event to trigger a callback onto the queue
            self._queue.push(func);

            // only one runner at a time
            if (self._queueRunning) {
                return;
            } else {
                self._queueRunning = true;

                // recursive call to work queue
                var l = () => {
                    var f = self._queue.shift();
                    if (f) {
                        f((result: any) => {
                            l(); // recurse
                        });
                    } else {
                        self._queueRunning = false;
                    }ue
                };

                // initiate queue run
                l();
            }
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
