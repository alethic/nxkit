/// <reference path="XFormsNodeViewModel.ts" />

module NXKit.View.XForms {

    export class GroupViewModel
        extends XFormsNodeViewModel {

        constructor(context: KnockoutBindingContext, node: Node) {
            super(context, node);
        }

        public get LabelAppearance(): KnockoutObservable<string> {
            return this.Label != null ? ViewModelUtil.GetAppearance(this.Label) : null;
        }

        public get Count(): number {
            return this.Contents.length;
        }

        public get CountEnabled(): number {
            return ko.utils.arrayFilter(this.Contents, _ => ViewModelUtil.GetRelevant(_)()).length;
        }

    }

}
