/// <reference path="LayoutNodeViewModel.ts" />

module NXKit.View.XForms.Layout {

    export class AnchorViewModel
        extends LayoutNodeViewModel {

        constructor(context: KnockoutBindingContext, node: Node) {
            super(context, node);
        }

        public get Href(): KnockoutObservable<string> {
            var p = this.Node.Property('NXKit.XForms.Layout.Anchor', 'Href');
            return p != null ? p.ValueAsString : null;
        }

    }

}