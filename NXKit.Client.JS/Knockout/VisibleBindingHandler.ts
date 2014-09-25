module NXKit.Web.Knockout {

    export class VisibleBindingHandler
        implements KnockoutBindingHandler {

        init(element: HTMLElement, valueAccessor: () => any) {
            var value = valueAccessor();
            $(element).toggle(ko.utils.unwrapObservable(value));
            ko.utils.unwrapObservable(value) ? $(element).slideDown() : $(element).slideUp();
        }

        update(element: HTMLElement, valueAccessor: () => any) {
            var value = valueAccessor();
            ko.utils.unwrapObservable(value) ? $(element).slideDown() : $(element).slideUp();
        }

    }

    ko.bindingHandlers['nxkit_visible'] = new VisibleBindingHandler();
    ko.virtualElements.allowedBindings['nxkit_visible'] = true;

}
