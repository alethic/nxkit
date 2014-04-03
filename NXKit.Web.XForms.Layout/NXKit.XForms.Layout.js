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
            var LayoutNodeViewModel = (function (_super) {
                __extends(LayoutNodeViewModel, _super);
                function LayoutNodeViewModel(context, node) {
                    _super.call(this, context, node);
                }
                return LayoutNodeViewModel;
            })(NXKit.Web.XForms.XFormsNodeViewModel);
            XForms.LayoutNodeViewModel = LayoutNodeViewModel;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="LayoutNodeViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            (function (Layout) {
                var FormViewModel = (function (_super) {
                    __extends(FormViewModel, _super);
                    function FormViewModel(context, node) {
                        _super.call(this, context, node);
                        this.StepChanged = new TypedEvent();
                        var self = this;

                        self._activeStep = ko.observable();
                        self._rootStep = new NXKit.Web.XForms.Layout.FormUtil.Step(node, null, function (_) {
                            return _ === self._activeStep();
                        }, function (_) {
                            return self._activeStep(_);
                        });
                        self._activeStep(self.GetNextStep(self._rootStep));
                    }
                    Object.defineProperty(FormViewModel.prototype, "RootStep", {
                        get: function () {
                            return this._rootStep;
                        },
                        enumerable: true,
                        configurable: true
                    });

                    Object.defineProperty(FormViewModel.prototype, "ActiveStep", {
                        get: function () {
                            return this._activeStep;
                        },
                        enumerable: true,
                        configurable: true
                    });

                    FormViewModel.prototype.GetPreviousStep = function (step) {
                        var self = this;
                        if (step.Parent != null) {
                            for (var i = step.Parent.Steps.indexOf(step) - 1; i >= 0; i--) {
                                if (!step.Parent.Steps[i].Disabled()) {
                                    return step.Parent.Steps[i];
                                }
                            }
                        }

                        return null;
                    };

                    Object.defineProperty(FormViewModel.prototype, "HasPreviousStep", {
                        get: function () {
                            var self = this;
                            return ko.computed(function () {
                                return self.GetPreviousStep(self.ActiveStep()) != null;
                            });
                        },
                        enumerable: true,
                        configurable: true
                    });

                    FormViewModel.prototype.GoPreviousStep = function () {
                        var self = this;
                        var p = self.GetPreviousStep(self.ActiveStep());
                        if (p != null) {
                            self.ActiveStep(p);
                            self.Node.Invoke('NXKit.DOMEvents.INXEventTarget', 'DispatchEvent', {
                                type: 'xforms-layout-step-previous'
                            });
                        }
                    };

                    FormViewModel.prototype.GetNextStep = function (step) {
                        var self = this;

                        // if step has children
                        if (step.Steps.length > 0) {
                            var s = step.Steps[0];
                            return !s.Disabled() ? s : self.GetNextStep(s);
                        }

                        if (step.Parent != null) {
                            for (var i = step.Parent.Steps.indexOf(step) + 1; i < step.Parent.Steps.length; i++) {
                                if (!step.Parent.Steps[i].Disabled()) {
                                    return step.Parent.Steps[i];
                                }
                            }
                        }

                        return null;
                    };

                    Object.defineProperty(FormViewModel.prototype, "HasNextStep", {
                        get: function () {
                            var self = this;
                            return ko.computed(function () {
                                return self.GetNextStep(self.ActiveStep()) != null;
                            });
                        },
                        enumerable: true,
                        configurable: true
                    });

                    FormViewModel.prototype.GoNextStep = function () {
                        var self = this;
                        var p = self.GetNextStep(self.ActiveStep());
                        if (p != null) {
                            self.ActiveStep(p);
                            self.Node.Invoke('NXKit.DOMEvents.INXEventTarget', 'DispatchEvent', {
                                type: 'xforms-layout-step-next'
                            });
                        }
                    };
                    return FormViewModel;
                })(NXKit.Web.XForms.LayoutNodeViewModel);
                Layout.FormViewModel = FormViewModel;
            })(XForms.Layout || (XForms.Layout = {}));
            var Layout = XForms.Layout;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="LayoutNodeViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            (function (Layout) {
                var ParagraphViewModel = (function (_super) {
                    __extends(ParagraphViewModel, _super);
                    function ParagraphViewModel(context, node) {
                        _super.call(this, context, node);
                    }
                    return ParagraphViewModel;
                })(NodeViewModel);
                Layout.ParagraphViewModel = ParagraphViewModel;
            })(XForms.Layout || (XForms.Layout = {}));
            var Layout = XForms.Layout;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="LayoutNodeViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            (function (Layout) {
                var TableViewModel = (function (_super) {
                    __extends(TableViewModel, _super);
                    function TableViewModel(context, node) {
                        _super.call(this, context, node);
                    }
                    return TableViewModel;
                })(NXKit.Web.XForms.LayoutNodeViewModel);
                Layout.TableViewModel = TableViewModel;
            })(XForms.Layout || (XForms.Layout = {}));
            var Layout = XForms.Layout;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
