ko.bindingHandlers.nxkit_template = {

    get_data: function (valueAccessor, bindingContext) {
        var data = NXKit.Web.Utils.GetVisualTemplateData(valueAccessor());

        // specified data value
        if (typeof value.data !== 'undefined')
            return value.data;

        // specified visual value
        if (typeof value.visual !== 'undefined')
            if (value.hasOwnProperty('IsVisual'))
                if (value.IsVisual)
                    return value.visual;

        // value itself is a visual
        if (value.hasOwnProperty('IsVisual'))
            if (value.IsVisual)
                return value;

        // default to existing context
        return bindingContext.data;
    },

    convert_value_accessor: function (valueAccessor, bindingContext) {
        return function () {
            return {
                data: ko.bindingHandlers.nxkit_template.get_data(valueAccessor, bindingContext),
                name: NXKit.Web.Utils.GetLayoutManager(bindingContext).GetTemplate(valueAccessor()),
            };
        }
    },

    get_value_accessor: function (valueAccessor, bindingContext) {
        bindingContext.nxkit_template = bindingContext.nxkit_template || {};
        bindingContext.nxkit_template.valueAccessor = bindingContext.nxkit_template.valueAccessor ||
            ko.bindingHandlers.nxkit_template.convert_value_accessor(valueAccessor, bindingContext);

        return bindingContext.nxkit_template.valueAccessor;
    },

    init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        return ko.bindingHandlers['template']['init'](
            element,
            ko.bindingHandlers.nxkit_template.get_value_accessor(valueAccessor, bindingContext),
            allBindings,
            viewModel,
            bindingContext);
    },
    update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        return ko.bindingHandlers['template']['update'](
            element,
            ko.bindingHandlers.nxkit_template.get_value_accessor(valueAccessor, bindingContext),
            allBindings,
            viewModel,
            bindingContext);
    }
};

ko.virtualElements.allowedBindings.nxkit_template = true;
