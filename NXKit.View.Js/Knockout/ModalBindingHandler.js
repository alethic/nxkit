var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (Knockout) {
            var ModalBindingHandler = (function () {
                function ModalBindingHandler() {
                }
                ModalBindingHandler.prototype.init = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                    var f = ko.utils.extend(allBindings(), {
                        clickBubble: false
                    });

                    ko.bindingHandlers.click.init(element, // inject click handler that shows modal
                    function () {
                        return function () {
                            setTimeout(function () {
                                var id = valueAccessor();
                                if (id) {
                                    $('#' + id).modal('show');
                                }
                            }, 5);
                        };
                    }, allBindings, viewModel, bindingContext);
                };
                return ModalBindingHandler;
            })();
            Knockout.ModalBindingHandler = ModalBindingHandler;

            ko.bindingHandlers['nxkit_modal'] = new ModalBindingHandler();
        })(Web.Knockout || (Web.Knockout = {}));
        var Knockout = Web.Knockout;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
//# sourceMappingURL=ModalBindingHandler.js.map
