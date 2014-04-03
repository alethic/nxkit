var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (Knockout) {
            var InputBindingHandler = (function () {
                function InputBindingHandler() {
                }
                InputBindingHandler._init = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                    ko.bindingHandlers['value'].init(element, valueAccessor, allBindings, viewModel, bindingContext);
                };

                InputBindingHandler._update = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                    ko.bindingHandlers['value'].update(element, valueAccessor, allBindings, viewModel, bindingContext);
                };

                InputBindingHandler.prototype.init = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                    InputBindingHandler._init(element, valueAccessor, allBindings, viewModel, bindingContext);
                };

                InputBindingHandler.prototype.update = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                    InputBindingHandler._update(element, valueAccessor, allBindings, viewModel, bindingContext);
                };
                return InputBindingHandler;
            })();

            ko.bindingHandlers['nxkit_input'] = new InputBindingHandler();
        })(Web.Knockout || (Web.Knockout = {}));
        var Knockout = Web.Knockout;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (Util) {
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
                        if (!Util.DeepEquals(a[i], b[i]))
                            return false;
                    }
                }

                for (var i in b) {
                    if (b.hasOwnProperty(i)) {
                        if (!a.hasOwnProperty(i))
                            return false;
                        if (!Util.DeepEquals(b[i], a[i]))
                            return false;
                    }
                }

                return true;
            }
            Util.DeepEquals = DeepEquals;

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
            Util.GenerateGuid = GenerateGuid;

            /**
            * Gets the unique document ID of the given node.
            */
            function GetUniqueId(node) {
                return node.Property('NXKit.NXElement', 'UniqueId').ValueAsString();
            }
            Util.GetUniqueId = GetUniqueId;

            /**
            * Returns the entire context item chain from the specified context upwards.
            */
            function GetContextItems(context) {
                return [context.$data].concat(context.$parents);
            }
            Util.GetContextItems = GetContextItems;

            /**
            * Gets the layout manager in scope of the given binding context.
            */
            function GetLayoutManager(context) {
                return ko.utils.arrayFirst(GetContextItems(context), function (_) {
                    return _ instanceof NXKit.Web.LayoutManager;
                });
            }
            Util.GetLayoutManager = GetLayoutManager;
        })(Web.Util || (Web.Util = {}));
        var Util = Web.Util;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="../Util.ts" />
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
jQuery.fn.extend({
    slideRightShow: function () {
        return this.each(function () {
            $(this).show('slide', { direction: 'right' }, 1000);
        });
    },
    slideLeftHide: function () {
        return this.each(function () {
            $(this).hide('slide', { direction: 'left' }, 1000);
        });
    },
    slideRightHide: function () {
        return this.each(function () {
            $(this).hide('slide', { direction: 'right' }, 1000);
        });
    },
    slideLeftShow: function () {
        return this.each(function () {
            $(this).show('slide', { direction: 'left' }, 1000);
        });
    }
});

