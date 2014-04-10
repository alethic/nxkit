/// <reference path="Node.ts" />
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
        public GetTemplateOptions(valueAccessor: KnockoutObservable<any>, viewModel: any, bindingContext: KnockoutBindingContext, options: any): any {
            try {
                options = super.GetTemplateOptions(valueAccessor, viewModel, bindingContext, options);
                var node = super.GetNode(valueAccessor, viewModel, bindingContext);
                var value = ko.unwrap(valueAccessor());

                // find element types by element name
                if (node != null &&
                    node.Type == NodeType.Element) {
                    options.name = node.Name;
                }

                // find text node by type
                if (node != null &&
                    node.Type == NodeType.Text) {
                    options.type = NodeType.Text.ToString();
                }

                // name specified explicitely
                if (value != null &&
                    value.name != null) {
                    options.name = <string>ko.unwrap(value.name);
                }

                // extract layout binding
                if (value != null &&
                    value.layout != null) {
                    options.layout = ko.unwrap(value.layout);
                }

                return options;
            } catch (ex) {
                ex.message = "DefaultLayoutManager.GetTemplateOptions()" + '\nMessage: ' + ex.message;
                throw ex;
            }
        }

        /**
         * Gets all available templates currently in the document.
         */
        public GetLocalTemplates(): HTMLElement[] {
            return <HTMLElement[]>$('script[type="text/html"]').toArray();
        }

    }

}
