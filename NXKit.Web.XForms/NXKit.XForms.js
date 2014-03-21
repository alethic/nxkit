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
            var XFormsVisualViewModel = (function (_super) {
                __extends(XFormsVisualViewModel, _super);
                function XFormsVisualViewModel(context, visual) {
                    _super.call(this, context, visual);
                }
                Object.defineProperty(XFormsVisualViewModel.prototype, "Value", {
                    get: function () {
                        return NXKit.Web.XForms.Utils.GetValue(this.Visual);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsVisualViewModel.prototype, "ValueAsString", {
                    get: function () {
                        return NXKit.Web.XForms.Utils.GetValueAsString(this.Visual);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsVisualViewModel.prototype, "ValueAsBoolean", {
                    get: function () {
                        return NXKit.Web.XForms.Utils.GetValueAsBoolean(this.Visual);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsVisualViewModel.prototype, "ValueAsNumber", {
                    get: function () {
                        return NXKit.Web.XForms.Utils.GetValueAsNumber(this.Visual);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsVisualViewModel.prototype, "Relevant", {
                    get: function () {
                        return NXKit.Web.XForms.Utils.GetRelevant(this.Visual);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsVisualViewModel.prototype, "Type", {
                    get: function () {
                        return NXKit.Web.XForms.Utils.GetType(this.Visual);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsVisualViewModel.prototype, "Appearance", {
                    get: function () {
                        return NXKit.Web.XForms.Utils.GetAppearance(this.Visual);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsVisualViewModel.prototype, "Label", {
                    get: function () {
                        return NXKit.Web.XForms.Utils.GetLabel(this.Visual);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsVisualViewModel.prototype, "Help", {
                    get: function () {
                        return NXKit.Web.XForms.Utils.GetHelp(this.Visual);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsVisualViewModel.prototype, "Hint", {
                    get: function () {
                        return NXKit.Web.XForms.Utils.GetHint(this.Visual);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(XFormsVisualViewModel.prototype, "Contents", {
                    get: function () {
                        return NXKit.Web.XForms.Utils.GetContents(this.Visual);
                    },
                    enumerable: true,
                    configurable: true
                });
                XFormsVisualViewModel.ControlVisualTypes = [
                    'NXKit.XForms.XFormsInputVisual',
                    'NXKit.XForms.XFormsRangeVisual',
                    'NXKit.XForms.XFormsSelect1Visual',
                    'NXKit.XForms.XFormsSelectVisual'
                ];

                XFormsVisualViewModel.MetadataVisualTypes = [
                    'NXKit.XForms.XFormsLabelVisual',
                    'NXKit.XForms.XFormsHelpVisual',
                    'NXKit.XForms.XFormsHintVisual',
                    'NXKit.XForms.XFormsAlertVisual'
                ];

                XFormsVisualViewModel.TransparentVisualTypes = [
                    'NXKit.XForms.XFormsRepeatVisual',
                    'NXKit.XForms.XFormsRepeatItemVisual'
                ];
                return XFormsVisualViewModel;
            })(NXKit.Web.VisualViewModel);
            XForms.XFormsVisualViewModel = XFormsVisualViewModel;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="XFormsVisualViewModel.ts" />
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
/// <reference path="XFormsVisualViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            (function (GroupViewModel_) {
                /**
                * Represents a sub-item of a top-level group.
                */
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
                GroupViewModel_.Item = Item;

                /**
                * Describes an item that will render a raw visual.
                */
                var VisualItem = (function (_super) {
                    __extends(VisualItem, _super);
                    function VisualItem(viewModel, itemVisual, level) {
                        _super.call(this, viewModel, level);
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
                        return NXKit.Web.XForms.Utils.GetRelevant(this._itemVisual);
                    };

                    VisualItem.prototype.GetLabel = function () {
                        if (this._itemVisual.Type == 'NXKit.XForms.XFormsInputVisual' && NXKit.Web.XForms.Utils.GetType(this._itemVisual)() == '{http://www.w3.org/2001/XMLSchema}boolean')
                            // boolean inputs provide their own label
                            return null;
                        else
                            return NXKit.Web.XForms.Utils.GetLabel(this._itemVisual);
                    };

                    VisualItem.prototype.GetHelp = function () {
                        return NXKit.Web.XForms.Utils.GetHelp(this._itemVisual);
                    };

                    VisualItem.prototype.GetLayout = function () {
                        return {
                            visual: this.ViewModel.Visual,
                            data: this,
                            layout: 'visual',
                            level: this.Level
                        };
                    };
                    return VisualItem;
                })(Item);
                GroupViewModel_.VisualItem = VisualItem;

                var InputItem = (function (_super) {
                    __extends(InputItem, _super);
                    function InputItem(viewModel, inputVisual, level) {
                        _super.call(this, viewModel, inputVisual, level);
                    }
                    Object.defineProperty(InputItem.prototype, "InputVisual", {
                        get: function () {
                            return this.ItemVisual;
                        },
                        enumerable: true,
                        configurable: true
                    });

                    InputItem.prototype.GetLayout = function () {
                        return {
                            visual: this.ViewModel.Visual,
                            data: this,
                            layout: 'input',
                            type: NXKit.Web.XForms.Utils.GetType(this.InputVisual),
                            level: this.Level
                        };
                    };
                    return InputItem;
                })(VisualItem);
                GroupViewModel_.InputItem = InputItem;

                /**
                * Describes a sub-item of a top-level group that will render a single underlying item.
                */
                var SingleItem = (function (_super) {
                    __extends(SingleItem, _super);
                    function SingleItem(viewModel, level) {
                        _super.call(this, viewModel, level);
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


                    Object.defineProperty(SingleItem.prototype, "Force", {
                        get: function () {
                            return this._force;
                        },
                        set: function (force) {
                            this._force = force;
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

                    SingleItem.prototype.GetHelp = function () {
                        return this._item.Help;
                    };

                    SingleItem.prototype.GetLayout = function () {
                        return {
                            visual: this.ViewModel.Visual,
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
                    function DoubleItem(viewModel, level) {
                        _super.call(this, viewModel, level);
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

                    DoubleItem.prototype.GetHelp = function () {
                        return null;
                    };

                    DoubleItem.prototype.GetLayout = function () {
                        return {
                            visual: this.ViewModel.Visual,
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
                    function GroupItem(viewModel, groupVisual, level) {
                        _super.call(this, viewModel, level);
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
                        return NXKit.Web.XForms.Utils.GetRelevant(this._groupVisual);
                    };

                    GroupItem.prototype.GetLabel = function () {
                        return NXKit.Web.XForms.Utils.GetLabel(this._groupVisual);
                    };

                    GroupItem.prototype.GetHelp = function () {
                        return NXKit.Web.XForms.Utils.GetHelp(this._groupVisual);
                    };

                    GroupItem.prototype.GetLayout = function () {
                        return {
                            visual: this.ViewModel.Visual,
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
                    var item = new GroupViewModel_.GroupItem(this, visual, level);
                    item.Items = this.GetItems(visual, level + 1);
                    return item;
                };

                GroupViewModel.prototype.GetItems = function (visual, level) {
                    var list = new Array();
                    var cnts = NXKit.Web.XForms.Utils.GetContents(visual);
                    for (var i = 0; i < cnts.length; i++) {
                        var v = cnts[i];

                        // nested group obtains single child
                        if (v.Type == 'NXKit.XForms.XFormsGroupVisual') {
                            var groupItem = this.GetGroupItem(v, level);
                            list.push(groupItem);
                            continue;
                        } else if (v.Type == 'NXKit.XForms.XFormsTextAreaVisual') {
                            var textAreaItem = new GroupViewModel_.SingleItem(this, level);
                            textAreaItem.Force = true;
                            list.push(textAreaItem);
                            continue;
                        }

                        // check if last inserted item was a single item, if so, replace with a double item
                        var item = list.pop();
                        if (item instanceof GroupViewModel_.SingleItem && !item.Force) {
                            var item1 = item;
                            var item2 = new GroupViewModel_.DoubleItem(this, level);
                            item2.Item1 = item1.Item;
                            item2.Item2 = new GroupViewModel_.VisualItem(this, v, level);
                            list.push(item2);
                        } else {
                            // put previous item back into list
                            if (item != null)
                                list.push(item);

                            // insert new single item
                            var item1 = new GroupViewModel_.SingleItem(this, level);
                            item1.Item = new GroupViewModel_.VisualItem(this, v, level);
                            list.push(item1);
                        }
                    }

                    return list;
                };
                return GroupViewModel;
            })(NXKit.Web.XForms.XFormsVisualViewModel);
            XForms.GroupViewModel = GroupViewModel;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="XFormsVisualViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            var HelpViewModel = (function (_super) {
                __extends(HelpViewModel, _super);
                function HelpViewModel(context, visual) {
                    _super.call(this, context, visual);
                }
                Object.defineProperty(HelpViewModel.prototype, "Text", {
                    get: function () {
                        return this.ValueAsString;
                    },
                    enumerable: true,
                    configurable: true
                });
                return HelpViewModel;
            })(NXKit.Web.XForms.XFormsVisualViewModel);
            XForms.HelpViewModel = HelpViewModel;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="XFormsVisualViewModel.ts" />
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
            })(NXKit.Web.XForms.XFormsVisualViewModel);
            XForms.HintViewModel = HintViewModel;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="XFormsVisualViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            var InputViewModel = (function (_super) {
                __extends(InputViewModel, _super);
                function InputViewModel(context, visual) {
                    _super.call(this, context, visual);
                }
                Object.defineProperty(InputViewModel.prototype, "ShowLabel", {
                    get: function () {
                        return !NXKit.Web.LayoutOptions.GetArgs(this.Context).SuppressLabel;
                    },
                    enumerable: true,
                    configurable: true
                });
                return InputViewModel;
            })(NXKit.Web.XForms.XFormsVisualViewModel);
            XForms.InputViewModel = InputViewModel;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="XFormsVisualViewModel.ts" />
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
            })(NXKit.Web.XForms.XFormsVisualViewModel);
            XForms.LabelViewModel = LabelViewModel;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="XFormsVisualViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            var RangeViewModel = (function (_super) {
                __extends(RangeViewModel, _super);
                function RangeViewModel(context, visual) {
                    _super.call(this, context, visual);
                }
                return RangeViewModel;
            })(NXKit.Web.XForms.XFormsVisualViewModel);
            XForms.RangeViewModel = RangeViewModel;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="XFormsVisualViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            var Select1ViewModel = (function (_super) {
                __extends(Select1ViewModel, _super);
                function Select1ViewModel(context, visual) {
                    _super.call(this, context, visual);
                }
                Select1ViewModel.GetSelectedItemVisualId = function (visual) {
                    return ko.computed({
                        read: function () {
                            if (visual != null && visual.Properties['SelectedItemVisualId'] != null)
                                return visual.Properties['SelectedItemVisualId'].ValueAsString();
                            else
                                return null;
                        },
                        write: function (_) {
                            if (visual != null && visual.Properties['SelectedItemVisualId'] != null)
                                visual.Properties['SelectedItemVisualId'].Value(_);
                        }
                    });
                };

                Object.defineProperty(Select1ViewModel.prototype, "SelectedItemVisualId", {
                    get: function () {
                        return Select1ViewModel.GetSelectedItemVisualId(this.Visual);
                    },
                    enumerable: true,
                    configurable: true
                });
                return Select1ViewModel;
            })(NXKit.Web.XForms.XFormsVisualViewModel);
            XForms.Select1ViewModel = Select1ViewModel;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="XFormsVisualViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            var TextAreaViewModel = (function (_super) {
                __extends(TextAreaViewModel, _super);
                function TextAreaViewModel(context, visual) {
                    _super.call(this, context, visual);
                }
                return TextAreaViewModel;
            })(NXKit.Web.XForms.XFormsVisualViewModel);
            XForms.TextAreaViewModel = TextAreaViewModel;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            (function (Utils) {
                function GetValue(visual) {
                    return ko.computed({
                        read: function () {
                            if (visual != null && visual.Properties['Value'] != null)
                                return visual.Properties['Value'].Value();
                            else
                                return null;
                        },
                        write: function (_) {
                            if (visual != null && visual.Properties['Value'] != null)
                                visual.Properties['Value'].Value(_);
                        }
                    });
                }
                Utils.GetValue = GetValue;

                function GetValueAsString(visual) {
                    return ko.computed({
                        read: function () {
                            if (visual != null && visual.Properties['Value'] != null)
                                return visual.Properties['Value'].ValueAsString();
                            else
                                return null;
                        },
                        write: function (_) {
                            if (visual != null && visual.Properties['Value'] != null)
                                visual.Properties['Value'].ValueAsString(_);
                        }
                    });
                }
                Utils.GetValueAsString = GetValueAsString;

                function GetValueAsBoolean(visual) {
                    return ko.computed({
                        read: function () {
                            if (visual != null && visual.Properties['Value'] != null)
                                return visual.Properties['Value'].ValueAsBoolean();
                            else
                                return null;
                        },
                        write: function (_) {
                            if (visual != null && visual.Properties['Value'] != null)
                                visual.Properties['Value'].ValueAsBoolean(_);
                        }
                    });
                }
                Utils.GetValueAsBoolean = GetValueAsBoolean;

                function GetValueAsNumber(visual) {
                    return ko.computed({
                        read: function () {
                            if (visual != null && visual.Properties['Value'] != null)
                                return visual.Properties['Value'].ValueAsNumber();
                            else
                                return null;
                        },
                        write: function (_) {
                            if (visual != null && visual.Properties['Value'] != null)
                                visual.Properties['Value'].ValueAsNumber(_);
                        }
                    });
                }
                Utils.GetValueAsNumber = GetValueAsNumber;

                function GetRelevant(visual) {
                    return ko.computed(function () {
                        if (visual != null && visual.Properties['Relevant'] != null)
                            return visual.Properties['Relevant'].ValueAsBoolean();
                        else
                            return null;
                    });
                }
                Utils.GetRelevant = GetRelevant;

                function GetType(visual) {
                    return ko.computed(function () {
                        if (visual != null && visual.Properties['Type'] != null)
                            return visual.Properties['Type'].ValueAsString();
                        else
                            return null;
                    });
                }
                Utils.GetType = GetType;

                function GetAppearance(visual) {
                    return ko.computed(function () {
                        if (visual != null && visual.Properties['Appearance'] != null)
                            return visual.Properties['Appearance'].ValueAsString();
                        else
                            return null;
                    });
                }
                Utils.GetAppearance = GetAppearance;

                function IsMetadataVisual(visual) {
                    return NXKit.Web.XForms.XFormsVisualViewModel.MetadataVisualTypes.some(function (_) {
                        return visual.Type == _;
                    });
                }
                Utils.IsMetadataVisual = IsMetadataVisual;

                function GetLabel(visual) {
                    return ko.utils.arrayFirst(visual.Visuals(), function (_) {
                        return _.Type == 'NXKit.XForms.XFormsLabelVisual';
                    });
                }
                Utils.GetLabel = GetLabel;

                function GetHelp(visual) {
                    return ko.utils.arrayFirst(visual.Visuals(), function (_) {
                        return _.Type == 'NXKit.XForms.XFormsHelpVisual';
                    });
                }
                Utils.GetHelp = GetHelp;

                function GetHint(visual) {
                    return ko.utils.arrayFirst(visual.Visuals(), function (_) {
                        return _.Type == 'NXKit.XForms.XFormsHintVisual';
                    });
                }
                Utils.GetHint = GetHint;

                function GetAlert(visual) {
                    return ko.utils.arrayFirst(visual.Visuals(), function (_) {
                        return _.Type == 'NXKit.XForms.XFormsAlertVisual';
                    });
                }
                Utils.GetAlert = GetAlert;

                function IsControlVisual(visual) {
                    return this.ControlVisualTypes.some(function (_) {
                        return visual.Type == _;
                    });
                }
                Utils.IsControlVisual = IsControlVisual;

                function HasControlVisual(visual) {
                    return visual.Visuals().some(function (_) {
                        return IsControlVisual(_);
                    });
                }
                Utils.HasControlVisual = HasControlVisual;

                function GetControlVisuals(visual) {
                    return visual.Visuals().filter(function (_) {
                        return IsControlVisual(_);
                    });
                }
                Utils.GetControlVisuals = GetControlVisuals;

                function IsTransparentVisual(visual) {
                    return NXKit.Web.XForms.XFormsVisualViewModel.TransparentVisualTypes.some(function (_) {
                        return visual.Type == _;
                    });
                }
                Utils.IsTransparentVisual = IsTransparentVisual;

                function GetContents(visual) {
                    var l = visual.Visuals().filter(function (_) {
                        return !IsMetadataVisual(_);
                    });

                    var r = new Array();
                    for (var i = 0; i < l.length; i++) {
                        var v = l[i];
                        if (IsTransparentVisual(v)) {
                            var s = GetContents(v);
                            for (var j = 0; j < s.length; j++)
                                r.push(s[j]);
                        } else {
                            r.push(v);
                        }
                    }

                    return r;
                }
                Utils.GetContents = GetContents;
            })(XForms.Utils || (XForms.Utils = {}));
            var Utils = XForms.Utils;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
//# sourceMappingURL=NXKit.XForms.js.map
