/// <reference path="XFormsNodeViewModel.ts" />

module NXKit.Web.XForms {


    export class GroupViewModel
        extends XFormsNodeViewModel {

        private _count: number;

        constructor(context: KnockoutBindingContext, node: Node, count: number) {
            super(context, node);

            this._count = count;
        }

        public get BindingContents(): NXKit.Web.XForms.GroupUtil.ViewModel_.Item[] {
            return GroupUtil.GetItems(this, this.Node, 1);
        }
    }

}