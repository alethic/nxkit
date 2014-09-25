/// <reference path="../Util.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (Knockout) {
            var OptionsBindingHandler = (function () {
                function OptionsBindingHandler() {
                }
                OptionsBindingHandler.prototype.init = function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
                    var opts = new Web.LayoutOptions(valueAccessor());

                    // inject context containing options
                    var ctx1 = bindingContext.createChildContext(opts, null, null);

                    // inject context containing initial view model
                    var ctx2 = ctx1.createChildContext(viewModel, null, null);

                    // apply to descendants
                    ko.applyBindingsToDescendants(ctx2, element);

                    // prevent built-in application
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
//# sourceMappingURL=OptionsBindingHandler.js.map
