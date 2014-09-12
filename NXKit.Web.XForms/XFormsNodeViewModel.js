var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
define(["require", "exports"], function(require, exports) {
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
                            return ViewModelUtil.GetDataValue(this.Node);
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Object.defineProperty(XFormsNodeViewModel.prototype, "ValueAsString", {
                        get: function () {
                            return ViewModelUtil.GetDataValueAsString(this.Node);
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Object.defineProperty(XFormsNodeViewModel.prototype, "ValueAsBoolean", {
                        get: function () {
                            return ViewModelUtil.GetDataValueAsBoolean(this.Node);
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Object.defineProperty(XFormsNodeViewModel.prototype, "ValueAsNumber", {
                        get: function () {
                            return ViewModelUtil.GetDataValueAsNumber(this.Node);
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Object.defineProperty(XFormsNodeViewModel.prototype, "Relevant", {
                        get: function () {
                            return ViewModelUtil.GetRelevant(this.Node);
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Object.defineProperty(XFormsNodeViewModel.prototype, "ReadOnly", {
                        get: function () {
                            return ViewModelUtil.GetReadOnly(this.Node);
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Object.defineProperty(XFormsNodeViewModel.prototype, "Required", {
                        get: function () {
                            return ViewModelUtil.GetRequired(this.Node);
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Object.defineProperty(XFormsNodeViewModel.prototype, "Valid", {
                        get: function () {
                            return ViewModelUtil.GetValid(this.Node);
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Object.defineProperty(XFormsNodeViewModel.prototype, "Type", {
                        get: function () {
                            return ViewModelUtil.GetDataType(this.Node);
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Object.defineProperty(XFormsNodeViewModel.prototype, "Appearance", {
                        get: function () {
                            return ViewModelUtil.GetAppearance(this.Node);
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Object.defineProperty(XFormsNodeViewModel.prototype, "Label", {
                        get: function () {
                            try  {
                                return ViewModelUtil.GetLabelNode(this.Node);
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
                            return this.Label != null ? ViewModelUtil.GetAppearance(this.Label) : null;
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Object.defineProperty(XFormsNodeViewModel.prototype, "Help", {
                        get: function () {
                            return ViewModelUtil.GetHelpNode(this.Node);
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Object.defineProperty(XFormsNodeViewModel.prototype, "Hint", {
                        get: function () {
                            return ViewModelUtil.GetHintNode(this.Node);
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Object.defineProperty(XFormsNodeViewModel.prototype, "Alert", {
                        get: function () {
                            return ViewModelUtil.GetAlertNode(this.Node);
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Object.defineProperty(XFormsNodeViewModel.prototype, "CountEnabled", {
                        get: function () {
                            return ko.utils.arrayFilter(this.Contents, function (_) {
                                return ViewModelUtil.GetRelevant(_)();
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
    })(exports.NXKit || (exports.NXKit = {}));
    var NXKit = exports.NXKit;
});
