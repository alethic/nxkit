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
/// <reference path="TypedEvent.ts" />
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
/// <reference path="TypedEvent.ts" />
/// <reference path="Property.ts" />
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
                this.ValueChanged = new NXKit.Web.TypedEvent();
                this._type = null;
                this._baseTypes = new Array();
                this._properties = new NXKit.Web.PropertyMap();
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
                var d = "0123456789abcdef";
                for (var i = 0; i < 36; i++) {
                    s[i] = d.substr(Math.floor(Math.random() * 0x10), 1);
                }
                s[14] = "4"; // bits 12-15 of the time_hi_and_version field to 0010
                s[19] = d.substr((s[19] & 0x3) | 0x8, 1); // bits 6-7 of the clock_seq_hi_and_reserved to 01
                s[8] = s[13] = s[18] = s[23] = "-";

                return s.join("");
            }
            Utils.GenerateGuid = GenerateGuid;

            /**
            * Gets the unique document ID of the given visual.
            */
            function GetUniqueId(visual) {
                return visual != null && visual.Properties['UniqueId'] != null ? visual.Properties['UniqueId'].ValueAsString() : null;
            }
            Utils.GetUniqueId = GetUniqueId;

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
        })(Web.Utils || (Web.Utils = {}));
        var Utils = Web.Utils;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="Visual.ts" />
