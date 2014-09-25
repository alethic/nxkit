/// <reference path="XFormsNodeViewModel.ts" />

module NXKit.View.XForms {

    export class AlertViewModel
        extends NXKit.View.XForms.XFormsNodeViewModel {

        constructor(context: KnockoutBindingContext, node: Node) {
            super(context, node);
        }

        get Text(): KnockoutObservable<string> {
            return this.ValueAsString;
        }

    }

}