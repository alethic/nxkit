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
                                if (NodeBindingHandler.Match(node, data)) {

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

        /**
         * Checks whether the given data matches this node.
         */
        static Match(node: Node, data: any): boolean {

            var test = (a: any, b: any) => {
                if (a == null &&
                    b == null)
                    return true;

                if (typeof a !== typeof b)
                    return false;

                if (typeof a === 'boolean' &&
                    typeof b === 'boolean')
                    return a === b;

                if (typeof a === 'string' &&
                    typeof b === 'string')
                    return a === b;

                if (typeof a === 'number' &&
                    typeof b === 'number')
                    return a === b;

                if (typeof a === 'function' &&
                    typeof b === 'function')
                    return a.toString() === b.toString();

                for (var i in a) {
                    if (!b.hasOwnProperty(i)) {
                        return false;
                    } else {
                        if (!test(a[i], b[i])) {
                            return false;
                        }
                    }
                }
            };

            var work = (data: any) => {

                // check for node name
                if (data.Name) {
                    if (data.Name !== node.Name) {
                        return false;
                    }
                }

                // check for node type
                if (data.Type) {
                    if ((<string>data.Type).toLowerCase() !== node.Type.ToString().toLowerCase()) {
                        return false;
                    }
                }

                // process interface specifications
                for (var name in data) {
                    if ((<string>name).indexOf('.') >= 0) {
                        var dataInterface = data[name];
                        var nodeInterface = node.Interfaces[name];
                        if (nodeInterface) {
                            for (var propertyName in dataInterface) {
                                var dataProperty = dataInterface[propertyName];
                                var nodeProperty = nodeInterface.Properties[propertyName];
                                if (nodeProperty) {
                                    var dataValue = dataProperty;
                                    var nodeValue = nodeProperty.Value();
                                    if (!test(dataValue, nodeValue)) {
                                        return false;
                                    }
                                } else {
                                    return false;
                                }
                            }
                        } else {
                            return false;
                        }
                    }
                }

                return true;
            }

            if (Array.isArray(data)) {
                var datas = (<any[]>data).reverse(); // reverse to search from bottom up
                for (var i in datas) {
                    if (work(datas[i])) {
                        return true;
                    }
                }
            } else if (work(data)) {
                return true;
            }

            return false;
        }

    }

    ko.bindingHandlers['nx_node'] = new NodeBindingHandler();
    ko.virtualElements.allowedBindings['nx_node'] = true;

}
