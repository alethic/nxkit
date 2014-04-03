/// <reference path="../Util.ts" />

module NXKit.Web.Knockout {

    export class TemplateBindingHandler
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

            // value itself is a node
            if (value != null &&
                ko.unwrap(value) instanceof Node)
                return ko.unwrap(value);

            // specified data value
            if (value != null &&
                value.data != null)
                return ko.unwrap(value.data);

            // specified node value
            if (value != null &&
                value.node != null &&
                ko.unwrap(value.node) instanceof Node)
                return ko.unwrap(value.node);

            // default to existing context
            return null;
        }

        /**
         * Extracts template index data from the given binding information.
         */
        static GetTemplateBinding(valueAccessor: () => any, viewModel: any, bindingContext: KnockoutBindingContext): any {
            return NXKit.Web.Util.GetLayoutManager(bindingContext).ParseTemplateBinding(valueAccessor, viewModel, bindingContext, {});
        }

        /**
         * Determines the named template from the given extracted data and context.
         */
        static GetTemplateName(bindingContext: KnockoutBindingContext, data: any): KnockoutObservable<string> {
            return NXKit.Web.Util.GetLayoutManager(bindingContext).GetTemplateName(data);
        }

    }

    ko.bindingHandlers['nxkit_template'] = new TemplateBindingHandler();
    ko.virtualElements.allowedBindings['nxkit_template'] = true;

}
