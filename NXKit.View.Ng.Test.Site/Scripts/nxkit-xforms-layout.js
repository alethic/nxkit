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
    (function (View) {
        (function (XForms) {
            var LayoutNodeViewModel = (function (_super) {
                __extends(LayoutNodeViewModel, _super);
                function LayoutNodeViewModel(context, node) {
                    _super.call(this, context, node);
                }
                return LayoutNodeViewModel;
            })(NXKit.View.XForms.XFormsNodeViewModel);
            XForms.LayoutNodeViewModel = LayoutNodeViewModel;
        })(View.XForms || (View.XForms = {}));
        var XForms = View.XForms;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));
/// <reference path="LayoutNodeViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (View) {
        (function (XForms) {
            (function (Layout) {
                var FormViewModel = (function (_super) {
                    __extends(FormViewModel, _super);
                    function FormViewModel(context, node) {
                        _super.call(this, context, node);
                    }
                    return FormViewModel;
                })(XForms.LayoutNodeViewModel);
                Layout.FormViewModel = FormViewModel;
            })(XForms.Layout || (XForms.Layout = {}));
            var Layout = XForms.Layout;
        })(View.XForms || (View.XForms = {}));
        var XForms = View.XForms;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));
/// <reference path="LayoutNodeViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (View) {
        (function (XForms) {
            (function (Layout) {
                var IconViewModel = (function (_super) {
                    __extends(IconViewModel, _super);
                    function IconViewModel(context, node) {
                        _super.call(this, context, node);
                    }
                    Object.defineProperty(IconViewModel.prototype, "Name", {
                        get: function () {
                            var p = this.Node.Property('NXKit.XForms.Layout.Icon', 'Name');
                            return p != null ? p.ValueAsString : null;
                        },
                        enumerable: true,
                        configurable: true
                    });
                    return IconViewModel;
                })(XForms.LayoutNodeViewModel);
                Layout.IconViewModel = IconViewModel;
            })(XForms.Layout || (XForms.Layout = {}));
            var Layout = XForms.Layout;
        })(View.XForms || (View.XForms = {}));
        var XForms = View.XForms;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));
/// <reference path="LayoutNodeViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (View) {
        (function (XForms) {
            (function (Layout) {
                var ItemViewModel = (function (_super) {
                    __extends(ItemViewModel, _super);
                    function ItemViewModel(context, node) {
                        _super.call(this, context, node);
                    }
                    Object.defineProperty(ItemViewModel.prototype, "LabelAppearance", {
                        get: function () {
                            return this.Label != null ? XForms.ViewModelUtil.GetAppearance(this.Label) : null;
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Object.defineProperty(ItemViewModel.prototype, "Count", {
                        get: function () {
                            return this.Contents.length;
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Object.defineProperty(ItemViewModel.prototype, "CountEnabled", {
                        get: function () {
                            return ko.utils.arrayFilter(this.Contents, function (_) {
                                return XForms.ViewModelUtil.GetRelevant(_)();
                            }).length;
                        },
                        enumerable: true,
                        configurable: true
                    });
                    return ItemViewModel;
                })(XForms.LayoutNodeViewModel);
                Layout.ItemViewModel = ItemViewModel;
            })(XForms.Layout || (XForms.Layout = {}));
            var Layout = XForms.Layout;
        })(View.XForms || (View.XForms = {}));
        var XForms = View.XForms;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));
/// <reference path="LayoutNodeViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (View) {
        (function (XForms) {
            (function (Layout) {
                var ListViewModel = (function (_super) {
                    __extends(ListViewModel, _super);
                    function ListViewModel(context, node) {
                        _super.call(this, context, node);
                    }
                    Object.defineProperty(ListViewModel.prototype, "Items", {
                        get: function () {
                            return ko.utils.arrayFilter(this.Contents, function (_) {
                                return _.Name == '{http://schemas.nxkit.org/2014/xforms-layout}item';
                            });
                        },
                        enumerable: true,
                        configurable: true
                    });
                    return ListViewModel;
                })(XForms.LayoutNodeViewModel);
                Layout.ListViewModel = ListViewModel;
            })(XForms.Layout || (XForms.Layout = {}));
            var Layout = XForms.Layout;
        })(View.XForms || (View.XForms = {}));
        var XForms = View.XForms;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));
/// <reference path="LayoutNodeViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (View) {
        (function (XForms) {
            (function (Layout) {
                var StrongViewModel = (function (_super) {
                    __extends(StrongViewModel, _super);
                    function StrongViewModel(context, node) {
                        _super.call(this, context, node);
                    }
                    return StrongViewModel;
                })(XForms.LayoutNodeViewModel);
                Layout.StrongViewModel = StrongViewModel;
            })(XForms.Layout || (XForms.Layout = {}));
            var Layout = XForms.Layout;
        })(View.XForms || (View.XForms = {}));
        var XForms = View.XForms;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));
