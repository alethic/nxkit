/// <reference path="../Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="VisualViewModel.ts" />

module NXKit.Web.XForms {

    export class LabelViewModel
        extends NXKit.Web.XForms.VisualViewModel {

        constructor(context: KnockoutBindingContext, visual: Visual) {
            super(context, visual);
        }

        get Text(): KnockoutComputed<string> {
            return this.ValueAsString;
        }

    }

}