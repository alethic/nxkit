/// <reference path="XFormsNodeViewModel.ts" />

module NXKit.Web.XForms {

    export class InputViewModel
        extends NXKit.Web.XForms.XFormsNodeViewModel {

        constructor(context: KnockoutBindingContext, node: Node) {
            super(context, node);
        }

        get ShowLabel(): boolean {
            return !NXKit.Web.LayoutOptions.GetArgs(this.Context).SuppressLabel;
        }

    }

}