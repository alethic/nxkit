var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (Knockout) {
            var LayoutManagerExportBindingHandler = (function () {
                function LayoutManagerExportBindingHandler() {
                }
                LayoutManagerExportBindingHandler._init = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                    var ctx = bindingContext;
                    var mgr = NXKit.Web.ViewModelUtil.LayoutManagers;

                    for (var i = 0; i < mgr.length; i++) {
                        ctx = ctx.createChildContext(mgr[i](ctx), null, null);
                    }

                    // replace context data item with original value
                    ctx = ctx.createChildContext(viewModel);
                    ctx = ctx.createChildContext(viewModel);

                    // apply new child context to element
                    ko.applyBindingsToDescendants(ctx, element);

                    return {
                        controlsDescendantBindings: true
                    };
                };

                LayoutManagerExportBindingHandler.prototype.init = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                    return LayoutManagerExportBindingHandler._init(element, valueAccessor, allBindings, viewModel, bindingContext);
                };
                return LayoutManagerExportBindingHandler;
            })();
            Knockout.LayoutManagerExportBindingHandler = LayoutManagerExportBindingHandler;

            ko.bindingHandlers['nxkit_layout_manager_export'] = new LayoutManagerExportBindingHandler();
            ko.virtualElements.allowedBindings['nxkit_layout_manager_export'] = true;
        })(Web.Knockout || (Web.Knockout = {}));
        var Knockout = Web.Knockout;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
//# sourceMappingURL=LayoutManagerExportBindingHandler.js.map
