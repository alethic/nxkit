/// <reference path="XFormsNodeViewModel.ts" />

module NXKit.Web.XForms {

    export class OutputViewModel
        extends NXKit.Web.XForms.XFormsNodeViewModel {

        constructor(context: KnockoutBindingContext, node: Node) {
            super(context, node);
        }

        get Text(): KnockoutObservable<string> {
            return ko.computed(() => this.ValueAsString() || this.Node.Property('NXKit.XForms.Output', 'Value').ValueAsString());
        }

    }

}