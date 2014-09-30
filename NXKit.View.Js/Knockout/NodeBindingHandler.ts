/// <reference path="../Util.ts" />

module NXKit.View.Knockout {

    export class NodeBindingHandler
        implements KnockoutBindingHandler {

        public init(element: HTMLElement, valueAccessor: () => any, allBindingsAccessor: KnockoutAllBindingsAccessor, viewModel: any, bindingContext: KnockoutBindingContext): void {
            var value = NodeBindingHandler.ConvertValueAccessor(element, valueAccessor, viewModel, bindingContext);
            if (value == null)
                return;
            
            ko.bindingHandlers.template.init(
                element,
                () => value,
                allBindingsAccessor,
                viewModel,
                bindingContext);
        }

        public update(element: HTMLElement, valueAccessor: () => any, allBindingsAccessor: KnockoutAllBindingsAccessor, viewModel: any, bindingContext: KnockoutBindingContext): void {
            var value = NodeBindingHandler.ConvertValueAccessor(element, valueAccessor, viewModel, bindingContext);
            if (value == null)
                return;

            ko.bindingHandlers.template.update(
                element,
                () => value,
                allBindingsAccessor,
                viewModel,
                bindingContext);
        }

        /**
          * Converts the given value accessor into a value accessor compatible with the default template implementation.
          */
        static ConvertValueAccessor(element: HTMLElement, valueAccessor: () => any, viewModel: any, bindingContext: KnockoutBindingContext): any {

            // determine bound node
            var node = <Node>valueAccessor() || <Node>viewModel;
            var name = ko.observable<string>('NXKit.View.Loading');
            if (node == null) {
                return {
                    data: <any>null,
                    name: name,
                };
            }

            // parse JSON and ignore errors
            var json = (s: string) => {
                try {
                    return JSON.parse(s);
                } catch (e) {
                    console.error(e);
                    return null;
                }
            }

            // node specifies required modules
            var modulesProperty = node.Property('NXKit.View.Js.ViewModule', 'Require');
            var modules = modulesProperty != null ? (<string[]>modulesProperty.Value.peek() || []) : [];

            // wait for required modules
            NXKit.require(modules, (...deps: any[]) => {

                // search dependencies in reverse, so that overloads can come after
                for (var i = deps.length - 1; i >= 0; i--) {

                    // dependency must be HTML tag
                    var host = deps[i];
                    if (host instanceof HTMLElement) {

                        // search script elements from bottom up, so that overloads can come after
                        var elements = $(host).find('script[type="text/html"]').get().reverse();
                        for (var j in elements) {
                            var html = $(elements[j]);
                            var data = html.data('nx-node-view-data');
                            if (data == null) {
                                var attr = html.attr('data-nx-node-view');
                                if (attr) {
                                    // cache data result
                                    data = html.data('nx-node-view-data', json(attr)).data('nx-node-view-data');
                                }
                            }

                            // match data to node
                            if (data != null) {
                                if (node.Match(data)) {

                                    // generate unique id if not available
                                    var id = html.attr('id');
                                    if (id == null || id == '') {
                                        html.attr('id', id = 'NXKit.View__' + Util.GenerateGuid().replace(/-/g, ''));
                                    }

                                    // successful update of template
                                    if (name() != id) {
                                        name(id);
                                    }

                                    return;
                                }
                            }
                        }
                    }
                }

                // display unknown template
                name('NXKit.View.Unknown');
            });

            // template object with dynamic name
            return {
                data: node,
                name: name,
            };
        }

    }

    ko.bindingHandlers['nx_node'] = new NodeBindingHandler();
    ko.virtualElements.allowedBindings['nx_node'] = true;

}