/// <reference path="LayoutNodeViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (View) {
        (function (XForms) {
            (function (Layout) {
                var ParagraphViewModel = (function (_super) {
                    __extends(ParagraphViewModel, _super);
                    function ParagraphViewModel(context, node) {
                        _super.call(this, context, node);
                    }
                    return ParagraphViewModel;
                })(XForms.LayoutNodeViewModel);
                Layout.ParagraphViewModel = ParagraphViewModel;
            })(XForms.Layout || (XForms.Layout = {}));
            var Layout = XForms.Layout;
        })(View.XForms || (View.XForms = {}));
        var XForms = View.XForms;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));
/// <reference path="LayoutNodeViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (View) {
        (function (XForms) {
            (function (Layout) {
                var SegmentViewModel = (function (_super) {
                    __extends(SegmentViewModel, _super);
                    function SegmentViewModel(context, node) {
                        _super.call(this, context, node);
                    }
                    return SegmentViewModel;
                })(XForms.LayoutNodeViewModel);
                Layout.SegmentViewModel = SegmentViewModel;
            })(XForms.Layout || (XForms.Layout = {}));
            var Layout = XForms.Layout;
        })(View.XForms || (View.XForms = {}));
        var XForms = View.XForms;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));
/// <reference path="LayoutNodeViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (View) {
        (function (XForms) {
            (function (Layout) {
                var TableCellViewModel = (function (_super) {
                    __extends(TableCellViewModel, _super);
                    function TableCellViewModel(context, node) {
                        _super.call(this, context, node);
                    }
                    TableCellViewModel.prototype.Activate = function () {
                        var self = this;

                        // ensure property changes or non-focus events flush first
                        setTimeout(function () {
                            return self.Node.Invoke('NXKit.DOMEvents.EventTarget', 'Dispatch', {
                                type: 'DOMActivate'
                            });
                        }, 50);
                    };
                    return TableCellViewModel;
                })(XForms.LayoutNodeViewModel);
                Layout.TableCellViewModel = TableCellViewModel;
            })(XForms.Layout || (XForms.Layout = {}));
            var Layout = XForms.Layout;
        })(View.XForms || (View.XForms = {}));
        var XForms = View.XForms;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));
/// <reference path="LayoutNodeViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (View) {
        (function (XForms) {
            (function (Layout) {
                var TableViewModel = (function (_super) {
                    __extends(TableViewModel, _super);
                    function TableViewModel(context, node) {
                        _super.call(this, context, node);
                    }
                    return TableViewModel;
                })(XForms.LayoutNodeViewModel);
                Layout.TableViewModel = TableViewModel;
            })(XForms.Layout || (XForms.Layout = {}));
            var Layout = XForms.Layout;
        })(View.XForms || (View.XForms = {}));
        var XForms = View.XForms;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));
/// <reference path="LayoutNodeViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (View) {
        (function (XForms) {
            (function (Layout) {
                var SectionViewModel = (function (_super) {
                    __extends(SectionViewModel, _super);
                    function SectionViewModel(context, node) {
                        _super.call(this, context, node);
                    }
                    return SectionViewModel;
                })(XForms.LayoutNodeViewModel);
                Layout.SectionViewModel = SectionViewModel;
            })(XForms.Layout || (XForms.Layout = {}));
            var Layout = XForms.Layout;
        })(View.XForms || (View.XForms = {}));
        var XForms = View.XForms;
    })(NXKit.View || (NXKit.View = {}));
    var View = NXKit.View;
})(NXKit || (NXKit = {}));


        return NXKit;
    };

    NXKit.define("nxkit-xforms-layout", ['jquery', 'knockout', 'nxkit', 'nxkit-xforms'], function ($, ko, NXKit) {
        return init($, ko, NXKit);
    });
})();