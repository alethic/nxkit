/// <reference path="Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="Scripts/typings/knockout/knockout.d.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        var Property = (function () {
            function Property(source) {
                /**
                * Raised when the Property's value has changed.
                */
                this.onValueChanged = new NXKit.Web.TypedEvent();
                var self = this;

                self._value = ko.observable();
                self._value.subscribe(function (_) {
                    // version is set below zero when integrating changes
                    if (self._version() >= 0) {
                        self._version(self._version() + 1);
                        self.onValueChanged.trigger(self);
                    }
                });

                self._version = ko.observable();
                self._version.subscribe(function (_) {
                    console.debug('version+1');
                });

                if (source != null)
                    self.update(source);
            }
            Object.defineProperty(Property.prototype, "value", {
                get: function () {
                    return this._value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Property.prototype, "version", {
                get: function () {
                    return this._version;
                },
                enumerable: true,
                configurable: true
            });

            Property.prototype.update = function (source) {
                var self = this;
                if (self._value() !== source.Value) {
                    self._version(-1);
                    self._value(source.Value);
                    self._version(0);
                }
            };

            Property.prototype.toData = function () {
                return {
                    Value: this.value(),
                    Version: this.version()
                };
            };
            return Property;
        })();
        Web.Property = Property;

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
                this._properties = new Array();
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
            Visual.prototype.updateProperty = function (name, source) {
                var self = this;
                var prop = self._properties[name];
                if (prop == null) {
                    prop = self._properties[name] = new Property(source);
                    prop.onValueChanged.add(function (_) {
                        self.pushRequest(self);
                    });
                } else {
                    prop.update(source);
                }
            };

            /**
            * Integrates the set of content Visuals with the given object values.
            */
            Visual.prototype.updateVisuals = function (sources) {
                var _this = this;
                for (var source in sources) {
                    var v = new Visual(sources[source]);
                    v.onPushRequest.add(function (_) {
                        _this.pushRequest(_);
                    });
                    this._visuals.push(v);
                }
            };

            Visual.prototype.toData = function () {
                return {
                    Type: this._type,
                    BaseTypes: this._baseTypes,
                    Properties: this.propertiesToData(),
                    Visuals: this.visualsToData()
                };
            };

            /**
            * Transforms the given Property array into a list of data to push.
            */
            Visual.prototype.propertiesToData = function () {
                var l = {};
                for (var p in this._properties) {
                    l[p] = this._properties[p].toData();
                }
                return l;
            };

            /**
            * Transforms the given Property array into a list of data to push.
            */
            Visual.prototype.visualsToData = function () {
                return ko.utils.arrayMap(this._visuals(), function (v) {
                    return v.toData();
                });
            };

            /**
            * Initiates a push of new values to the server.
            */
            Visual.prototype.pushRequest = function (visual) {
                this.onPushRequest.trigger(visual);
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
                    var self = this;

                    if (typeof (value) === 'string')
                        self._data = JSON.parse(value);
                    else
                        self._data = value;

                    // raise the value changed event
                    self._refresh();
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
                self._root.onPushRequest.add(function (_) {
                    return self._onPushRequest(_);
                });
                self._applyBindings();
            };

            /**
            * Invoked when the view model initiates a request to push updates.
            */
            View.prototype._onPushRequest = function (visual) {
                var self = this;

                self.onPushRequest.trigger(self._root.toData());
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
