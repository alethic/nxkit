/// <reference path="XFormsNodeViewModel.ts" />

module NXKit.Web.XForms {

    export class OutputViewModel
        extends NXKit.Web.XForms.XFormsNodeViewModel {

        constructor(context: KnockoutBindingContext, node: Node) {
            super(context, node);
        }

        get Text(): KnockoutObservable<string> {
            return this.ValueAsString;
        }

    }

}