/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="../VisualLayoutManager.ts" />

module NXKit.Web.XForms {

    export class GroupLayoutManager
        extends NXKit.Web.VisualLayoutManager {

        private _viewModel: GroupViewModel;

        constructor(context: KnockoutBindingContext, viewModel: GroupViewModel) {
            super(context, viewModel.Visual);

            if (viewModel == null)
                throw new Error('viewModel: null');

            this._viewModel = viewModel;
        }

        get ViewModel(): GroupViewModel {
            return this._viewModel;
        }

        get Level(): number {
            for (var i in this.Context.$parents) {
                var l = this.Context.$parents[i];
                if (i instanceof GroupLayoutManager)
                    return (<GroupLayoutManager>l).Level + 1;
            }

            return 1;
        }

        get Layout(): number {
            var l = this.Level;
            var a = this.ViewModel.Appearance();

            if (l == 1 && a == "full")
                return 1;
            if (l == 1)
                return 1;

            if (l == 2 && a == "full")
                return 1;
            if (l == 2)
                return 1;

            return 1;
        }

    }

}
