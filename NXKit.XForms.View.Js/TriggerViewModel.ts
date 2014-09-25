/// <reference path="XFormsNodeViewModel.ts" />

module NXKit.View.XForms {

    export class TriggerViewModel
        extends NXKit.View.XForms.XFormsNodeViewModel {

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
