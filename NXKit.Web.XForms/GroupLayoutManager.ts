/// <reference path="XFormsNodeViewModel.ts" />

module NXKit.Web.XForms {

    export class GroupLayoutManager
        extends NXKit.Web.LayoutManager {

        constructor(context: KnockoutBindingContext) {
            super(context);
        }

        /**
         * Applies the 'level' and 'layout' bindings to the template search.
         */
        public ParseTemplateBinding(valueAccessor: KnockoutObservable<any>, viewModel: any, bindingContext: KnockoutBindingContext, data: any): any {
            data = super.ParseTemplateBinding(valueAccessor, viewModel, bindingContext, data);

            // extract level binding
            var value = valueAccessor();
            if (value != null &&
                value.level != null &&
                ko.unwrap(value.level) != null)
                data.level = ko.unwrap(value.level);

            return data;
        }

    }

}
