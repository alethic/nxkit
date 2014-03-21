/// <reference path="XFormsVisualViewModel.ts" />

module NXKit.Web.XForms {

    export class InputViewModel
        extends NXKit.Web.XForms.XFormsVisualViewModel {

        constructor(context: KnockoutBindingContext, visual: Visual) {
            super(context, visual);
        }

        get ShowLabel(): boolean {
            return !NXKit.Web.LayoutOptions.GetArgs(this.Context).SuppressLabel;
        }

    }

}