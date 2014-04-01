/// <reference path="../Util.ts" />

module NXKit.Web.Knockout {

    class OptionsBindingHandler
        implements KnockoutBindingHandler {

        public init(element: HTMLElement, valueAccessor: () => any, allBindingsAccessor: () => any, viewModel: any, bindingContext: KnockoutBindingContext): any {
            var opts = new LayoutOptions(valueAccessor());

            // inject context containing options
            var ctx1 = <KnockoutBindingContext>bindingContext.createChildContext(
                opts,
                null,
                null);

            // inject context containing initial view model
            var ctx2 = ctx1.createChildContext(
                viewModel,
                null,
                null);

            // apply to descendants
            ko.applyBindingsToDescendants(ctx2, element);

            // prevent built-in application
            return {
                controlsDescendantBindings: true,
            };
        }

    }

    ko.bindingHandlers['nxkit_layout'] = new OptionsBindingHandler();
    ko.virtualElements.allowedBindings['nxkit_layout'] = true;

}
