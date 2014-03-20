/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="../VisualLayoutManager.ts" />

module NXKit.Web.XForms {

    export class GroupLayoutManager
        extends NXKit.Web.VisualLayoutManager {

        private _viewModel: GroupViewModel;
        private _groupParent: KnockoutComputed<GroupLayoutManager>;

        constructor(context: KnockoutBindingContext, viewModel: GroupViewModel) {
            super(context, viewModel.Visual);

            if (!(viewModel instanceof GroupViewModel))
                throw new Error('viewModel: null');

            this._viewModel = viewModel;
            this._groupParent = <KnockoutComputed<GroupLayoutManager>>ko.computed(() =>
                ko.utils.arrayFirst(Utils.GetContextItems(this.Context), _ => _ instanceof GroupLayoutManager));
        }

        public ParseTemplateBinding(valueAccessor: KnockoutObservable<any>, viewModel: any, bindingContext: KnockoutBindingContext, data: any): any {
            data = super.ParseTemplateBinding(valueAccessor, viewModel, bindingContext, data);

            // extract level binding
            var value = valueAccessor();
            if (value != null &&
                value.level != null &&
                ko.unwrap(value.level) != null)
                data.level = ko.unwrap(value.level);

            // extract layout binding
            var value = valueAccessor();
            if (value != null &&
                value.layout != null &&
                ko.unwrap(value.layout) != null)
                data.layout = ko.unwrap(value.layout);

            return data;
        }

        get ViewModel(): GroupViewModel {
            return this._viewModel;
        }

        get GroupParent(): GroupLayoutManager {
            return this._groupParent();
        }

        get Level(): number {
            return this.GroupParent != null ? this.GroupParent.Level + 1 : 1;
        }

    }

}
