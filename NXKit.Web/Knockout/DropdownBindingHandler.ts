module NXKit.Web.Knockout {

    class DropdownBindingHandler
        implements KnockoutBindingHandler {

        static _init(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext) {
            setTimeout(function () {
                $(element).dropdown();
                $(element).dropdown({
                    onChange: function (value: any) {
                        var v1 = $(element).dropdown('get value');
                        var v2 = ko.unwrap(valueAccessor());
                        if (typeof v1 === 'string') {
                            if (v1 != v2)
                                valueAccessor()(v1);
                        }
                    },
                });
                DropdownBindingHandler._update(element, valueAccessor, allBindings, viewModel, bindingContext);
            }, 1000);
        }

        static _update(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext) {
            setTimeout(function () {
                var v1 = ko.unwrap(valueAccessor());
                $(element).dropdown('set value', v1);
            }, 2000);
        }

        public init(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext) {
            DropdownBindingHandler._init(element, valueAccessor, allBindings, viewModel, bindingContext);
        }

        public update(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext) {
            DropdownBindingHandler._update(element, valueAccessor, allBindings, viewModel, bindingContext);
        }

    }

    ko.bindingHandlers['nxkit_dropdown'] = new DropdownBindingHandler();

}
