ko.bindingHandlers.dropdown = {
    init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        setTimeout(function () {
            $(element).dropdown();
            $(element).dropdown('setting', {
                onChange: function (value) {
                    var v1 = $(element).dropdown('get value');
                    var v2 = ko.unwrap(valueAccessor());
                    if (typeof v1 === 'string') {
                        if (v1 != v2)
                            valueAccessor()(v1);
                    }
                },
            });
        }, 2000);
    },
    update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        setTimeout(function () {
            var v1 = ko.unwrap(valueAccessor());
            var v2 = $(element).dropdown('get value');
            if (typeof v2 === 'string')
                if (v1 != v2)
                    $(element).dropdown('set value', v1);
        }, 1000);
    },
};
