module NXKit.View.XForms.Knockout {

    export class Select1BindingHandler
        implements KnockoutBindingHandler {

        static _init(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext) {
            var body = $(element).find('.body');
            setTimeout(function () {
                $(body).dropdown();
                $(body).dropdown({
                    onChange: function (value: any) {
                        var v1 = <string>$(body).dropdown('get value') || null;
                        var v2 = <string>ko.unwrap(valueAccessor()) || null;
                        if (v1 != v2) {
                            valueAccessor()(v1);
                        }
                    },
                });
                Select1BindingHandler._update(element, valueAccessor, allBindings, viewModel, bindingContext);
            }, 1000);
        }

        static _update(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext) {
            var body = $(element).find('.body');
            setTimeout(function () {
                var v1 = <string>ko.unwrap(valueAccessor()) || null;
                $(body).dropdown('set value', v1);
            }, 2000);
        }

        public init(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext) {
            Select1BindingHandler._init(element, valueAccessor, allBindings, viewModel, bindingContext);
        }

        public update(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext) {
            Select1BindingHandler._update(element, valueAccessor, allBindings, viewModel, bindingContext);
        }

    }

    ko.bindingHandlers['nxkit_xforms_select1'] = new Select1BindingHandler();

}
