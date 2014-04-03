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

                // template specified
                if (value != null &&
                    value.template != null) {
                    var tmpl = <string>ko.unwrap(value.template);
                    if (tmpl != null) {
                        data.template = tmpl;
                    }
                }

                // value with template
                if (data.template == null &&
                    data.node == null &&
                    value != null &&
                    ko.unwrap(value) instanceof Node) {
                    var node = <Node>ko.unwrap(value);
                    var intf = node.Interfaces['NXKit.Web.ITemplate'];
                    if (intf != null) {
                        var prop = intf.Properties['Name'];
                        if (prop != null) {
                            var tmpl = prop.ValueAsString();
                            if (tmpl != null) {
                                data.template = tmpl;
                            }
                        }
                    }
                }

                // value
                if (data.template == null &&
                    data.node == null &&
                    value != null &&
                    ko.unwrap(value) instanceof Node) {
                    var node = <Node>ko.unwrap(value);
                    if (node.Type != null) {
                        data.node = node.Type;
                    }
                }

                // value.node with template
                if (data.template == null &&
                    data.node == null &&
                    value != null &&
                    value.node != null &&
                    ko.unwrap(value.node) instanceof Node) {
                    var node = <Node>ko.unwrap(value.node);
                    var intf = node.Interfaces['NXKit.Web.ITemplate'];
                    if (intf != null) {
                        var prop = intf.Properties['Name'];
                        if (prop != null) {
                            var tmpl = prop.ValueAsString();
                            if (tmpl != null) {
                                data.template = tmpl;
                            }
                        }
                    }
                }

                // value.node
                if (data.template == null &&
                    data.node == null &&
                    value != null &&
                    value.node != null &&
                    ko.unwrap(value.node) instanceof Node) {
                    var node = <Node>ko.unwrap(value.node);
                    if (node.Type != null) {
                        data.node = node.Type;
                    }
                }

                // viewModel with template
                if (data.template == null &&
                    data.node == null) {
                    if (viewModel instanceof Node) {
                        var node = <Node>viewModel;
                        var intf = node.Interfaces['NXKit.Web.ITemplate'];
                        if (intf != null) {
                            var prop = intf.Properties['Name'];
                            if (prop != null) {
                                var tmpl = prop.ValueAsString();
                                if (tmpl != null) {
                                    data.template = tmpl;
                                }
                            }
                        }
                    }
                }

                // viewModel
                if (data.template == null &&
                    data.node == null) {
                    if (viewModel instanceof Node) {
                        var node = <Node>viewModel;
                        if (node.Type != null) {
                            data.node = node.Type;
                        }
                    }
                }

                // node specified as string
                if (value != null &&
                    value.node != null &&
                    typeof value.node === 'string') {
                    data.node = <string>value.node;
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
