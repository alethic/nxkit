module NXKit.Web.Knockout {

    class InputBindingHandler
        implements KnockoutBindingHandler {

        static _init(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext) {
            ko.bindingHandlers['value'].init(element, valueAccessor, allBindings, viewModel, bindingContext);
        }

        static _update(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext) {
            ko.bindingHandlers['value'].update(element, valueAccessor, allBindings, viewModel, bindingContext);
        }

        public init(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext) {
            InputBindingHandler._init(element, valueAccessor, allBindings, viewModel, bindingContext);
        }

        public update(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext) {
            InputBindingHandler._update(element, valueAccessor, allBindings, viewModel, bindingContext);
        }

    }

    ko.bindingHandlers['nxkit_input'] = new InputBindingHandler();

}
