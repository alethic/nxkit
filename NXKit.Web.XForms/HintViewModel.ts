﻿/// <reference path="XFormsVisualViewModel.ts" />

module NXKit.Web.XForms {

    export class HintViewModel
        extends NXKit.Web.XForms.XFormsVisualViewModel {

        constructor(context: KnockoutBindingContext, visual: Visual) {
            super(context, visual);
        }

        get Text(): KnockoutComputed<string> {
            return this.ValueAsString;
        }

    }

}