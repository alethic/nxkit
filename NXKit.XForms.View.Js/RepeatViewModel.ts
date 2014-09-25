/// <reference path="XFormsNodeViewModel.ts" />

module NXKit.Web.XForms {

    export class RepeatViewModel
        extends NXKit.Web.XForms.XFormsNodeViewModel {

        private _items: KnockoutComputed<Node[]>;

        constructor(context: KnockoutBindingContext, node: Node) {
            super(context, node);
        }

    }

}
