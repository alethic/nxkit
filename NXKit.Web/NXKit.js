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
                var self = this;
                self._context = context;
            }
            Object.defineProperty(LayoutManager.prototype, "Context", {
                get: function () {
                    return this._context;
                },
                enumerable: true,
                configurable: true
            });

            LayoutManager.prototype.GetTemplate = function (descriptor) {
                console.error('LayoutManager.GetTemplate not implemented');
                return null;
            };

            LayoutManager.prototype.GetVisualTemplate = function (visual) {
                return this.GetTemplate({ Type: visual.Type });
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
            DefaultLayoutManager.prototype.GetTemplate = function (data) {
                var _this = this;
                var node = $('script').filter(function (_) {
                    return NXKit.Web.Utils.DeepEquals($(_this).data(), data);
                })[0];

                // no template found, invent an error
                if (node == null)
                    node = $('<script />', {
                        'id': 'NXKit.Web__Unknown',
                        'type': 'text/html',
                        'text': '<span class="error">no template</span>'
                    }).appendTo('body')[0];

                return node.id;
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
        (function (Utils) {
            function DeepEquals(a, b) {
                for (var i in a) {
                    if (a.hasOwnProperty(i)) {
                        if (!b.hasOwnProperty(i))
                            return false;
                        if (a[i] != b[i])
                            return false;
                    }
                }

                for (var i in b) {
                    if (b.hasOwnProperty(i)) {
                        if (!a.hasOwnProperty(i))
                            return false;
                        if (b[i] != a[i])
                            return false;
                    }
                }

                return true;
            }
            Utils.DeepEquals = DeepEquals;

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

            function GetLayoutManager(context) {
                // advance through context until we find a layout manager
                var ctx = context;
                while (ctx != null) {
                    if (ctx.$data instanceof NXKit.Web.LayoutManager)
                        return ctx.$data;

                    ctx = ctx.$parentContext || null;
                }

                // return the layout manager
                return new NXKit.Web.DefaultLayoutManager(context);
            }
            Utils.GetLayoutManager = GetLayoutManager;

            function GetVisualTemplateData(data) {
                // extract
                var _ = {};
                if (data == null)
                    return _;

                // specified visual value
                if (data.visual != null && data.visual instanceof NXKit.Web.Visual)
                    _.visual = data.visual.Type;

                // value itself is a visual
                if (data instanceof NXKit.Web.Visual)
                    _.visual = data.visual.Type;

                if (data.type != null)
                    _.type = data.type;

                return _;
            }
            Utils.GetVisualTemplateData = GetVisualTemplateData;
        })(Web.Utils || (Web.Utils = {}));
        var Utils = Web.Utils;
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
/// <reference path="Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="Visual.ts" />
/// <reference path="LayoutManager.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        var VisualLayoutManager = (function (_super) {
            __extends(VisualLayoutManager, _super);
            function VisualLayoutManager(context, visual) {
                _super.call(this, context);

                this._visual = visual;
            }
            Object.defineProperty(VisualLayoutManager.prototype, "Visual", {
                get: function () {
                    return this._visual;
                },
                enumerable: true,
                configurable: true
            });
            return VisualLayoutManager;
        })(NXKit.Web.LayoutManager);
        Web.VisualLayoutManager = VisualLayoutManager;
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

            Object.defineProperty(VisualViewModel.prototype, "LayoutManager", {
                get: function () {
                    return NXKit.Web.Utils.GetLayoutManager(this._context);
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(VisualViewModel.prototype, "Template", {
                get: function () {
                    return this.LayoutManager.GetVisualTemplate(this._visual);
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
                    return this.MetadataVisualTypes.some(function (_) {
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
                    var _this = this;
                    return visual.Visuals().some(function (_) {
                        return _this.IsControlVisual(_);
                    });
                };

                VisualViewModel.GetControlVisuals = function (visual) {
                    var _this = this;
                    return visual.Visuals().filter(function (_) {
                        return _this.IsControlVisual(_);
                    });
                };

                VisualViewModel.GetContents = function (visual) {
                    var _this = this;
                    return visual.Visuals().filter(function (_) {
                        return !_this.IsMetadataVisual(_);
                    });
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
                return VisualViewModel;
            })(NXKit.Web.VisualViewModel);
            XForms.VisualViewModel = VisualViewModel;
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
            (function (GroupLayout) {
                GroupLayout[GroupLayout["Fluid"] = 1] = "Fluid";
                GroupLayout[GroupLayout["Single"] = 2] = "Single";
                GroupLayout[GroupLayout["Double"] = 3] = "Double";
                GroupLayout[GroupLayout["Expand"] = 4] = "Expand";
            })(XForms.GroupLayout || (XForms.GroupLayout = {}));
            var GroupLayout = XForms.GroupLayout;

            var GroupLayoutItem = (function () {
                function GroupLayoutItem() {
                }
                return GroupLayoutItem;
            })();
            XForms.GroupLayoutItem = GroupLayoutItem;

            var GroupLayoutItemGroup = (function (_super) {
                __extends(GroupLayoutItemGroup, _super);
                function GroupLayoutItemGroup(getLayout) {
                    _super.call(this);
                    this._getLayout = getLayout;
                }
                Object.defineProperty(GroupLayoutItemGroup.prototype, "Layout", {
                    get: function () {
                        return this._getLayout();
                    },
                    enumerable: true,
                    configurable: true
                });
                return GroupLayoutItemGroup;
            })(GroupLayoutItem);
            XForms.GroupLayoutItemGroup = GroupLayoutItemGroup;

            var GroupLayoutSingleItemGroupFromGroup = (function (_super) {
                __extends(GroupLayoutSingleItemGroupFromGroup, _super);
                function GroupLayoutSingleItemGroupFromGroup(visual) {
                    _super.call(this);
                    this._visual = visual;
                }
                Object.defineProperty(GroupLayoutSingleItemGroupFromGroup.prototype, "Visual", {
                    get: function () {
                        return this._visual;
                    },
                    enumerable: true,
                    configurable: true
                });
                return GroupLayoutSingleItemGroupFromGroup;
            })(GroupLayoutItem);
            XForms.GroupLayoutSingleItemGroupFromGroup = GroupLayoutSingleItemGroupFromGroup;

            var GroupLayoutManager = (function (_super) {
                __extends(GroupLayoutManager, _super);
                function GroupLayoutManager(context, visual) {
                    _super.call(this, context, visual);
                }
                Object.defineProperty(GroupLayoutManager.prototype, "Level", {
                    get: function () {
                        var ctx = this.Context;
                        while ((ctx = ctx.$parentContext) != null)
                            if (ctx.$data instanceof GroupLayoutManager)
                                return ctx.$data.Level + 1;

                        return 1;
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(GroupLayoutManager.prototype, "Layout", {
                    get: function () {
                        var l = this.Level;
                        var a = this.Appearance();

                        if (l == 1 && a == "full")
                            return 1 /* Fluid */;
                        if (l == 1)
                            return 1 /* Fluid */;

                        if (l == 2 && a == "full")
                            return 1 /* Fluid */;
                        if (l == 2)
                            return 1 /* Fluid */;

                        return 2 /* Single */;
                    },
                    enumerable: true,
                    configurable: true
                });
                return GroupLayoutManager;
            })(NXKit.Web.XForms.VisualViewModel);
            XForms.GroupLayoutManager = GroupLayoutManager;
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
            var GroupViewModel = (function (_super) {
                __extends(GroupViewModel, _super);
                function GroupViewModel(context, visual) {
                    _super.call(this, context, visual);
                }
                return GroupViewModel;
            })(NXKit.Web.XForms.VisualViewModel);
            XForms.GroupViewModel = GroupViewModel;
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
/// <reference path="../../Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="../../VisualLayoutManager.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            (function (Layout) {
                var FormLayoutManager = (function (_super) {
                    __extends(FormLayoutManager, _super);
                    function FormLayoutManager(context, visual) {
                        _super.call(this, context, visual);
                    }
                    return FormLayoutManager;
                })(NXKit.Web.VisualLayoutManager);
                Layout.FormLayoutManager = FormLayoutManager;
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
//# sourceMappingURL=NXKit.js.map
