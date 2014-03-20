/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="../LayoutManager.ts" />

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

            // extract layout binding
            var value = valueAccessor();
            if (value != null &&
                value.layout != null &&
                ko.unwrap(value.layout) != null)
                data.layout = ko.unwrap(value.layout);

            return data;
        }

    }

}
