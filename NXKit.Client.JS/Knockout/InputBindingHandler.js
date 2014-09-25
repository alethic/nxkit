var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (Knockout) {
            var InputBindingHandler = (function () {
                function InputBindingHandler() {
                }
                InputBindingHandler._init = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                    ko.bindingHandlers['value'].init(element, valueAccessor, allBindings, viewModel, bindingContext);
                };

                InputBindingHandler._update = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                    ko.bindingHandlers['value'].update(element, valueAccessor, allBindings, viewModel, bindingContext);
                };

                InputBindingHandler.prototype.init = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                    InputBindingHandler._init(element, valueAccessor, allBindings, viewModel, bindingContext);
                };

                InputBindingHandler.prototype.update = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                    InputBindingHandler._update(element, valueAccessor, allBindings, viewModel, bindingContext);
                };
                return InputBindingHandler;
            })();
            Knockout.InputBindingHandler = InputBindingHandler;

            ko.bindingHandlers['nxkit_input'] = new InputBindingHandler();
        })(Web.Knockout || (Web.Knockout = {}));
        var Knockout = Web.Knockout;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
//# sourceMappingURL=InputBindingHandler.js.map
