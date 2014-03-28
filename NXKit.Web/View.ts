/// <reference path="Node.ts" />
/// <reference path="TypedEvent.ts" />

module NXKit.Web {

    /**
     * Main NXKit client-side view class. Injects the view interface into a set of HTML elements.
     */
    export class View {

        private _body: HTMLElement;
        private _root: Node;
        private _bind: boolean;

        private _onNodeValueChanged: (node: Node, property: Property) => void;
        private _onNodeMethodInvoked: (node: Node, $interface: Interface, method: Method, params: any) => void;

        private _queue: Array<(cb: ICallbackComplete) => void>;
        private _queueRunning: boolean;

        /**
         * Raised when the Node has changes to be pushed to the server.
         */
        public CallbackRequest: ICallbackRequestEvent = new TypedEvent();

        constructor(body: HTMLElement) {
            var self = this;

            self._body = body;
            self._root = null;
            self._bind = true;

            self._queue = new Array<any>();
            self._queueRunning = false;

            self._onNodeValueChanged = (node: Node, property: Property) => {
                self.OnRootNodeValueChanged(node, property);
            };

            self._onNodeMethodInvoked = (node: Node, $interface: Interface, method: Method, params: any) => {
                self.OnRootNodeMethodInvoked(node, $interface, method, params);
            };
        }

        public get Body(): HTMLElement {
            return this._body;
        }

        public get Data(): any {
            return this._root.ToData();
        }

        public set Data(value: any) {
            var self = this;

            if (typeof (value) === 'string')
                // update the form with the parsed data
                self.Update(JSON.parse(<string>value));
            else
                // update the form with the data itself
                self.Update(value);
        }

        /**
         * Initiates a refresh of the view model.
         */
        private Update(data: any) {
            var self = this;

            if (self._root == null) {
                // generate new node tree
                self._root = new Node(data);
                self._root.ValueChanged.add(self._onNodeValueChanged);
                self._root.MethodInvoked.add(self._onNodeMethodInvoked);
            }
            else {
                // update existing node tree
                self._root.ValueChanged.remove(self._onNodeValueChanged);
                self._root.MethodInvoked.remove(self._onNodeMethodInvoked);
                self._root.Update(data);
                self._root.ValueChanged.add(self._onNodeValueChanged);
                self._root.MethodInvoked.add(self._onNodeMethodInvoked);
            }

            self.ApplyBindings();
        }

        /**
         * Invoked to handle root node value change events.
         */
        OnRootNodeValueChanged(node: Node, property: Property) {
            this.Push();
        }

        /**
         * Invoked to handle root node method invocations.
         */
        OnRootNodeMethodInvoked(node: Node, $interface: Interface, method: Method, params: any) {
            this.Push();
        }

        /**
         * Invoked when the view model initiates a request to push updates.
         */
        Push() {
            var self = this;

            this.Queue((cb: ICallbackComplete) => {
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
                    }
                };

                // initiate queue run
                l();
            }
        }

        /**
         * Applies the bindings to the view if possible.
         */
        ApplyBindings() {
            var self = this;

            // apply bindings to our element and our view model
            if (self._bind &&
                self._body != null &&
                self._root != null) {

                // clear existing bindings
                ko.cleanNode(
                    self._body);

                // apply knockout to view node
                ko.applyBindings(
                    self._root,
                    self._body);

                self._bind = false;
            }

        }

    }

}
