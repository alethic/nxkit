module NXKit.Web.Knockout {

    export class InputBooleanBindingHandler
        implements KnockoutBindingHandler {

        static _init(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext) {
            var self = this;
            var checkbox = $(element);
            setTimeout(function () {
                //$(checkbox).checkbox();
                //$(checkbox).checkbox('setting', {
                //    onEnable: function () {
                //        var v1 = true;
                //        var v2 = ko.unwrap(valueAccessor());
                //        if (typeof v2 === 'boolean') {
                //            if (v1 != v2)
                //                valueAccessor()(v1);
                //        } else if (typeof v2 === 'string') {
                //            var v2_ = v2.toLowerCase() === 'true' ? true : false;
                //            if (v1 != v2_)
                //                valueAccessor()(v1 ? 'true' : 'false');
                //        }
                //    },
                //    onDisable: function () {
                //        var v1 = false;
                //        var v2 = ko.unwrap(valueAccessor());
                //        if (typeof v2 === 'boolean') {
                //            if (v1 != v2)
                //                valueAccessor()(v1);
                //        } else if (typeof v2 === 'string') {
                //            var v2_ = v2.toLowerCase() === 'true' ? true : false;
                //            if (v1 != v2_)
                //                valueAccessor()(v1 ? 'true' : 'false');
                //        }
                //    },
                //});
               InputBooleanBindingHandler._update(element, valueAccessor, allBindings, viewModel, bindingContext);
            }, 2000);
        }

        static _update(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext) {
            var self = this;
            var checkbox = $(element);
            setTimeout(function () {
                var v1 = ko.unwrap(valueAccessor());
                if (typeof v1 === 'boolean') {
                    //$(checkbox).checkbox(v1 ? 'enable' : 'disable')
                } else if (typeof v1 === 'string') {
                    var v1_ = v1.toLowerCase() === 'true' ? true : false;
                    //$(checkbox).checkbox(v1_ ? 'enable' : 'disable')
                }
            }, 1000);
        }

        public init(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext) {
            InputBooleanBindingHandler._init(element, valueAccessor, allBindings, viewModel, bindingContext);
        }

        public update(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext) {
            InputBooleanBindingHandler._update(element, valueAccessor, allBindings, viewModel, bindingContext);
        }

    }

    ko.bindingHandlers['nxkit_xforms_input_boolean'] = new InputBooleanBindingHandler();

}
