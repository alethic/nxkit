/// <reference path="Visual.ts" />
/// <reference path="LayoutManager.ts" />

module NXKit.Web {

    export class DefaultLayoutManager
        extends LayoutManager {

        constructor(context: KnockoutBindingContext) {
            super(context);
        }

        /**
         * Parses the given template binding information for a data structure to pass to the template lookup procedures.
         */
        public ParseTemplateBinding(valueAccessor: KnockoutObservable<any>, viewModel: any, bindingContext: KnockoutBindingContext, data: any): any {
            data = super.ParseTemplateBinding(valueAccessor, viewModel, bindingContext, data);

            // extract data to be used to search for a template
            var value = valueAccessor();

            // value is itself a visual
            if (value != null &&
                ko.unwrap(value) instanceof Visual) {
                data.visual = (<Visual>ko.unwrap(value)).Type;
                return data;
            }

            // specified visual value
            if (value != null &&
                value.visual != null &&
                ko.unwrap(value.visual) instanceof Visual)
                data.visual = (<Visual>ko.unwrap(value.visual)).Type;

            if (data.visual == null)
                if (viewModel instanceof Visual)
                    data.visual = (<Visual>viewModel).Type;

            // specified data type
            if (value != null &&
                value.type != null)
                data.type = ko.unwrap(value.type);

            return data;
        }

        /**
         * Gets all available templates currently in the document.
         */
        public GetLocalTemplates(): HTMLElement[] {
            return <HTMLElement[]>$('script[type="text/html"]').toArray();
        }

    }

}
