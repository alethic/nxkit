/// <reference path="Scripts/typings/knockout/knockout.d.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        var PropertyMap = (function () {
            function PropertyMap() {
            }
            return PropertyMap;
        })();
        Web.PropertyMap = PropertyMap;

        var Visual = (function () {
            /**
            * Initializes a new instance from the given initial data.
            */
            function Visual(source) {
                /**
                * Raised when the Visual has changes to be pushed to the server.
                */
                this.ValueChanged = new NXKit.Web.TypedEvent();
                this._type = null;
                this._baseTypes = new Array();
                this._properties = new PropertyMap();
                this._visuals = ko.observableArray();

                // update from source data
                if (source != null)
                    this.Update(source);
            }
            Object.defineProperty(Visual.prototype, "IsVisual", {
                get: function () {
                    return true;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Visual.prototype, "Type", {
                /**
                * Gets the type of this visual.
                */
                get: function () {
                    return this._type;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Visual.prototype, "BaseTypes", {
                /**
                * Gets the inheritence hierarchy of this visual.
                */
                get: function () {
                    return this._baseTypes;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Visual.prototype, "Properties", {
                /**
                * Gets the interactive properties of this visual.
                */
                get: function () {
                    return this._properties;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Visual.prototype, "Visuals", {
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
            Visual.prototype.Update = function (source) {
                this.UpdateType(source.Type);
                this.UpdateBaseTypes(source.BaseTypes);
                this.UpdateProperties(source.Properties);
                this.UpdateVisuals(source.Visuals);
            };

            /**
            * Updates the type of this Visual with the new value.
            */
            Visual.prototype.UpdateType = function (type) {
                this._type = type;
            };

            /**
            * Updates the base types of this Visual with the new set of values.
            */
            Visual.prototype.UpdateBaseTypes = function (baseTypes) {
                this._baseTypes = baseTypes;
            };

            /**
            * Integrates the set of properties given with this Visual.
            */
            Visual.prototype.UpdateProperties = function (source) {
                for (var i in source) {
                    this.UpdateProperty(i, source[i]);
                }
            };

            /**
            * Updates the property given by the specified name with the specified value.
            */
            Visual.prototype.UpdateProperty = function (name, source) {
                var self = this;
                var prop = self._properties[name];
                if (prop == null) {
                    prop = self._properties[name] = new NXKit.Web.Property(source);
                    prop.ValueChanged.add(function (_) {
                        self.OnValueChanged(self, _);
                    });
                } else {
                    prop.Update(source);
                }
            };

            /**
            * Integrates the set of content Visuals with the given object values.
            */
            Visual.prototype.UpdateVisuals = function (sources) {
                var self = this;

                // clear visuals if none
                if (sources == null) {
                    self._visuals.removeAll();
                    return;
                }

                for (var i = 0; i < sources.length; i++) {
                    if (self._visuals().length < i + 1) {
                        var v = new Visual(sources[i]);
                        v.ValueChanged.add(function (_, __) {
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
            };

            Visual.prototype.ToData = function () {
                return {
                    Type: this._type,
                    BaseTypes: this._baseTypes,
                    Properties: this.PropertiesToData(),
                    Visuals: this.VisualsToData()
                };
            };

            /**
            * Transforms the given Property array into a list of data to push.
            */
            Visual.prototype.PropertiesToData = function () {
                var l = {};
                for (var p in this._properties) {
                    l[p] = this._properties[p].ToData();
                }
                return l;
            };

            /**
            * Transforms the given Property array into a list of data to push.
            */
            Visual.prototype.VisualsToData = function () {
                return ko.utils.arrayMap(this._visuals(), function (v) {
                    return v.ToData();
                });
            };

            /**
            * Initiates a push of new values to the server.
            */
            Visual.prototype.OnValueChanged = function (visual, property) {
                this.ValueChanged.trigger(visual, property);
            };
            return Visual;
        })();
        Web.Visual = Visual;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="Visual.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        var LayoutManager = (function () {
            function LayoutManager(context) {
                this._context = null;
                this._parent = null;
                var self = this;

                if (context == null)
                    throw new Error('context: null');

                self._context = context;

                // calculates the parent layout manager
                self._parent = ko.computed(function () {
                    var l = [self._context.$data].concat(self._context.$parents);
                    for (var i in l) {
                        var p = l[i];
                        if (p instanceof LayoutManager)
                            if (p != self)
                                return p;
                    }
                    return null;
                });
            }
            Object.defineProperty(LayoutManager.prototype, "Context", {
                /**
                * Gets the context inside which this layout manager was created.
                */
                get: function () {
                    return this._context;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(LayoutManager.prototype, "Parent", {
                /**
                * Gets the parent layout manager.
                */
                get: function () {
                    return this._parent();
                },
                enumerable: true,
                configurable: true
            });

            /**
            * Parses the given template binding information for a data structure to pass to the template lookup procedures.
            */
            LayoutManager.prototype.ParseTemplateBinding = function (valueAccessor, viewModel, bindingContext, data) {
                return this.Parent != null ? this.Parent.ParseTemplateBinding(valueAccessor, viewModel, bindingContext, data || {}) : data || {};
            };

            /**
            * Gets the templates provided by this layout manager for the given data.
            */
            LayoutManager.prototype.GetLocalTemplates = function () {
                return new Array();
            };

            /**
            * Gets the set of available templates for the given data.
            */
            LayoutManager.prototype.GetTemplates = function () {
                // append parent templates to local templates
                return this.GetLocalTemplates().concat(this.Parent != null ? this.Parent.GetTemplates() : new Array());
            };

            /**
            * Gets the fallback template for the given data.
            */
            LayoutManager.prototype.GetUnknownTemplate = function (data) {
                return $('<script />', {
                    'type': 'text/html',
                    'text': '<span class="ui red label">' + JSON.stringify(data) + '</span>'
                }).appendTo('body')[0];
            };

            /**
            * Extracts a JSON representation of a template node's data-nxkit bindings.
            */
            LayoutManager.prototype.GetTemplateNodeData = function (node) {
                // check whether we've already cached the node data
                var d = $(node).data('nxkit');
                if (d != null)
                    return d;

                // begin collecting data from node attributes
                d = {};
                for (var i = 0; i < node.attributes.length; i++) {
                    var a = node.attributes.item(i);
                    if (a.nodeName.indexOf('data-nxkit-') == 0) {
                        var n = a.nodeName.substring(11);
                        d[n] = $(node).data('nxkit-' + n);
                    }
                }

                // store new data on the node, and return
                return $(node).data('nxkit', d).data('nxkit');
            };

            /**
            * Tests whether a template node matches the given data.
            */
            LayoutManager.prototype.TemplatePredicate = function (node, data) {
                var d1 = this.GetTemplateNodeData(node);
                var d2 = data;
                return NXKit.Web.Utils.DeepEquals(d1, d2);
            };

            /**
            * Tests each given node against the predicate function.
            */
            LayoutManager.prototype.TemplateFilter = function (nodes, data) {
                var self = this;
                return nodes.filter(function (_) {
                    return self.TemplatePredicate(_, data);
                });
            };

            /**
            * Gets the appropriate template for the given data.
            */
            LayoutManager.prototype.GetTemplate = function (data) {
                return this.TemplateFilter(this.GetTemplates(), data)[0] || this.GetUnknownTemplate(data);
            };

            /**
            * Gets the template that applies for the given data.
            */
            LayoutManager.prototype.GetTemplateName = function (data) {
                var node = this.GetTemplate(data);
                if (node == null)
                    throw new Error('GetTemplate: no template located');

                // ensure the node has a valid and unique id
                if (node.id == '')
                    node.id = 'NXKit.Web__' + NXKit.Web.Utils.GenerateGuid().replace(/-/g, '');

                // caller expects the id
                return node.id;
            };
            return LayoutManager;
        })();
        Web.LayoutManager = LayoutManager;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="LayoutManager.ts" />
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var NXKit;
(function (NXKit) {
    (function (Web) {
        var DefaultLayoutManager = (function (_super) {
            __extends(DefaultLayoutManager, _super);
            function DefaultLayoutManager(context) {
                _super.call(this, context);
            }
            /**
            * Parses the given template binding information for a data structure to pass to the template lookup procedures.
            */
            DefaultLayoutManager.prototype.ParseTemplateBinding = function (valueAccessor, viewModel, bindingContext, data) {
                data = _super.prototype.ParseTemplateBinding.call(this, valueAccessor, viewModel, bindingContext, data);

                // extract data to be used to search for a template
                var value = valueAccessor();

                // value is itself a visual
                if (value != null && ko.unwrap(value) instanceof NXKit.Web.Visual) {
                    data.visual = ko.unwrap(value).Type;
                    return data;
                }

                // specified visual value
                if (value != null && value.visual != null && ko.unwrap(value.visual) instanceof NXKit.Web.Visual)
                    data.visual = ko.unwrap(value.visual).Type;

                if (data.visual == null)
                    if (viewModel instanceof NXKit.Web.Visual)
                        data.visual = viewModel.Type;

                // specified data type
                if (value != null && value.type != null)
                    data.type = ko.unwrap(value.type);

                return data;
            };

            /**
            * Gets all available templates currently in the document.
            */
            DefaultLayoutManager.prototype.GetLocalTemplates = function () {
                return $('script[type="text/html"]').toArray();
            };
            return DefaultLayoutManager;
        })(NXKit.Web.LayoutManager);
        Web.DefaultLayoutManager = DefaultLayoutManager;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="Scripts/typings/knockout/knockout.d.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (Utils) {
            /**
            * Tests two objects for equality.
            */
            function DeepEquals(a, b) {
                if (a == null && b === null)
                    return true;

                if (typeof a !== typeof b)
                    return false;

                if (typeof a === 'boolean' && typeof b === 'boolean')
                    return a === b;

                if (typeof a === 'string' && typeof b === 'string')
                    return a === b;

                if (typeof a === 'number' && typeof b === 'number')
                    return a === b;

                if (typeof a === 'function' && typeof b === 'function')
                    return a.toString() === b.toString();

                for (var i in a) {
                    if (a.hasOwnProperty(i)) {
                        if (!b.hasOwnProperty(i))
                            return false;
                        if (!Utils.DeepEquals(a[i], b[i]))
                            return false;
                    }
                }

                for (var i in b) {
                    if (b.hasOwnProperty(i)) {
                        if (!a.hasOwnProperty(i))
                            return false;
                        if (!Utils.DeepEquals(b[i], a[i]))
                            return false;
                    }
                }

                return true;
            }
            Utils.DeepEquals = DeepEquals;

            /**
            * Generates a unique identifier.
            */
            function GenerateGuid() {
                // http://www.ietf.org/rfc/rfc4122.txt
                var s = [];
                var hexDigits = "0123456789abcdef";
                for (var i = 0; i < 36; i++) {
                    s[i] = hexDigits.substr(Math.floor(Math.random() * 0x10), 1);
                }
                s[14] = "4"; // bits 12-15 of the time_hi_and_version field to 0010
                s[19] = hexDigits.substr((s[19] & 0x3) | 0x8, 1); // bits 6-7 of the clock_seq_hi_and_reserved to 01
                s[8] = s[13] = s[18] = s[23] = "-";

                return s.join("");
            }
            Utils.GenerateGuid = GenerateGuid;

            /**
            * Returns the entire context item chain from the specified context upwards.
            */
            function GetContextItems(context) {
                return [context.$data].concat(context.$parents);
            }
            Utils.GetContextItems = GetContextItems;

            /**
            * Gets the layout manager in scope of the given binding context.
            */
            function GetLayoutManager(context) {
                return ko.utils.arrayFirst(GetContextItems(context), function (_) {
                    return _ instanceof NXKit.Web.LayoutManager;
                });
            }
            Utils.GetLayoutManager = GetLayoutManager;

            /**
            * Gets the recommended view model for the given binding information.
            */
            function GetTemplateViewModel(valueAccessor, viewModel, bindingContext) {
                var value = valueAccessor() || viewModel;

                // value itself is a visual
                if (value != null && ko.unwrap(value) instanceof NXKit.Web.Visual)
                    return ko.unwrap(value);

                // specified data value
                if (value != null && value.data != null)
                    return ko.unwrap(value.data);

                // specified visual value
                if (value != null && value.visual != null && ko.unwrap(value.visual) instanceof NXKit.Web.Visual)
                    return ko.unwrap(value.visual);

                // default to existing context
                return null;
            }
            Utils.GetTemplateViewModel = GetTemplateViewModel;

            /**
            * Extracts template index data from the given binding information.
            */
            function GetTemplateBinding(valueAccessor, viewModel, bindingContext) {
                return GetLayoutManager(bindingContext).ParseTemplateBinding(valueAccessor, viewModel, bindingContext, {});
            }
            Utils.GetTemplateBinding = GetTemplateBinding;

            /**
            * Determines the named template from the given extracted data and context.
            */
            function GetTemplateName(bindingContext, data) {
                return GetLayoutManager(bindingContext).GetTemplateName(data);
            }
            Utils.GetTemplateName = GetTemplateName;
        })(Web.Utils || (Web.Utils = {}));
        var Utils = Web.Utils;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="../LayoutManager.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            var GroupLayoutManager = (function (_super) {
                __extends(GroupLayoutManager, _super);
                function GroupLayoutManager(context) {
                    _super.call(this, context);
                }
                /**
                * Applies the 'level' and 'layout' bindings to the template search.
                */
                GroupLayoutManager.prototype.ParseTemplateBinding = function (valueAccessor, viewModel, bindingContext, data) {
                    data = _super.prototype.ParseTemplateBinding.call(this, valueAccessor, viewModel, bindingContext, data);

                    // extract level binding
                    var value = valueAccessor();
                    if (value != null && value.level != null && ko.unwrap(value.level) != null)
                        data.level = ko.unwrap(value.level);

                    // extract layout binding
                    var value = valueAccessor();
                    if (value != null && value.layout != null && ko.unwrap(value.layout) != null)
                        data.layout = ko.unwrap(value.layout);

                    return data;
                };
                return GroupLayoutManager;
            })(NXKit.Web.LayoutManager);
            XForms.GroupLayoutManager = GroupLayoutManager;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (Web) {
        var TypedEvent = (function () {
            function TypedEvent() {
                this._listeners = [];
            }
            TypedEvent.prototype.add = function (listener) {
                /// <summary>Registers a new listener for the event.</summary>
                /// <param name="listener">The callback function to register.</param>
                this._listeners.push(listener);
            };

            TypedEvent.prototype.remove = function (listener) {
                /// <summary>Unregisters a listener from the event.</summary>
                /// <param name="listener">The callback function that was registered. If missing then all listeners will be removed.</param>
                if (typeof listener === 'function') {
                    for (var i = 0, l = this._listeners.length; i < l; l++) {
                        if (this._listeners[i] === listener) {
                            this._listeners.splice(i, 1);
                            break;
                        }
                    }
                } else {
                    this._listeners = [];
                }
            };

            TypedEvent.prototype.trigger = function () {
                var a = [];
                for (var _i = 0; _i < (arguments.length - 0); _i++) {
                    a[_i] = arguments[_i + 0];
                }
                /// <summary>Invokes all of the listeners for this event.</summary>
                /// <param name="args">Optional set of arguments to pass to listners.</param>
                var context = {};
                var listeners = this._listeners.slice(0);
                for (var i = 0, l = listeners.length; i < l; i++) {
                    listeners[i].apply(context, a || []);
                }
            };
            return TypedEvent;
        })();
        Web.TypedEvent = TypedEvent;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="Scripts/typings/knockout/knockout.d.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        var Property = (function () {
            function Property(source) {
                /**
                * Raised when the Property's value has changed.
                */
                this.ValueChanged = new NXKit.Web.TypedEvent();
                var self = this;

                self._value = ko.observable();
                self._value.subscribe(function (_) {
                    // version is set below zero when integrating changes
                    if (self._version() >= 0) {
                        self._version(self._version() + 1);
                        self.ValueChanged.trigger(self);
                    }
                });

                self._version = ko.observable();
                self._version.subscribe(function (_) {
                });

                self._valueAsString = ko.computed({
                    read: function () {
                        var s = self._value() != null ? String(self._value()).trim() : null;
                        return s ? s : null;
                    },
                    write: function (value) {
                        var s = value != null ? value.trim() : null;
                        return self._value(s ? s : null);
                    }
                });

                self._valueAsBoolean = ko.computed({
                    read: function () {
                        return self._value() === true || self._value() == 'true' || self._value() == 'True';
                    },
                    write: function (value) {
                        self._value(value === true ? "true" : "false");
                    }
                });

                self._valueAsBoolean = ko.computed({
                    read: function () {
                        return self._value() === true || self._value() == 'true' || self._value() == 'True';
                    },
                    write: function (value) {
                        self._value(value === true ? "true" : "false");
                    }
                });

                self._valueAsNumber = ko.computed({
                    read: function () {
                        return self._value() != '' ? parseFloat(self._value()) : null;
                    },
                    write: function (value) {
                        self._value(value != null ? value.toString() : null);
                    }
                });

                self._valueAsDate = ko.computed({
                    read: function () {
                        return self._value() != null ? new Date(self._value()) : null;
                    },
                    write: function (value) {
                        if (value instanceof Date)
                            self._value(value.toDateString());
                        else if (typeof (value) === 'string')
                            self._value(value != null ? new Date(value) : null);
                        else
                            self._value(null);
                    }
                });

                if (source != null)
                    self.Update(source);
            }
            Object.defineProperty(Property.prototype, "Value", {
                get: function () {
                    return this._value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Property.prototype, "ValueAsString", {
                get: function () {
                    return this._valueAsString;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Property.prototype, "ValueAsBoolean", {
                get: function () {
                    return this._valueAsBoolean;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Property.prototype, "ValueAsNumber", {
                get: function () {
                    return this._valueAsNumber;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Property.prototype, "ValueAsDate", {
                get: function () {
                    return this._valueAsDate;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Property.prototype, "Version", {
                get: function () {
                    return this._version;
                },
                enumerable: true,
                configurable: true
            });

            Property.prototype.Update = function (source) {
                var self = this;
                if (self._value() !== source.Value) {
                    self._version(-1);
                    self._value(source.Value);
                    self._version(0);
                }
            };

            Property.prototype.ToData = function () {
                return {
                    Value: this.Value(),
                    Version: this.Version()
                };
            };
            return Property;
        })();
        Web.Property = Property;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="Scripts/typings/knockout/knockout.d.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        var View = (function () {
            function View(body) {
                /**
                * Raised when the Visual has changes to be pushed to the server.
                */
                this.CallbackRequest = new NXKit.Web.TypedEvent();
                this._body = body;
                this._data = null;
                this._root = null;
                this._bind = true;
            }
            Object.defineProperty(View.prototype, "Body", {
                get: function () {
                    return this._body;
                },
                set: function (value) {
                    this._body = value;
                },
                enumerable: true,
                configurable: true
            });


            Object.defineProperty(View.prototype, "Data", {
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
                    self.Update();
                },
                enumerable: true,
                configurable: true
            });


            /**
            * Initiates a refresh of the view model.
            */
            View.prototype.Update = function () {
                var self = this;

                if (self._root == null)
                    // generate new visual tree
                    self._root = new NXKit.Web.Visual(self._data);
                else
                    // update existing visual tree
                    self._root.Update(self._data);

                self._root.ValueChanged.add(function (_, __) {
                    return self.OnCallbackRequest();
                });
                self.ApplyBindings();
            };

            /**
            * Invoked when the view model initiates a request to push updates.
            */
            View.prototype.OnCallbackRequest = function () {
                var self = this;

                self.CallbackRequest.trigger(self._root.ToData());
            };

            /**
            * Applies the bindings to the view if possible.
            */
            View.prototype.ApplyBindings = function () {
                // apply bindings to our element and our view model
                if (this._bind && this._body != null && this._root != null) {
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
/// <reference path="Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="Utils.ts" />
/// <reference path="Visual.ts" />
/// <reference path="LayoutManager.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        var VisualViewModel = (function () {
            function VisualViewModel(context, visual) {
                var self = this;

                if (context == null)
                    throw new Error('context: null');

                if (!(visual instanceof NXKit.Web.Visual))
                    throw new Error('visual: null');

                self._context = context;
                self._visual = visual;
            }
            VisualViewModel.GetUniqueId = function (visual) {
                return visual != null ? visual.Properties['UniqueId'].ValueAsString() : null;
            };

            Object.defineProperty(VisualViewModel.prototype, "Context", {
                get: function () {
                    return this._context;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(VisualViewModel.prototype, "Visual", {
                get: function () {
                    return this._visual;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(VisualViewModel.prototype, "UniqueId", {
                get: function () {
                    return VisualViewModel.GetUniqueId(this.Visual);
                },
                enumerable: true,
                configurable: true
            });
            return VisualViewModel;
        })();
        Web.VisualViewModel = VisualViewModel;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="../VisualViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            var VisualViewModel = (function (_super) {
                __extends(VisualViewModel, _super);
                function VisualViewModel(context, visual) {
                    _super.call(this, context, visual);
                    var self = this;
                }
                VisualViewModel.GetValueAsString = function (visual) {
                    return ko.computed(function () {
                        if (visual != null && visual.Properties['Value'] != null)
                            return visual.Properties['Value'].ValueAsString();
                        else
                            return null;
                    });
                };

                VisualViewModel.GetRelevant = function (visual) {
                    return ko.computed(function () {
                        if (visual != null && visual.Properties['Relevant'] != null)
                            return visual.Properties['Relevant'].ValueAsBoolean();
                        else
                            return null;
                    });
                };

                VisualViewModel.GetAppearance = function (visual) {
                    return ko.computed(function () {
                        if (visual != null && visual.Properties['Appearance'] != null)
                            return visual.Properties['Appearance'].ValueAsString();
                        else
                            return null;
                    });
                };

                VisualViewModel.IsMetadataVisual = function (visual) {
                    return VisualViewModel.MetadataVisualTypes.some(function (_) {
                        return visual.Type == _;
                    });
                };

                VisualViewModel.GetLabel = function (visual) {
                    return ko.utils.arrayFirst(visual.Visuals(), function (_) {
                        return _.Type == 'NXKit.XForms.XFormsLabelVisual';
                    });
                };

                VisualViewModel.GetHelp = function (visual) {
                    return ko.utils.arrayFirst(visual.Visuals(), function (_) {
                        return _.Type == 'NXKit.XForms.XFormsHelpVisual';
                    });
                };

                VisualViewModel.GetHint = function (visual) {
                    return ko.utils.arrayFirst(visual.Visuals(), function (_) {
                        return _.Type == 'NXKit.XForms.XFormsHintVisual';
                    });
                };

                VisualViewModel.GetAlert = function (visual) {
                    return ko.utils.arrayFirst(visual.Visuals(), function (_) {
                        return _.Type == 'NXKit.XForms.XFormsAlertVisual';
                    });
                };

                VisualViewModel.IsControlVisual = function (visual) {
                    return this.ControlVisualTypes.some(function (_) {
                        return visual.Type == _;
                    });
                };

                VisualViewModel.HasControlVisual = function (visual) {
                    return visual.Visuals().some(function (_) {
                        return VisualViewModel.IsControlVisual(_);
                    });
                };

                VisualViewModel.GetControlVisuals = function (visual) {
                    return visual.Visuals().filter(function (_) {
                        return VisualViewModel.IsControlVisual(_);
                    });
                };

                VisualViewModel.IsTransparentVisual = function (visual) {
                    return VisualViewModel.TransparentVisualTypes.some(function (_) {
                        return visual.Type == _;
                    });
                };

                VisualViewModel.GetContents = function (visual) {
                    var l = visual.Visuals().filter(function (_) {
                        return !VisualViewModel.IsMetadataVisual(_);
                    });

                    var r = new Array();
                    for (var i = 0; i < l.length; i++) {
                        var v = l[i];
                        if (VisualViewModel.IsTransparentVisual(v)) {
                            var s = VisualViewModel.GetContents(v);
                            for (var j = 0; j < s.length; j++)
                                r.push(s[j]);
                        } else {
                            r.push(v);
                        }
                    }

                    return r;
                };

                Object.defineProperty(VisualViewModel.prototype, "ValueAsString", {
                    get: function () {
                        return VisualViewModel.GetValueAsString(this.Visual);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(VisualViewModel.prototype, "Relevant", {
                    get: function () {
                        return VisualViewModel.GetRelevant(this.Visual);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(VisualViewModel.prototype, "Appearance", {
                    get: function () {
                        return VisualViewModel.GetAppearance(this.Visual);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(VisualViewModel.prototype, "Label", {
                    get: function () {
                        return VisualViewModel.GetLabel(this.Visual);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(VisualViewModel.prototype, "Help", {
                    get: function () {
                        return VisualViewModel.GetHelp(this.Visual);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(VisualViewModel.prototype, "Hint", {
                    get: function () {
                        return VisualViewModel.GetHint(this.Visual);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(VisualViewModel.prototype, "Contents", {
                    get: function () {
                        return VisualViewModel.GetContents(this.Visual);
                    },
                    enumerable: true,
                    configurable: true
                });
                VisualViewModel.ControlVisualTypes = [
                    'NXKit.XForms.XFormsInputVisual',
                    'NXKit.XForms.XFormsRangeVisual',
                    'NXKit.XForms.XFormsSelect1Visual',
                    'NXKit.XForms.XFormsSelectVisual'
                ];

                VisualViewModel.MetadataVisualTypes = [
                    'NXKit.XForms.XFormsLabelVisual',
                    'NXKit.XForms.XFormsHelpVisual',
                    'NXKit.XForms.XFormsHintVisual',
                    'NXKit.XForms.XFormsAlertVisual'
                ];

                VisualViewModel.TransparentVisualTypes = [
                    'NXKit.XForms.XFormsRepeatVisual',
                    'NXKit.XForms.XFormsRepeatItemVisual'
                ];
                return VisualViewModel;
            })(NXKit.Web.VisualViewModel);
            XForms.VisualViewModel = VisualViewModel;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="../Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="VisualViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            var HintViewModel = (function (_super) {
                __extends(HintViewModel, _super);
                function HintViewModel(context, visual) {
                    _super.call(this, context, visual);
                }
                Object.defineProperty(HintViewModel.prototype, "Text", {
                    get: function () {
                        return this.ValueAsString;
                    },
                    enumerable: true,
                    configurable: true
                });
                return HintViewModel;
            })(NXKit.Web.XForms.VisualViewModel);
            XForms.HintViewModel = HintViewModel;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="../../Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="../VisualViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            (function (Layout) {
                var FormViewModel = (function (_super) {
                    __extends(FormViewModel, _super);
                    function FormViewModel(context, visual) {
                        _super.call(this, context, visual);
                    }
                    return FormViewModel;
                })(NXKit.Web.XForms.VisualViewModel);
                Layout.FormViewModel = FormViewModel;
            })(XForms.Layout || (XForms.Layout = {}));
            var Layout = XForms.Layout;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="../../Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="../VisualViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            (function (Layout) {
                var PageViewModel = (function (_super) {
                    __extends(PageViewModel, _super);
                    function PageViewModel(context, visual) {
                        _super.call(this, context, visual);
                    }
                    return PageViewModel;
                })(NXKit.Web.XForms.VisualViewModel);
                Layout.PageViewModel = PageViewModel;
            })(XForms.Layout || (XForms.Layout = {}));
            var Layout = XForms.Layout;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="../../Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="../VisualViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            (function (Layout) {
                var ParagraphViewModel = (function (_super) {
                    __extends(ParagraphViewModel, _super);
                    function ParagraphViewModel(context, visual) {
                        _super.call(this, context, visual);
                    }
                    return ParagraphViewModel;
                })(NXKit.Web.XForms.VisualViewModel);
                Layout.ParagraphViewModel = ParagraphViewModel;
            })(XForms.Layout || (XForms.Layout = {}));
            var Layout = XForms.Layout;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="../../Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="../VisualViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            (function (Layout) {
                var SectionViewModel = (function (_super) {
                    __extends(SectionViewModel, _super);
                    function SectionViewModel(context, visual) {
                        _super.call(this, context, visual);
                    }
                    return SectionViewModel;
                })(NXKit.Web.XForms.VisualViewModel);
                Layout.SectionViewModel = SectionViewModel;
            })(XForms.Layout || (XForms.Layout = {}));
            var Layout = XForms.Layout;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="../Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="VisualViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            var Select1ViewModel = (function (_super) {
                __extends(Select1ViewModel, _super);
                function Select1ViewModel(context, visual) {
                    _super.call(this, context, visual);
                }
                return Select1ViewModel;
            })(NXKit.Web.XForms.VisualViewModel);
            XForms.Select1ViewModel = Select1ViewModel;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="../Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="VisualViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            var LabelViewModel = (function (_super) {
                __extends(LabelViewModel, _super);
                function LabelViewModel(context, visual) {
                    _super.call(this, context, visual);
                }
                Object.defineProperty(LabelViewModel.prototype, "Text", {
                    get: function () {
                        return this.ValueAsString;
                    },
                    enumerable: true,
                    configurable: true
                });
                return LabelViewModel;
            })(NXKit.Web.XForms.VisualViewModel);
            XForms.LabelViewModel = LabelViewModel;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="VisualViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            (function (GroupViewModel_) {
                /**
                * Represents a sub-item of a top-level group.
                */
                var Item = (function () {
                    function Item(visual, level) {
                        this._visual = visual;
                        this._level = level;
                    }
                    Object.defineProperty(Item.prototype, "Visual", {
                        get: function () {
                            return this._visual;
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Object.defineProperty(Item.prototype, "Level", {
                        get: function () {
                            return this._level;
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Object.defineProperty(Item.prototype, "Relevant", {
                        get: function () {
                            return this.GetRelevant();
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Item.prototype.GetRelevant = function () {
                        return ko.computed(function () {
                            return true;
                        });
                    };

                    Object.defineProperty(Item.prototype, "Label", {
                        get: function () {
                            return this.GetLabel();
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Item.prototype.GetLabel = function () {
                        throw new Error('GetLabel not implemented');
                    };

                    Object.defineProperty(Item.prototype, "Layout", {
                        get: function () {
                            return this.GetLayout();
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Item.prototype.GetLayout = function () {
                        throw new Error('GetLayout not implemented');
                    };
                    return Item;
                })();
                GroupViewModel_.Item = Item;

                /**
                * Describes an item that will render a raw visual.
                */
                var VisualItem = (function (_super) {
                    __extends(VisualItem, _super);
                    function VisualItem(visual, itemVisual, level) {
                        _super.call(this, visual, level);
                        this._itemVisual = itemVisual;
                    }
                    Object.defineProperty(VisualItem.prototype, "ItemVisual", {
                        get: function () {
                            return this._itemVisual;
                        },
                        enumerable: true,
                        configurable: true
                    });

                    VisualItem.prototype.GetRelevant = function () {
                        return NXKit.Web.XForms.VisualViewModel.GetRelevant(this._itemVisual);
                    };

                    VisualItem.prototype.GetLabel = function () {
                        return NXKit.Web.XForms.VisualViewModel.GetLabel(this._itemVisual);
                    };

                    VisualItem.prototype.GetLayout = function () {
                        return {
                            visual: this.Visual,
                            data: this,
                            layout: 'visual',
                            level: this.Level
                        };
                    };
                    return VisualItem;
                })(Item);
                GroupViewModel_.VisualItem = VisualItem;

                /**
                * Describes a sub-item of a top-level group that will render a single underlying item.
                */
                var SingleItem = (function (_super) {
                    __extends(SingleItem, _super);
                    function SingleItem(visual, level) {
                        _super.call(this, visual, level);
                    }
                    Object.defineProperty(SingleItem.prototype, "Item", {
                        get: function () {
                            return this._item;
                        },
                        set: function (item) {
                            this._item = item;
                        },
                        enumerable: true,
                        configurable: true
                    });


                    SingleItem.prototype.GetRelevant = function () {
                        return this.Item.Relevant;
                    };

                    SingleItem.prototype.GetLabel = function () {
                        return this._item.Label;
                    };

                    SingleItem.prototype.GetLayout = function () {
                        return {
                            visual: this.Visual,
                            data: this,
                            layout: 'single',
                            level: this.Level
                        };
                    };
                    return SingleItem;
                })(Item);
                GroupViewModel_.SingleItem = SingleItem;

                /**
                * Describes a sub-item of a top-level group which will render two items.
                */
                var DoubleItem = (function (_super) {
                    __extends(DoubleItem, _super);
                    function DoubleItem(visual, level) {
                        _super.call(this, visual, level);
                    }
                    Object.defineProperty(DoubleItem.prototype, "Item1", {
                        get: function () {
                            return this._item1;
                        },
                        set: function (item) {
                            this._item1 = item;
                        },
                        enumerable: true,
                        configurable: true
                    });


                    Object.defineProperty(DoubleItem.prototype, "Item2", {
                        get: function () {
                            return this._item2;
                        },
                        set: function (item) {
                            this._item2 = item;
                        },
                        enumerable: true,
                        configurable: true
                    });


                    DoubleItem.prototype.GetRelevant = function () {
                        var _this = this;
                        return ko.computed(function () {
                            return _this._item1.Relevant() && _this._item2.Relevant();
                        });
                    };

                    DoubleItem.prototype.GetLabel = function () {
                        return null;
                    };

                    DoubleItem.prototype.GetLayout = function () {
                        return {
                            visual: this.Visual,
                            data: this,
                            layout: 'double',
                            level: this.Level
                        };
                    };
                    return DoubleItem;
                })(Item);
                GroupViewModel_.DoubleItem = DoubleItem;

                var GroupItem = (function (_super) {
                    __extends(GroupItem, _super);
                    function GroupItem(visual, groupVisual, level) {
                        _super.call(this, visual, level);
                        this._groupVisual = groupVisual;
                        this._items = new Array();
                    }
                    Object.defineProperty(GroupItem.prototype, "Items", {
                        get: function () {
                            return this._items;
                        },
                        set: function (items) {
                            this._items = items;
                        },
                        enumerable: true,
                        configurable: true
                    });


                    GroupItem.prototype.GetRelevant = function () {
                        return NXKit.Web.XForms.VisualViewModel.GetRelevant(this._groupVisual);
                    };

                    GroupItem.prototype.GetLabel = function () {
                        return NXKit.Web.XForms.VisualViewModel.GetLabel(this._groupVisual);
                    };

                    GroupItem.prototype.GetLayout = function () {
                        return {
                            visual: this.Visual,
                            data: this,
                            layout: 'group',
                            level: this.Level
                        };
                    };
                    return GroupItem;
                })(Item);
                GroupViewModel_.GroupItem = GroupItem;
            })(XForms.GroupViewModel_ || (XForms.GroupViewModel_ = {}));
            var GroupViewModel_ = XForms.GroupViewModel_;

            var GroupViewModel = (function (_super) {
                __extends(GroupViewModel, _super);
                function GroupViewModel(context, visual, count) {
                    _super.call(this, context, visual);

                    this._count = count;
                }
                Object.defineProperty(GroupViewModel.prototype, "BindingContents", {
                    /**
                    * Gets the set of contents expressed as template binding objects.
                    */
                    get: function () {
                        return this.GetBindingContents();
                    },
                    enumerable: true,
                    configurable: true
                });

                GroupViewModel.prototype.GetBindingContents = function () {
                    return this.GetItems(this.Visual, 1);
                };

                /**
                * Gets the set of contents expressed as template binding objects.
                */
                GroupViewModel.prototype.GetGroupItem = function (visual, level) {
                    var item = new GroupViewModel_.GroupItem(this.Visual, visual, level);
                    item.Items = this.GetItems(visual, level + 1);
                    return item;
                };

                GroupViewModel.prototype.GetItems = function (visual, level) {
                    var list = new Array();
                    var cnts = NXKit.Web.XForms.VisualViewModel.GetContents(visual);
                    for (var i = 0; i < cnts.length; i++) {
                        var v = cnts[i];

                        // nested group obtains single child
                        if (v.Type == 'NXKit.XForms.XFormsGroupVisual') {
                            list.push(this.GetGroupItem(v, level));
                            continue;
                        }

                        // check if last inserted item was a single item, if so, replace with a double item
                        var item = list.pop();
                        if (item instanceof GroupViewModel_.SingleItem) {
                            var item1 = item;
                            var item2 = new GroupViewModel_.DoubleItem(this.Visual, level);
                            item2.Item1 = item1.Item;
                            item2.Item2 = new GroupViewModel_.VisualItem(this.Visual, v, level);
                            list.push(item2);
                        } else {
                            // put previous item back into list
                            if (item != null)
                                list.push(item);

                            // insert new single item
                            var item1 = new GroupViewModel_.SingleItem(this.Visual, level);
                            item1.Item = new GroupViewModel_.VisualItem(this.Visual, v, level);
                            list.push(item1);
                        }
                    }

                    return list;
                };
                return GroupViewModel;
            })(NXKit.Web.XForms.VisualViewModel);
            XForms.GroupViewModel = GroupViewModel;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
//# sourceMappingURL=NXKit.js.map
