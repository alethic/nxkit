/// <reference path="XFormsNodeViewModel.ts" />

module NXKit.View.XForms {

    export class RepeatViewModel
        extends NXKit.View.XForms.XFormsNodeViewModel {

        private _items: KnockoutComputed<Node[]>;

        constructor(context: KnockoutBindingContext, node: Node) {
            super(context, node);
        }

    }

}
