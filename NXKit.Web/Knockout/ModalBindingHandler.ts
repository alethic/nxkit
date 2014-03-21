module NXKit.Web.Knockout {

    class ModalBindingHandler
        implements KnockoutBindingHandler {

        init(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext) {
            ko.bindingHandlers.click.init(element, () => function () {
                var id = valueAccessor();
                if (id) {
                    $('#' + id).modal('show');
                }
            }, allBindings, viewModel, bindingContext);
        }
}

ko.bindingHandlers['nxkit_modal'] = new ModalBindingHandler();

}
