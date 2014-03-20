/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="../VisualLayoutManager.ts" />

module NXKit.Web.XForms {

    export class GroupLevel1LayoutManager
        extends VisualLayoutManager {

        constructor(context: KnockoutBindingContext, viewModel: GroupViewModel) {
            super(context, viewModel.Visual);

            if (!(viewModel instanceof GroupViewModel))
                throw new Error('viewModel: null');
        }

        get Layout(): string {
            return "double";
        }

    }

}
