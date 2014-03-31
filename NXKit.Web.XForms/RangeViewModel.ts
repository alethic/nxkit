/// <reference path="XFormsNodeViewModel.ts" />

module NXKit.Web.XForms {

    export class RangeViewModel
        extends NXKit.Web.XForms.XFormsNodeViewModel {

        constructor(context: KnockoutBindingContext, node: Node) {
            super(context, node);
        }

        public get Start(): KnockoutObservable<number> {
            return this.Node.ValueAsNumber('NXKit.XForms.Range', 'Start');
        }

        public get End(): KnockoutObservable<number> {
            return this.Node.ValueAsNumber('NXKit.XForms.Range', 'End');
        }

        public get Step(): KnockoutObservable<number> {
            return this.Node.ValueAsNumber('NXKit.XForms.Range', 'Step');
        }

    }

}
