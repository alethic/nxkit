ko.bindingHandlers.nxkit_visible = {
    init: function (element, valueAccessor) {
        var value = valueAccessor();
        $(element).toggle(ko.utils.unwrapObservable(value));
        ko.utils.unwrapObservable(value) ? $(element).slideDown() : $(element).slideUp();
    },
    update: function (element, valueAccessor) {
        var value = valueAccessor();
        ko.utils.unwrapObservable(value) ? $(element).slideDown() : $(element).slideUp();
    }
};
