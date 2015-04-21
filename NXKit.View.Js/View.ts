/// <reference path="Node.ts" />
/// <reference path="TypedEvent.ts" />

module NXKit.View {

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
            var self = this;

            self._save = args.Save || self._save;
            self._hash = args.Hash || self._hash;

            var node = args.Node || null;
            if (node != null) {
                self.ReceiveNode(node);
            }

            var commands = args.Commands || null;
            if (commands != null) {
                self.ReceiveCommands(commands);
            }
        }

        /**
         * Updates the view in response to a received data package.
         */
        public ReceiveNode(node: any) {
            var self = this;

            if (node != null) {
                self.Apply(node || null);
            }
        }

        /**
         * Updates the messages of the view with the specified items.
         */
        public ReceiveCommands(commands: any[]) {
            var self = this;

            for (var i = 0; i < commands.length; i++) {
                var command = commands[i];
                if (command != null) {

                    if (command.$type === 'NXKit.View.Server.Commands.Trace, NXKit.View.Server') {
                        if (command.Message != null) {
                            if (command.Message.Severity === 'Debug') {
                                Log.Debug(command.Message.Text);
                            } else if (command.Message.Severity === 'Information') {
                                Log.Information(command.Message.Text);
                            } else if (command.Message.Severity === 'Warning') {
                                Log.Warning(command.Message.Text);
                            } else if (command.Message.Severity === 'Error') {
                                Log.Error(command.Message.Text);
                            }
                        }
                    }

                    if (command.$type === 'NXKit.View.Server.Commands.Script, NXKit.View.Server') {
                        if (command.Code != null) {
                            eval(command.Code);
                        }
                    }

                }
            }
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

            // generate update command
            var command = {
                $type: 'NXKit.View.Server.Commands.Update, NXKit.View.Server',
                NodeId: node.Id,
                Interface: $interface.Name,
                Property: property.Name,
                Value: value,
            };

            self.Queue(command);
        }

        PushInvoke(node: Node, interfaceName: string, methodName: string, parameters: any) {
            var self = this;
            Log.Debug('View.PushInvoke');

            // generate push action
            var data = {
                $type: 'NXKit.View.Server.Commands.Invoke, NXKit.View.Server',

                NodeId: node.Id,
                Interface: interfaceName,
                Method: methodName,
                Parameters: parameters,
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
                var todo = new Array<any>();

                // delay processing in case of new commands
                setTimeout(() => {
                    self._busy(true);

                    // recursive call to work queue
                    var push = () => {
                        var send = self._queue.splice(0);

                        // callback for server response
                        var cb = (args: any) => {
                            if (args.Status == 200) {

                                // receive saved state hash
                                var hash = args.Hash || null;
                                if (hash != null) {
                                    this._hash = hash;
                                }

                                // receive saved state
                                var save = args.Save || null;
                                if (save != null) {
                                    this._save = save;
                                }

                                // buffer application of node
                                if (args.Node != null) {
                                    node = args.Node;
                                }

                                // buffer commands
                                if (args.Commands != null) {
                                    ko.utils.arrayPushAll(todo, <any[]>args.Commands);
                                }

                                // only update node data if no outstanding commands
                                if (self._queue.length == 0) {
                                    self.Receive({
                                        Hash: this._hash,
                                        Save: this._save,
                                        Node: node,
                                        Commands: todo,
                                    });
                                }

                                // recurse
                                push();
                            } else if (args.Status == 400) {
                                // resend with save data
                                self._server({
                                    Save: self._save,
                                    Hash: self._hash,
                                    Commands: send,
                                }, cb);
                            } else if (args.Status == 500) {
                                for (var i = 0; i < args.Errors.length; i++) {
                                    throw new Error(args.Errors[i].Message || "");
                                }
                            } else {
                                throw new Error('unexpected response code');
                            }
                        };

                        if (send.length > 0) {
                            // send commands
                            self._server({
                                Hash: self._hash,
                                Commands: send,
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

                // execute after deferral
                NXKit.require(['nx-html!nxkit.html'], () => {

                    // ensure body is to render template
                    $(self._body)
                        .attr('data-bind', 'template: { name: \'NXKit.View\' }');

                    // apply knockout to view node
                    ko.applyBindings(
                        self,
                        self._body);

                    self._bind = false;
                });
            }

        }

    }

}
