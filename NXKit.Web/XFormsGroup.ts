/// <reference path="Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="View.ts" />
/// <reference path="XForms.ts" />

module NXKit.Web.XForms {

    export class GroupViewModel extends VisualViewModel {

        constructor(context: KnockoutBindingContext, visual: Visual) {
            super(context, visual);
            var self = this;
        }

        get Label(): Visual {
            return VisualViewModel.GetLabel(this.Visual);
        }

        get Help(): Visual {
            return VisualViewModel.GetHelp(this.Visual);
        }

        get Contents(): KnockoutObservableArray<Visual> {
            return VisualViewModel.GetRenderableContents(this.Visual);
        }

    }

}