var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (Knockout) {
            var HorizontalVisibleBindingHandler = (function () {
                function HorizontalVisibleBindingHandler() {
                }
                HorizontalVisibleBindingHandler.prototype.init = function (element, valueAccessor) {
                    var value = valueAccessor();
                    $(element).toggle(ko.utils.unwrapObservable(value));
                    ko.utils.unwrapObservable(value) ? $(element)['slideLeftShow']() : $(element)['slideLeftHide']();
                };

                HorizontalVisibleBindingHandler.prototype.update = function (element, valueAccessor) {
                    var value = valueAccessor();
                    ko.utils.unwrapObservable(value) ? $(element)['slideLeftShow']() : $(element)['slideLeftHide']();
                };
                return HorizontalVisibleBindingHandler;
            })();

            ko.bindingHandlers['nxkit_hvisible'] = new HorizontalVisibleBindingHandler();
            ko.virtualElements.allowedBindings['nxkit_hvisible'] = true;
        })(Web.Knockout || (Web.Knockout = {}));
        var Knockout = Web.Knockout;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="Util.ts" />
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
                var c = NXKit.Web.Util.GetContextItems(bindingContext);
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
            function Property(name, source) {
                this._suspend = false;
                /**
                * Raised when the Property's value has changed.
                */
                this.PropertyChanged = new NXKit.Web.TypedEvent();
                var self = this;

                self._name = name;

                self._value = ko.observable();
                self._value.subscribe(function (_) {
                    if (!self._suspend) {
                        self.PropertyChanged.trigger(self, self._value());
                    }
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
                        return self._value() === true || self._value() === 'true' || self._value() === 'True';
                    },
                    write: function (value) {
                        self._value(value ? 'true' : 'false');
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
            Object.defineProperty(Property.prototype, "Name", {
                get: function () {
                    return this._name;
                },
                enumerable: true,
                configurable: true
            });

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

            Property.prototype.Update = function (source) {
                var self = this;
                if (self._value() !== source) {
                    self._suspend = true;
                    self._value(source);
                    self._suspend = false;
                }
            };

            Property.prototype.ToData = function () {
                return this._value();
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
        var Node = (function () {
            /**
            * Initializes a new instance from the given initial data.
            */
            function Node(source) {
                /**
                * Raised when the node has changes to be pushed to the server.
                */
                this.PropertyChanged = new NXKit.Web.TypedEvent();
                /**
                * Raised when the node has methods to be invoked on the server.
                */
                this.MethodInvoked = new NXKit.Web.TypedEvent();
                this._type = null;
                this._baseTypes = new Array();
                this._interfaces = new NXKit.Web.InterfaceMap();
                this._nodes = ko.observableArray();

                // update from source data
                if (source != null)
                    this.Update(source);
            }
            Object.defineProperty(Node.prototype, "IsNode", {
                get: function () {
                    return true;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Node.prototype, "Type", {
                /**
                * Gets the type of this node.
                */
                get: function () {
                    return this._type;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Node.prototype, "BaseTypes", {
                /**
                * Gets the inheritence hierarchy of this node.
                */
                get: function () {
                    return this._baseTypes;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Node.prototype, "Interfaces", {
                /**
                * Gets the exposed interfaces of this node.
                */
                get: function () {
                    return this._interfaces;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Node.prototype, "Nodes", {
                /**
                * Gets the content of this node.
                */
                get: function () {
                    return this._nodes;
                },
                enumerable: true,
                configurable: true
            });

            /**
            * Gets the named property on the named interface.
            */
            Node.prototype.Property = function (interfaceName, propertyName) {
                var i = this._interfaces[interfaceName];
                if (i == null)
                    throw new Error('Unknown interface [' + interfaceName + ':' + propertyName + ']');

                var p = i.Properties[propertyName];
                if (p == null)
                    throw new Error('Unknown property [' + interfaceName + ':' + propertyName + ']');

                return p;
            };

            /**
            * Gets the property value accessor for the named property on the specified interface.
            */
            Node.prototype.Value = function (interfaceName, propertyName) {
                return this.Property(interfaceName, propertyName).Value;
            };

            /**
            * Gets the property value accessor for the named property on the specified interface as a string.
            */
            Node.prototype.ValueAsString = function (interfaceName, propertyName) {
                return this.Property(interfaceName, propertyName).ValueAsString;
            };

            /**
            * Gets the property value accessor for the named property on the specified interface as a boolean.
            */
            Node.prototype.ValueAsBoolean = function (interfaceName, propertyName) {
                return this.Property(interfaceName, propertyName).ValueAsBoolean;
            };

            /**
            * Gets the property value accessor for the named property on the specified interface as a number.
            */
            Node.prototype.ValueAsNumber = function (interfaceName, propertyName) {
                return this.Property(interfaceName, propertyName).ValueAsNumber;
            };

            /**
            * Gets the property value accessor for the named property on the specified interface as a date.
            */
            Node.prototype.ValueAsDate = function (interfaceName, propertyName) {
                return this.Property(interfaceName, propertyName).ValueAsDate;
            };

            /**
            * Gets the named method on the named interface.
            */
            Node.prototype.Method = function (interfaceName, methodName) {
                var i = this._interfaces[interfaceName];
                if (i == null)
                    throw new Error('Unknown interface');

                var m = i.Methods[methodName];
                if (m == null)
                    throw new Error('Unknown method');

                return m;
            };

            /**
            * Invokes a named method on the specified interface.
            */
            Node.prototype.Invoke = function (interfaceName, methodName, params) {
                this.Method(interfaceName, methodName).Invoke(params);
            };

            /**
            * Integrates the data given by the node parameter into this node.
            */
            Node.prototype.Update = function (source) {
                this.UpdateType(source.Type);
                this.UpdateBaseTypes(source.BaseTypes);
                this.UpdateInterfaces(source);
                this.UpdateNodes(source.Nodes);
            };

            /**
            * Updates the type of this node with the new value.
            */
            Node.prototype.UpdateType = function (type) {
                this._type = type;
            };

            /**
            * Updates the base types of this node with the new set of values.
            */
            Node.prototype.UpdateBaseTypes = function (baseTypes) {
                this._baseTypes = baseTypes;
            };

            /**
            * Integrates the set of interfaces given with this node.
            */
            Node.prototype.UpdateInterfaces = function (source) {
                for (var i in source) {
                    if (i.indexOf('.') > -1)
                        this.UpdateInterface(i, source[i]);
                }
            };

            /**
            * Updates the property given by the specified name with the specified value.
            */
            Node.prototype.UpdateInterface = function (name, source) {
                var self = this;
                var intf = self._interfaces[name];
                if (intf == null) {
                    intf = self._interfaces[name] = new NXKit.Web.Interface(name, source);
                    intf.PropertyChanged.add(function (_, property, value) {
                        self.OnPropertyChanged(_, property, value);
                    });
                    intf.MethodInvoked.add(function (_, method, params) {
                        self.OnMethodInvoked(_, method, params);
                    });
                } else {
                    intf.Update(source);
                }
            };

            /**
            * Integrates the set of content nodes with the given object values.
            */
            Node.prototype.UpdateNodes = function (sources) {
                var _this = this;
                var self = this;

                // clear nodes if none
                if (sources == null) {
                    self._nodes.removeAll();
                    return;
                }

                for (var i = 0; i < sources.length; i++) {
                    if (self._nodes().length < i + 1) {
                        var v = new Node(sources[i]);
                        v.PropertyChanged.add(function (n, intf, property, value) {
                            _this.PropertyChanged.trigger(n, intf, property, value);
                        });
                        v.MethodInvoked.add(function (n, intf, method, params) {
                            _this.MethodInvoked.trigger(n, intf, method, params);
                        });
                        self._nodes.push(v);
                    } else {
                        self._nodes()[i].Update(sources[i]);
                    }
                }

                // delete trailing values
                if (self._nodes().length > sources.length)
                    self._nodes.splice(sources.length);
            };

            Node.prototype.ToData = function () {
                var r = {
                    Type: this._type,
                    BaseTypes: this._baseTypes,
                    Nodes: this.NodesToData()
                };

                for (var i in this._interfaces)
                    r[i] = this._interfaces[i].ToData();

                return r;
            };

            /**
            * Transforms the given Property array into a list of data to push.
            */
            Node.prototype.NodesToData = function () {
                return ko.utils.arrayMap(this._nodes(), function (v) {
                    return v.ToData();
                });
            };

            /**
            * Initiates a push of new values to the server.
            */
            Node.prototype.OnPropertyChanged = function ($interface, property, value) {
                this.PropertyChanged.trigger(this, $interface, property, value);
            };

            /**
            * Initiates a push of method invocations to the server.
            */
            Node.prototype.OnMethodInvoked = function ($interface, method, params) {
                this.MethodInvoked.trigger(this, $interface, method, params);
            };
            return Node;
        })();
        Web.Node = Node;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="Node.ts" />
/// <reference path="Util.ts" />
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
                return NXKit.Web.Util.DeepEquals(d1, d2);
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
                        node.id = 'NXKit.Web__' + NXKit.Web.Util.GenerateGuid().replace(/-/g, '');

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
/// <reference path="Node.ts" />
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
                try  {
                    data = _super.prototype.ParseTemplateBinding.call(this, valueAccessor, viewModel, bindingContext, data);

                    // extract data to be used to search for a template
                    var value = valueAccessor();

                    // template specified
                    if (value != null && value.template != null) {
                        var tmpl = ko.unwrap(value.template);
                        if (tmpl != null) {
                            data.template = tmpl;
                        }
                    }

                    // value with template
                    if (data.template == null && data.node == null && value != null && ko.unwrap(value) instanceof NXKit.Web.Node) {
                        var node = ko.unwrap(value);
                        var intf = node.Interfaces['NXKit.Web.ITemplate'];
                        if (intf != null) {
                            var prop = intf.Properties['Name'];
                            if (prop != null) {
                                var tmpl = prop.ValueAsString();
                                if (tmpl != null) {
                                    data.template = tmpl;
                                }
                            }
                        }
                    }

                    // value
                    if (data.template == null && data.node == null && value != null && ko.unwrap(value) instanceof NXKit.Web.Node) {
                        var node = ko.unwrap(value);
                        if (node.Type != null) {
                            data.node = node.Type;
                        }
                    }

                    // value.node with template
                    if (data.template == null && data.node == null && value != null && value.node != null && ko.unwrap(value.node) instanceof NXKit.Web.Node) {
                        var node = ko.unwrap(value.node);
                        var intf = node.Interfaces['NXKit.Web.ITemplate'];
                        if (intf != null) {
                            var prop = intf.Properties['Name'];
                            if (prop != null) {
                                var tmpl = prop.ValueAsString();
                                if (tmpl != null) {
                                    data.template = tmpl;
                                }
                            }
                        }
                    }

                    // value.node
                    if (data.template == null && data.node == null && value != null && value.node != null && ko.unwrap(value.node) instanceof NXKit.Web.Node) {
                        var node = ko.unwrap(value.node);
                        if (node.Type != null) {
                            data.node = node.Type;
                        }
                    }

                    // viewModel with template
                    if (data.template == null && data.node == null) {
                        if (viewModel instanceof NXKit.Web.Node) {
                            var node = viewModel;
                            var intf = node.Interfaces['NXKit.Web.ITemplate'];
                            if (intf != null) {
                                var prop = intf.Properties['Name'];
                                if (prop != null) {
                                    var tmpl = prop.ValueAsString();
                                    if (tmpl != null) {
                                        data.template = tmpl;
                                    }
                                }
                            }
                        }
                    }

                    // viewModel
                    if (data.template == null && data.node == null) {
                        if (viewModel instanceof NXKit.Web.Node) {
                            var node = viewModel;
                            if (node.Type != null) {
                                data.node = node.Type;
                            }
                        }
                    }

                    // node specified as string
                    if (value != null && value.node != null && typeof value.node === 'string') {
                        data.node = value.node;
                    }

                    // specified data type
                    if (value != null && value.type != null) {
                        data.type = ko.unwrap(value.type);
                    }

                    // extract layout binding
                    if (value != null && value.layout != null && ko.unwrap(value.layout) != null) {
                        data.layout = ko.unwrap(value.layout);
                    }

                    return data;
                } catch (ex) {
                    ex.message = "DefaultLayoutManager.ParseTemplateBinding()" + '\nMessage: ' + ex.message;
                    throw ex;
                }
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
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (Knockout) {
            var CheckboxBindingHandler = (function () {
                function CheckboxBindingHandler() {
                }
                CheckboxBindingHandler._init = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
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
                        CheckboxBindingHandler._update(element, valueAccessor, allBindings, viewModel, bindingContext);
                    }, 2000);
                };

                CheckboxBindingHandler._update = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                    var self = this;
                    setTimeout(function () {
                        var v1 = ko.unwrap(valueAccessor());
                        if (typeof v1 === 'boolean') {
                            $(element).checkbox(v1 ? 'enable' : 'disable');
                        } else if (typeof v1 === 'string') {
                            var v1_ = v1.toLowerCase() === 'true' ? true : false;
                            $(element).checkbox(v1_ ? 'enable' : 'disable');
                        }
                    }, 1000);
                };

                CheckboxBindingHandler.prototype.init = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                    CheckboxBindingHandler._init(element, valueAccessor, allBindings, viewModel, bindingContext);
                };

                CheckboxBindingHandler.prototype.update = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                    CheckboxBindingHandler._update(element, valueAccessor, allBindings, viewModel, bindingContext);
                };
                return CheckboxBindingHandler;
            })();

            ko.bindingHandlers['nxkit_checkbox'] = new CheckboxBindingHandler();
        })(Web.Knockout || (Web.Knockout = {}));
        var Knockout = Web.Knockout;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="TypedEvent.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        var Interface = (function () {
            function Interface(name, source) {
                this.PropertyChanged = new NXKit.Web.TypedEvent();
                this.MethodInvoked = new NXKit.Web.TypedEvent();
                var self = this;

                self._name = name;
                self._properties = new NXKit.Web.PropertyMap();
                self._methods = new NXKit.Web.MethodMap();

                if (source != null)
                    self.Update(source);
            }
            Object.defineProperty(Interface.prototype, "Name", {
                get: function () {
                    return this._name;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Interface.prototype, "Properties", {
                get: function () {
                    return this._properties;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Interface.prototype, "Methods", {
                get: function () {
                    return this._methods;
                },
                enumerable: true,
                configurable: true
            });

            Interface.prototype.Update = function (source) {
                var self = this;

                for (var i in source) {
                    var s = i;
                    if (s.indexOf('@') === 0) {
                        var n = s.substring(1, s.length);
                        var m = self._methods[n];
                        if (m == null) {
                            self._methods[n] = new NXKit.Web.Method(n, source[s]);
                            self._methods[n].MethodInvoked.add(function (_, params) {
                                self.OnMethodInvoked(_, params);
                            });
                        } else {
                            m.Update(source[s]);
                        }
                    } else {
                        var n = s;
                        var p = self._properties[n];
                        if (p == null) {
                            self._properties[n] = new NXKit.Web.Property(n, source[s]);
                            self._properties[n].PropertyChanged.add(function (_, value) {
                                self.OnPropertyChanged(_, value);
                            });
                        } else {
                            p.Update(source[s]);
                        }
                    }
                }
            };

            Interface.prototype.ToData = function () {
                var self = this;
                var r = {};

                for (var i in self._properties) {
                    var s = i;
                    var p = self._properties[s];
                    r[self._properties[s].Name] = p.ToData();
                }

                for (var i in self._methods) {
                    var s = i;
                    var m = self._methods[s];
                    r['@' + m.Name] = m.ToData();
                }

                return r;
            };

            Interface.prototype.OnPropertyChanged = function (property, value) {
                this.PropertyChanged.trigger(this, property, value);
            };

            Interface.prototype.OnMethodInvoked = function (method, params) {
                this.MethodInvoked.trigger(this, method, params);
            };
            return Interface;
        })();
        Web.Interface = Interface;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="TypedEvent.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        var Method = (function () {
            function Method(name, source) {
                this.MethodInvoked = new NXKit.Web.TypedEvent();
                var self = this;

                this._name = name;
                this._data = [];
            }
            Object.defineProperty(Method.prototype, "Name", {
                get: function () {
                    return this._name;
                },
                enumerable: true,
                configurable: true
            });

            Method.prototype.Invoke = function (params) {
                this._data.push(params);
                this.OnMethodInvoked(params);
            };

            Method.prototype.Update = function (source) {
                this._data = source;
            };

            Method.prototype.ToData = function () {
                return this._data.length > 0 ? this._data : null;
            };

            Method.prototype.OnMethodInvoked = function (params) {
                this.MethodInvoked.trigger(this, params);
            };
            return Method;
        })();
        Web.Method = Method;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (Web) {
        var InterfaceMap = (function () {
            function InterfaceMap() {
            }
            return InterfaceMap;
        })();
        Web.InterfaceMap = InterfaceMap;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (Web) {
        var MethodMap = (function () {
            function MethodMap() {
            }
            return MethodMap;
        })();
        Web.MethodMap = MethodMap;
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
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (ViewModelUtil) {
            /**
            * Node types which represent a grouping element.
            */
            ViewModelUtil.GroupNodeTypes = [];

            /**
            * Node types which are considered to be control elements.
            */
            ViewModelUtil.ControlNodeTypes = [];

            /**
            * Node types which are considered to be metadata elements for their parents.
            */
            ViewModelUtil.MetadataNodeTypes = [];

            /**
            * Node types which are considered to be transparent, and ignored when calculating content membership.
            */
            ViewModelUtil.TransparentNodeTypes = [];

            /**
            * Returns true if the given node is a control node.
            */
            function IsGroupNode(node) {
                return ViewModelUtil.GroupNodeTypes.some(function (_) {
                    return node.Type == _;
                });
            }
            ViewModelUtil.IsGroupNode = IsGroupNode;

            /**
            * Returns true if the given node set contains a control node.
            */
            function HasGroupNode(nodes) {
                return nodes.some(function (_) {
                    return IsGroupNode(_);
                });
            }
            ViewModelUtil.HasGroupNode = HasGroupNode;

            /**
            * Filters out the given node set for control nodes.
            */
            function GetGroupNodes(nodes) {
                return nodes.filter(function (_) {
                    return IsGroupNode(_);
                });
            }
            ViewModelUtil.GetGroupNodes = GetGroupNodes;

            /**
            * Returns true if the given node is a control node.
            */
            function IsControlNode(node) {
                return ViewModelUtil.ControlNodeTypes.some(function (_) {
                    return node.Type == _;
                });
            }
            ViewModelUtil.IsControlNode = IsControlNode;

            /**
            * Returns true if the given node set contains a control node.
            */
            function HasControlNode(nodes) {
                return nodes.some(function (_) {
                    return IsControlNode(_);
                });
            }
            ViewModelUtil.HasControlNode = HasControlNode;

            /**
            * Filters out the given node set for control nodes.
            */
            function GetControlNodes(nodes) {
                return nodes.filter(function (_) {
                    return IsControlNode(_);
                });
            }
            ViewModelUtil.GetControlNodes = GetControlNodes;

            /**
            * Returns true if the given node is a transparent node.
            */
            function IsMetadataNode(node) {
                return ViewModelUtil.MetadataNodeTypes.some(function (_) {
                    return node.Type == _;
                });
            }
            ViewModelUtil.IsMetadataNode = IsMetadataNode;

            /**
            * Returns true if the given node set contains a metadata node.
            */
            function HasMetadataNode(nodes) {
                return nodes.some(function (_) {
                    return IsMetadataNode(_);
                });
            }
            ViewModelUtil.HasMetadataNode = HasMetadataNode;

            /**
            * Filters out the given node set for control nodes.
            */
            function GetMetadataNodes(nodes) {
                return nodes.filter(function (_) {
                    return IsMetadataNode(_);
                });
            }
            ViewModelUtil.GetMetadataNodes = GetMetadataNodes;

            /**
            * Returns true if the given node is a transparent node.
            */
            function IsTransparentNode(node) {
                return ViewModelUtil.TransparentNodeTypes.some(function (_) {
                    return node.Type == _;
                });
            }
            ViewModelUtil.IsTransparentNode = IsTransparentNode;

            /**
            * Returns true if the given node set contains a transparent node.
            */
            function HasTransparentNode(nodes) {
                return nodes.some(function (_) {
                    return IsTransparentNode(_);
                });
            }
            ViewModelUtil.HasTransparentNode = HasTransparentNode;

            /**
            * Filters out the given node set for transparent nodes.
            */
            function GetTransparentNodes(nodes) {
                return nodes.filter(function (_) {
                    return IsControlNode(_);
                });
            }
            ViewModelUtil.GetTransparentNodes = GetTransparentNodes;

            /**
            * Filters out the given node set for content nodes. This descends through transparent nodes.
            */
            function GetContentNodes(nodes) {
                try  {
                    var l = nodes.filter(function (_) {
                        return !IsMetadataNode(_);
                    });
                    var r = new Array();
                    for (var i = 0; i < l.length; i++) {
                        var v = l[i];
                        if (IsTransparentNode(v)) {
                            var s = GetContentNodes(v.Nodes());
                            for (var j = 0; j < s.length; j++)
                                r.push(s[j]);
                        } else {
                            r.push(v);
                        }
                    }

                    return r;
                } catch (ex) {
                    ex.message = 'ViewModelUtil.GetContentNodes()' + '"\nMessage: ' + ex.message;
                    throw ex;
                }
            }
            ViewModelUtil.GetContentNodes = GetContentNodes;

            /**
            * Gets the content nodes of the given node. This descends through transparent nodes.
            */
            function GetContents(node) {
                try  {
                    return GetContentNodes(node.Nodes());
                } catch (ex) {
                    ex.message = 'ViewModelUtil.GetContents()' + '"\nMessage: ' + ex.message;
                    throw ex;
                }
            }
            ViewModelUtil.GetContents = GetContents;
        })(Web.ViewModelUtil || (Web.ViewModelUtil = {}));
        var ViewModelUtil = Web.ViewModelUtil;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="Node.ts" />
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
                * Raised when the Node has changes to be pushed to the server.
                */
                this.CallbackRequest = new NXKit.Web.TypedEvent();
                var self = this;

                self._body = body;
                self._root = null;
                self._bind = true;

                self._queue = new Array();
                self._queueRunning = false;

                self._onNodePropertyChanged = function (node, $interface, property, value) {
                    self.OnRootNodePropertyChanged(node, $interface, property, value);
                };

                self._onNodeMethodInvoked = function (node, $interface, method, params) {
                    self.OnRootNodeMethodInvoked(node, $interface, method, params);
                };
            }
            Object.defineProperty(View.prototype, "Body", {
                get: function () {
                    return this._body;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(View.prototype, "Data", {
                get: function () {
                    return this._root.ToData();
                },
                set: function (value) {
                    var self = this;

                    if (typeof (value) === 'string')
                        // update the form with the parsed data
                        self.Update(JSON.parse(value));
                    else
                        // update the form with the data itself
                        self.Update(value);
                },
                enumerable: true,
                configurable: true
            });


            /**
            * Initiates a refresh of the view model.
            */
            View.prototype.Update = function (data) {
                var self = this;

                if (self._root == null) {
                    // generate new node tree
                    self._root = new NXKit.Web.Node(data);
                    self._root.PropertyChanged.add(self._onNodePropertyChanged);
                    self._root.MethodInvoked.add(self._onNodeMethodInvoked);
                } else {
                    // update existing node tree
                    self._root.PropertyChanged.remove(self._onNodePropertyChanged);
                    self._root.MethodInvoked.remove(self._onNodeMethodInvoked);
                    self._root.Update(data);
                    self._root.PropertyChanged.add(self._onNodePropertyChanged);
                    self._root.MethodInvoked.add(self._onNodeMethodInvoked);
                }

                self.ApplyBindings();
            };

            /**
            * Invoked to handle root node value change events.
            */
            View.prototype.OnRootNodePropertyChanged = function (node, $interface, property, value) {
                this.Push();
            };

            /**
            * Invoked to handle root node method invocations.
            */
            View.prototype.OnRootNodeMethodInvoked = function (node, $interface, method, params) {
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
                var self = this;

                // apply bindings to our element and our view model
                if (self._bind && self._body != null && self._root != null) {
                    // clear existing bindings
                    ko.cleanNode(self._body);

                    // apply knockout to view node
                    ko.applyBindings(self._root, self._body);

                    self._bind = false;
                }
            };
            return View;
        })();
        Web.View = View;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="Node.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        /**
        * Base view model class for wrapping a node.
        */
        var NodeViewModel = (function () {
            function NodeViewModel(context, node) {
                var self = this;

                if (context == null)
                    throw new Error('context: null');

                if (!(node instanceof NXKit.Web.Node))
                    throw new Error('node: null');

                self._context = context;
                self._node = node;
            }
            Object.defineProperty(NodeViewModel.prototype, "Context", {
                /**
                * Gets the binding context available at the time the view model was created.
                */
                get: function () {
                    return this._context;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(NodeViewModel.prototype, "Node", {
                /**
                * Gets the node that is wrapped by this view model.
                */
                get: function () {
                    return this._node;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(NodeViewModel.prototype, "UniqueId", {
                /**
                * Gets the unique document ID of the wrapped node.
                */
                get: function () {
                    return NXKit.Web.Util.GetUniqueId(this.Node);
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(NodeViewModel.prototype, "Contents", {
                /**
                * Gets the content nodes of the current node.
                */
                get: function () {
                    return this.GetContents();
                },
                enumerable: true,
                configurable: true
            });

            NodeViewModel.prototype.GetContents = function () {
                try  {
                    return NXKit.Web.ViewModelUtil.GetContents(this.Node);
                } catch (ex) {
                    ex.message = 'NodeViewModel.GetContents()' + '"\nMessage: ' + ex.message;
                    throw ex;
                }
            };
            return NodeViewModel;
        })();
        Web.NodeViewModel = NodeViewModel;
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
                DropdownBindingHandler._init = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                    setTimeout(function () {
                        $(element).dropdown();
                        $(element).dropdown({
                            onChange: function (value) {
                                var v1 = $(element).dropdown('get value');
                                var v2 = ko.unwrap(valueAccessor());
                                if (typeof v1 === 'string') {
                                    if (v1 != v2)
                                        valueAccessor()(v1);
                                }
                            }
                        });
                        DropdownBindingHandler._update(element, valueAccessor, allBindings, viewModel, bindingContext);
                    }, 1000);
                };

                DropdownBindingHandler._update = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                    setTimeout(function () {
                        var v1 = ko.unwrap(valueAccessor());
                        $(element).dropdown('set value', v1);
                    }, 2000);
                };

                DropdownBindingHandler.prototype.init = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                    DropdownBindingHandler._init(element, valueAccessor, allBindings, viewModel, bindingContext);
                };

                DropdownBindingHandler.prototype.update = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                    DropdownBindingHandler._update(element, valueAccessor, allBindings, viewModel, bindingContext);
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
/// <reference path="../Util.ts" />
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

                    // value itself is a node
                    if (value != null && ko.unwrap(value) instanceof NXKit.Web.Node)
                        return ko.unwrap(value);

                    // specified data value
                    if (value != null && value.data != null)
                        return ko.unwrap(value.data);

                    // specified node value
                    if (value != null && value.node != null && ko.unwrap(value.node) instanceof NXKit.Web.Node)
                        return ko.unwrap(value.node);

                    // default to existing context
                    return null;
                };

                /**
                * Extracts template index data from the given binding information.
                */
                TemplateBindingHandler.GetTemplateBinding = function (valueAccessor, viewModel, bindingContext) {
                    return NXKit.Web.Util.GetLayoutManager(bindingContext).ParseTemplateBinding(valueAccessor, viewModel, bindingContext, {});
                };

                /**
                * Determines the named template from the given extracted data and context.
                */
                TemplateBindingHandler.GetTemplateName = function (bindingContext, data) {
                    return NXKit.Web.Util.GetLayoutManager(bindingContext).GetTemplateName(data);
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
//# sourceMappingURL=NXKit.js.map
