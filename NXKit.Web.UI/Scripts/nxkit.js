(function() {
var init = function ($, ko) {

var NXKit;
(function (NXKit) {
    (function (View) {
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
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));
/// <reference path="TypedEvent.ts" />
var NXKit;
(function (NXKit) {
    (function (View) {
        var Property = (function () {
            function Property(intf, name, source) {
                this._suspend = false;
                var self = this;

                self._intf = intf;
                self._name = name;

                self._value = ko.observable(null);
                self._value.subscribe(function (_) {
                    if (!self._suspend) {
                        self.OnUpdate();
                    }
                });

                self._valueAsString = View.Util.Computed({
                    read: function () {
                        var s = self._value() != null ? String(self._value()).trim() : null;
                        return s ? s : null;
                    },
                    write: function (value) {
                        var s = value != null ? value.trim() : null;
                        return self._value(s ? s : null);
                    }
                });

                self._valueAsBoolean = View.Util.Computed({
                    read: function () {
                        return self._value() === true || self._value() === 'true' || self._value() === 'True';
                    },
                    write: function (value) {
                        self._value(value ? 'true' : 'false');
                    }
                });

                self._valueAsNumber = View.Util.Computed({
                    read: function () {
                        return self._value() != '' ? parseFloat(self._value()) : null;
                    },
                    write: function (value) {
                        self._value(value != null ? value.toString() : null);
                    }
                });

                self._valueAsDate = View.Util.Computed({
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
            Object.defineProperty(Property.prototype, "Interface", {
                get: function () {
                    return this._intf;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Property.prototype, "Node", {
                get: function () {
                    return this._intf.Node;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Property.prototype, "View", {
                get: function () {
                    return this._intf.View;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Property.prototype, "Name", {
                get: function () {
                    return this._name;
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
                try  {
                    var self = this;

                    if (source != null && source.Type != null && source.Type === 'Object') {
                        self._suspend = true;
                        if (self._value() != null && self._value() instanceof View.Node) {
                            self._value().Apply(source);
                            View.Log.Debug(self.Name + ': ' + 'Node' + '=>' + 'Node');
                        } else {
                            var node = new View.Node(self._intf.View, source);
                            self._value(node);
                            View.Log.Debug(self.Name + ': ' + 'Node' + '+>' + 'Node');
                        }
                        self._suspend = false;

                        return;
                    }

                    var old = self._value();
                    if (old !== source) {
                        self._suspend = true;
                        self._value(source);
                        View.Log.Debug(self.Name + ': ' + old + '=>' + source);
                        self._suspend = false;
                    }
                } catch (ex) {
                    ex.message = "Property.Update()" + '\nMessage: ' + ex.message;
                    throw ex;
                }
            };

            Property.prototype.ToData = function () {
                return this._value();
            };

            Property.prototype.OnUpdate = function () {
                this.View.PushUpdate(this.Node, this.Interface, this, this._value());
            };
            return Property;
        })();
        View.Property = Property;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));
/// <reference path="TypedEvent.ts" />
/// <reference path="Property.ts" />
var NXKit;
(function (NXKit) {
    (function (View) {
        var Node = (function () {
            /**
            * Initializes a new instance from the given initial data.
            */
            function Node(view, source) {
                this._view = view;
                this._id = -1;
                this._data = null;
                this._type = null;
                this._name = null;
                this._value = ko.observable();
                this._intfs = new View.InterfaceMap();
                this._nodes = ko.observableArray();

                // update from source data
                if (source != null)
                    this.Apply(source);
            }
            Object.defineProperty(Node.prototype, "View", {
                get: function () {
                    return this._view;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Node.prototype, "IsNode", {
                get: function () {
                    return true;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Node.prototype, "Data", {
                /**
                * Gets the data of this node.
                */
                get: function () {
                    return this._data;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Node.prototype, "Id", {
                /**
                * Gets the unique ID of this node.
                */
                get: function () {
                    return this._id;
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

            Object.defineProperty(Node.prototype, "Name", {
                /**
                * Gets the name of this node.
                */
                get: function () {
                    return this._name;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Node.prototype, "Value", {
                /**
                * Gets the value of this node.
                */
                get: function () {
                    return this._value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Node.prototype, "Interfaces", {
                /**
                * Gets the exposed interfaces of this node.
                */
                get: function () {
                    return this._intfs;
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
                try  {
                    var i = this._intfs[interfaceName];
                    if (i == null)
                        return null;

                    var p = i.Properties[propertyName];
                    if (p == null)
                        return null;

                    return p;
                } catch (ex) {
                    ex.message = "Node.Property()" + '\nMessage: ' + ex.message;
                    throw ex;
                }
            };

            /**
            * Invokes a named method on the specified interface.
            */
            Node.prototype.Invoke = function (interfaceName, methodName, params) {
                this.View.PushInvoke(this, interfaceName, methodName, params);
            };

            /**
            * Integrates the data given by the node parameter into this node.
            */
            Node.prototype.Apply = function (source) {
                var self = this;

                var next = function () {
                    try  {
                        self._data = source;
                        self.ApplyId(source.Id);
                        self.ApplyType(source.Type);
                        self.ApplyName(source.Name);
                        self.ApplyValue(source.Value);
                        self.ApplyInterfaces(source);
                        self.ApplyNodes(source.Nodes);
                    } catch (ex) {
                        ex.message = "Node.Apply()" + '\nMessage: ' + ex.message;
                        throw ex;
                    }
                };

                for (var i in source) {
                    if (i === 'NXKit.View.Js.ViewModule') {
                        var deps = source[i]['Require'];
                        if (deps != null) {
                            self._view.Require(deps, next);
                            return;
                        }
                    }
                }

                // no requirements, continue
                next();
            };

            /**
            * Updates the type of this node with the new value.
            */
            Node.prototype.ApplyId = function (id) {
                this._id = id;
            };

            /**
            * Updates the type of this node with the new value.
            */
            Node.prototype.ApplyType = function (type) {
                this._type = View.NodeType.Parse(type);
            };

            /**
            * Updates the name of this node with the new value.
            */
            Node.prototype.ApplyName = function (name) {
                this._name = name;
            };

            /**
            * Updates the value of this node with the new value.
            */
            Node.prototype.ApplyValue = function (value) {
                this._value(value);
            };

            /**
            * Integrates the set of interfaces given with this node.
            */
            Node.prototype.ApplyInterfaces = function (source) {
                try  {
                    var self = this;
                    for (var i in source) {
                        if (i.indexOf('.') > -1)
                            self.UpdateInterface(i, source[i]);
                    }
                } catch (ex) {
                    ex.message = "Node.UpdateInterfaces()" + '\nMessage: ' + ex.message;
                    throw ex;
                }
            };

            /**
            * Updates the property given by the specified name with the specified value.
            */
            Node.prototype.UpdateInterface = function (name, source) {
                try  {
                    var self = this;
                    var intf = self._intfs[name];
                    if (intf == null) {
                        intf = self._intfs[name] = new View.Interface(self, name, source);
                    } else {
                        intf.Apply(source);
                    }
                } catch (ex) {
                    ex.message = "Node.UpdateInterface()" + '\nMessage: ' + ex.message;
                    throw ex;
                }
            };

            /**
            * Integrates the set of content nodes with the given object values.
            */
            Node.prototype.ApplyNodes = function (sources) {
                try  {
                    var self = this;

                    // clear nodes if none
                    if (sources == null) {
                        self._nodes.removeAll();
                        return;
                    }

                    for (var i = 0; i < sources.length; i++) {
                        if (self._nodes().length < i + 1) {
                            var v = new Node(self._view, sources[i]);
                            self._nodes.push(v);
                        } else {
                            self._nodes()[i].Apply(sources[i]);
                        }
                    }

                    // delete trailing values
                    if (self._nodes().length > sources.length)
                        self._nodes.splice(sources.length);
                } catch (ex) {
                    ex.message = "Node.UpdateNodes()" + '\nMessage: ' + ex.message;
                    throw ex;
                }
            };

            /**
            * Transforms the node and its hierarchy into JSON data.
            */
            Node.prototype.ToData = function () {
                var self = this;

                var r = {
                    Id: self._id
                };

                for (var i in self._intfs)
                    r[i] = self._intfs[i].ToData();

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
            return Node;
        })();
        View.Node = Node;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (View) {
        (function (Util) {
            function Observable(value) {
                return ko.observable(value).extend({
                    rateLimit: {
                        timeout: 50,
                        method: "notifyWhenChangesStop"
                    }
                });
            }
            Util.Observable = Observable;

            function ObservableArray(value) {
                return ko.observableArray(value).extend({
                    rateLimit: {
                        timeout: 50,
                        method: "notifyWhenChangesStop"
                    }
                });
            }
            Util.ObservableArray = ObservableArray;

            function Computed(def) {
                return ko.computed(def).extend({
                    rateLimit: {
                        timeout: 50,
                        method: "notifyWhenChangesStop"
                    }
                });
            }
            Util.Computed = Computed;

            /**
            * Tests two objects for equality.
            */
            function DeepEquals(a, b, f) {
                // allow overrides
                if (f != null) {
                    var t = f(a, b);
                    if (t != null) {
                        return t;
                    }
                }

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
                        if (!b.hasOwnProperty(i)) {
                            if (!Util.DeepEquals(a[i], null, f)) {
                                return false;
                            }
                        } else if (!Util.DeepEquals(a[i], b[i], f)) {
                            return false;
                        }
                    }
                }

                for (var i in b) {
                    if (b.hasOwnProperty(i)) {
                        if (!a.hasOwnProperty(i)) {
                            if (!Util.DeepEquals(null, b[i], f)) {
                                return false;
                            }
                        } else if (!Util.DeepEquals(a[i], b[i], f)) {
                            return false;
                        }
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
                    return _ instanceof View.LayoutManager;
                });
            }
            Util.GetLayoutManager = GetLayoutManager;
        })(View.Util || (View.Util = {}));
        var Util = View.Util;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));
/// <reference path="Node.ts" />
/// <reference path="Util.ts" />
var NXKit;
(function (NXKit) {
    (function (View) {
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
            LayoutManager.prototype.GetTemplateOptions_ = function (valueAccessor, viewModel, bindingContext, options) {
                return this.GetTemplateOptions(valueAccessor, viewModel, bindingContext, options);
            };

            /**
            * Parses the given template binding information for a data structure to pass to the template lookup procedures.
            */
            LayoutManager.prototype.GetTemplateOptions = function (valueAccessor, viewModel, bindingContext, options) {
                return this.Parent != null ? this.Parent.GetTemplateOptions(valueAccessor, viewModel, bindingContext, options || {}) : options || {};
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
            * Helper method for resolving a node given a Knockout context.
            */
            LayoutManager.prototype.GetNode = function (valueAccessor, viewModel, bindingContext) {
                // extract data to be used to search for a template
                var value = ko.unwrap(valueAccessor());

                // node specified as existing view model
                if (viewModel != null) {
                    var viewModel_ = ko.unwrap(viewModel);
                    if (viewModel_ instanceof View.Node) {
                        return viewModel_;
                    }
                }

                // node specified explicitely as value
                if (value != null && value instanceof View.Node) {
                    return value;
                }

                // node specified as value.node
                if (value != null && value.node != null) {
                    return ko.unwrap(value.node);
                }
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
            LayoutManager.GetTemplateNodeData = function (node) {
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
            LayoutManager.TemplatePredicate = function (node, opts) {
                return View.Log.Group('TemplatePredicate', function () {
                    // data has no properties
                    if (opts != null && Object.getOwnPropertyNames(opts).length == 0) {
                        View.Log.Debug('opts: empty');
                        return false;
                    }

                    // extract data-nxkit attributes from template node
                    var tmpl = LayoutManager.GetTemplateNodeData(node);

                    // template has no properties, should not correspond with anything
                    if (Object.getOwnPropertyNames(tmpl).length == 0) {
                        View.Log.Debug('tmpl: empty');
                        return false;
                    }

                    View.Log.Object({
                        tmpl: tmpl,
                        opts: opts
                    });

                    return View.Util.DeepEquals(tmpl, opts, function (a, b) {
                        return (a === '*' || b === '*') ? true : null;
                    });
                });
            };

            /**
            * Tests each given node against the predicate function.
            */
            LayoutManager.TemplateFilter = function (nodes, data) {
                return nodes.filter(function (_) {
                    return LayoutManager.TemplatePredicate(_, data);
                });
            };

            /**
            * Gets the appropriate template for the given data.
            */
            LayoutManager.prototype.GetTemplate = function (data) {
                return LayoutManager.TemplateFilter(this.GetTemplates(), data)[0] || this.GetUnknownTemplate(data);
            };

            /**
            * Gets the template that applies for the given data.
            */
            LayoutManager.prototype.GetTemplateName = function (data) {
                var _this = this;
                return View.Log.Group('LayoutManager.GetTemplateName', function () {
                    var node = _this.GetTemplate(data);
                    if (node == null)
                        throw new Error('LayoutManager.GetTemplate: no template located');

                    // ensure the node has a valid and unique id
                    if (node.id == '')
                        node.id = 'NXKit.Web__' + View.Util.GenerateGuid().replace(/-/g, '');

                    // log result
                    View.Log.Object({
                        id: node.id,
                        data: LayoutManager.GetTemplateNodeData(node)
                    });

                    // caller expects the id
                    return ko.observable(node.id);
                });
            };
            return LayoutManager;
        })();
        View.LayoutManager = LayoutManager;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
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
    (function (View) {
        var DefaultLayoutManager = (function (_super) {
            __extends(DefaultLayoutManager, _super);
            function DefaultLayoutManager(context) {
                _super.call(this, context);
            }
            /**
            * Parses the given template binding information for a data structure to pass to the template lookup procedures.
            */
            DefaultLayoutManager.prototype.GetTemplateOptions = function (valueAccessor, viewModel, bindingContext, options) {
                try  {
                    options = _super.prototype.GetTemplateOptions.call(this, valueAccessor, viewModel, bindingContext, options);
                    var node = _super.prototype.GetNode.call(this, valueAccessor, viewModel, bindingContext);
                    var value = ko.unwrap(valueAccessor());

                    // find element types by element name
                    if (node != null && node.Type == View.NodeType.Element) {
                        options.name = node.Name;
                    }

                    // find text node by type
                    if (node != null && node.Type == View.NodeType.Text) {
                        options.type = View.NodeType.Text.ToString();
                    }

                    // name specified explicitely
                    if (value != null && value.name != null) {
                        options.name = ko.unwrap(value.name);
                    }

                    // extract layout binding
                    if (value != null && value.layout != null) {
                        options.layout = ko.unwrap(value.layout);
                    }

                    return options;
                } catch (ex) {
                    ex.message = "DefaultLayoutManager.GetTemplateOptions()" + '\nMessage: ' + ex.message;
                    throw ex;
                }
            };

            /**
            * Gets all available templates currently in the document.
            */
            DefaultLayoutManager.prototype.GetLocalTemplates = function () {
                if (this._templates == null) {
                    this._templates = $('script[type="text/html"]').get();
                }

                return this._templates;
            };
            return DefaultLayoutManager;
        })(View.LayoutManager);
        View.DefaultLayoutManager = DefaultLayoutManager;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (View) {
        var DeferredExecutorItem = (function () {
            function DeferredExecutorItem(cb) {
                this.callback = cb;
            }
            Object.defineProperty(DeferredExecutorItem.prototype, "Promise", {
                get: function () {
                    var self = this;

                    // promise already generated
                    if (self.deferred != null)
                        return self.deferred.promise();

                    // generate promise and begin execution
                    self.deferred = $.Deferred();
                    self.callback(self.deferred);

                    // return new promise
                    return self.deferred.progress();
                },
                enumerable: true,
                configurable: true
            });
            return DeferredExecutorItem;
        })();

        /**
        * Executes a series of callbacks once for the first waiter.
        * @class NXKit.Web.DeferredExecutor
        */
        var DeferredExecutor = (function () {
            function DeferredExecutor() {
                this._queue = new Array();
            }
            /**
            * Registers a callback to be executed. The callback is passed a JQueryDeferred that it can resolve upon
            * completion.
            * @method Register
            */
            DeferredExecutor.prototype.Register = function (cb) {
                var self = this;

                self._queue.push(new DeferredExecutorItem(cb));
            };

            /**
            * Invokes the given callback when the registered callbacks are completed.
            * @method Wait
            */
            DeferredExecutor.prototype.Wait = function (cb) {
                var self = this;

                var wait = new Array();
                for (var i = 0; i < self._queue.length; i++)
                    wait[i] = self._queue[i].Promise;

                $.when.apply($, wait).done(cb);
            };
            return DeferredExecutor;
        })();
        View.DeferredExecutor = DeferredExecutor;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));
/// <reference path="TypedEvent.ts" />
var NXKit;
(function (NXKit) {
    (function (View) {
        var Interface = (function () {
            function Interface(node, name, source) {
                var self = this;

                self._node = node;
                self._name = name;
                self._properties = new View.PropertyMap();

                if (source != null)
                    self.Apply(source);
            }
            Object.defineProperty(Interface.prototype, "Node", {
                get: function () {
                    return this._node;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Interface.prototype, "View", {
                get: function () {
                    return this._node.View;
                },
                enumerable: true,
                configurable: true
            });

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

            Interface.prototype.Apply = function (source) {
                try  {
                    var self = this;

                    var removeP = [];
                    for (var i in self._properties)
                        removeP.push(self._properties[i]);

                    for (var i in source) {
                        var s = i;
                        var n = s;
                        var p = self._properties[n];
                        if (p == null) {
                            self._properties[n] = new View.Property(self, n, source[s]);
                        } else {
                            p.Update(source[s]);
                        }

                        var index = removeP.indexOf(p);
                        if (index != -1) {
                            removeP[index] = null;
                        }
                    }

                    for (var j = 0; j < removeP.length; j++) {
                        var p = removeP[j];
                        if (p != null) {
                            delete self._properties[p.Name];
                        }
                    }
                } catch (ex) {
                    ex.message = "Interface.Apply()" + '\nMessage: ' + ex.message;
                    throw ex;
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

                return r;
            };
            return Interface;
        })();
        View.Interface = Interface;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (View) {
        var InterfaceMap = (function () {
            function InterfaceMap() {
            }
            return InterfaceMap;
        })();
        View.InterfaceMap = InterfaceMap;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (View) {
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
            Knockout.CheckboxBindingHandler = CheckboxBindingHandler;

            ko.bindingHandlers['nxkit_checkbox'] = new CheckboxBindingHandler();
        })(View.Knockout || (View.Knockout = {}));
        var Knockout = View.Knockout;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));
$.fn.extend({
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
    (function (View) {
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
            Knockout.HorizontalVisibleBindingHandler = HorizontalVisibleBindingHandler;

            ko.bindingHandlers['nxkit_hvisible'] = new HorizontalVisibleBindingHandler();
            ko.virtualElements.allowedBindings['nxkit_hvisible'] = true;
        })(View.Knockout || (View.Knockout = {}));
        var Knockout = View.Knockout;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (View) {
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
            Knockout.InputBindingHandler = InputBindingHandler;

            ko.bindingHandlers['nxkit_input'] = new InputBindingHandler();
        })(View.Knockout || (View.Knockout = {}));
        var Knockout = View.Knockout;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (View) {
        (function (Knockout) {
            var LayoutManagerExportBindingHandler = (function () {
                function LayoutManagerExportBindingHandler() {
                }
                LayoutManagerExportBindingHandler._init = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                    var ctx = bindingContext;
                    var mgr = NXKit.View.ViewModelUtil.LayoutManagers;

                    for (var i = 0; i < mgr.length; i++) {
                        ctx = ctx.createChildContext(mgr[i](ctx), null, null);
                    }

                    // replace context data item with original value
                    ctx = ctx.createChildContext(viewModel);
                    ctx = ctx.createChildContext(viewModel);

                    // apply new child context to element
                    ko.applyBindingsToDescendants(ctx, element);

                    return {
                        controlsDescendantBindings: true
                    };
                };

                LayoutManagerExportBindingHandler.prototype.init = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                    return LayoutManagerExportBindingHandler._init(element, valueAccessor, allBindings, viewModel, bindingContext);
                };
                return LayoutManagerExportBindingHandler;
            })();
            Knockout.LayoutManagerExportBindingHandler = LayoutManagerExportBindingHandler;

            ko.bindingHandlers['nxkit_layout_manager_export'] = new LayoutManagerExportBindingHandler();
            ko.virtualElements.allowedBindings['nxkit_layout_manager_export'] = true;
        })(View.Knockout || (View.Knockout = {}));
        var Knockout = View.Knockout;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (View) {
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
            Knockout.ModalBindingHandler = ModalBindingHandler;

            ko.bindingHandlers['nxkit_modal'] = new ModalBindingHandler();
        })(View.Knockout || (View.Knockout = {}));
        var Knockout = View.Knockout;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));
/// <reference path="../Util.ts" />
var NXKit;
(function (NXKit) {
    (function (View) {
        (function (Knockout) {
            var OptionsBindingHandler = (function () {
                function OptionsBindingHandler() {
                }
                OptionsBindingHandler.prototype.init = function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
                    var opts = new View.LayoutOptions(valueAccessor());

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
            Knockout.OptionsBindingHandler = OptionsBindingHandler;

            ko.bindingHandlers['nxkit_layout'] = new OptionsBindingHandler();
            ko.virtualElements.allowedBindings['nxkit_layout'] = true;
        })(View.Knockout || (View.Knockout = {}));
        var Knockout = View.Knockout;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));
/// <reference path="../Util.ts" />
var NXKit;
(function (NXKit) {
    (function (View) {
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
                    return function () {
                        return View.Log.Group('TemplateBindingHandler.ConvertValueAccessor', function () {
                            View.Log.Object({
                                value: valueAccessor(),
                                viewModel: viewModel
                            });

                            // resolve the view model to be passed to the template
                            var data = _this.GetTemplateViewModel(valueAccessor, viewModel, bindingContext);
                            if (data == null || Object.getOwnPropertyNames(data).length == 0) {
                                throw new Error('unknown viewModel');
                            }

                            // resolve the options to use to look up the template
                            var opts = _this.GetTemplateOptions(valueAccessor, viewModel, bindingContext);
                            if (opts == null || Object.getOwnPropertyNames(opts).length == 0) {
                                throw new Error('unknown template options');
                            }

                            // resolve the template name from the options
                            var name = _this.GetTemplateName(bindingContext, opts);
                            if (name == null) {
                                throw new Error('unknown template');
                            }

                            View.Log.Object({
                                data: data,
                                opts: opts,
                                name: name
                            });

                            return {
                                data: data,
                                name: name
                            };
                        });
                    };
                };

                /**
                * Gets the recommended view model for the given binding information.
                */
                TemplateBindingHandler.GetTemplateViewModel = function (valueAccessor, viewModel, bindingContext) {
                    var value = valueAccessor();

                    // value itself is a node
                    if (value != null && ko.unwrap(value) instanceof View.Node)
                        return value;

                    // specified data value
                    if (value != null && value.data != null)
                        return value.data;

                    // specified node value
                    if (value != null && value.node != null && ko.unwrap(value.node) instanceof View.Node)
                        return value.node;

                    // default to existing view model
                    return viewModel;
                };

                /**
                * Extracts template index data from the given binding information.
                */
                TemplateBindingHandler.GetTemplateOptions = function (valueAccessor, viewModel, bindingContext) {
                    return NXKit.View.Util.GetLayoutManager(bindingContext).GetTemplateOptions_(valueAccessor, viewModel, bindingContext, {});
                };

                /**
                * Determines the named template from the given extracted data and context.
                */
                TemplateBindingHandler.GetTemplateName = function (bindingContext, data) {
                    return NXKit.View.Util.GetLayoutManager(bindingContext).GetTemplateName(data);
                };
                return TemplateBindingHandler;
            })();
            Knockout.TemplateBindingHandler = TemplateBindingHandler;

            ko.bindingHandlers['nxkit_template'] = new TemplateBindingHandler();
            ko.virtualElements.allowedBindings['nxkit_template'] = true;
        })(View.Knockout || (View.Knockout = {}));
        var Knockout = View.Knockout;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (View) {
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
            Knockout.VisibleBindingHandler = VisibleBindingHandler;

            ko.bindingHandlers['nxkit_visible'] = new VisibleBindingHandler();
            ko.virtualElements.allowedBindings['nxkit_visible'] = true;
        })(View.Knockout || (View.Knockout = {}));
        var Knockout = View.Knockout;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));
/// <reference path="Util.ts" />
var NXKit;
(function (NXKit) {
    (function (View) {
        var LayoutOptions = (function () {
            function LayoutOptions(args) {
                this._args = args;
            }
            /**
            * Gets the full set of currently applied layout option args for the given context.
            */
            LayoutOptions.GetArgs = function (bindingContext) {
                var a = {};
                var c = View.Util.GetContextItems(bindingContext);
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
        View.LayoutOptions = LayoutOptions;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (View) {
        var Log = (function () {
            function Log() {
            }
            Log.Group = function (title, func) {
                // start group
                if (typeof console.group === 'function')
                    if (Log.Verbose)
                        console.group(title);

                var result = func != null ? func() : null;
                if (Log.Verbose)
                    console.dir(result);

                // end group
                if (typeof console.groupEnd === 'function')
                    if (Log.Verbose)
                        console.groupEnd();

                return result;
            };

            Log.Debug = function (message) {
                var args = [];
                for (var _i = 0; _i < (arguments.length - 1); _i++) {
                    args[_i] = arguments[_i + 1];
                }
                if (Log.Verbose)
                    console.debug(message, args);
            };

            Log.Object = function (object) {
                var args = [];
                for (var _i = 0; _i < (arguments.length - 1); _i++) {
                    args[_i] = arguments[_i + 1];
                }
                if (Log.Verbose)
                    console.dir(object, args);
            };
            Log.Verbose = false;
            return Log;
        })();
        View.Log = Log;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));
/// <reference path="TypedEvent.ts" />
var NXKit;
(function (NXKit) {
    (function (View) {
        var Message = (function () {
            function Message(severity, text) {
                var self = this;

                this._severity = severity;
                this._text = text;
            }
            Object.defineProperty(Message.prototype, "Severity", {
                get: function () {
                    return this._severity;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Message.prototype, "Text", {
                get: function () {
                    return this._text;
                },
                enumerable: true,
                configurable: true
            });
            return Message;
        })();
        View.Message = Message;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (View) {
        var NodeType = (function () {
            function NodeType(value) {
                this._value = value;
            }
            NodeType.Parse = function (value) {
                switch (value.toLowerCase()) {
                    case 'text':
                        return NodeType.Text;
                    case 'element':
                        return NodeType.Element;
                }

                return NodeType.Object;
            };

            NodeType.prototype.ToString = function () {
                return this._value;
            };
            NodeType.Object = new NodeType("object");
            NodeType.Text = new NodeType("text");
            NodeType.Element = new NodeType("element");
            return NodeType;
        })();
        View.NodeType = NodeType;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));
/// <reference path="Node.ts" />
var NXKit;
(function (NXKit) {
    (function (View) {
        /**
        * Base view model class for wrapping a node.
        */
        var NodeViewModel = (function () {
            function NodeViewModel(context, node) {
                var self = this;

                if (context == null)
                    throw new Error('context: null');

                if (!(node instanceof View.Node))
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
                    return View.ViewModelUtil.GetContents(this.Node);
                } catch (ex) {
                    ex.message = 'NodeViewModel.GetContents()' + '"\nMessage: ' + ex.message;
                    throw ex;
                }
            };

            Object.defineProperty(NodeViewModel.prototype, "ContentsCount", {
                get: function () {
                    return this.Contents.length;
                },
                enumerable: true,
                configurable: true
            });
            return NodeViewModel;
        })();
        View.NodeViewModel = NodeViewModel;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (View) {
        var PropertyMap = (function () {
            function PropertyMap() {
            }
            return PropertyMap;
        })();
        View.PropertyMap = PropertyMap;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (View) {
        (function (Severity) {
            Severity[Severity["Verbose"] = 1] = "Verbose";
            Severity[Severity["Information"] = 2] = "Information";
            Severity[Severity["Warning"] = 3] = "Warning";
            Severity[Severity["Error"] = 4] = "Error";
        })(View.Severity || (View.Severity = {}));
        var Severity = View.Severity;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));
/// <reference path="Node.ts" />
/// <reference path="TypedEvent.ts" />
var NXKit;
(function (NXKit) {
    (function (_View) {
        /**
        * Main NXKit client-side view class. Injects the view interface into a set of HTML elements.
        */
        var View = (function () {
            function View(body, require, server) {
                var self = this;

                self._server = server;
                self._require = require;
                self._body = body;
                self._save = null;
                self._hash = null;
                self._root = null;
                self._bind = true;

                self._threshold = 3 /* Warning */;

                self._queue = new Array();
                self._queueRunning = false;
                self._busy = ko.observable(false);
            }
            Object.defineProperty(View.prototype, "Busy", {
                get: function () {
                    return this._busy;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(View.prototype, "Body", {
                get: function () {
                    return this._body;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(View.prototype, "Data", {
                get: function () {
                    return {
                        Save: this._save,
                        Hash: this._hash
                    };
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(View.prototype, "Root", {
                get: function () {
                    return this._root;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(View.prototype, "Threshold", {
                get: function () {
                    return this._threshold;
                },
                set: function (threshold) {
                    this._threshold = threshold;
                },
                enumerable: true,
                configurable: true
            });


            /**
            * Dispatches a request to the current require framework.
            */
            View.prototype.Require = function (deps, cb) {
                this._require(deps, cb);
            };

            /**
            * Updates the view in response to a received message.
            */
            View.prototype.Receive = function (args) {
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
            };

            /**
            * Updates the view in response to a received data package.
            */
            View.prototype.ReceiveNode = function (node) {
                var self = this;

                if (node != null) {
                    self.Apply(node || null);
                }
            };

            /**
            * Updates the messages of the view with the specified items.
            */
            View.prototype.ReceiveCommands = function (commands) {
                var self = this;

                for (var i = 0; i < commands.length; i++) {
                    var command = commands[i];
                    if (command != null) {
                        if (command.$type === 'NXKit.Server.Commands.Trace, NXKit.Server') {
                            if (command.Message != null && typeof console === 'object') {
                            }
                        }

                        if (command.$type === 'NXKit.Server.Commands.Script, NXKit.Server') {
                            if (command.Code != null) {
                                eval(command.Code);
                            }
                        }
                    }
                }
            };

            /**
            * Initiates a refresh of the view model.
            */
            View.prototype.Apply = function (data) {
                var self = this;

                if (self._root == null) {
                    // generate new node tree
                    self._root = new _View.Node(self, data);
                } else {
                    // update existing node tree
                    self._root.Apply(data);
                }

                self.ApplyBindings();
            };

            /**
            * Invoked when the view model initiates a request to push an update to a node.
            */
            View.prototype.PushUpdate = function (node, $interface, property, value) {
                var self = this;
                _View.Log.Debug('View.PushUpdate');

                // generate update command
                var command = {
                    $type: 'NXKit.Server.Commands.Update, NXKit.Server',
                    NodeId: node.Id,
                    Interface: $interface.Name,
                    Property: property.Name,
                    Value: value
                };

                self.Queue(command);
            };

            View.prototype.PushInvoke = function (node, interfaceName, methodName, parameters) {
                var self = this;
                _View.Log.Debug('View.PushInvoke');

                // generate push action
                var data = {
                    $type: 'NXKit.Server.Commands.Invoke, NXKit.Server',
                    NodeId: node.Id,
                    Interface: interfaceName,
                    Method: methodName,
                    Parameters: parameters
                };

                self.Queue(data);
            };

            /**
            * Queues the given data to be sent to the server.
            */
            View.prototype.Queue = function (command) {
                var _this = this;
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
                    var todo = new Array();

                    // delay processing in case of new commands
                    setTimeout(function () {
                        self._busy(true);

                        // recursive call to work queue
                        var push = function () {
                            var send = self._queue.splice(0);

                            // callback for server response
                            var cb = function (args) {
                                if (args.Status == 200) {
                                    // receive saved state hash
                                    var hash = args.Hash || null;
                                    if (hash != null) {
                                        _this._hash = hash;
                                    }

                                    // receive saved state
                                    var save = args.Save || null;
                                    if (save != null) {
                                        _this._save = save;
                                    }

                                    // buffer application of node
                                    if (args.Node != null) {
                                        node = args.Node;
                                    }

                                    // buffer commands
                                    if (args.Commands != null) {
                                        ko.utils.arrayPushAll(todo, args.Commands);
                                    }

                                    // only update node data if no outstanding commands
                                    if (self._queue.length == 0) {
                                        self.Receive({
                                            Hash: _this._hash,
                                            Save: _this._save,
                                            Node: node,
                                            Commands: todo
                                        });
                                    }

                                    // recurse
                                    push();
                                } else if (args.Status == 400) {
                                    // resend with save data
                                    self._server({
                                        Save: self._save,
                                        Hash: self._hash,
                                        Commands: send
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
                                    Commands: send
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

                    // execute after deferral
                    self.Require(['nx-view!nxkit.html'], function () {
                        // ensure body is to render template
                        $(self._body).attr('data-bind', 'template: { name: \'NXKit.View\' }');

                        // apply knockout to view node
                        ko.applyBindings(self, self._body);

                        self._bind = false;
                    });
                }
            };
            return View;
        })();
        _View.View = View;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (View) {
        (function (ViewModelUtil) {
            /**
            * Set of functions to inject layout managers at the top of the hierarchy.
            */
            ViewModelUtil.LayoutManagers = [
                function (c) {
                    return new View.DefaultLayoutManager(c);
                }
            ];

            /**
            * Nodes which represent a grouping element.
            */
            ViewModelUtil.GroupNodes = [];

            /**
            * Nodes which are considered to be control elements.
            */
            ViewModelUtil.ControlNodes = [];

            /**
            * Nodes which are considered to be metadata elements for their parents.
            */
            ViewModelUtil.MetadataNodes = [];

            /**
            * Nodes which are considered to be transparent, and ignored when calculating content membership.
            */
            ViewModelUtil.TransparentNodes = [];

            /**
            * Nodes which are considered to be transparent, and ignored when calculating content membership.
            */
            ViewModelUtil.TransparentNodePredicates = [
                function (n) {
                    return ViewModelUtil.TransparentNodes.some(function (_) {
                        return _ === n.Name;
                    });
                }
            ];

            /**
            * Returns true of the given node is an empty text node.
            */
            function IsEmptyTextNode(node) {
                return node.Type == View.NodeType.Text && (node.Value() || '').trim() === '';
            }
            ViewModelUtil.IsEmptyTextNode = IsEmptyTextNode;

            /**
            * Returns true if the current node is one that should be completely ignored.
            */
            function IsIgnoredNode(node) {
                return node == null || IsEmptyTextNode(node);
            }
            ViewModelUtil.IsIgnoredNode = IsIgnoredNode;

            /**
            * Returns true if the given node is a control node.
            */
            function IsGroupNode(node) {
                return !IsIgnoredNode(node) && ViewModelUtil.GroupNodes.some(function (_) {
                    return node.Name == _;
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
                return !IsIgnoredNode(node) && ViewModelUtil.ControlNodes.some(function (_) {
                    return node.Name == _;
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
                return !IsIgnoredNode(node) && ViewModelUtil.MetadataNodes.some(function (_) {
                    return node.Name == _;
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
                return IsIgnoredNode(node) || ViewModelUtil.TransparentNodePredicates.some(function (_) {
                    return _(node);
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
                        if (v == null) {
                            throw new Error('ViewModelUtil.GetContentNodes(): prospective Node is null');
                        }
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
        })(View.ViewModelUtil || (View.ViewModelUtil = {}));
        var ViewModelUtil = View.ViewModelUtil;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));
//# sourceMappingURL=nxkit.ts.js.map


    window.NXKit = NXKit;
    return NXKit;
};

if (typeof define === "function" && define.amd) {
    define("nxkit", ['jquery', 'knockout'], function ($, ko) {
        return init($, ko);
    });
} else {
    var hold = false;
    var loop = function () {
        if (typeof $ === "function" && typeof ko === "object") {
            init($, ko);
            if (hold) {
                $.holdReady(hold = false);
            }
        } else {
            if (typeof $ === "function") {
                $.holdReady(hold = true);
            }

            if (typeof console.warn === "function") {
                console.warn("nxkit: RequireJS missing or jQuery and knockout missing, retrying.");
            }

            window.setTimeout(loop, 1000);
        }
    };
    loop();
}
}());