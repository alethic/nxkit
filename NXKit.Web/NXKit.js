define('nxkit', [
    'knockout',
    'jquery',
], function (ko, $) {
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
            Knockout.InputBindingHandler = InputBindingHandler;

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

            function DeepEquals(a, b, f) {
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

            function GenerateGuid() {
                var s = [];
                var d = "0123456789abcdef";
                for (var i = 0; i < 36; i++) {
                    s[i] = d.substr(Math.floor(Math.random() * 0x10), 1);
                }
                s[14] = "4";
                s[19] = d.substr((s[19] & 0x3) | 0x8, 1);
                s[8] = s[13] = s[18] = s[23] = "-";

                return s.join("");
            }
            Util.GenerateGuid = GenerateGuid;

            function GetContextItems(context) {
                return [context.$data].concat(context.$parents);
            }
            Util.GetContextItems = GetContextItems;

            function GetLayoutManager(context) {
                return ko.utils.arrayFirst(GetContextItems(context), function (_) {
                    return _ instanceof Web.LayoutManager;
                });
            }
            Util.GetLayoutManager = GetLayoutManager;
        })(Web.Util || (Web.Util = {}));
        var Util = Web.Util;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (Knockout) {
            var OptionsBindingHandler = (function () {
                function OptionsBindingHandler() {
                }
                OptionsBindingHandler.prototype.init = function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
                    var opts = new Web.LayoutOptions(valueAccessor());

                    var ctx1 = bindingContext.createChildContext(opts, null, null);

                    var ctx2 = ctx1.createChildContext(viewModel, null, null);

                    ko.applyBindingsToDescendants(ctx2, element);

                    return {
                        controlsDescendantBindings: true
                    };
                };
                return OptionsBindingHandler;
            })();
            Knockout.OptionsBindingHandler = OptionsBindingHandler;

            ko.bindingHandlers['nxkit_layout'] = new OptionsBindingHandler();
            ko.virtualElements.allowedBindings['nxkit_layout'] = true;
        })(Web.Knockout || (Web.Knockout = {}));
        var Knockout = Web.Knockout;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
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
            Knockout.HorizontalVisibleBindingHandler = HorizontalVisibleBindingHandler;

            ko.bindingHandlers['nxkit_hvisible'] = new HorizontalVisibleBindingHandler();
            ko.virtualElements.allowedBindings['nxkit_hvisible'] = true;
        })(Web.Knockout || (Web.Knockout = {}));
        var Knockout = Web.Knockout;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (Knockout) {
            var LayoutManagerExportBindingHandler = (function () {
                function LayoutManagerExportBindingHandler() {
                }
                LayoutManagerExportBindingHandler._init = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                    var ctx = bindingContext;
                    var mgr = NXKit.Web.ViewModelUtil.LayoutManagers;

                    for (var i = 0; i < mgr.length; i++) {
                        ctx = ctx.createChildContext(mgr[i](ctx), null, null);
                    }

                    ctx = ctx.createChildContext(viewModel);
                    ctx = ctx.createChildContext(viewModel);

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
        })(Web.Knockout || (Web.Knockout = {}));
        var Knockout = Web.Knockout;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (Web) {
        var LayoutOptions = (function () {
            function LayoutOptions(args) {
                this._args = args;
            }
            LayoutOptions.GetArgs = function (bindingContext) {
                var a = {};
                var c = Web.Util.GetContextItems(bindingContext);
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
                this._listeners.push(listener);
            };

            TypedEvent.prototype.remove = function (listener) {
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
                var context = {};
                var listeners = this._listeners.slice(0);
                for (var i = 0, l = listeners.length; i < l; i++) {
                    listeners[i].apply(context, a || []);
                }
            };
            return TypedEvent;
        })();
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (Web) {
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

                self._valueAsString = Web.Util.Computed({
                    read: function () {
                        var s = self._value() != null ? String(self._value()).trim() : null;
                        return s ? s : null;
                    },
                    write: function (value) {
                        var s = value != null ? value.trim() : null;
                        return self._value(s ? s : null);
                    }
                });

                self._valueAsBoolean = Web.Util.Computed({
                    read: function () {
                        return self._value() === true || self._value() === 'true' || self._value() === 'True';
                    },
                    write: function (value) {
                        self._value(value ? 'true' : 'false');
                    }
                });

                self._valueAsNumber = Web.Util.Computed({
                    read: function () {
                        return self._value() != '' ? parseFloat(self._value()) : null;
                    },
                    write: function (value) {
                        self._value(value != null ? value.toString() : null);
                    }
                });

                self._valueAsDate = Web.Util.Computed({
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
                        if (self._value() != null && self._value() instanceof Web.Node) {
                            self._value().Apply(source);
                            Web.Log.Debug(self.Name + ': ' + 'Node' + '=>' + 'Node');
                        } else {
                            var node = new Web.Node(self._intf.View, source);
                            self._value(node);
                            Web.Log.Debug(self.Name + ': ' + 'Node' + '+>' + 'Node');
                        }
                        self._suspend = false;

                        return;
                    }

                    var old = self._value();
                    if (old !== source) {
                        self._suspend = true;
                        self._value(source);
                        Web.Log.Debug(self.Name + ': ' + old + '=>' + source);
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
        Web.Property = Property;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (Web) {
        var Node = (function () {
            function Node(view, source) {
                this._view = view;
                this._id = -1;
                this._data = null;
                this._type = null;
                this._name = null;
                this._value = ko.observable();
                this._intfs = new Web.InterfaceMap();
                this._nodes = ko.observableArray();

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
                get: function () {
                    return this._data;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Node.prototype, "Id", {
                get: function () {
                    return this._id;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Node.prototype, "Type", {
                get: function () {
                    return this._type;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Node.prototype, "Name", {
                get: function () {
                    return this._name;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Node.prototype, "Value", {
                get: function () {
                    return this._value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Node.prototype, "Interfaces", {
                get: function () {
                    return this._intfs;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Node.prototype, "Nodes", {
                get: function () {
                    return this._nodes;
                },
                enumerable: true,
                configurable: true
            });

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

            Node.prototype.Invoke = function (interfaceName, methodName, params) {
                this.View.PushInvoke(this, interfaceName, methodName, params);
            };

            Node.prototype.Apply = function (source) {
                try  {
                    this._data = source;
                    this.ApplyId(source.Id);
                    this.ApplyType(source.Type);
                    this.ApplyName(source.Name);
                    this.ApplyValue(source.Value);
                    this.ApplyInterfaces(source);
                    this.ApplyNodes(source.Nodes);
                } catch (ex) {
                    ex.message = "Node.Apply()" + '\nMessage: ' + ex.message;
                    throw ex;
                }
            };

            Node.prototype.ApplyId = function (id) {
                this._id = id;
            };

            Node.prototype.ApplyType = function (type) {
                this._type = Web.NodeType.Parse(type);
            };

            Node.prototype.ApplyName = function (name) {
                this._name = name;
            };

            Node.prototype.ApplyValue = function (value) {
                this._value(value);
            };

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

            Node.prototype.UpdateInterface = function (name, source) {
                try  {
                    var self = this;
                    var intf = self._intfs[name];
                    if (intf == null) {
                        intf = self._intfs[name] = new Web.Interface(self, name, source);
                    } else {
                        intf.Apply(source);
                    }
                } catch (ex) {
                    ex.message = "Node.UpdateInterface()" + '\nMessage: ' + ex.message;
                    throw ex;
                }
            };

            Node.prototype.ApplyNodes = function (sources) {
                try  {
                    var self = this;

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

                    if (self._nodes().length > sources.length)
                        self._nodes.splice(sources.length);
                } catch (ex) {
                    ex.message = "Node.UpdateNodes()" + '\nMessage: ' + ex.message;
                    throw ex;
                }
            };

            Node.prototype.ToData = function () {
                var self = this;

                var r = {
                    Id: self._id
                };

                for (var i in self._intfs)
                    r[i] = self._intfs[i].ToData();

                return r;
            };

            Node.prototype.NodesToData = function () {
                return ko.utils.arrayMap(this._nodes(), function (v) {
                    return v.ToData();
                });
            };
            return Node;
        })();
        Web.Node = Node;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
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
                get: function () {
                    return this._context;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(LayoutManager.prototype, "Parent", {
                get: function () {
                    return this._parent();
                },
                enumerable: true,
                configurable: true
            });

            LayoutManager.prototype.GetTemplateOptions_ = function (valueAccessor, viewModel, bindingContext, options) {
                return this.GetTemplateOptions(valueAccessor, viewModel, bindingContext, options);
            };

            LayoutManager.prototype.GetTemplateOptions = function (valueAccessor, viewModel, bindingContext, options) {
                return this.Parent != null ? this.Parent.GetTemplateOptions(valueAccessor, viewModel, bindingContext, options || {}) : options || {};
            };

            LayoutManager.prototype.GetLocalTemplates = function () {
                return new Array();
            };

            LayoutManager.prototype.GetTemplates = function () {
                return this.GetLocalTemplates().concat(this.Parent != null ? this.Parent.GetTemplates() : new Array());
            };

            LayoutManager.prototype.GetNode = function (valueAccessor, viewModel, bindingContext) {
                var value = ko.unwrap(valueAccessor());

                if (viewModel != null) {
                    var viewModel_ = ko.unwrap(viewModel);
                    if (viewModel_ instanceof Web.Node) {
                        return viewModel_;
                    }
                }

                if (value != null && value instanceof Web.Node) {
                    return value;
                }

                if (value != null && value.node != null) {
                    return ko.unwrap(value.node);
                }
            };

            LayoutManager.prototype.GetUnknownTemplate = function (data) {
                return $('<script />', {
                    'type': 'text/html',
                    'text': '<span class="ui red label">' + JSON.stringify(data) + '</span>'
                }).appendTo('body')[0];
            };

            LayoutManager.GetTemplateNodeData = function (node) {
                var d = $(node).data('nxkit');
                if (d != null)
                    return d;

                d = {};
                for (var i = 0; i < node.attributes.length; i++) {
                    var a = node.attributes.item(i);
                    if (a.nodeName.indexOf('data-nxkit-') == 0) {
                        var n = a.nodeName.substring(11);
                        d[n] = $(node).data('nxkit-' + n);
                    }
                }

                return $(node).data('nxkit', d).data('nxkit');
            };

            LayoutManager.TemplatePredicate = function (node, opts) {
                return Web.Log.Group('TemplatePredicate', function () {
                    if (opts != null && Object.getOwnPropertyNames(opts).length == 0) {
                        Web.Log.Debug('opts: empty');
                        return false;
                    }

                    var tmpl = LayoutManager.GetTemplateNodeData(node);

                    if (Object.getOwnPropertyNames(tmpl).length == 0) {
                        Web.Log.Debug('tmpl: empty');
                        return false;
                    }

                    Web.Log.Object({
                        tmpl: tmpl,
                        opts: opts
                    });

                    return Web.Util.DeepEquals(tmpl, opts, function (a, b) {
                        return (a === '*' || b === '*') ? true : null;
                    });
                });
            };

            LayoutManager.TemplateFilter = function (nodes, data) {
                return nodes.filter(function (_) {
                    return LayoutManager.TemplatePredicate(_, data);
                });
            };

            LayoutManager.prototype.GetTemplate = function (data) {
                return LayoutManager.TemplateFilter(this.GetTemplates(), data)[0] || this.GetUnknownTemplate(data);
            };

            LayoutManager.prototype.GetTemplateName = function (data) {
                var _this = this;
                return Web.Log.Group('LayoutManager.GetTemplateName', function () {
                    var node = _this.GetTemplate(data);
                    if (node == null)
                        throw new Error('LayoutManager.GetTemplate: no template located');

                    if (node.id == '')
                        node.id = 'NXKit.Web__' + Web.Util.GenerateGuid().replace(/-/g, '');

                    Web.Log.Object({
                        id: node.id,
                        data: LayoutManager.GetTemplateNodeData(node)
                    });

                    return node.id;
                });
            };
            return LayoutManager;
        })();
        Web.LayoutManager = LayoutManager;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
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
            DefaultLayoutManager.prototype.GetTemplateOptions = function (valueAccessor, viewModel, bindingContext, options) {
                try  {
                    options = _super.prototype.GetTemplateOptions.call(this, valueAccessor, viewModel, bindingContext, options);
                    var node = _super.prototype.GetNode.call(this, valueAccessor, viewModel, bindingContext);
                    var value = ko.unwrap(valueAccessor());

                    if (node != null && node.Type == Web.NodeType.Element) {
                        options.name = node.Name;
                    }

                    if (node != null && node.Type == Web.NodeType.Text) {
                        options.type = Web.NodeType.Text.ToString();
                    }

                    if (value != null && value.name != null) {
                        options.name = ko.unwrap(value.name);
                    }

                    if (value != null && value.layout != null) {
                        options.layout = ko.unwrap(value.layout);
                    }

                    return options;
                } catch (ex) {
                    ex.message = "DefaultLayoutManager.GetTemplateOptions()" + '\nMessage: ' + ex.message;
                    throw ex;
                }
            };

            DefaultLayoutManager.prototype.GetLocalTemplates = function () {
                return $('script[type="text/html"]').toArray();
            };
            return DefaultLayoutManager;
        })(Web.LayoutManager);
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

                    ko.bindingHandlers.click.init(element, function () {
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
            Knockout.CheckboxBindingHandler = CheckboxBindingHandler;

            ko.bindingHandlers['nxkit_checkbox'] = new CheckboxBindingHandler();
        })(Web.Knockout || (Web.Knockout = {}));
        var Knockout = Web.Knockout;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (Web) {
        var Interface = (function () {
            function Interface(node, name, source) {
                var self = this;

                self._node = node;
                self._name = name;
                self._properties = new Web.PropertyMap();

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
                            self._properties[n] = new Web.Property(self, n, source[s]);
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
        Web.Interface = Interface;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (Web) {
        var Log = (function () {
            function Log() {
            }
            Log.Group = function (title, func) {
                if (typeof console.group === 'function')
                    if (Log.Verbose)
                        console.group(title);

                var result = func != null ? func() : null;
                if (Log.Verbose)
                    console.dir(result);

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
        Web.Log = Log;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (Web) {
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
        Web.Message = Message;
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
        Web.NodeType = NodeType;
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
        (function (Severity) {
            Severity[Severity["Verbose"] = 1] = "Verbose";
            Severity[Severity["Information"] = 2] = "Information";
            Severity[Severity["Warning"] = 3] = "Warning";
            Severity[Severity["Error"] = 4] = "Error";
        })(Web.Severity || (Web.Severity = {}));
        var Severity = Web.Severity;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (ViewModelUtil) {
            ViewModelUtil.LayoutManagers = [
                function (c) {
                    return new Web.DefaultLayoutManager(c);
                }
            ];

            ViewModelUtil.GroupNodes = [];

            ViewModelUtil.ControlNodes = [];

            ViewModelUtil.MetadataNodes = [];

            ViewModelUtil.TransparentNodes = [];

            ViewModelUtil.TransparentNodePredicates = [
                function (n) {
                    return ViewModelUtil.TransparentNodes.some(function (_) {
                        return _ === n.Name;
                    });
                }
            ];

            function IsEmptyTextNode(node) {
                return node.Type == Web.NodeType.Text && (node.Value() || '').trim() === '';
            }
            ViewModelUtil.IsEmptyTextNode = IsEmptyTextNode;

            function IsIgnoredNode(node) {
                return node == null || IsEmptyTextNode(node);
            }
            ViewModelUtil.IsIgnoredNode = IsIgnoredNode;

            function IsGroupNode(node) {
                return !IsIgnoredNode(node) && ViewModelUtil.GroupNodes.some(function (_) {
                    return node.Name == _;
                });
            }
            ViewModelUtil.IsGroupNode = IsGroupNode;

            function HasGroupNode(nodes) {
                return nodes.some(function (_) {
                    return IsGroupNode(_);
                });
            }
            ViewModelUtil.HasGroupNode = HasGroupNode;

            function GetGroupNodes(nodes) {
                return nodes.filter(function (_) {
                    return IsGroupNode(_);
                });
            }
            ViewModelUtil.GetGroupNodes = GetGroupNodes;

            function IsControlNode(node) {
                return !IsIgnoredNode(node) && ViewModelUtil.ControlNodes.some(function (_) {
                    return node.Name == _;
                });
            }
            ViewModelUtil.IsControlNode = IsControlNode;

            function HasControlNode(nodes) {
                return nodes.some(function (_) {
                    return IsControlNode(_);
                });
            }
            ViewModelUtil.HasControlNode = HasControlNode;

            function GetControlNodes(nodes) {
                return nodes.filter(function (_) {
                    return IsControlNode(_);
                });
            }
            ViewModelUtil.GetControlNodes = GetControlNodes;

            function IsMetadataNode(node) {
                return !IsIgnoredNode(node) && ViewModelUtil.MetadataNodes.some(function (_) {
                    return node.Name == _;
                });
            }
            ViewModelUtil.IsMetadataNode = IsMetadataNode;

            function HasMetadataNode(nodes) {
                return nodes.some(function (_) {
                    return IsMetadataNode(_);
                });
            }
            ViewModelUtil.HasMetadataNode = HasMetadataNode;

            function GetMetadataNodes(nodes) {
                return nodes.filter(function (_) {
                    return IsMetadataNode(_);
                });
            }
            ViewModelUtil.GetMetadataNodes = GetMetadataNodes;

            function IsTransparentNode(node) {
                return IsIgnoredNode(node) || ViewModelUtil.TransparentNodePredicates.some(function (_) {
                    return _(node);
                });
            }
            ViewModelUtil.IsTransparentNode = IsTransparentNode;

            function HasTransparentNode(nodes) {
                return nodes.some(function (_) {
                    return IsTransparentNode(_);
                });
            }
            ViewModelUtil.HasTransparentNode = HasTransparentNode;

            function GetTransparentNodes(nodes) {
                return nodes.filter(function (_) {
                    return IsControlNode(_);
                });
            }
            ViewModelUtil.GetTransparentNodes = GetTransparentNodes;

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
var NXKit;
(function (NXKit) {
    (function (Web) {
        var View = (function () {
            function View(body, server) {
                var self = this;

                self._server = server;
                self._body = body;
                self._save = null;
                self._hash = null;
                self._root = null;
                self._bind = true;

                self._messages = Web.Util.ObservableArray();
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

            Object.defineProperty(View.prototype, "Messages", {
                get: function () {
                    return this._messages;
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


            View.prototype.Receive = function (args) {
                this._save = args['Save'] || this._save;
                this._hash = args['Hash'] || this._hash;

                var data = args['Data'] || null;
                if (data != null) {
                    this.ReceiveData(data);
                }
            };

            View.prototype.ReceiveData = function (data) {
                if (data != null) {
                    this.Apply(data['Node'] || null);
                    this.AppendMessages(data['Messages'] || []);
                    this.ExecuteScripts(data['Scripts'] || []);
                }
            };

            View.prototype.AppendMessages = function (messages) {
                var self = this;

                for (var i = 0; i < messages.length; i++) {
                    var severity = (Web.Severity[(messages[i].Severity)]);
                    var text = messages[i].Text || '';
                    if (severity >= self._threshold)
                        self._messages.push(new Web.Message(severity, text));
                }
            };

            View.prototype.ExecuteScripts = function (scripts) {
                for (var i = 0; i < scripts.length; i++) {
                    var script = scripts[i];
                    if (script != null)
                        eval(script);
                }
            };

            View.prototype.RemoveMessage = function (message) {
                this._messages.remove(message);
            };

            View.prototype.Apply = function (data) {
                var self = this;

                if (self._root == null) {
                    self._root = new Web.Node(self, data);
                } else {
                    self._root.Apply(data);
                }

                self.ApplyBindings();
            };

            View.prototype.PushUpdate = function (node, $interface, property, value) {
                var self = this;
                Web.Log.Debug('View.PushUpdate');

                var data = {
                    Action: 'Update',
                    Args: {
                        NodeId: node.Id,
                        Interface: $interface.Name,
                        Property: property.Name,
                        Value: value
                    }
                };

                self.Queue(data);
            };

            View.prototype.PushInvoke = function (node, interfaceName, methodName, params) {
                var self = this;
                Web.Log.Debug('View.PushInvoke');

                var data = {
                    Action: 'Invoke',
                    Args: {
                        NodeId: node.Id,
                        Interface: interfaceName,
                        Method: methodName,
                        Params: params
                    }
                };

                self.Queue(data);
            };

            View.prototype.Queue = function (command) {
                var _this = this;
                var self = this;

                self._queue.push(command);

                if (self._queueRunning) {
                    return;
                } else {
                    self._queueRunning = true;

                    var node = {};
                    var scripts = new Array();
                    var messages = new Array();

                    setTimeout(function () {
                        self._busy(true);

                        var push = function () {
                            var commands = self._queue.splice(0);

                            var cb = function (args) {
                                if (args.Code == 200) {
                                    var save = args.Save || null;
                                    if (save != null) {
                                        _this._save = save;
                                    }

                                    var hash = args.Hash || null;
                                    if (hash != null) {
                                        _this._hash = hash;
                                    }

                                    var data = args.Data || null;
                                    if (data != null) {
                                        node = data['Node'] || null;
                                        ko.utils.arrayPushAll(scripts, data['Scripts']);
                                        ko.utils.arrayPushAll(messages, data['Messages']);

                                        if (self._queue.length == 0) {
                                            self.ReceiveData({
                                                Node: node,
                                                Scripts: scripts,
                                                Messages: messages
                                            });
                                        }
                                    }

                                    push();
                                } else if (args.Code == 500) {
                                    self._server({
                                        Save: self._save,
                                        Hash: self._hash,
                                        Commands: commands
                                    }, cb);
                                } else if (args.Code == 501) {
                                    for (var i = 0; i < args.Errors.length; i++) {
                                        throw new Error(args.Errors[i].Message || "");
                                    }
                                } else {
                                    throw new Error('unexpected response code');
                                }
                            };

                            if (commands.length > 0) {
                                self._server({
                                    Hash: self._hash,
                                    Commands: commands
                                }, cb);
                            } else {
                                self._queueRunning = false;
                                self._busy(false);
                            }
                        };

                        push();
                    }, 50);
                }
            };

            View.prototype.ApplyBindings = function () {
                var self = this;

                if (self._bind && self._body != null && self._root != null) {
                    ko.cleanNode(self._body);

                    $(self._body).attr('data-bind', 'template: { name: \'NXKit.View\' }');

                    ko.applyBindings(self, self._body);

                    self._bind = false;
                }
            };
            return View;
        })();
        Web.View = View;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (Web) {
        var NodeViewModel = (function () {
            function NodeViewModel(context, node) {
                var self = this;

                if (context == null)
                    throw new Error('context: null');

                if (!(node instanceof Web.Node))
                    throw new Error('node: null');

                self._context = context;
                self._node = node;
            }
            Object.defineProperty(NodeViewModel.prototype, "Context", {
                get: function () {
                    return this._context;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(NodeViewModel.prototype, "Node", {
                get: function () {
                    return this._node;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(NodeViewModel.prototype, "Contents", {
                get: function () {
                    return this.GetContents();
                },
                enumerable: true,
                configurable: true
            });

            NodeViewModel.prototype.GetContents = function () {
                try  {
                    return Web.ViewModelUtil.GetContents(this.Node);
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
        Web.NodeViewModel = NodeViewModel;
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
            Knockout.VisibleBindingHandler = VisibleBindingHandler;

            ko.bindingHandlers['nxkit_visible'] = new VisibleBindingHandler();
            ko.virtualElements.allowedBindings['nxkit_visible'] = true;
        })(Web.Knockout || (Web.Knockout = {}));
        var Knockout = Web.Knockout;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
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

                TemplateBindingHandler.ConvertValueAccessor = function (valueAccessor, viewModel, bindingContext) {
                    var _this = this;
                    return function () {
                        return Web.Log.Group('TemplateBindingHandler.ConvertValueAccessor', function () {
                            Web.Log.Object({
                                value: valueAccessor(),
                                viewModel: viewModel
                            });

                            var data = _this.GetTemplateViewModel(valueAccessor, viewModel, bindingContext);
                            if (data == null || Object.getOwnPropertyNames(data).length == 0) {
                                throw new Error('unknown viewModel');
                            }

                            var opts = _this.GetTemplateOptions(valueAccessor, viewModel, bindingContext);
                            if (opts == null || Object.getOwnPropertyNames(opts).length == 0) {
                                throw new Error('unknown template options');
                            }

                            var name = _this.GetTemplateName(bindingContext, opts);
                            if (name == null) {
                                throw new Error('unknown template');
                            }

                            Web.Log.Object({
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

                TemplateBindingHandler.GetTemplateViewModel = function (valueAccessor, viewModel, bindingContext) {
                    var value = valueAccessor();

                    if (value != null && ko.unwrap(value) instanceof Web.Node)
                        return value;

                    if (value != null && value.data != null)
                        return value.data;

                    if (value != null && value.node != null && ko.unwrap(value.node) instanceof Web.Node)
                        return value.node;

                    return viewModel;
                };

                TemplateBindingHandler.GetTemplateOptions = function (valueAccessor, viewModel, bindingContext) {
                    return NXKit.Web.Util.GetLayoutManager(bindingContext).GetTemplateOptions_(valueAccessor, viewModel, bindingContext, {});
                };

                TemplateBindingHandler.GetTemplateName = function (bindingContext, data) {
                    return NXKit.Web.Util.GetLayoutManager(bindingContext).GetTemplateName(data);
                };
                return TemplateBindingHandler;
            })();
            Knockout.TemplateBindingHandler = TemplateBindingHandler;

            ko.bindingHandlers['nxkit_template'] = new TemplateBindingHandler();
            ko.virtualElements.allowedBindings['nxkit_template'] = true;
        })(Web.Knockout || (Web.Knockout = {}));
        var Knockout = Web.Knockout;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));


    window.NXKit = NXKit;
    return NXKit;
});
