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
