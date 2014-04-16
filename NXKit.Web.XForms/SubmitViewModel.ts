/// <reference path="XFormsNodeViewModel.ts" />

module NXKit.Web.XForms {

    export class SubmitViewModel
        extends NXKit.Web.XForms.XFormsNodeViewModel {

        constructor(context: KnockoutBindingContext, node: Node) {
            super(context, node);
        }

        public Activate() {
            this.Node.Invoke('NXKit.DOMEvents.INXEventTarget', 'DispatchEvent', {
                type: 'DOMActivate'
            });
        }

    }

}
