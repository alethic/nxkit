module NXKit.Web.Knockout {

    class CheckboxBindingHandler
        implements KnockoutBindingHandler {

        static _init(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext) {
            setTimeout(function () {
                $(element).checkbox();
                $(element).checkbox('setting', {
                    onEnable: function () {
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
                    onDisable: function () {
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
               CheckboxBindingHandler._update(element, valueAccessor, allBindings, viewModel, bindingContext);
            }, 2000);
        }

        static _update(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext) {
            var self = this;
            setTimeout(function () {
                var v1 = ko.unwrap(valueAccessor());
                if (typeof v1 === 'boolean') {
                    $(element).checkbox(v1 ? 'enable' : 'disable')
                } else if (typeof v1 === 'string') {
                    var v1_ = v1.toLowerCase() === 'true' ? true : false;
                    $(element).checkbox(v1_ ? 'enable' : 'disable')
                }
            }, 1000);
        }

        public init(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext) {
            CheckboxBindingHandler._init(element, valueAccessor, allBindings, viewModel, bindingContext);
        }

        public update(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext) {
            CheckboxBindingHandler._update(element, valueAccessor, allBindings, viewModel, bindingContext);
        }

    }

    ko.bindingHandlers['nxkit_checkbox'] = new CheckboxBindingHandler();

}
