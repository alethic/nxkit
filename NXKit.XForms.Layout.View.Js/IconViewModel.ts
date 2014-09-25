/// <reference path="LayoutNodeViewModel.ts" />

module NXKit.View.XForms.Layout {

    export class IconViewModel
        extends LayoutNodeViewModel {

        constructor(context: KnockoutBindingContext, node: Node) {
            super(context, node);
        }

        public get Name(): KnockoutObservable<string> {
            var p = this.Node.Property('NXKit.XForms.Layout.Icon', 'Name');
            return p != null ? p.ValueAsString : null;
        }

    }

}