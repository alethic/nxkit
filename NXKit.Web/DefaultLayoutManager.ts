/// <reference path="Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="LayoutManager.ts" />

module NXKit.Web {

    export class DefaultLayoutManager
        extends LayoutManager {

        constructor(context: KnockoutBindingContext) {
            super(context);
        }

        public GetTemplate(data: any): string {
            var node = $('script').filter(_ => {
                return NXKit.Web.Utils.DeepEquals($(this).data(), data);
            })[0];

            // no template found, invent an error
            if (node == null)
                node = $('<script />', {
                    'id': 'NXKit.Web__Unknown',
                    'type': 'text/html',
                    'text': '<span class="error">no template</span>',
                }).appendTo('body')[0];

            return node.id;
        }

    }

}
