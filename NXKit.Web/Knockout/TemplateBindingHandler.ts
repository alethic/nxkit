/// <reference path="../Utils.ts" />

module NXKit.Web.Knockout {

    class TemplateBindingHandler
        implements KnockoutBindingHandler {

        public init(element: HTMLElement, valueAccessor: () => any, allBindingsAccessor: () => any, viewModel: any, bindingContext: KnockoutBindingContext): void {
            return ko.bindingHandlers.template.init(
                element,
                TemplateBindingHandler.ConvertValueAccessor(valueAccessor, viewModel, bindingContext),
                allBindingsAccessor,
                viewModel,
                bindingContext);
        }

        public update(element: HTMLElement, valueAccessor: () => any, allBindingsAccessor: () => any, viewModel: any, bindingContext: KnockoutBindingContext): void {
            return ko.bindingHandlers.template.update(
                element,
                TemplateBindingHandler.ConvertValueAccessor(valueAccessor, viewModel, bindingContext),
                allBindingsAccessor,
                viewModel,
                bindingContext);
        }

        /**
          * Converts the given value accessor into a value accessor compatible with the default template implementation.
          */
        static ConvertValueAccessor(valueAccessor: () => any, viewModel: any, bindingContext: KnockoutBindingContext): () => any {
            return ko.computed(() => {

                var data = this.GetTemplateViewModel(valueAccessor, viewModel, bindingContext);
                var opts = this.GetTemplateBinding(valueAccessor, viewModel, bindingContext);
                var name = this.GetTemplateName(bindingContext, opts);

                return {
                    data: data,
                    name: name,
                };
            })
        }

        /**
         * Gets the recommended view model for the given binding information.
         */
        static GetTemplateViewModel(valueAccessor: () => any, viewModel: any, bindingContext: KnockoutBindingContext): any {
            var value = valueAccessor() || viewModel;

            // value itself is a visual
            if (value != null &&
                ko.unwrap(value) instanceof Visual)
                return ko.unwrap(value);

            // specified data value
            if (value != null &&
                value.data != null)
                return ko.unwrap(value.data);

            // specified visual value
            if (value != null &&
                value.visual != null &&
                ko.unwrap(value.visual) instanceof Visual)
                return ko.unwrap(value.visual);

            // default to existing context
            return null;
        }

        /**
         * Extracts template index data from the given binding information.
         */
        static GetTemplateBinding(valueAccessor: () => any, viewModel: any, bindingContext: KnockoutBindingContext): any {
            return NXKit.Web.Utils.GetLayoutManager(bindingContext).ParseTemplateBinding(valueAccessor, viewModel, bindingContext, {});
        }

        /**
         * Determines the named template from the given extracted data and context.
         */
        static GetTemplateName(bindingContext: KnockoutBindingContext, data: any): string {
            return NXKit.Web.Utils.GetLayoutManager(bindingContext).GetTemplateName(data);
        }

    }

    ko.bindingHandlers['nxkit_template'] = new TemplateBindingHandler();
    ko.virtualElements.allowedBindings['nxkit_template'] = true;

}
