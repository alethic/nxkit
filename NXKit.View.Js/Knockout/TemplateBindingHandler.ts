/// <reference path="../Util.ts" />

module NXKit.View.Knockout {

    export class TemplateBindingHandler
        implements KnockoutBindingHandler {

        public init(element: HTMLElement, valueAccessor: () => any, allBindingsAccessor: KnockoutAllBindingsAccessor, viewModel: any, bindingContext: KnockoutBindingContext): void {
            return ko.bindingHandlers.template.init(
                element,
                TemplateBindingHandler.ConvertValueAccessor(valueAccessor, viewModel, bindingContext),
                allBindingsAccessor,
                viewModel,
                bindingContext);
        }

        public update(element: HTMLElement, valueAccessor: () => any, allBindingsAccessor: KnockoutAllBindingsAccessor, viewModel: any, bindingContext: KnockoutBindingContext): void {
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
            return () => Log.Group('TemplateBindingHandler.ConvertValueAccessor', () => {
                Log.Object({
                    value: valueAccessor(),
                    viewModel: viewModel,
                });

                // resolve the view model to be passed to the template
                var data = this.GetTemplateViewModel(valueAccessor, viewModel, bindingContext);
                if (data == null ||
                    Object.getOwnPropertyNames(data).length == 0) {
                    throw new Error('unknown viewModel');
                }

                // resolve the options to use to look up the template
                var opts = this.GetTemplateOptions(valueAccessor, viewModel, bindingContext);
                if (opts == null ||
                    Object.getOwnPropertyNames(opts).length == 0) {
                    throw new Error('unknown template options');
                }

                // resolve the template name from the options
                var name = this.GetTemplateName(bindingContext, opts);
                if (name == null) {
                    throw new Error('unknown template');
                }

                Log.Object({
                    data: data,
                    opts: opts,
                    name: name,
                });

                return {
                    data: data,
                    name: name,
                };
            });
        }

        /**
         * Gets the recommended view model for the given binding information.
         */
        static GetTemplateViewModel(valueAccessor: () => any, viewModel: any, bindingContext: KnockoutBindingContext): any {
            var value = valueAccessor();

            // value itself is a node
            if (value != null &&
                ko.unwrap(value) instanceof Node)
                return value;

            // specified data value
            if (value != null &&
                value.data != null)
                return value.data;

            // specified node value
            if (value != null &&
                value.node != null &&
                ko.unwrap(value.node) instanceof Node)
                return value.node;

            // default to existing view model
            return viewModel;
        }

        /**
         * Extracts template index data from the given binding information.
         */
        static GetTemplateOptions(valueAccessor: () => any, viewModel: any, bindingContext: KnockoutBindingContext): any {
            return NXKit.View.Util.GetLayoutManager(bindingContext).GetTemplateOptions_(valueAccessor, viewModel, bindingContext, {});
        }

        /**
         * Determines the named template from the given extracted data and context.
         */
        static GetTemplateName(bindingContext: KnockoutBindingContext, data: any): KnockoutObservable<string> {
            return NXKit.View.Util.GetLayoutManager(bindingContext).GetTemplateName(data);
        }

    }

    ko.bindingHandlers['nxkit_template'] = new TemplateBindingHandler();
    ko.virtualElements.allowedBindings['nxkit_template'] = true;

}
