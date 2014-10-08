/// <reference path="LayoutNodeViewModel.ts" />

module NXKit.View.XForms.Layout {

    export class ListViewModel
        extends LayoutNodeViewModel {

        constructor(context: KnockoutBindingContext, node: Node) {
            super(context, node);
        }

        public get Items(): Node[] {
            return ko.utils.arrayFilter(this.Contents, _ => _.Name == '{http://schemas.nxkit.org/2014/xforms-layout}item');
        }

    }

}