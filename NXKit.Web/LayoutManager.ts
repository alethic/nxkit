/// <reference path="Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="Visual.ts" />

module NXKit.Web {

    export class LayoutManager {

        private _context: KnockoutBindingContext = null;

        constructor(context: KnockoutBindingContext) {
            var self = this;

            if (context == null)
                throw new Error('context: null');

            self._context = context;
        }

        public get Context(): KnockoutBindingContext {
            return this._context;
        }

        public GetTemplates(data: any): HTMLElement[] {
            var l = this._context.$parents;
            for (var i in l) {
                var p = l[i];
                if (p instanceof LayoutManager)
                    if (p != this)
                        return (<LayoutManager>p).GetTemplates(data);
            }

            // return empty array
            return new Array<HTMLElement>();
        }

        public GetTemplate(data: any): string {
            // locate first matching template
            var node = this.GetTemplates(data)[0];

            // no template found, invent an error
            if (node == null) {
                node = $('<script />', {
                    'type': 'text/html',
                    'text': '<span class="ui red label">' + JSON.stringify(data) + '</span>',
                }).appendTo('body')[0];
            }

            // if the node has a missing id
            if (!node.id)
                node.id = 'NXKit.Web__' + Utils.GenerateGuid().replace(/-/g, '');

            return node.id;
        }

    }

}