/// <reference path="LayoutNodeViewModel.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            (function (Layout) {
                var SectionViewModel = (function (_super) {
                    __extends(SectionViewModel, _super);
                    function SectionViewModel(context, node) {
                        _super.call(this, context, node);
                    }
                    return SectionViewModel;
                })(NXKit.Web.XForms.LayoutNodeViewModel);
                Layout.SectionViewModel = SectionViewModel;
            })(XForms.Layout || (XForms.Layout = {}));
            var Layout = XForms.Layout;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            (function (Layout) {
                (function (Utils) {
                    function GetPages(node) {
                        var l = node.Nodes();
                        var r = new Array();
                        for (var i = 0; i < l.length; i++) {
                            var v = l[i];
                            if (v.Type !== 'NXKit.XForms.Layout.Page') {
                                var s = NXKit.Web.XForms.Layout.Utils.GetPages(v);
                                for (var j = 0; j < s.length; j++)
                                    r.push(s[j]);
                            } else {
                                r.push(v);
                            }
                        }

                        return r;
                    }
                    Utils.GetPages = GetPages;
                })(Layout.Utils || (Layout.Utils = {}));
                var Utils = Layout.Utils;
            })(XForms.Layout || (XForms.Layout = {}));
            var Layout = XForms.Layout;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            (function (Layout) {
                (function (FormUtil) {
                    /**
                    * Node types which represent a grouping element.
                    */
                    FormUtil.StepNodeTypes = [
                        'NXKit.XForms.Layout.Section'
                    ];

                    /**
                    * Returns true if the given node is a step node.
                    */
                    function IsStepNode(node) {
                        return NXKit.Web.XForms.Layout.FormUtil.StepNodeTypes.some(function (_) {
                            return node.Type == _;
                        });
                    }
                    FormUtil.IsStepNode = IsStepNode;

                    /**
                    * Returns true if the given node set contains a step node.
                    */
                    function HasStepNodes(nodes) {
                        return nodes.some(function (_) {
                            return NXKit.Web.XForms.Layout.FormUtil.IsStepNode(_);
                        });
                    }
                    FormUtil.HasStepNodes = HasStepNodes;

                    /**
                    * Filters out the given node set for step nodes.
                    */
                    function GetStepNodes(nodes) {
                        return nodes.filter(function (_) {
                            return NXKit.Web.XForms.Layout.FormUtil.IsStepNode(_);
                        });
                    }
                    FormUtil.GetStepNodes = GetStepNodes;

                    /**
                    * Represents a sub-item of a top-level group.
                    */
                    var Step = (function () {
                        function Step(node, parent, isActive, setActive) {
                            var self = this;
                            self._node = node;
                            self._parent = parent;
                            self._isActive = isActive;
                            self._setActive = setActive;
                            self._active = ko.computed(function () {
                                return self._isActive(self);
                            });
                            self._disabled = ko.computed(function () {
                                return NXKit.Web.XForms.ViewModelUtil.IsModelItemBinding(self._node) ? !NXKit.Web.XForms.ViewModelUtil.GetRelevant(self._node)() : false;
                            });
                            self._steps = GetSteps(node, self, isActive, setActive);
                        }
                        Object.defineProperty(Step.prototype, "Node", {
                            get: function () {
                                return this._node;
                            },
                            enumerable: true,
                            configurable: true
                        });

                        Object.defineProperty(Step.prototype, "Parent", {
                            get: function () {
                                return this._parent;
                            },
                            enumerable: true,
                            configurable: true
                        });

                        Object.defineProperty(Step.prototype, "Steps", {
                            get: function () {
                                return this._steps;
                            },
                            enumerable: true,
                            configurable: true
                        });

                        Object.defineProperty(Step.prototype, "Label", {
                            get: function () {
                                return NXKit.Web.XForms.ViewModelUtil.GetLabelNode(this._node);
                            },
                            enumerable: true,
                            configurable: true
                        });

                        Object.defineProperty(Step.prototype, "Active", {
                            get: function () {
                                return this._active;
                            },
                            enumerable: true,
                            configurable: true
                        });

                        Object.defineProperty(Step.prototype, "Disabled", {
                            get: function () {
                                return this._disabled;
                            },
                            enumerable: true,
                            configurable: true
                        });

                        Step.prototype.SetActive = function () {
                            this._setActive(this);
                        };
                        return Step;
                    })();
                    FormUtil.Step = Step;

                    /**
                    * Converts each node into a step item.
                    */
                    function GetSteps(node, parent, isActive, setActive) {
                        return node.Nodes().filter(function (_) {
                            return NXKit.Web.XForms.Layout.FormUtil.IsStepNode(_);
                        }).map(function (_) {
                            return new NXKit.Web.XForms.Layout.FormUtil.Step(_, parent, isActive, setActive);
                        });
                    }
                })(Layout.FormUtil || (Layout.FormUtil = {}));
                var FormUtil = Layout.FormUtil;
            })(XForms.Layout || (XForms.Layout = {}));
            var Layout = XForms.Layout;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
//# sourceMappingURL=NXKit.XForms.Layout.js.map
