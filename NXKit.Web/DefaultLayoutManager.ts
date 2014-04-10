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
        public ParseTemplateBinding(valueAccessor: KnockoutObservable<any>, viewModel: any, bindingContext: KnockoutBindingContext, data: any): any {
            try {
                data = super.ParseTemplateBinding(valueAccessor, viewModel, bindingContext, data);

                // extract data to be used to search for a template
                var value = valueAccessor();

                // name specified
                if (value != null &&
                    value.name != null) {
                    var name = <string>ko.unwrap(value.name);
                    if (name != null) {
                        data.name = name;
                    }
                }

                // node
                if (data.name == null &&
                    value != null &&
                    ko.unwrap(value) instanceof Node) {
                    var node = <Node>ko.unwrap(value);
                    if (node.Name != null) {
                        data.name = node.Name;
                    }
                }

                // value.node
                if (data.name == null &&
                    value != null &&
                    value.node != null &&
                    ko.unwrap(value.node) instanceof Node) {
                    var node = <Node>ko.unwrap(value.node);
                    if (node.Name != null) {
                        data.name = node.Name;
                    }
                }

                // viewModel with template
                if (data.name == null &&
                    viewModel instanceof Node) {
                    var node = <Node>viewModel;
                    if (node.Name != null) {
                        data.name = node.Name;
                    }
                }

                // specified data type
                if (value != null &&
                    value.type != null) {
                    data.type = ko.unwrap(value.type);
                }

                // extract layout binding
                if (value != null &&
                    value.layout != null &&
                    ko.unwrap(value.layout) != null) {
                    data.layout = ko.unwrap(value.layout);
                }

                return data;
            } catch (ex) {
                ex.message = "DefaultLayoutManager.ParseTemplateBinding()" + '\nMessage: ' + ex.message;
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
