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
//# sourceMappingURL=CheckboxBindingHandler.js.map
