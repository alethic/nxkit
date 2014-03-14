/// <reference path="Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="Scripts/typings/knockout/knockout.d.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        var Visual = (function () {
            function Visual(visual) {
                this._type = null;
                this._baseTypes = new Array();
                this._properties = {};
                this._visuals = ko.observableArray();

                // update from source data
                if (visual != null)
                    this.update(visual);
            }
            Object.defineProperty(Visual.prototype, "type", {
                get: function () {
                    return this._type;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Visual.prototype, "baseTypes", {
                get: function () {
                    return this._baseTypes;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Visual.prototype, "properties", {
                get: function () {
                    return this._properties;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Visual.prototype, "visuals", {
                get: function () {
                    return this._visuals;
                },
                enumerable: true,
                configurable: true
            });

            Visual.prototype.update = function (visual) {
                console.debug('update');

                this.updateType(visual.Type);
                this.updateBaseTypes(visual.BaseTypes);
                this.updateProperties(visual.Properties);
                this.updateVisuals(visual.Visuals);
            };

            Visual.prototype.updateType = function (type) {
                console.debug('updateType');
                this._type = type;
            };

            Visual.prototype.updateBaseTypes = function (baseTypes) {
                console.debug('updateBaseTypes');
                this._baseTypes = baseTypes;
            };

            Visual.prototype.updateProperties = function (properties) {
                console.debug('updateProperties');

                for (var i in properties) {
                    if (this._properties[i] == undefined)
                        this._properties[i] = ko.observable(properties[i]);
                    else
                        this._properties[i](properties[i]);
                }
            };

            Visual.prototype.updateVisuals = function (visuals) {
                console.debug('updateVisuals');

                for (var i in visuals) {
                    this._visuals.push(new Visual(visuals[i]));
                }
            };

            Object.defineProperty(Visual.prototype, "template", {
                get: function () {
                    return this._getTemplate();
                },
                enumerable: true,
                configurable: true
            });

            Visual.prototype._getTemplate = function () {
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

                return this._type;
            };
            return Visual;
        })();
        Web.Visual = Visual;

        var View = (function () {
            function View(element) {
                var _this = this;
                this.onVisualChanged = new NXKit.Web.TypedEvent();
                this._element = element;
                this._model = null;
                this._viewModel = null;

                this.onVisualChanged.add(function () {
                    return _this._onVisualChangedHandler();
                });
            }
            Object.defineProperty(View.prototype, "element", {
                get: function () {
                    return this._element;
                },
                set: function (value) {
                    this._element = value;
                },
                enumerable: true,
                configurable: true
            });


            Object.defineProperty(View.prototype, "model", {
                get: function () {
                    return this._model;
                },
                set: function (value) {
                    if (typeof (value) === 'string')
                        this._model = JSON.parse(value);
                    else
                        this._model = value;

                    // raise the value changed event
                    this.onVisualChanged.trigger();
                },
                enumerable: true,
                configurable: true
            });


            View.prototype._onVisualChangedHandler = function () {
                console.debug('_onVisualChangedHandler');

                this._viewModel = new Visual(this._model);
                this._applyBindings();
            };

            View.prototype._applyBindings = function () {
                // apply bindings to our element and our view model
                if (this._element != null && this._viewModel != null) {
                    // apply knockout to view node
                    ko.applyBindings(this._viewModel, this._element);
                }
            };
            return View;
        })();
        Web.View = View;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
//# sourceMappingURL=View.js.map
