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
            data = super.ParseTemplateBinding(valueAccessor, viewModel, bindingContext, data);

            // extract data to be used to search for a template
            var value = valueAccessor();

            // value is itself a node
            if (value != null &&
                ko.unwrap(value) instanceof Node) {
                data.node = (<Node>ko.unwrap(value)).Type;
                return data;
            }

            // specified node value
            if (value != null &&
                value.node != null &&
                ko.unwrap(value.node) instanceof Node)
                data.node = (<Node>ko.unwrap(value.node)).Type;

            // grab node from viewmode if present
            if (data.node == null)
                if (viewModel instanceof Node)
                    data.node = (<Node>viewModel).Type;

            // node specified as string
            if (value != null &&
                value.node != null &&
                typeof value.node === 'string')
                data.node = <string>value.node;

            // specified data type
            if (value != null &&
                value.type != null)
                data.type = ko.unwrap(value.type);

            // extract layout binding
            if (value != null &&
                value.layout != null &&
                ko.unwrap(value.layout) != null)
                data.layout = ko.unwrap(value.layout);

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
