(function () {
var init = function ($, ko, NXKit) {

var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            var XFormsNodeViewModel = (function (_super) {
                __extends(XFormsNodeViewModel, _super);
                function XFormsNodeViewModel(context, node) {
                    _super.call(this, context, node);
                }
                Object.defineProperty(XFormsNodeViewModel.prototype, "Value", {
                    get: function () {
                        return XForms.ViewModelUtil.GetDataValue(this.Node);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsNodeViewModel.prototype, "ValueAsString", {
                    get: function () {
                        return XForms.ViewModelUtil.GetDataValueAsString(this.Node);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsNodeViewModel.prototype, "ValueAsBoolean", {
                    get: function () {
                        return XForms.ViewModelUtil.GetDataValueAsBoolean(this.Node);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsNodeViewModel.prototype, "ValueAsNumber", {
                    get: function () {
                        return XForms.ViewModelUtil.GetDataValueAsNumber(this.Node);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsNodeViewModel.prototype, "Relevant", {
                    get: function () {
                        return XForms.ViewModelUtil.GetRelevant(this.Node);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsNodeViewModel.prototype, "ReadOnly", {
                    get: function () {
                        return XForms.ViewModelUtil.GetReadOnly(this.Node);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsNodeViewModel.prototype, "Required", {
                    get: function () {
                        return XForms.ViewModelUtil.GetRequired(this.Node);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsNodeViewModel.prototype, "Valid", {
                    get: function () {
                        return XForms.ViewModelUtil.GetValid(this.Node);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsNodeViewModel.prototype, "Type", {
                    get: function () {
                        return XForms.ViewModelUtil.GetDataType(this.Node);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsNodeViewModel.prototype, "Appearance", {
                    get: function () {
                        return XForms.ViewModelUtil.GetAppearance(this.Node);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsNodeViewModel.prototype, "Label", {
                    get: function () {
                        try  {
                            return XForms.ViewModelUtil.GetLabelNode(this.Node);
                        } catch (ex) {
                            ex.message = 'XFormsNodeViewModel.Label' + '"\nMessage: ' + ex.message;
                            throw ex;
                        }
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsNodeViewModel.prototype, "LabelAppearance", {
                    get: function () {
                        return this.Label != null ? XForms.ViewModelUtil.GetAppearance(this.Label) : null;
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsNodeViewModel.prototype, "Help", {
                    get: function () {
                        return XForms.ViewModelUtil.GetHelpNode(this.Node);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsNodeViewModel.prototype, "Hint", {
                    get: function () {
                        return XForms.ViewModelUtil.GetHintNode(this.Node);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsNodeViewModel.prototype, "Alert", {
                    get: function () {
                        return XForms.ViewModelUtil.GetAlertNode(this.Node);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsNodeViewModel.prototype, "CountEnabled", {
                    get: function () {
                        return ko.utils.arrayFilter(this.Contents, function (_) {
                            return XForms.ViewModelUtil.GetRelevant(_)();
                        }).length;
                    },
                    enumerable: true,
                    configurable: true
                });
                return XFormsNodeViewModel;
            })(NXKit.Web.NodeViewModel);
            XForms.XFormsNodeViewModel = XFormsNodeViewModel;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="XFormsNodeViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            var DefaultLayoutManager = (function (_super) {
                __extends(DefaultLayoutManager, _super);
                function DefaultLayoutManager(context) {
                    _super.call(this, context);
                }
                /**
                * Applies the 'level' and 'layout' bindings to the template search.
                */
                DefaultLayoutManager.prototype.GetTemplateOptions = function (valueAccessor, viewModel, bindingContext, options) {
                    options = _super.prototype.GetTemplateOptions.call(this, valueAccessor, viewModel, bindingContext, options);
                    var node = _super.prototype.GetNode.call(this, valueAccessor, viewModel, bindingContext);
                    var value = ko.unwrap(valueAccessor());

                    if (node != null && node.Type == Web.NodeType.Element) {
                        var dataType = XForms.ViewModelUtil.GetDataType(node)();
                        if (dataType != null) {
                            options['data-type'] = dataType;
                        }
                    }

                    // specified data type
                    if (value != null && value['data-type'] != null) {
                        options['data-type'] = ko.unwrap(value['data-type']);
                    }

                    // extract level binding
                    var value = valueAccessor();
                    if (value != null && value.level != null) {
                        options.level = ko.unwrap(value.level);
                    }

                    return options;
                };
                return DefaultLayoutManager;
            })(NXKit.Web.LayoutManager);
            XForms.DefaultLayoutManager = DefaultLayoutManager;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            NXKit.Web.TemplateManager.Default.Register('nxkit-xforms.html');
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="XFormsNodeViewModel.ts" />
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
                GroupLayoutManager.prototype.GetTemplateOptions = function (valueAccessor, viewModel, bindingContext, options) {
                    options = _super.prototype.GetTemplateOptions.call(this, valueAccessor, viewModel, bindingContext, options);
                    var value = ko.unwrap(valueAccessor());

                    // extract level binding
                    if (value != null && value.level != null)
                        options.level = ko.unwrap(value.level);

                    return options;
                };
                return GroupLayoutManager;
            })(NXKit.Web.LayoutManager);
            XForms.GroupLayoutManager = GroupLayoutManager;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="XFormsNodeViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            var OutputViewModel = (function (_super) {
                __extends(OutputViewModel, _super);
                function OutputViewModel(context, node) {
                    _super.call(this, context, node);
                }
                Object.defineProperty(OutputViewModel.prototype, "Text", {
                    get: function () {
                        var _this = this;
                        return ko.computed(function () {
                            return _this.ValueAsString() || _this.Node.Property('NXKit.XForms.Output', 'Value').ValueAsString();
                        });
                    },
                    enumerable: true,
                    configurable: true
                });
                return OutputViewModel;
            })(NXKit.Web.XForms.XFormsNodeViewModel);
            XForms.OutputViewModel = OutputViewModel;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="XFormsNodeViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            var RepeatViewModel = (function (_super) {
                __extends(RepeatViewModel, _super);
                function RepeatViewModel(context, node) {
                    _super.call(this, context, node);
                }
                return RepeatViewModel;
            })(NXKit.Web.XForms.XFormsNodeViewModel);
            XForms.RepeatViewModel = RepeatViewModel;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (Knockout) {
            var InputBooleanBindingHandler = (function () {
                function InputBooleanBindingHandler() {
                }
                InputBooleanBindingHandler._init = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                    var self = this;
                    var checkbox = $(element);
                    setTimeout(function () {
                        $(checkbox).checkbox();
                        $(checkbox).checkbox('setting', {
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
                        InputBooleanBindingHandler._update(element, valueAccessor, allBindings, viewModel, bindingContext);
                    }, 2000);
                };

                InputBooleanBindingHandler._update = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                    var self = this;
                    var checkbox = $(element);
                    setTimeout(function () {
                        var v1 = ko.unwrap(valueAccessor());
                        if (typeof v1 === 'boolean') {
                            $(checkbox).checkbox(v1 ? 'enable' : 'disable');
                        } else if (typeof v1 === 'string') {
                            var v1_ = v1.toLowerCase() === 'true' ? true : false;
                            $(checkbox).checkbox(v1_ ? 'enable' : 'disable');
                        }
                    }, 1000);
                };

                InputBooleanBindingHandler.prototype.init = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                    InputBooleanBindingHandler._init(element, valueAccessor, allBindings, viewModel, bindingContext);
                };

                InputBooleanBindingHandler.prototype.update = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                    InputBooleanBindingHandler._update(element, valueAccessor, allBindings, viewModel, bindingContext);
                };
                return InputBooleanBindingHandler;
            })();
            Knockout.InputBooleanBindingHandler = InputBooleanBindingHandler;

            ko.bindingHandlers['nxkit_xforms_input_boolean'] = new InputBooleanBindingHandler();
        })(Web.Knockout || (Web.Knockout = {}));
        var Knockout = Web.Knockout;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            (function (Knockout) {
                var Select1BindingHandler = (function () {
                    function Select1BindingHandler() {
                    }
                    Select1BindingHandler._init = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                        var body = $(element).find('.body');
                        setTimeout(function () {
                            $(body).dropdown();
                            $(body).dropdown({
                                onChange: function (value) {
                                    var v1 = $(body).dropdown('get value') || null;
                                    var v2 = ko.unwrap(valueAccessor()) || null;
                                    if (v1 != v2) {
                                        valueAccessor()(v1);
                                    }
                                }
                            });
                            Select1BindingHandler._update(element, valueAccessor, allBindings, viewModel, bindingContext);
                        }, 1000);
                    };

                    Select1BindingHandler._update = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                        var body = $(element).find('.body');
                        setTimeout(function () {
                            var v1 = ko.unwrap(valueAccessor()) || null;
                            $(body).dropdown('set value', v1);
                        }, 2000);
                    };

                    Select1BindingHandler.prototype.init = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                        Select1BindingHandler._init(element, valueAccessor, allBindings, viewModel, bindingContext);
                    };

                    Select1BindingHandler.prototype.update = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                        Select1BindingHandler._update(element, valueAccessor, allBindings, viewModel, bindingContext);
                    };
                    return Select1BindingHandler;
                })();
                Knockout.Select1BindingHandler = Select1BindingHandler;

                ko.bindingHandlers['nxkit_xforms_select1'] = new Select1BindingHandler();
            })(XForms.Knockout || (XForms.Knockout = {}));
            var Knockout = XForms.Knockout;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="XFormsNodeViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            var SubmitViewModel = (function (_super) {
                __extends(SubmitViewModel, _super);
                function SubmitViewModel(context, node) {
                    _super.call(this, context, node);
                }
                SubmitViewModel.prototype.Activate = function () {
                    this.Node.Invoke('NXKit.DOMEvents.EventTarget', 'Dispatch', {
                        type: 'DOMActivate'
                    });
                };
                return SubmitViewModel;
            })(NXKit.Web.XForms.XFormsNodeViewModel);
            XForms.SubmitViewModel = SubmitViewModel;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="XFormsNodeViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            var TriggerViewModel = (function (_super) {
                __extends(TriggerViewModel, _super);
                function TriggerViewModel(context, node) {
                    _super.call(this, context, node);
                }
                TriggerViewModel.prototype.Activate = function () {
                    var self = this;

                    // ensure property changes or non-focus events flush first
                    setTimeout(function () {
                        return self.Node.Invoke('NXKit.DOMEvents.EventTarget', 'Dispatch', {
                            type: 'DOMActivate'
                        });
                    }, 50);
                };
                return TriggerViewModel;
            })(NXKit.Web.XForms.XFormsNodeViewModel);
            XForms.TriggerViewModel = TriggerViewModel;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            (function (SelectUtil) {
                var Selectable = (function () {
                    function Selectable(viewModel, node) {
                        this._viewModel = viewModel;
                        this._node = node;
                    }
                    Object.defineProperty(Selectable.prototype, "ViewModel", {
                        get: function () {
                            return this._viewModel;
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Object.defineProperty(Selectable.prototype, "Node", {
                        get: function () {
                            return this._node;
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Object.defineProperty(Selectable.prototype, "Id", {
                        get: function () {
                            return this.GetId();
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Selectable.prototype.GetId = function () {
                        return this._node.Property('NXKit.XForms.ISelectable', 'Id').ValueAsString();
                    };

                    Object.defineProperty(Selectable.prototype, "Label", {
                        get: function () {
                            return this.GetLabelNode();
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Selectable.prototype.GetLabelNode = function () {
                        return XForms.ViewModelUtil.GetLabelNode(this._node);
                    };

                    Object.defineProperty(Selectable.prototype, "Value", {
                        get: function () {
                            return this.GetValueNode();
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Selectable.prototype.GetValueNode = function () {
                        return XForms.ViewModelUtil.GetValueNode(this._node);
                    };
                    return Selectable;
                })();
                SelectUtil.Selectable = Selectable;

                /**
                * Gets the select item-set. This consists of the item nodes of the given select node.
                */
                function GetSelectables(viewModel, node, level) {
                    try  {
                        return node.Nodes().filter(function (_) {
                            return _.Interfaces['NXKit.XForms.ISelectable'] != null;
                        }).map(function (_) {
                            return new Selectable(viewModel, _);
                        });
                    } catch (ex) {
                        ex.message = 'SelectUtil.GetSelectables()' + '"\nMessage: ' + ex.message;
                        throw ex;
                    }
                }
                SelectUtil.GetSelectables = GetSelectables;
            })(XForms.SelectUtil || (XForms.SelectUtil = {}));
            var SelectUtil = XForms.SelectUtil;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            (function (GroupUtil) {
                var Item = (function () {
                    function Item(viewModel, level) {
                        this._viewModel = viewModel;
                        this._level = level;
                    }
                    Object.defineProperty(Item.prototype, "ViewModel", {
                        get: function () {
                            return this._viewModel;
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

                    Object.defineProperty(Item.prototype, "ReadOnly", {
                        get: function () {
                            return this.GetReadOnly();
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Item.prototype.GetReadOnly = function () {
                        return ko.computed(function () {
                            return false;
                        });
                    };

                    Object.defineProperty(Item.prototype, "Required", {
                        get: function () {
                            return this.GetRequired();
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Item.prototype.GetRequired = function () {
                        return ko.computed(function () {
                            return false;
                        });
                    };

                    Object.defineProperty(Item.prototype, "Valid", {
                        get: function () {
                            return this.GetValid();
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Item.prototype.GetValid = function () {
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

                    Object.defineProperty(Item.prototype, "Help", {
                        get: function () {
                            return this.GetHelp();
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Item.prototype.GetHelp = function () {
                        throw new Error('GetHelp not implemented');
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

                    Item.prototype.SetFocus = function () {
                        console.log('Item: SetFocus');
                    };
                    return Item;
                })();
                GroupUtil.Item = Item;

                /**
                * Describes an item that will render a raw node.
                */
                var NodeItem = (function (_super) {
                    __extends(NodeItem, _super);
                    function NodeItem(viewModel, itemNode, level) {
                        _super.call(this, viewModel, level);
                        this._itemNode = itemNode;
                    }
                    Object.defineProperty(NodeItem.prototype, "ItemNode", {
                        get: function () {
                            return this._itemNode;
                        },
                        enumerable: true,
                        configurable: true
                    });

                    NodeItem.prototype.GetRelevant = function () {
                        var _this = this;
                        return ko.computed(function () {
                            return XForms.ViewModelUtil.IsUINode(_this._itemNode) ? XForms.ViewModelUtil.GetRelevant(_this._itemNode)() : true;
                        });
                    };

                    NodeItem.prototype.GetReadOnly = function () {
                        var _this = this;
                        return ko.computed(function () {
                            return XForms.ViewModelUtil.IsUINode(_this._itemNode) ? XForms.ViewModelUtil.GetReadOnly(_this._itemNode)() : false;
                        });
                    };

                    NodeItem.prototype.GetRequired = function () {
                        var _this = this;
                        return ko.computed(function () {
                            return XForms.ViewModelUtil.IsUINode(_this._itemNode) ? XForms.ViewModelUtil.GetRequired(_this._itemNode)() : false;
                        });
                    };

                    NodeItem.prototype.GetValid = function () {
                        var _this = this;
                        return ko.computed(function () {
                            return XForms.ViewModelUtil.IsUINode(_this._itemNode) ? XForms.ViewModelUtil.GetValid(_this._itemNode)() : true;
                        });
                    };

                    NodeItem.prototype.GetLabel = function () {
                        var self = this;
                        if (self._itemNode.Name == '{http://www.w3.org/2002/xforms}input' && XForms.ViewModelUtil.GetDataType(self._itemNode)() == '{http://www.w3.org/2001/XMLSchema}boolean')
                            // boolean inputs provide their own label
                            return null;
                        else
                            return XForms.ViewModelUtil.GetLabelNode(self._itemNode);
                    };

                    NodeItem.prototype.GetHelp = function () {
                        return XForms.ViewModelUtil.GetHelpNode(this._itemNode);
                    };

                    NodeItem.prototype.GetLayout = function () {
                        return {
                            name: '{http://www.w3.org/2002/xforms}group',
                            data: this,
                            layout: 'node',
                            level: this.Level
                        };
                    };
                    return NodeItem;
                })(Item);
                GroupUtil.NodeItem = NodeItem;

                /**
                * Describes a sub-item of a top-level group which will render a row of items.
                */
                var Row = (function (_super) {
                    __extends(Row, _super);
                    function Row(viewModel, level) {
                        _super.call(this, viewModel, level);
                        this._items = new Array();
                    }
                    Object.defineProperty(Row.prototype, "Items", {
                        get: function () {
                            return this._items;
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Object.defineProperty(Row.prototype, "Done", {
                        get: function () {
                            return this._done;
                        },
                        set: function (value) {
                            this._done = value;
                        },
                        enumerable: true,
                        configurable: true
                    });


                    Row.prototype.GetRelevant = function () {
                        var _this = this;
                        return ko.computed(function () {
                            return _this._items.some(function (_) {
                                return _.Relevant();
                            });
                        });
                    };

                    Row.prototype.GetReadOnly = function () {
                        var _this = this;
                        return ko.computed(function () {
                            return _this._items.every(function (_) {
                                return _.ReadOnly();
                            });
                        });
                    };

                    Row.prototype.GetRequired = function () {
                        var _this = this;
                        return ko.computed(function () {
                            return _this._items.every(function (_) {
                                return _.Required();
                            });
                        });
                    };

                    Row.prototype.GetValid = function () {
                        var _this = this;
                        return ko.computed(function () {
                            return _this._items.every(function (_) {
                                return _.Valid();
                            });
                        });
                    };

                    Row.prototype.GetLabel = function () {
                        return null;
                    };

                    Row.prototype.GetHelp = function () {
                        return null;
                    };

                    Row.prototype.GetLayout = function () {
                        return {
                            name: '{http://www.w3.org/2002/xforms}group',
                            data: this,
                            layout: this.GetLayoutName(),
                            level: this.Level
                        };
                    };

                    Row.prototype.GetLayoutName = function () {
                        switch (this._items.length) {
                            case 1:
                                return "single";
                            case 2:
                                return "double";
                            case 3:
                                return "triple";
                            default:
                                throw new Error("Unhandled row size");
                        }
                    };
                    return Row;
                })(Item);
                GroupUtil.Row = Row;

                var GroupItem = (function (_super) {
                    __extends(GroupItem, _super);
                    function GroupItem(viewModel, groupNode, level) {
                        _super.call(this, viewModel, level);
                        this._groupNode = groupNode;
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
                        return XForms.ViewModelUtil.GetRelevant(this._groupNode);
                    };

                    GroupItem.prototype.GetReadOnly = function () {
                        return XForms.ViewModelUtil.GetReadOnly(this._groupNode);
                    };

                    GroupItem.prototype.GetRequired = function () {
                        return XForms.ViewModelUtil.GetRequired(this._groupNode);
                    };

                    GroupItem.prototype.GetValid = function () {
                        return XForms.ViewModelUtil.GetValid(this._groupNode);
                    };

                    GroupItem.prototype.GetLabel = function () {
                        return XForms.ViewModelUtil.GetLabelNode(this._groupNode);
                    };

                    GroupItem.prototype.GetHelp = function () {
                        return XForms.ViewModelUtil.GetHelpNode(this._groupNode);
                    };

                    GroupItem.prototype.GetLayout = function () {
                        return {
                            name: '{http://www.w3.org/2002/xforms}group',
                            data: this,
                            layout: 'group',
                            level: this.Level
                        };
                    };

                    GroupItem.prototype.SetFocus = function () {
                        console.log('GroupItem: SetFocus');
                    };
                    return GroupItem;
                })(Item);
                GroupUtil.GroupItem = GroupItem;

                function GetGroupItem(viewModel, node, level) {
                    var item = new GroupItem(viewModel, node, level);
                    item.Items = GetItems(viewModel, node, level + 1);
                    return item;
                }
                GroupUtil.GetGroupItem = GetGroupItem;

                /**
                * Gets the group item-set. This consists of the content nodes of the group organized by row.
                */
                function GetItems(viewModel, node, level) {
                    try  {
                        var list = new Array();
                        var cnts = NXKit.Web.ViewModelUtil.GetContents(node);
                        for (var i = 0; i < cnts.length; i++) {
                            var v = cnts[i];

                            // nested group obtains single child
                            if (v.Name == '{http://www.w3.org/2002/xforms}group') {
                                var groupItem = GetGroupItem(viewModel, v, level);
                                list.push(groupItem);
                                continue;
                            } else if (v.Name == '{http://www.w3.org/2002/xforms}textarea') {
                                var textAreaItem = new Row(viewModel, level);
                                textAreaItem.Done = true;
                                list.push(textAreaItem);
                                continue;
                            }

                            // check if last inserted item was a single item, if so, replace with a double item
                            var item = list.pop();
                            if (item instanceof Row && !item.Done) {
                                var item_ = item;
                                item_.Items.push(new NodeItem(viewModel, v, level));
                                list.push(item_);

                                // end row
                                if (item_.Items.length >= 2)
                                    item_.Done = true;
                            } else {
                                // put previous item back into list
                                if (item != null)
                                    list.push(item);

                                // insert new row
                                var item_ = new Row(viewModel, level);
                                item_.Items.push(new NodeItem(viewModel, v, level));
                                list.push(item_);
                            }
                        }

                        return list;
                    } catch (ex) {
                        ex.message = 'GroupUtil.GetItems()' + '\nMessage: ' + ex.message;
                        throw ex;
                    }
                }
                GroupUtil.GetItems = GetItems;
            })(XForms.GroupUtil || (XForms.GroupUtil = {}));
            var GroupUtil = XForms.GroupUtil;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="XFormsNodeViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            var GroupViewModel = (function (_super) {
                __extends(GroupViewModel, _super);
                function GroupViewModel(context, node) {
                    _super.call(this, context, node);
                }
                Object.defineProperty(GroupViewModel.prototype, "LabelAppearance", {
                    get: function () {
                        return this.Label != null ? XForms.ViewModelUtil.GetAppearance(this.Label) : null;
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(GroupViewModel.prototype, "Count", {
                    get: function () {
                        return this.Contents.length;
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(GroupViewModel.prototype, "CountEnabled", {
                    get: function () {
                        return ko.utils.arrayFilter(this.Contents, function (_) {
                            return XForms.ViewModelUtil.GetRelevant(_)();
                        }).length;
                    },
                    enumerable: true,
                    configurable: true
                });
                return GroupViewModel;
            })(XForms.XFormsNodeViewModel);
            XForms.GroupViewModel = GroupViewModel;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="XFormsNodeViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            var HelpViewModel = (function (_super) {
                __extends(HelpViewModel, _super);
                function HelpViewModel(context, node) {
                    _super.call(this, context, node);
                }
                Object.defineProperty(HelpViewModel.prototype, "Text", {
                    get: function () {
                        return this.ValueAsString;
                    },
                    enumerable: true,
                    configurable: true
                });
                return HelpViewModel;
            })(NXKit.Web.XForms.XFormsNodeViewModel);
            XForms.HelpViewModel = HelpViewModel;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="XFormsNodeViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            var AlertViewModel = (function (_super) {
                __extends(AlertViewModel, _super);
                function AlertViewModel(context, node) {
                    _super.call(this, context, node);
                }
                Object.defineProperty(AlertViewModel.prototype, "Text", {
                    get: function () {
                        return this.ValueAsString;
                    },
                    enumerable: true,
                    configurable: true
                });
                return AlertViewModel;
            })(NXKit.Web.XForms.XFormsNodeViewModel);
            XForms.AlertViewModel = AlertViewModel;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="XFormsNodeViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            var HintViewModel = (function (_super) {
                __extends(HintViewModel, _super);
                function HintViewModel(context, node) {
                    _super.call(this, context, node);
                }
                Object.defineProperty(HintViewModel.prototype, "Text", {
                    get: function () {
                        return this.ValueAsString;
                    },
                    enumerable: true,
                    configurable: true
                });
                return HintViewModel;
            })(NXKit.Web.XForms.XFormsNodeViewModel);
            XForms.HintViewModel = HintViewModel;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="XFormsNodeViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            var InputViewModel = (function (_super) {
                __extends(InputViewModel, _super);
                function InputViewModel(context, node) {
                    _super.call(this, context, node);
                }
                Object.defineProperty(InputViewModel.prototype, "ShowLabel", {
                    get: function () {
                        return !NXKit.Web.LayoutOptions.GetArgs(this.Context).SuppressLabel;
                    },
                    enumerable: true,
                    configurable: true
                });

                InputViewModel.prototype.GetHintText = function () {
                    var n = this.Hint;
                    if (n == null)
                        return null;

                    var s = '';
                    var l = n.Nodes();
                    for (var i = 0; i < l.length; i++)
                        if (l[i].Type === Web.NodeType.Text)
                            s += l[i].Value;
                    return s;
                };

                Object.defineProperty(InputViewModel.prototype, "PlaceHolderText", {
                    get: function () {
                        var n = this.Hint;
                        if (n == null)
                            return null;

                        if (XForms.ViewModelUtil.GetAppearance(n)() != 'minimal')
                            return null;

                        return this.GetHintText();
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(InputViewModel.prototype, "ShowAdvice", {
                    get: function () {
                        var _this = this;
                        return ko.computed(function () {
                            return _this.Hint != null && XForms.ViewModelUtil.GetAppearance(_this.Hint)() !== 'minimal';
                        });
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(InputViewModel.prototype, "ShowError", {
                    get: function () {
                        var _this = this;
                        return ko.computed(function () {
                            return _this.Alert != null && !_this.Valid();
                        });
                    },
                    enumerable: true,
                    configurable: true
                });

                InputViewModel.prototype.FocusIn = function () {
                    this.Node.Invoke('NXKit.DOMEvents.EventTarget', 'Dispatch', {
                        type: 'DOMFocusIn'
                    });
                };

                InputViewModel.prototype.FocusOut = function () {
                    this.Node.Invoke('NXKit.DOMEvents.EventTarget', 'Dispatch', {
                        type: 'DOMFocusOut'
                    });
                };
                return InputViewModel;
            })(NXKit.Web.XForms.XFormsNodeViewModel);
            XForms.InputViewModel = InputViewModel;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="XFormsNodeViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            var LabelViewModel = (function (_super) {
                __extends(LabelViewModel, _super);
                function LabelViewModel(context, node) {
                    _super.call(this, context, node);
                }
                Object.defineProperty(LabelViewModel.prototype, "Text", {
                    get: function () {
                        return this.ValueAsString;
                    },
                    enumerable: true,
                    configurable: true
                });
                return LabelViewModel;
            })(NXKit.Web.XForms.XFormsNodeViewModel);
            XForms.LabelViewModel = LabelViewModel;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="XFormsNodeViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            var RangeViewModel = (function (_super) {
                __extends(RangeViewModel, _super);
                function RangeViewModel(context, node) {
                    _super.call(this, context, node);
                }
                Object.defineProperty(RangeViewModel.prototype, "Start", {
                    get: function () {
                        return this.Node.Property('NXKit.XForms.Range', 'Start').ValueAsNumber;
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(RangeViewModel.prototype, "End", {
                    get: function () {
                        return this.Node.Property('NXKit.XForms.Range', 'End').ValueAsNumber;
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(RangeViewModel.prototype, "Step", {
                    get: function () {
                        return this.Node.Property('NXKit.XForms.Range', 'Step').ValueAsNumber;
                    },
                    enumerable: true,
                    configurable: true
                });
                return RangeViewModel;
            })(NXKit.Web.XForms.XFormsNodeViewModel);
            XForms.RangeViewModel = RangeViewModel;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="XFormsNodeViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            var Select1ViewModel = (function (_super) {
                __extends(Select1ViewModel, _super);
                function Select1ViewModel(context, node) {
                    _super.call(this, context, node);
                }
                Object.defineProperty(Select1ViewModel.prototype, "Selectables", {
                    get: function () {
                        return XForms.SelectUtil.GetSelectables(this, this.Node, 1);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(Select1ViewModel.prototype, "SelectedId", {
                    get: function () {
                        var self = this;

                        return ko.computed({
                            read: function () {
                                if (self.Node != null && self.Node.Property('NXKit.XForms.Select1', 'SelectedId') != null) {
                                    var id = self.Node.Property('NXKit.XForms.Select1', 'SelectedId').ValueAsString();
                                    var ls = self.Selectables;
                                    for (var i = 0; i < ls.length; i++)
                                        if (ls[i].Id == id)
                                            return ls[i].Id;
                                } else
                                    return null;
                            },
                            write: function (id) {
                                var ls = self.Selectables;
                                for (var i = 0; i < ls.length; i++)
                                    if (ls[i].Id == id)
                                        self.Node.Property('NXKit.XForms.Select1', 'SelectedId').ValueAsString(id);
                            }
                        });
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(Select1ViewModel.prototype, "Selected", {
                    get: function () {
                        var self = this;

                        return ko.computed({
                            read: function () {
                                var id = self.SelectedId();
                                var ls = self.Selectables;
                                for (var i = 0; i < ls.length; i++)
                                    if (ls[i].Id == id)
                                        return ls[i];

                                return null;
                            },
                            write: function (_) {
                                self.SelectedId(_.Id);
                            }
                        });
                    },
                    enumerable: true,
                    configurable: true
                });

                Select1ViewModel.prototype.FocusIn = function () {
                    this.Node.Invoke('NXKit.DOMEvents.EventTarget', 'Dispatch', {
                        type: 'DOMFocusIn'
                    });
                };

                Select1ViewModel.prototype.FocusOut = function () {
                    this.Node.Invoke('NXKit.DOMEvents.EventTarget', 'Dispatch', {
                        type: 'DOMFocusOut'
                    });
                };
                return Select1ViewModel;
            })(NXKit.Web.XForms.XFormsNodeViewModel);
            XForms.Select1ViewModel = Select1ViewModel;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="XFormsNodeViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            var TextAreaViewModel = (function (_super) {
                __extends(TextAreaViewModel, _super);
                function TextAreaViewModel(context, node) {
                    _super.call(this, context, node);
                }
                return TextAreaViewModel;
            })(NXKit.Web.XForms.XFormsNodeViewModel);
            XForms.TextAreaViewModel = TextAreaViewModel;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
NXKit.Web.ViewModelUtil.ControlNodes.push('{http://www.w3.org/2002/xforms}input', '{http://www.w3.org/2002/xforms}range', '{http://www.w3.org/2002/xforms}select1', '{http://www.w3.org/2002/xforms}select', '{http://www.w3.org/2002/xforms}textarea');

NXKit.Web.ViewModelUtil.MetadataNodes.push('{http://www.w3.org/2002/xforms}label', '{http://www.w3.org/2002/xforms}help', '{http://www.w3.org/2002/xforms}hint', '{http://www.w3.org/2002/xforms}alert');

//NXKit.Web.ViewModelUtil.TransparentNodes.push(
//    '{http://www.w3.org/2002/xforms}repeat');
//NXKit.Web.ViewModelUtil.TransparentNodePredicates.push(
//    // repeat items are transparent
//    (n: NXKit.Web.Node) =>
//        n.Interfaces['NXKit.XForms.RepeatItem'] != null &&
//        n.Property('NXKit.XForms.RepeatItem', 'IsRepeatItem').ValueAsBoolean() == true);
NXKit.Web.ViewModelUtil.LayoutManagers.push(function (c) {
    return new NXKit.Web.XForms.DefaultLayoutManager(c);
});

var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            var Constants = (function () {
                function Constants() {
                }
                Constants.UINode = "NXKit.XForms.IUINode";
                Constants.DataNode = "NXKit.XForms.IDataNode";
                return Constants;
            })();
            XForms.Constants = Constants;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));

var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            (function (ViewModelUtil) {
                function GetValue(node) {
                    return node.Value;
                }
                ViewModelUtil.GetValue = GetValue;

                function IsDataNode(node) {
                    return node.Interfaces[XForms.Constants.DataNode] != null;
                }
                ViewModelUtil.IsDataNode = IsDataNode;

                function GetDataValue(node) {
                    return node.Property(XForms.Constants.DataNode, 'Value').ValueAsString;
                }
                ViewModelUtil.GetDataValue = GetDataValue;

                function GetDataValueAsString(node) {
                    return node.Property(XForms.Constants.DataNode, 'Value').ValueAsString;
                }
                ViewModelUtil.GetDataValueAsString = GetDataValueAsString;

                function GetDataValueAsBoolean(node) {
                    return node.Property(XForms.Constants.DataNode, 'Value').ValueAsBoolean;
                }
                ViewModelUtil.GetDataValueAsBoolean = GetDataValueAsBoolean;

                function GetDataValueAsNumber(node) {
                    return node.Property(XForms.Constants.DataNode, 'Value').ValueAsNumber;
                }
                ViewModelUtil.GetDataValueAsNumber = GetDataValueAsNumber;

                function GetDataValueAsDate(node) {
                    return node.Property(XForms.Constants.DataNode, 'Value').ValueAsDate;
                }
                ViewModelUtil.GetDataValueAsDate = GetDataValueAsDate;

                function GetDataType(node) {
                    var p = node.Property(XForms.Constants.DataNode, 'DataType');
                    return p != null ? p.ValueAsString : null;
                }
                ViewModelUtil.GetDataType = GetDataType;

                function IsUINode(node) {
                    return node.Interfaces[XForms.Constants.UINode] != null;
                }
                ViewModelUtil.IsUINode = IsUINode;

                function GetRelevant(node) {
                    var p = node.Property(XForms.Constants.UINode, 'Relevant');
                    return p != null ? p.ValueAsBoolean : null;
                }
                ViewModelUtil.GetRelevant = GetRelevant;

                function GetReadOnly(node) {
                    var p = node.Property(XForms.Constants.UINode, 'ReadOnly');
                    return p != null ? p.ValueAsBoolean : null;
                }
                ViewModelUtil.GetReadOnly = GetReadOnly;

                function GetRequired(node) {
                    var p = node.Property(XForms.Constants.UINode, 'Required');
                    return p != null ? p.ValueAsBoolean : null;
                }
                ViewModelUtil.GetRequired = GetRequired;

                function GetValid(node) {
                    var p = node.Property(XForms.Constants.UINode, 'Valid');
                    return p != null ? p.ValueAsBoolean : null;
                }
                ViewModelUtil.GetValid = GetValid;

                function GetAppearance(node) {
                    var p = node.Property(XForms.Constants.UINode, 'Appearance');
                    return p != null ? p.ValueAsString : null;
                }
                ViewModelUtil.GetAppearance = GetAppearance;

                function GetDataItem(node) {
                    return ko.computed(function () {
                        return [
                            GetValid(node)() ? 'valid' : 'invalid',
                            GetRelevant(node)() ? 'enabled' : 'disabled',
                            GetReadOnly(node)() ? 'readonly' : 'readwrite',
                            GetRequired(node)() ? 'required' : 'optional'
                        ].join(' ');
                    });
                }
                ViewModelUtil.GetDataItem = GetDataItem;

                function GetLabelNode(node) {
                    return ko.utils.arrayFirst(node.Nodes(), function (_) {
                        return _.Name == '{http://www.w3.org/2002/xforms}label';
                    });
                }
                ViewModelUtil.GetLabelNode = GetLabelNode;

                function GetHelpNode(node) {
                    return ko.utils.arrayFirst(node.Nodes(), function (_) {
                        return _.Name == '{http://www.w3.org/2002/xforms}help';
                    });
                }
                ViewModelUtil.GetHelpNode = GetHelpNode;

                function GetHintNode(node) {
                    return ko.utils.arrayFirst(node.Nodes(), function (_) {
                        return _.Name == '{http://www.w3.org/2002/xforms}hint';
                    });
                }
                ViewModelUtil.GetHintNode = GetHintNode;

                function GetAlertNode(node) {
                    return ko.utils.arrayFirst(node.Nodes(), function (_) {
                        return _.Name == '{http://www.w3.org/2002/xforms}alert';
                    });
                }
                ViewModelUtil.GetAlertNode = GetAlertNode;

                function GetValueNode(node) {
                    return ko.utils.arrayFirst(node.Nodes(), function (_) {
                        return _.Name == '{http://www.w3.org/2002/xforms}value';
                    });
                }
                ViewModelUtil.GetValueNode = GetValueNode;
            })(XForms.ViewModelUtil || (XForms.ViewModelUtil = {}));
            var ViewModelUtil = XForms.ViewModelUtil;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));


    return NXKit;
};

if (typeof define === "function" && define.amd) {
    define("nxkit-xforms", ['jquery', 'knockout', 'nxkit'], function ($, ko, NXKit) {
        return init($, ko, NXKit);
    });
} else {
    var hold = false;
    var loop = function () {
        if (typeof $ === "function" && 
            typeof ko === "object" && 
            typeof NXKit === "object" && 
            typeof NXKit.Web === "object") {
            init($, ko, NXKit);
            if (hold) {
                $.holdReady(hold = false);
            }
        } else {
            if (typeof $ === "function") {
                $.holdReady(hold = true);
            }

            if (typeof console.warn === "function") {
                console.warn("nxkit-xforms: RequireJS missing or jQuery, knockout or NXKit missing, retrying.");
            }

            window.setTimeout(loop, 100);
        }
    };
    loop();
}
})();