/// <reference path="Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="Scripts/typings/knockout/knockout.d.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        var Visual = (function () {
            /**
            * Initializes a new instance from the given initial data.
            */
            function Visual(source) {
                /**
                * Raised when the Visual has changes to be pushed to the server.
                */
                this.onPushRequest = new NXKit.Web.TypedEvent();
                this._type = null;
                this._baseTypes = new Array();
                this._properties = {};
                this._visuals = ko.observableArray();

                // update from source data
                if (source != null)
                    this.update(source);
            }
            Object.defineProperty(Visual.prototype, "type", {
                /**
                * Gets the type of this visual.
                */
                get: function () {
                    return this._type;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Visual.prototype, "baseTypes", {
                /**
                * Gets the inheritence hierarchy of this visual.
                */
                get: function () {
                    return this._baseTypes;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Visual.prototype, "properties", {
                /**
                * Gets the interactive properties of this visual.
                */
                get: function () {
                    return this._properties;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Visual.prototype, "visuals", {
                /**
                * Gets the content of this visual.
                */
                get: function () {
                    return this._visuals;
                },
                enumerable: true,
                configurable: true
            });

            /**
            * Integrates the data given by the visual parameter into this Visual.
            */
            Visual.prototype.update = function (visual) {
                this.updateType(visual.Type);
                this.updateBaseTypes(visual.BaseTypes);
                this.updateProperties(visual.Properties);
                this.updateVisuals(visual.Visuals);
            };

            /**
            * Updates the type of this Visual with the new value.
            */
            Visual.prototype.updateType = function (type) {
                this._type = type;
            };

            /**
            * Updates the base types of this Visual with the new set of values.
            */
            Visual.prototype.updateBaseTypes = function (baseTypes) {
                this._baseTypes = baseTypes;
            };

            /**
            * Integrates the set of properties given with this Visual.
            */
            Visual.prototype.updateProperties = function (source) {
                for (var i in source) {
                    this.updateProperty(i, source[i]);
                }
            };

            /**
            * Updates the property given by the specified name with the specified value.
            */
            Visual.prototype.updateProperty = function (name, value) {
                var _this = this;
                if (this._properties[name] == undefined) {
                    // create new observable and subscribe to changes
                    var o = ko.observable(value);
                    o.subscribe(function (v) {
                        _this.pushRequest(_this, name, _this._properties[name]());
                    });
                    this._properties[name] = o;
                } else
                    this._properties[name](value);
            };

            /**
            * Integrates the set of content Visuals with the given object values.
            */
            Visual.prototype.updateVisuals = function (sources) {
                var _this = this;
                for (var source in sources) {
                    var v = new Visual(sources[source]);
                    v.onPushRequest.add(function (visual, name, value) {
                        _this.pushRequest(visual, name, value);
                    });
                    this._visuals.push(v);
                }
            };

            /**
            * Initiates a push of new values to the server.
            */
            Visual.prototype.pushRequest = function (visual, name, value) {
                this.onPushRequest.trigger(visual, name, value);
            };

            Object.defineProperty(Visual.prototype, "template", {
                /**
                * Gets the template that should be used to render this Visual.
                */
                get: function () {
                    // result standard template
                    var node = document.getElementById(this._type);

                    for (var i in this._baseTypes)
                        if (node == null)
                            node = document.getElementById(this._baseTypes[i]);

                    // no template found, invent an error
                    if (node == null)
                        node = $('<script />', {
                            'type': 'text/html',
                            'id': this._type,
                            'text': '<p>no template for ' + this._type + '</p>'
                        }).appendTo('body')[0];

                    return node.id;
                },
                enumerable: true,
                configurable: true
            });
            return Visual;
        })();
        Web.Visual = Visual;

        var View = (function () {
            function View(body) {
                /**
                * Raised when the Visual has changes to be pushed to the server.
                */
                this.onPushRequest = new NXKit.Web.TypedEvent();
                this._body = body;
                this._data = null;
                this._root = null;
                this._bind = true;
            }
            Object.defineProperty(View.prototype, "body", {
                get: function () {
                    return this._body;
                },
                set: function (value) {
                    this._body = value;
                },
                enumerable: true,
                configurable: true
            });


            Object.defineProperty(View.prototype, "data", {
                get: function () {
                    return this._data;
                },
                set: function (value) {
                    if (typeof (value) === 'string')
                        this._data = JSON.parse(value);
                    else
                        this._data = value;

                    // raise the value changed event
                    this._refresh();
                },
                enumerable: true,
                configurable: true
            });


            /**
            * Initiates a refresh of the view model.
            */
            View.prototype._refresh = function () {
                console.debug('_onModelChangedHandler');

                var self = this;
                self._root = new Visual(self._data);
                self._root.onPushRequest.add(function (visual, name, value) {
                    return self._onPushRequest(visual, name, value);
                });
                self._applyBindings();
            };

            /**
            * Invoked when the view model initiates a request to push updates.
            */
            View.prototype._onPushRequest = function (visual, name, value) {
                this.onPushRequest.trigger(visual, name, value);
            };

            /**
            * Applies the bindings to the view if possible.
            */
            View.prototype._applyBindings = function () {
                // apply bindings to our element and our view model
                if (this._body != null && this._root != null) {
                    // clear existing bindings
                    ko.cleanNode(this._body);

                    // apply knockout to view node
                    ko.applyBindings(this._root, this._body);

                    this._bind = false;
                }
            };
            return View;
        })();
        Web.View = View;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
//# sourceMappingURL=View.js.map
