/// <reference path="LayoutNodeViewModel.ts" />

module NXKit.Web.XForms.Layout {

    export class TableCellViewModel
        extends LayoutNodeViewModel {

        constructor(context: KnockoutBindingContext, node: Node) {
            super(context, node);
        }

        public Activate() {
            var self = this;

            // ensure property changes or non-focus events flush first
            setTimeout(() =>
                self.Node.Invoke('NXKit.DOMEvents.EventTarget', 'Dispatch', {
                    type: 'DOMActivate'
                }), 50);
        }

    }

}
