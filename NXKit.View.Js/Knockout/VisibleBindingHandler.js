var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (Knockout) {
            var VisibleBindingHandler = (function () {
                function VisibleBindingHandler() {
                }
                VisibleBindingHandler.prototype.init = function (element, valueAccessor) {
                    var value = valueAccessor();
                    $(element).toggle(ko.utils.unwrapObservable(value));
                    ko.utils.unwrapObservable(value) ? $(element).slideDown() : $(element).slideUp();
                };

                VisibleBindingHandler.prototype.update = function (element, valueAccessor) {
                    var value = valueAccessor();
                    ko.utils.unwrapObservable(value) ? $(element).slideDown() : $(element).slideUp();
                };
                return VisibleBindingHandler;
            })();
            Knockout.VisibleBindingHandler = VisibleBindingHandler;

            ko.bindingHandlers['nxkit_visible'] = new VisibleBindingHandler();
            ko.virtualElements.allowedBindings['nxkit_visible'] = true;
        })(Web.Knockout || (Web.Knockout = {}));
        var Knockout = Web.Knockout;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
//# sourceMappingURL=VisibleBindingHandler.js.map
