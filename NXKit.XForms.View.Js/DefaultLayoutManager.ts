/// <reference path="XFormsNodeViewModel.ts" />

module NXKit.Web.XForms {

    export class DefaultLayoutManager
        extends NXKit.Web.LayoutManager {

        constructor(context: KnockoutBindingContext) {
            super(context);
        }

        /**
         * Applies the 'level' and 'layout' bindings to the template search.
         */
        public GetTemplateOptions(valueAccessor: KnockoutObservable<any>, viewModel: any, bindingContext: KnockoutBindingContext, options: any): any {
            options = super.GetTemplateOptions(valueAccessor, viewModel, bindingContext, options);
            var node = super.GetNode(valueAccessor, viewModel, bindingContext);
            var value = ko.unwrap(valueAccessor());

            if (node != null &&
                node.Type == NodeType.Element) {
                var dataType = ViewModelUtil.GetDataType(node)();
                if (dataType != null) {
                    options['data-type'] = dataType;
                }
            }

            // specified data type
            if (value != null &&
                value['data-type'] != null) {
                options['data-type'] = ko.unwrap(value['data-type']);
            }

            // extract level binding
            var value = valueAccessor();
            if (value != null &&
                value.level != null) {
                options.level = ko.unwrap(value.level);
            }

            return options;
        }

    }

}
