/// <reference path="Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="Visual.ts" />

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
         * Gets the templates provided by this layout manager for the given data.
         */
        public GetLocalTemplates(data: any): HTMLElement[] {
            return new Array<HTMLElement>();
        }

        /**
         * Gets the set of available templates for the given data.
         */
        public GetTemplates(data: any): HTMLElement[]{
            // append parent templates to local templates
            return this.GetLocalTemplates(data)
                .concat(this.Parent != null ? this.Parent.GetTemplates(data) : new Array<HTMLElement>());
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
         * Gets the appropriate template for the given data.
         */
        public GetTemplate(data: any): HTMLElement {
            return this.GetTemplates(data)[0] || this.GetUnknownTemplate(data);
        }

        /**
         * Gets the template that applies for the given data.
         */
        public GetTemplateName(data: any): string {
            var node = this.GetTemplate(data);
            if (node == null)
                throw new Error('GetTemplate: no template located');

            // ensure the node has a valid and unique id
            if (node.id == '')
                node.id = 'NXKit.Web__' + Utils.GenerateGuid().replace(/-/g, '');

            // caller expects the id
            return node.id;
        }

    }

}
