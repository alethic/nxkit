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
                        return NXKit.Web.XForms.ViewModelUtil.GetValue(this.Node);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsNodeViewModel.prototype, "ValueAsString", {
                    get: function () {
                        return NXKit.Web.XForms.ViewModelUtil.GetValueAsString(this.Node);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsNodeViewModel.prototype, "ValueAsBoolean", {
                    get: function () {
                        return NXKit.Web.XForms.ViewModelUtil.GetValueAsBoolean(this.Node);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsNodeViewModel.prototype, "ValueAsNumber", {
                    get: function () {
                        return NXKit.Web.XForms.ViewModelUtil.GetValueAsNumber(this.Node);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsNodeViewModel.prototype, "Relevant", {
                    get: function () {
                        return NXKit.Web.XForms.ViewModelUtil.GetRelevant(this.Node);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsNodeViewModel.prototype, "ReadOnly", {
                    get: function () {
                        return NXKit.Web.XForms.ViewModelUtil.GetReadOnly(this.Node);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsNodeViewModel.prototype, "Required", {
                    get: function () {
                        return NXKit.Web.XForms.ViewModelUtil.GetRequired(this.Node);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsNodeViewModel.prototype, "Valid", {
                    get: function () {
                        return NXKit.Web.XForms.ViewModelUtil.GetValid(this.Node);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsNodeViewModel.prototype, "Type", {
                    get: function () {
                        return NXKit.Web.XForms.ViewModelUtil.GetType(this.Node);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsNodeViewModel.prototype, "Appearance", {
                    get: function () {
                        return NXKit.Web.XForms.ViewModelUtil.GetAppearance(this.Node);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsNodeViewModel.prototype, "Label", {
                    get: function () {
                        try  {
                            return NXKit.Web.XForms.ViewModelUtil.GetLabelNode(this.Node);
                        } catch (ex) {
                            ex.message = 'XFormsNodeViewModel.Label' + '"\nMessage: ' + ex.message;
                            throw ex;
                        }
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsNodeViewModel.prototype, "Help", {
                    get: function () {
                        return NXKit.Web.XForms.ViewModelUtil.GetHelpNode(this.Node);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsNodeViewModel.prototype, "Hint", {
                    get: function () {
                        return NXKit.Web.XForms.ViewModelUtil.GetHintNode(this.Node);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsNodeViewModel.prototype, "Alert", {
                    get: function () {
                        return NXKit.Web.XForms.ViewModelUtil.GetAlertNode(this.Node);
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
                    this.Node.Invoke('NXKit.DOMEvents.INXEventTarget', 'DispatchEvent', {
                        type: 'DOMActivate'
                    });
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
                var Item = (function () {
                    function Item(viewModel, node) {
                        this._viewModel = viewModel;
                        this._node = node;
                    }
                    Object.defineProperty(Item.prototype, "ViewModel", {
                        get: function () {
                            return this._viewModel;
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Object.defineProperty(Item.prototype, "Node", {
                        get: function () {
                            return this._node;
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Object.defineProperty(Item.prototype, "Id", {
                        get: function () {
                            return this.GetId();
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Item.prototype.GetId = function () {
                        return this._node.ValueAsString('NXKit.NXElement', 'UniqueId')();
                    };

                    Object.defineProperty(Item.prototype, "Label", {
                        get: function () {
                            return this.GetLabelNode();
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Item.prototype.GetLabelNode = function () {
                        return NXKit.Web.XForms.ViewModelUtil.GetLabelNode(this._node);
                    };

                    Object.defineProperty(Item.prototype, "Value", {
                        get: function () {
                            return this.GetValueNode();
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Item.prototype.GetValueNode = function () {
                        return NXKit.Web.XForms.ViewModelUtil.GetValueNode(this._node);
                    };
                    return Item;
                })();
                SelectUtil.Item = Item;

                /**
                * Gets the select item-set. This consists of the item nodes of the given select node.
                */
                function GetItems(viewModel, node, level) {
                    try  {
                        return node.Nodes().filter(function (_) {
                            return _.Type == 'NXKit.XForms.Item';
                        }).map(function (_) {
                            return new NXKit.Web.XForms.SelectUtil.Item(viewModel, _);
                        });
                    } catch (ex) {
                        ex.message = 'SelectUtil.GetItems()' + '"\nMessage: ' + ex.message;
                        throw ex;
                    }
                }
                SelectUtil.GetItems = GetItems;
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
                            return NXKit.Web.XForms.ViewModelUtil.IsModelItemBinding(_this._itemNode) ? NXKit.Web.XForms.ViewModelUtil.GetRelevant(_this._itemNode)() : true;
                        });
                    };

                    NodeItem.prototype.GetReadOnly = function () {
                        var _this = this;
                        return ko.computed(function () {
                            return NXKit.Web.XForms.ViewModelUtil.IsModelItemBinding(_this._itemNode) ? NXKit.Web.XForms.ViewModelUtil.GetReadOnly(_this._itemNode)() : false;
                        });
                    };

                    NodeItem.prototype.GetRequired = function () {
                        var _this = this;
                        return ko.computed(function () {
                            return NXKit.Web.XForms.ViewModelUtil.IsModelItemBinding(_this._itemNode) ? NXKit.Web.XForms.ViewModelUtil.GetRequired(_this._itemNode)() : false;
                        });
                    };

                    NodeItem.prototype.GetValid = function () {
                        var _this = this;
                        return ko.computed(function () {
                            return NXKit.Web.XForms.ViewModelUtil.IsModelItemBinding(_this._itemNode) ? NXKit.Web.XForms.ViewModelUtil.GetValid(_this._itemNode)() : true;
                        });
                    };

                    NodeItem.prototype.GetLabel = function () {
                        var self = this;
                        if (self._itemNode.Type == 'NXKit.XForms.Input' && NXKit.Web.XForms.ViewModelUtil.GetType(self._itemNode)() == '{http://www.w3.org/2001/XMLSchema}boolean')
                            // boolean inputs provide their own label
                            return null;
                        else
                            return NXKit.Web.XForms.ViewModelUtil.GetLabelNode(self._itemNode);
                    };

                    NodeItem.prototype.GetHelp = function () {
                        return NXKit.Web.XForms.ViewModelUtil.GetHelpNode(this._itemNode);
                    };

                    NodeItem.prototype.GetLayout = function () {
                        return {
                            template: 'NXKit.XForms.Group',
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
                            return _this._items.every(function (_) {
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
                            template: 'NXKit.XForms.Group',
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
                        return NXKit.Web.XForms.ViewModelUtil.GetRelevant(this._groupNode);
                    };

                    GroupItem.prototype.GetReadOnly = function () {
                        return NXKit.Web.XForms.ViewModelUtil.GetReadOnly(this._groupNode);
                    };

                    GroupItem.prototype.GetRequired = function () {
                        return NXKit.Web.XForms.ViewModelUtil.GetRequired(this._groupNode);
                    };

                    GroupItem.prototype.GetValid = function () {
                        return NXKit.Web.XForms.ViewModelUtil.GetValid(this._groupNode);
                    };

                    GroupItem.prototype.GetLabel = function () {
                        return NXKit.Web.XForms.ViewModelUtil.GetLabelNode(this._groupNode);
                    };

                    GroupItem.prototype.GetHelp = function () {
                        return NXKit.Web.XForms.ViewModelUtil.GetHelpNode(this._groupNode);
                    };

                    GroupItem.prototype.GetLayout = function () {
                        return {
                            template: 'NXKit.XForms.Group',
                            data: this,
                            layout: 'group',
                            level: this.Level
                        };
                    };
                    return GroupItem;
                })(Item);
                GroupUtil.GroupItem = GroupItem;

                function GetGroupItem(viewModel, node, level) {
                    var item = new NXKit.Web.XForms.GroupUtil.GroupItem(viewModel, node, level);
                    item.Items = NXKit.Web.XForms.GroupUtil.GetItems(viewModel, node, level + 1);
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
                            if (v.Type == 'NXKit.XForms.Group') {
                                var groupItem = NXKit.Web.XForms.GroupUtil.GetGroupItem(viewModel, v, level);
                                list.push(groupItem);
                                continue;
                            } else if (v.Type == 'NXKit.XForms.TextArea') {
                                var textAreaItem = new NXKit.Web.XForms.GroupUtil.Row(viewModel, level);
                                textAreaItem.Done = true;
                                list.push(textAreaItem);
                                continue;
                            }

                            // check if last inserted item was a single item, if so, replace with a double item
                            var item = list.pop();
                            if (item instanceof NXKit.Web.XForms.GroupUtil.Row && !item.Done) {
                                var item_ = item;
                                item_.Items.push(new NXKit.Web.XForms.GroupUtil.NodeItem(viewModel, v, level));
                                list.push(item_);

                                // end row
                                if (item_.Items.length >= 2)
                                    item_.Done = true;
                            } else {
                                // put previous item back into list
                                if (item != null)
                                    list.push(item);

                                // insert new row
                                var item_ = new NXKit.Web.XForms.GroupUtil.Row(viewModel, level);
                                item_.Items.push(new NXKit.Web.XForms.GroupUtil.NodeItem(viewModel, v, level));
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
                function GroupViewModel(context, node, count) {
                    _super.call(this, context, node);

                    this._count = count;
                }
                Object.defineProperty(GroupViewModel.prototype, "Items", {
                    get: function () {
                        return this.GetItems();
                    },
                    enumerable: true,
                    configurable: true
                });

                GroupViewModel.prototype.GetItems = function () {
                    try  {
                        return NXKit.Web.XForms.GroupUtil.GetItems(this, this.Node, 1);
                    } catch (ex) {
                        ex.message = 'GroupViewModel.GetItems()' + '"\nMessage: ' + ex.message;
                        throw ex;
                    }
                };
                return GroupViewModel;
            })(NXKit.Web.XForms.XFormsNodeViewModel);
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
                        if (l[i].Type === 'NXKit.NXText')
                            s += l[i].Property('NXKit.NXText', 'Value').ValueAsString();
                    return s;
                };

                Object.defineProperty(InputViewModel.prototype, "PlaceHolderText", {
                    get: function () {
                        var n = this.Hint;
                        if (n == null)
                            return null;

                        if (NXKit.Web.XForms.ViewModelUtil.GetAppearance(n)() != 'minimal')
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
                            return _this.Hint != null && NXKit.Web.XForms.ViewModelUtil.GetAppearance(_this.Hint)() !== 'minimal';
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
                    this.Node.Invoke('NXKit.DOMEvents.INXEventTarget', 'DispatchEvent', {
                        type: 'DOMFocusIn'
                    });
                };

                InputViewModel.prototype.FocusOut = function () {
                    this.Node.Invoke('NXKit.DOMEvents.INXEventTarget', 'DispatchEvent', {
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
                        return this.Node.ValueAsNumber('NXKit.XForms.Range', 'Start');
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(RangeViewModel.prototype, "End", {
                    get: function () {
                        return this.Node.ValueAsNumber('NXKit.XForms.Range', 'End');
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(RangeViewModel.prototype, "Step", {
                    get: function () {
                        return this.Node.ValueAsNumber('NXKit.XForms.Range', 'Step');
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
                Select1ViewModel.GetSelectedItemNodeId = function (node) {
                    return ko.computed({
                        read: function () {
                            if (node != null && node.ValueAsString('NXKit.XForms.Select1', 'SelectedItemNodeId') != null)
                                return node.ValueAsString('NXKit.XForms.Select1', 'SelectedItemNodeId')();
                            else
                                return null;
                        },
                        write: function (_) {
                            if (node != null && node.ValueAsString('NXKit.XForms.Select1', 'SelectedItemNodeId') != null)
                                node.ValueAsString('NXKit.XForms.Select1', 'SelectedItemNodeId')(_);
                        }
                    });
                };

                Object.defineProperty(Select1ViewModel.prototype, "Items", {
                    get: function () {
                        return NXKit.Web.XForms.SelectUtil.GetItems(this, this.Node, 1);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(Select1ViewModel.prototype, "SelectedItemNodeId", {
                    get: function () {
                        return NXKit.Web.XForms.Select1ViewModel.GetSelectedItemNodeId(this.Node);
                    },
                    enumerable: true,
                    configurable: true
                });
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
NXKit.Web.ViewModelUtil.ControlNodeTypes.push('NXKit.XForms.Input', 'NXKit.XForms.Range', 'NXKit.XForms.Select1', 'NXKit.XForms.Select', 'NXKit.XForms.TextArea');

NXKit.Web.ViewModelUtil.MetadataNodeTypes.push('NXKit.XForms.Label', 'NXKit.XForms.Help', 'NXKit.XForms.Hint', 'NXKit.XForms.Alert');

NXKit.Web.ViewModelUtil.TransparentNodeTypes.push('NXKit.XForms.Repeat', 'NXKit.XForms.RepeatItem');

var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            (function (ViewModelUtil) {
                function GetValue(node) {
                    return node.Value('NXKit.XForms.IModelItemValue', 'Value');
                }
                ViewModelUtil.GetValue = GetValue;

                function GetValueAsString(node) {
                    return node.ValueAsString('NXKit.XForms.IModelItemValue', 'Value');
                }
                ViewModelUtil.GetValueAsString = GetValueAsString;

                function GetValueAsBoolean(node) {
                    return node.ValueAsBoolean('NXKit.XForms.IModelItemValue', 'Value');
                }
                ViewModelUtil.GetValueAsBoolean = GetValueAsBoolean;

                function GetValueAsNumber(node) {
                    return node.ValueAsNumber('NXKit.XForms.IModelItemValue', 'Value');
                }
                ViewModelUtil.GetValueAsNumber = GetValueAsNumber;

                function GetValueAsDate(node) {
                    return node.ValueAsDate('NXKit.XForms.IModelItemValue', 'Value');
                }
                ViewModelUtil.GetValueAsDate = GetValueAsDate;

                function GetRelevant(node) {
                    return node.ValueAsBoolean('NXKit.XForms.IModelItemBinding', 'Relevant');
                }
                ViewModelUtil.GetRelevant = GetRelevant;

                function GetReadOnly(node) {
                    return node.ValueAsBoolean('NXKit.XForms.IModelItemBinding', 'ReadOnly');
                }
                ViewModelUtil.GetReadOnly = GetReadOnly;

                function GetRequired(node) {
                    return node.ValueAsBoolean('NXKit.XForms.IModelItemBinding', 'Required');
                }
                ViewModelUtil.GetRequired = GetRequired;

                function GetValid(node) {
                    return node.ValueAsBoolean('NXKit.XForms.IModelItemBinding', 'Valid');
                }
                ViewModelUtil.GetValid = GetValid;

                function GetType(node) {
                    return node.ValueAsString('NXKit.XForms.IModelItemBinding', 'DataType');
                }
                ViewModelUtil.GetType = GetType;

                function IsModelItemValue(node) {
                    return node.Interfaces['NXKit.XForms.IModelItemValue'] != null;
                }
                ViewModelUtil.IsModelItemValue = IsModelItemValue;

                function IsModelItemBinding(node) {
                    return node.Interfaces['NXKit.XForms.IModelItemBinding'] != null;
                }
                ViewModelUtil.IsModelItemBinding = IsModelItemBinding;

                function GetAppearance(node) {
                    return ko.computed(function () {
                        var p = node.Property('NXKit.XForms.IUIAppearance', "Appearance");
                        return p != null ? p.ValueAsString() : null;
                    });
                }
                ViewModelUtil.GetAppearance = GetAppearance;

                function GetLabelNode(node) {
                    return ko.utils.arrayFirst(node.Nodes(), function (_) {
                        return _.Type == 'NXKit.XForms.Label';
                    });
                }
                ViewModelUtil.GetLabelNode = GetLabelNode;

                function GetHelpNode(node) {
                    return ko.utils.arrayFirst(node.Nodes(), function (_) {
                        return _.Type == 'NXKit.XForms.Help';
                    });
                }
                ViewModelUtil.GetHelpNode = GetHelpNode;

                function GetHintNode(node) {
                    return ko.utils.arrayFirst(node.Nodes(), function (_) {
                        return _.Type == 'NXKit.XForms.Hint';
                    });
                }
                ViewModelUtil.GetHintNode = GetHintNode;

                function GetAlertNode(node) {
                    return ko.utils.arrayFirst(node.Nodes(), function (_) {
                        return _.Type == 'NXKit.XForms.Alert';
                    });
                }
                ViewModelUtil.GetAlertNode = GetAlertNode;

                function GetValueNode(node) {
                    return ko.utils.arrayFirst(node.Nodes(), function (_) {
                        return _.Type == 'NXKit.XForms.Value';
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
//# sourceMappingURL=NXKit.XForms.js.map
