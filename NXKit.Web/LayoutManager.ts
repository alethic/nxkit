/// <reference path="Node.ts" />
/// <reference path="Utils.ts" />

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
        public ParseTemplateBinding(valueAccessor: () => any, viewModel: any, bindingContext: KnockoutBindingContext, data: any): any {
            return this.Parent != null ? this.Parent.ParseTemplateBinding(valueAccessor, viewModel, bindingContext, data || {}) : data || {};
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
        public GetTemplateNodeData(node: HTMLElement): any {
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
        public TemplatePredicate(node: HTMLElement, data: any): boolean {
            var d1 = this.GetTemplateNodeData(node);
            var d2 = data;
            return Utils.DeepEquals(d1, d2);
        }

        /**
         * Tests each given node against the predicate function.
         */
        private TemplateFilter(nodes: HTMLElement[], data: any): HTMLElement[] {
            var self = this;
            return nodes.filter(_ => self.TemplatePredicate(_, data));
        }

        /**
         * Gets the appropriate template for the given data.
         */
        public GetTemplate(data: any): KnockoutObservable<HTMLElement> {
            return ko.computed<HTMLElement>(() => this.TemplateFilter(this.GetTemplates(), data)[0] || this.GetUnknownTemplate(data));
        }

        /**
         * Gets the template that applies for the given data.
         */
        public GetTemplateName(data: any): KnockoutObservable<string> {
            return ko.computed<string>(() => {
                var node = this.GetTemplate(data)();
                if (node == null)
                    throw new Error('GetTemplate: no template located');

                // ensure the node has a valid and unique id
                if (node.id == '')
                    node.id = 'NXKit.Web__' + Utils.GenerateGuid().replace(/-/g, '');

                // caller expects the id
                return node.id;
            });
        }

    }

}
