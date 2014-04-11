/// <reference path="Node.ts" />
/// <reference path="Util.ts" />

module NXKit.Web {

    export class LayoutManager {

        private _context: KnockoutBindingContext = null;
        private _parent: KnockoutComputed<LayoutManager> = null;

        constructor(context: KnockoutBindingContext) {
            var self = this;

            if (context == null)
                throw new Error('context: null');

            self._context = context;

            // calculates the parent layout manager
            self._parent = ko.computed<LayoutManager>(() => {
                var l = [self._context.$data].concat(self._context.$parents);
                for (var i in l) {
                    var p = l[i];
                    if (p instanceof LayoutManager)
                        if (p != self)
                            return (<LayoutManager>p);
                }
                return null;
            });
        }

        /**
         * Gets the context inside which this layout manager was created.
         */
        public get Context(): KnockoutBindingContext {
            return this._context;
        }

        /**
         * Gets the parent layout manager.
         */
        public get Parent(): LayoutManager {
            return this._parent();
        }

        /**
         * Parses the given template binding information for a data structure to pass to the template lookup procedures.
         */
        public GetTemplateOptions_(valueAccessor: () => any, viewModel: any, bindingContext: KnockoutBindingContext, options: any): any {
            return this.GetTemplateOptions(valueAccessor, viewModel, bindingContext, options);
        }

        /**
         * Parses the given template binding information for a data structure to pass to the template lookup procedures.
         */
        public GetTemplateOptions(valueAccessor: () => any, viewModel: any, bindingContext: KnockoutBindingContext, options: any): any {
            return this.Parent != null ? this.Parent.GetTemplateOptions(valueAccessor, viewModel, bindingContext, options || {}) : options || {};
        }

        /**
         * Gets the templates provided by this layout manager for the given data.
         */
        public GetLocalTemplates(): HTMLElement[] {
            return new Array<HTMLElement>();
        }

        /**
         * Gets the set of available templates for the given data.
         */
        public GetTemplates(): HTMLElement[] {
            // append parent templates to local templates
            return this.GetLocalTemplates()
                .concat(this.Parent != null ? this.Parent.GetTemplates() : new Array<HTMLElement>());
        }

        /**
         * Helper method for resolving a node given a Knockout context.
         */
        public GetNode(valueAccessor: KnockoutObservable<any>, viewModel: any, bindingContext: KnockoutBindingContext): Node {
            // extract data to be used to search for a template
            var value = ko.unwrap(valueAccessor());

            // node specified as existing view model
            if (viewModel != null) {
                var viewModel_ = ko.unwrap(viewModel);
                if (viewModel_ instanceof Node) {
                    return viewModel_;
                }
            }

            // node specified explicitely as value
            if (value != null &&
                value instanceof Node) {
                return value;
            }

            // node specified as value.node
            if (value != null &&
                value.node != null) {
                return ko.unwrap(value.node);
            }
        }

        /**
         * Gets the fallback template for the given data.
         */
        public GetUnknownTemplate(data: any): HTMLElement {
            return $('<script />', {
                'type': 'text/html',
                'text': '<span class="ui red label">' + JSON.stringify(data) + '</span>',
            }).appendTo('body')[0];
        }

        /**
         * Extracts a JSON representation of a template node's data-nxkit bindings.
         */
        public static GetTemplateNodeData(node: HTMLElement): any {
            // check whether we've already cached the node data
            var d = $(node).data('nxkit');
            if (d != null)
                return d;

            // begin collecting data from node attributes
            d = {};
            for (var i = 0; i < node.attributes.length; i++) {
                var a = node.attributes.item(i);
                if (a.nodeName.indexOf('data-nxkit-') == 0) {
                    var n = a.nodeName.substring(11);
                    d[n] = $(node).data('nxkit-' + n);
                }
            }

            // store new data on the node, and return
            return $(node).data('nxkit', d).data('nxkit');
        }

        /**
         * Tests whether a template node matches the given data.
         */
        public static TemplatePredicate(node: HTMLElement, opts: any): boolean {
            return Log.Group('TemplatePredicate', () => {
                // data has no properties
                if (opts != null &&
                    Object.getOwnPropertyNames(opts).length == 0) {
                    console.debug('opts: empty');
                    return false;
                }

                // extract data-nxkit attributes from template node
                var tmpl = LayoutManager.GetTemplateNodeData(node);

                // template has no properties, should not correspond with anything
                if (Object.getOwnPropertyNames(tmpl).length == 0) {
                    console.debug('tmpl: empty');
                    return false;
                }

                console.dir({
                    tmpl: tmpl,
                    opts: opts,
                });

                return Util.DeepEquals(tmpl, opts, (a, b) => {
                    return (a === '*' || b === '*') ? true : null;
                });
            });
        }

        /**
         * Tests each given node against the predicate function.
         */
        private static TemplateFilter(nodes: HTMLElement[], data: any): HTMLElement[] {
            return nodes.filter(_ => LayoutManager.TemplatePredicate(_, data));
        }

        /**
         * Gets the appropriate template for the given data.
         */
        public GetTemplate(data: any): HTMLElement {
            return LayoutManager.TemplateFilter(this.GetTemplates(), data)[0] || this.GetUnknownTemplate(data);
        }

        /**
         * Gets the template that applies for the given data.
         */
        public GetTemplateName(data: any): string {
            return Log.Group('LayoutManager.GetTemplateName', () => {
                var node = this.GetTemplate(data);
                if (node == null)
                    throw new Error('LayoutManager.GetTemplate: no template located');

                // ensure the node has a valid and unique id
                if (node.id == '')
                    node.id = 'NXKit.Web__' + Util.GenerateGuid().replace(/-/g, '');

                // log result
                console.dir({
                    id: node.id,
                    data: LayoutManager.GetTemplateNodeData(node)
                });

                // caller expects the id
                return node.id;
            });
        }

    }

}
