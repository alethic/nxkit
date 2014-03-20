ko.bindingHandlers.nxkit_checkbox = {
    init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        setTimeout(function () {
            $(element).checkbox();
            $(element).checkbox('setting', {
                onEnable: function() {
                    var v1 = true;
                    var v2 = ko.unwrap(valueAccessor());
                    if (typeof v2 === 'boolean') {
                        if (v1 != v2)
                            valueAccessor()(v1);
                    } else if (typeof v2 === 'string') {
                        var v2_ = v2.toLowerCase() === 'true' ? true : false;
                        if (v1 != v2_)
                            valueAccessor()(v1 ? 'true' : 'false');
                    }
                },
                onDisable: function() {
                    var v1 = false;
                    var v2 = ko.unwrap(valueAccessor());
                    if (typeof v2 === 'boolean') {
                        if (v1 != v2)
                            valueAccessor()(v1);
                    } else if (typeof v2 === 'string') {
                        var v2_ = v2.toLowerCase() === 'true' ? true : false;
                        if (v1 != v2_)
                            valueAccessor()(v1 ? 'true' : 'false');
                    }
                },
            });
        }, 2000);
    },
    update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        setTimeout(function () {
            var v1 = ko.unwrap(valueAccessor());
            var v2 = $(element).find('input').val() == 'on' ? true : false;
            if (typeof v1 === 'boolean') {
                if (v1 != v2)
                    $(element).checkbox(v1 ? 'enable' : 'disable')
            } else if (typeof v1 === 'string') {
                var v1_ = v1.toLowerCase() === 'true' ? true : false;
                if (v1_ != v2)
                    $(element).checkbox(v1_ ? 'enable' : 'disable')
            }
        }, 1000);
    },
};

ko.bindingHandlers.nxkit_dropdown = {
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
