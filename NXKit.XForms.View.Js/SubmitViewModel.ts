/// <reference path="XFormsNodeViewModel.ts" />

module NXKit.View.XForms {

    export class SubmitViewModel
        extends NXKit.View.XForms.XFormsNodeViewModel {

        constructor(context: KnockoutBindingContext, node: Node) {
            super(context, node);
        }

        public Activate() {
            this.Node.Invoke('NXKit.DOMEvents.EventTarget', 'Dispatch', {
                type: 'DOMActivate'
            });
        }

    }

}
