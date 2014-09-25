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
        public GetTemplateOptions(valueAccessor: KnockoutObservable<any>, viewModel: any, bindingContext: KnockoutBindingContext, options: any): any {
            options = super.GetTemplateOptions(valueAccessor, viewModel, bindingContext, options);
            var value = ko.unwrap(valueAccessor());

            // extract level binding
            if (value != null &&
                value.level != null)
                options.level = ko.unwrap(value.level);

            return options;
        }

    }

}
