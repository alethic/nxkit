module NXKit.Web.Knockout {

    class ModalBindingHandler
        implements KnockoutBindingHandler {

        init(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext) {

            // register click handler
            ko.bindingHandlers.click.init(element,
                () => function () {
                    var id = valueAccessor();
                    if (id) {
                        $('#' + id).modal('show');
                    }
                }, allBindings, viewModel, bindingContext);

            // disable event bubbling
            ko.bindingHandlers['click-bubble'].init(element, () => false, allBindings, viewModel, bindingContext);
        }
    }

    ko.bindingHandlers['nxkit_modal'] = new ModalBindingHandler();

}