/// <reference path="Utils.ts" />
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
                var _this = this;
                return ko.computed(function () {
                    return _this.TemplateFilter(_this.GetTemplates(), data)[0] || _this.GetUnknownTemplate(data);
                });
            };

            /**
            * Gets the template that applies for the given data.
            */
            LayoutManager.prototype.GetTemplateName = function (data) {
                var _this = this;
                return ko.computed(function () {
                    var node = _this.GetTemplate(data)();
                    if (node == null)
                        throw new Error('GetTemplate: no template located');

                    // ensure the node has a valid and unique id
                    if (node.id == '')
                        node.id = 'NXKit.Web__' + NXKit.Web.Utils.GenerateGuid().replace(/-/g, '');

                    // caller expects the id
                    return node.id;
                });
            };
            return LayoutManager;
        })();
        Web.LayoutManager = LayoutManager;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="Visual.ts" />
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
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (Knockout) {
            var CheckboxBindingHandler = (function () {
                function CheckboxBindingHandler() {
                }
                CheckboxBindingHandler.prototype.init = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                    setTimeout(function () {
                        $(element).checkbox();
                        $(element).checkbox('setting', {
                            onEnable: function () {
                                var v1 = true;
                                var v2 = ko.unwrap(valueAccessor());
                                if (typeof v2 === 'boolean') {
                                    if (v1 != v2)
                                        valueAccessor()(v1);
                                } else if (typeof v2 === 'string') {
                                    var v2_ = v2.toLowerCase() === 'true' ? true : false;
                                    if (v1 != v2_)
                                        valueAccessor()(v1 ? 'true' : 'false');
                                }
                            },
                            onDisable: function () {
                                var v1 = false;
                                var v2 = ko.unwrap(valueAccessor());
                                if (typeof v2 === 'boolean') {
                                    if (v1 != v2)
                                        valueAccessor()(v1);
                                } else if (typeof v2 === 'string') {
                                    var v2_ = v2.toLowerCase() === 'true' ? true : false;
                                    if (v1 != v2_)
                                        valueAccessor()(v1 ? 'true' : 'false');
                                }
                            }
                        });
                    }, 2000);
                };

                CheckboxBindingHandler.prototype.update = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                    setTimeout(function () {
                        var v1 = ko.unwrap(valueAccessor());
                        var v2 = $(element).find('input').val() == 'on' ? true : false;
                        if (typeof v1 === 'boolean') {
                            if (v1 != v2)
                                $(element).checkbox(v1 ? 'enable' : 'disable');
                        } else if (typeof v1 === 'string') {
                            var v1_ = v1.toLowerCase() === 'true' ? true : false;
                            if (v1_ != v2)
                                $(element).checkbox(v1_ ? 'enable' : 'disable');
                        }
                    }, 1000);
                };
                return CheckboxBindingHandler;
            })();

            ko.bindingHandlers['nxkit_checkbox'] = new CheckboxBindingHandler();
        })(Web.Knockout || (Web.Knockout = {}));
        var Knockout = Web.Knockout;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (Knockout) {
            var DropdownBindingHandler = (function () {
                function DropdownBindingHandler() {
                }
                DropdownBindingHandler.prototype.init = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                    setTimeout(function () {
                        $(element).dropdown();
                        $(element).dropdown('setting', {
                            onChange: function (value) {
                                var v1 = $(element).dropdown('get value');
                                var v2 = ko.unwrap(valueAccessor());
                                if (typeof v1 === 'string') {
                                    if (v1 != v2)
                                        valueAccessor()(v1);
                                }
                            }
                        });
                    }, 2000);
                };

                DropdownBindingHandler.prototype.update = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                    setTimeout(function () {
                        var v1 = ko.unwrap(valueAccessor());
                        var v2 = $(element).dropdown('get value');
                        if (typeof v2 === 'string')
                            if (v1 != v2)
                                $(element).dropdown('set value', v1);
                    }, 1000);
                };
                return DropdownBindingHandler;
            })();

            ko.bindingHandlers['nxkit_dropdown'] = new DropdownBindingHandler();
        })(Web.Knockout || (Web.Knockout = {}));
        var Knockout = Web.Knockout;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (Knockout) {
            var ModalBindingHandler = (function () {
                function ModalBindingHandler() {
                }
                ModalBindingHandler.prototype.init = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                    var f = ko.utils.extend(allBindings(), {
                        clickBubble: false
                    });

                    ko.bindingHandlers.click.init(element, // inject click handler that shows modal
                    function () {
                        return function () {
                            setTimeout(function () {
                                var id = valueAccessor();
                                if (id) {
                                    $('#' + id).modal('show');
                                }
                            }, 5);
                        };
                    }, allBindings, viewModel, bindingContext);
                };
                return ModalBindingHandler;
            })();

            ko.bindingHandlers['nxkit_modal'] = new ModalBindingHandler();
        })(Web.Knockout || (Web.Knockout = {}));
        var Knockout = Web.Knockout;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="../Utils.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (Knockout) {
            var OptionsBindingHandler = (function () {
                function OptionsBindingHandler() {
                }
                OptionsBindingHandler.prototype.init = function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
                    var opts = new NXKit.Web.LayoutOptions(valueAccessor());

                    // inject context containing options
                    var ctx1 = bindingContext.createChildContext(opts, null, null);

                    // inject context containing initial view model
                    var ctx2 = ctx1.createChildContext(viewModel, null, null);

                    // apply to descendants
                    ko.applyBindingsToDescendants(ctx2, element);

                    // prevent built-in application
                    return {
                        controlsDescendantBindings: true
                    };
                };
                return OptionsBindingHandler;
            })();

            ko.bindingHandlers['nxkit_layout'] = new OptionsBindingHandler();
            ko.virtualElements.allowedBindings['nxkit_layout'] = true;
        })(Web.Knockout || (Web.Knockout = {}));
        var Knockout = Web.Knockout;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="../Utils.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (Knockout) {
            var TemplateBindingHandler = (function () {
                function TemplateBindingHandler() {
                }
                TemplateBindingHandler.prototype.init = function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
                    return ko.bindingHandlers.template.init(element, TemplateBindingHandler.ConvertValueAccessor(valueAccessor, viewModel, bindingContext), allBindingsAccessor, viewModel, bindingContext);
                };

                TemplateBindingHandler.prototype.update = function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
                    return ko.bindingHandlers.template.update(element, TemplateBindingHandler.ConvertValueAccessor(valueAccessor, viewModel, bindingContext), allBindingsAccessor, viewModel, bindingContext);
                };

                /**
                * Converts the given value accessor into a value accessor compatible with the default template implementation.
                */
                TemplateBindingHandler.ConvertValueAccessor = function (valueAccessor, viewModel, bindingContext) {
                    var _this = this;
                    return ko.computed(function () {
                        var data = _this.GetTemplateViewModel(valueAccessor, viewModel, bindingContext);
                        var opts = _this.GetTemplateBinding(valueAccessor, viewModel, bindingContext);
                        var name = _this.GetTemplateName(bindingContext, opts);

                        return {
                            data: data,
                            name: name
                        };
                    });
                };

                /**
                * Gets the recommended view model for the given binding information.
                */
                TemplateBindingHandler.GetTemplateViewModel = function (valueAccessor, viewModel, bindingContext) {
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
                };

                /**
                * Extracts template index data from the given binding information.
                */
                TemplateBindingHandler.GetTemplateBinding = function (valueAccessor, viewModel, bindingContext) {
                    return NXKit.Web.Utils.GetLayoutManager(bindingContext).ParseTemplateBinding(valueAccessor, viewModel, bindingContext, {});
                };

                /**
                * Determines the named template from the given extracted data and context.
                */
                TemplateBindingHandler.GetTemplateName = function (bindingContext, data) {
                    return NXKit.Web.Utils.GetLayoutManager(bindingContext).GetTemplateName(data);
                };
                return TemplateBindingHandler;
            })();

            ko.bindingHandlers['nxkit_template'] = new TemplateBindingHandler();
            ko.virtualElements.allowedBindings['nxkit_template'] = true;
        })(Web.Knockout || (Web.Knockout = {}));
        var Knockout = Web.Knockout;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (Knockout) {
            var VisibleBindingHandler = (function () {
                function VisibleBindingHandler() {
                }
                VisibleBindingHandler.prototype.init = function (element, valueAccessor) {
                    var value = valueAccessor();
                    $(element).toggle(ko.utils.unwrapObservable(value));
                    ko.utils.unwrapObservable(value) ? $(element).slideDown() : $(element).slideUp();
                };

                VisibleBindingHandler.prototype.update = function (element, valueAccessor) {
                    var value = valueAccessor();
                    ko.utils.unwrapObservable(value) ? $(element).slideDown() : $(element).slideUp();
                };
                return VisibleBindingHandler;
            })();

            ko.bindingHandlers['nxkit_visible'] = new VisibleBindingHandler();
            ko.virtualElements.allowedBindings['nxkit_visible'] = true;
        })(Web.Knockout || (Web.Knockout = {}));
        var Knockout = Web.Knockout;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="Utils.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        var LayoutOptions = (function () {
            function LayoutOptions(args) {
                this._args = args;
            }
            /**
            * Gets the full set of currently applied layout option args for the given context.
            */
            LayoutOptions.GetArgs = function (bindingContext) {
                var a = {};
                var c = NXKit.Web.Utils.GetContextItems(bindingContext);
                for (var i = 0; i < c.length; i++)
                    if (c[i] instanceof LayoutOptions)
                        a = ko.utils.extend(a, c[i]);

                return a;
            };

            Object.defineProperty(LayoutOptions.prototype, "Args", {
                get: function () {
                    return this._args;
                },
                enumerable: true,
                configurable: true
            });
            return LayoutOptions;
        })();
        Web.LayoutOptions = LayoutOptions;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (Web) {
        var PropertyMap = (function () {
            function PropertyMap() {
            }
            return PropertyMap;
        })();
        Web.PropertyMap = PropertyMap;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="Visual.ts" />
/// <reference path="TypedEvent.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        /**
        * Main NXKit client-side view class. Injects the view interface into a set of HTML elements.
        */
        var View = (function () {
            function View(body) {
                /**
                * Raised when the Visual has changes to be pushed to the server.
                */
                this.CallbackRequest = new NXKit.Web.TypedEvent();
                var self = this;

                self._body = body;
                self._data = null;
                self._root = null;
                self._bind = true;

                self._queue = new Array();
                self._queueRunning = false;

                self._onVisualValueChanged = function (visual, property) {
                    self.OnRootVisualValueChanged(visual, property);
                };
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

                if (self._root == null) {
                    // generate new visual tree
                    self._root = new NXKit.Web.Visual(self._data);
                    self._root.ValueChanged.add(self._onVisualValueChanged);
                } else {
                    // update existing visual tree
                    self._root.ValueChanged.remove(self._onVisualValueChanged);
                    self._root.Update(self._data);
                    self._root.ValueChanged.add(self._onVisualValueChanged);
                }

                self.ApplyBindings();
            };

            /**
            * Invoked to handle root visual value change events.
            */
            View.prototype.OnRootVisualValueChanged = function (visual, property) {
                this.Push();
            };

            /**
            * Invoked when the view model initiates a request to push updates.
            */
            View.prototype.Push = function () {
                var self = this;

                this.Queue(function (cb) {
                    self.CallbackRequest.trigger(self._root.ToData(), cb);
                });
            };

            /**
            * Runs any available items in the queue.
            */
            View.prototype.Queue = function (func) {
                var self = this;

                // pushes a new event to trigger a callback onto the queue
                self._queue.push(func);

                // only one runner at a time
                if (self._queueRunning) {
                    return;
                } else {
                    self._queueRunning = true;

                    // recursive call to work queue
                    var l = function () {
                        var f = self._queue.shift();
                        if (f) {
                            f(function (result) {
                                l(); // recurse
                            });
                        } else {
                            self._queueRunning = false;
                        }
                    };

                    // initiate queue run
                    l();
                }
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
/// <reference path="Visual.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        /**
        * Base view model class for wrapping a Visual.
        */
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
            Object.defineProperty(VisualViewModel.prototype, "Context", {
                /**
                * Gets the binding context available at the time the view model was created.
                */
                get: function () {
                    return this._context;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(VisualViewModel.prototype, "Visual", {
                /**
                * Gets the visual that is wrapped by this view model.
                */
                get: function () {
                    return this._visual;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(VisualViewModel.prototype, "UniqueId", {
                /**
                * Gets the unique document ID of the wrapped visual.
                */
                get: function () {
                    return NXKit.Web.Utils.GetUniqueId(this.Visual);
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
