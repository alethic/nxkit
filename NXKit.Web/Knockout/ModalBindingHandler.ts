module NXKit.Web.Knockout {

    class ModalBindingHandler
        implements KnockoutBindingHandler {

        init(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext) {

            var f = ko.utils.extend(allBindings(), {
                clickBubble: false,
            });

            ko.bindingHandlers.click.init(
                element,
                // inject click handler that shows modal
                () => function () {
                    setTimeout(() => {
                        var id = valueAccessor();
                        if (id) {
                            $('#' + id).modal('show');
                        }
                    }, 5);
                },
                // add clickBubble: false binding
                // TODO broken
                allBindings,
                viewModel,
                bindingContext);
        }
    }

    ko.bindingHandlers['nxkit_modal'] = new ModalBindingHandler();

}
