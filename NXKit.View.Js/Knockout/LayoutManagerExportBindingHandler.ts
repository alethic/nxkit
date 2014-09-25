module NXKit.View.Knockout {

    export class LayoutManagerExportBindingHandler
        implements KnockoutBindingHandler {

        static _init(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext) {
            var ctx = bindingContext;
            var mgr = NXKit.View.ViewModelUtil.LayoutManagers;

            // generate a context tree from managers
            for (var i = 0; i < mgr.length; i++) {
                ctx = ctx.createChildContext(
                    mgr[i](ctx),
                    null,
                    null);
            }

            // replace context data item with original value
            ctx = ctx.createChildContext(viewModel);
            ctx = ctx.createChildContext(viewModel);

            // apply new child context to element
            ko.applyBindingsToDescendants(ctx, element);

            return {
                controlsDescendantBindings: true,
            };
        }

        public init(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext) {
            return LayoutManagerExportBindingHandler._init(element, valueAccessor, allBindings, viewModel, bindingContext);
        }

    }

    ko.bindingHandlers['nxkit_layout_manager_export'] = new LayoutManagerExportBindingHandler();
    ko.virtualElements.allowedBindings['nxkit_layout_manager_export'] = true;

}
