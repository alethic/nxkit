/// <reference path="Node.ts" />
/// <reference path="TypedEvent.ts" />

module NXKit.Web {

    /**
     * Main NXKit client-side view class. Injects the view interface into a set of HTML elements.
     */
    export class View {

        private _body: HTMLElement;
        private _server: IServerInvoke;

        private _save: string;
        private _hash: string;
        private _root: Node;
        private _bind: boolean;
        private _messages: KnockoutObservableArray<Message>;
        private _threshold: Severity;

        private _queue: any[];
        private _queueRunning: boolean;

        private _busy: KnockoutObservable<boolean>;

        constructor(body: HTMLElement, server: IServerInvoke) {
            var self = this;

            self._server = server;
            self._body = body;
            self._save = null;
            self._hash = null;
            self._root = null;
            self._bind = true;

            self._messages = Util.ObservableArray<Message>();
            self._threshold = Severity.Warning;

            self._queue = new Array<any>();
            self._queueRunning = false;
            self._busy = ko.observable(false);
        }

        public get Busy(): KnockoutObservable<boolean> {
            return this._busy;
        }

        public get Body(): HTMLElement {
            return this._body;
        }

        public get Data(): any {
            return {
                Save: this._save,
                Hash: this._hash,
            };
        }

        public get Root(): Node {
            return this._root;
        }

        public get Messages(): KnockoutObservableArray<Message> {
            return this._messages;
        }

        public get Threshold(): Severity {
            return this._threshold;
        }

        public set Threshold(threshold: Severity) {
            this._threshold = threshold;
        }

        /**
         * Updates the view in response to a received message.
         */
        public Receive(args: any) {
            this._save = args['Save'] || this._save;
            this._hash = args['Hash'] || this._hash;

            var data = args['Data'] || null;
            if (data != null) {
                this.ReceiveData(data);
            }
        }

        /**
         * Updates the view in response to a received data package.
         */
        public ReceiveData(data: any) {
            if (data != null) {
                this.Apply(data['Node'] || null);
                this.AppendMessages(data['Messages'] || []);
                this.ExecuteScripts(data['Scripts'] || []);
            }
        }

        /**
         * Updates the messages of the view with the specified items.
         */
        private AppendMessages(messages: any[]) {
            var self = this;

            for (var i = 0; i < messages.length; i++) {

                var severity = <Severity>((<any>Severity)[<string>(messages[i].Severity)]);
                var text = messages[i].Text || '';
                if (severity >= self._threshold)
                    self._messages.push(new Message(severity, text));
            }
        }

        /**
         * Executes the given scripts.
         */
        private ExecuteScripts(scripts: string[]) {
            for (var i = 0; i < scripts.length; i++) {
                var script = scripts[i];
                if (script != null)
                    eval(script);
            }
        }

        /**
         * Removes the current message from the set of messages.
         */
        public RemoveMessage(message: Message) {
            this._messages.remove(message);
        }

        /**
         * Initiates a refresh of the view model.
         */
        private Apply(data: any) {
            var self = this;

            if (self._root == null) {
                // generate new node tree
                self._root = new Node(self, data);
            }
            else {
                // update existing node tree
                self._root.Apply(data);
            }

            self.ApplyBindings();
        }

        /**
         * Invoked when the view model initiates a request to push an update to a node.
         */
        PushUpdate(node: Node, $interface: Interface, property: Property, value: any) {
            var self = this;
            Log.Debug('View.PushUpdate');

            // generate push action
            var data = {
                Action: 'Update',
                Args: {
                    NodeId: node.Id,
                    Interface: $interface.Name,
                    Property: property.Name,
                    Value: value,
                }
            };

            self.Queue(data);
        }

        PushInvoke(node: Node, interfaceName: string, methodName: string, params: any) {
            var self = this;
            Log.Debug('View.PushInvoke');

            // generate push action
            var data = {
                Action: 'Invoke',
                Args: {
                    NodeId: node.Id,
                    Interface: interfaceName,
                    Method: methodName,
                    Params: params,
                }
            };

            self.Queue(data);
        }

        /**
         * Queues the given data to be sent to the server.
         */
        Queue(command: any) {
            var self = this;

            // pushes a new action onto the queue
            self._queue.push(command);

            // only one runner at a time
            if (self._queueRunning) {
                return;
            } else {
                self._queueRunning = true;

                // compile buffers of incoming data
                var node = {};
                var scripts = new Array<any>();
                var messages = new Array<any>();

                // delay processing in case of new commands
                setTimeout(() => {
                    self._busy(true);

                    // recursive call to work queue
                    var push = () => {
                        var commands = self._queue.splice(0);

                        // callback for server response
                        var cb = (args: any) => {
                            if (args.Code == 200) {

                                // receive saved state
                                var save = args.Save || null;
                                if (save != null) {
                                    this._save = save;
                                }

                                // receive saved state hash
                                var hash = args.Hash || null;
                                if (hash != null) {
                                    this._hash = hash;
                                }

                                // receive data response
                                var data = args.Data || null;
                                if (data != null) {
                                    // push new items into receive queue
                                    node = data['Node'] || null;
                                    ko.utils.arrayPushAll(scripts, <any[]>data['Scripts']);
                                    ko.utils.arrayPushAll(messages, <any[]>data['Messages']);

                                    // only update node data if no outstanding commands
                                    if (self._queue.length == 0) {
                                        self.ReceiveData({
                                            Node: node,
                                            Scripts: scripts,
                                            Messages: messages,
                                        });
                                    }
                                }

                                // recurse
                                push();
                            } else if (args.Code == 500) {
                                // resend with save data
                                self._server({
                                    Save: self._save,
                                    Hash: self._hash,
                                    Commands: commands,
                                }, cb);
                            } else {
                                throw new Error('unexpected response code');
                            }
                        };

                        if (commands.length > 0) {
                            // send commands
                            self._server({
                                Hash: self._hash,
                                Commands: commands,
                            }, cb);
                        } else {
                            // no commands, exit
                            self._queueRunning = false;
                            self._busy(false);
                        }
                    };

                    // begin processing queue
                    push();
                }, 50);
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

                // ensure body is to render template
                $(self._body)
                    .attr('data-bind', 'template: { name: \'NXKit.View\' }');

                // apply knockout to view node
                ko.applyBindings(
                    self,
                    self._body);

                self._bind = false;
            }

        }

    }

}
