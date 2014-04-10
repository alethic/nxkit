/// <reference path="XFormsNodeViewModel.ts" />

module NXKit.Web.XForms {

    export class RangeViewModel
        extends NXKit.Web.XForms.XFormsNodeViewModel {

        constructor(context: KnockoutBindingContext, node: Node) {
            super(context, node);
        }

        public get Start(): KnockoutObservable<number> {
            return this.Node.Property('NXKit.XForms.Range', 'Start').ValueAsNumber;
        }

        public get End(): KnockoutObservable<number> {
            return this.Node.Property('NXKit.XForms.Range', 'End').ValueAsNumber;
        }

        public get Step(): KnockoutObservable<number> {
            return this.Node.Property('NXKit.XForms.Range', 'Step').ValueAsNumber;
        }

    }

}
