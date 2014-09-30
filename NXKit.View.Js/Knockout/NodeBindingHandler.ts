/// <reference path="../Util.ts" />

module NXKit.View.Knockout {

    export class NodeBindingHandler
        implements KnockoutBindingHandler {

        public init(element: HTMLElement, valueAccessor: () => any, allBindingsAccessor: KnockoutAllBindingsAccessor, viewModel: any, bindingContext: KnockoutBindingContext): void {
            return ko.bindingHandlers.template.init(
                element,
                NodeBindingHandler.ConvertValueAccessor(valueAccessor, viewModel, bindingContext),
                allBindingsAccessor,
                viewModel,
                bindingContext);
        }

        public update(element: HTMLElement, valueAccessor: () => any, allBindingsAccessor: KnockoutAllBindingsAccessor, viewModel: any, bindingContext: KnockoutBindingContext): void {
            return ko.bindingHandlers.template.update(
                element,
                NodeBindingHandler.ConvertValueAccessor(valueAccessor, viewModel, bindingContext),
                allBindingsAccessor,
                viewModel,
                bindingContext);
        }

        /**
          * Converts the given value accessor into a value accessor compatible with the default template implementation.
          */
        static ConvertValueAccessor(valueAccessor: () => any, viewModel: any, bindingContext: KnockoutBindingContext): () => any {
            var node = <Node>valueAccessor() || <Node>viewModel;
            var name = ko.observable<string>(null);
            if (node == null) {
                return;
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
            var modules = modulesProperty != null ? (<string[]>modulesProperty.Value() || []) : [];

            // wait for required modules
            NXKit.require(modules, (...deps: any[]) => {

                // search dependencies in reverse, so that overloads can come after
                for (var i = deps.length - 1; i >= 0; i--) {

                    // dependency must be HTML tag
                    var host = deps[i];
                    if (host instanceof HTMLElement) {

                        // search script elements from bottom up, so that overloads can come after
                        var elements = $(host).find('script[type="text/html"]').get().reverse();
                        for (var i in elements) {
                            var html = $(elements[i]);
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
                                    name(id);
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
            return () => ({
                data: node,
                name: name,
            });
        }

    }

    ko.bindingHandlers['nx_node'] = new NodeBindingHandler();
    ko.virtualElements.allowedBindings['nx_node'] = true;

}
