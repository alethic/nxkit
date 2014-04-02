module NXKit.Web.Knockout {

    class DropdownBindingHandler
        implements KnockoutBindingHandler {

        init(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext) {
            var self = this;
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
                self.update(element, valueAccessor, allBindings, viewModel, bindingContext);
            }, 2000);
        }

        update(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext) {
            var self = this;
            setTimeout(function () {
                var v1 = ko.unwrap(valueAccessor());
                $(element).dropdown('set value', v1);
            }, 1000);
        }

    }

    ko.bindingHandlers['nxkit_dropdown'] = new DropdownBindingHandler();

}
