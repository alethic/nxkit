/// <reference path="XFormsNodeViewModel.ts" />

module NXKit.View.XForms {

    export class OutputViewModel
        extends NXKit.View.XForms.XFormsNodeViewModel {

        constructor(context: KnockoutBindingContext, node: Node) {
            super(context, node);
        }

        get Text(): KnockoutObservable<string> {
            return ko.computed(() => this.ValueAsString() || this.Node.Property('NXKit.XForms.Output', 'Value').ValueAsString());
        }

    }

}