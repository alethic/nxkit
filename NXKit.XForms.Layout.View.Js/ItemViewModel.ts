/// <reference path="LayoutNodeViewModel.ts" />

module NXKit.View.XForms.Layout {

    export class ItemViewModel
        extends LayoutNodeViewModel {

        constructor(context: KnockoutBindingContext, node: Node) {
            super(context, node);
        }

        public get LabelAppearance(): KnockoutObservable<string> {
            return this.Label != null ? ViewModelUtil.GetAppearance(this.Label) : null;
        }

        public get Count(): KnockoutObservable<number> {
            var self = this;
            return ko.pureComputed(() => self.Contents().length);
        }

        public get CountEnabled(): KnockoutObservable<number> {
            var self = this;
            return ko.pureComputed(() => ko.utils.arrayFilter(self.Contents(), _ => ViewModelUtil.GetRelevant(_)()).length);
        }

    }

}