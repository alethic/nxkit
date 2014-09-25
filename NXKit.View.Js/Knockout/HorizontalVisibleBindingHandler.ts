$.fn.extend({
    slideRightShow: function () {
        return this.each(function () {
            $(this).show('slide', { direction: 'right' }, 1000);
        });
    },
    slideLeftHide: function () {
        return this.each(function () {
            $(this).hide('slide', { direction: 'left' }, 1000);
        });
    },
    slideRightHide: function () {
        return this.each(function () {
            $(this).hide('slide', { direction: 'right' }, 1000);
        });
    },
    slideLeftShow: function () {
        return this.each(function () {
            $(this).show('slide', { direction: 'left' }, 1000);
        });
    }
});

module NXKit.View.Knockout {

    export class HorizontalVisibleBindingHandler
        implements KnockoutBindingHandler {

        init(element: HTMLElement, valueAccessor: () => any) {
            var value = valueAccessor();
            $(element).toggle(ko.utils.unwrapObservable(value));
            ko.utils.unwrapObservable(value) ? $(element)['slideLeftShow']() : $(element)['slideLeftHide']();
        }

        update(element: HTMLElement, valueAccessor: () => any) {
            var value = valueAccessor();
            ko.utils.unwrapObservable(value) ? $(element)['slideLeftShow']() : $(element)['slideLeftHide']();
        }

    }

    ko.bindingHandlers['nxkit_hvisible'] = new HorizontalVisibleBindingHandler();
    ko.virtualElements.allowedBindings['nxkit_hvisible'] = true;
}
