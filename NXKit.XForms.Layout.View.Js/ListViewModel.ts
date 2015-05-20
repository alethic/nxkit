/// <reference path="LayoutNodeViewModel.ts" />

module NXKit.View.XForms.Layout {

    export class ListViewModel
        extends LayoutNodeViewModel {

        constructor(context: KnockoutBindingContext, node: Node) {
            super(context, node);
        }

        public get Items(): KnockoutObservable<Node[]> {
            var self = this;
            return ko.computed(() => ko.utils.arrayFilter(self.Contents(), _ => _.Name == '{http://schemas.nxkit.org/2014/xforms-layout}item'));
        }

    }

}