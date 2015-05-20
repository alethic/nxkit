module NXKit.View.XForms {

    export class XFormsNodeViewModel
        extends NXKit.View.NodeViewModel {

        constructor(context: KnockoutBindingContext, node: Node) {
            super(context, node);
        }

        public get Value(): KnockoutObservable<any> {
            return ViewModelUtil.GetDataValue(this.Node);
        }

        public get ValueAsString(): KnockoutObservable<string> {
            return ViewModelUtil.GetDataValueAsString(this.Node);
        }

        public get ValueAsBoolean(): KnockoutObservable<boolean> {
            return ViewModelUtil.GetDataValueAsBoolean(this.Node);
        }

        public get ValueAsNumber(): KnockoutObservable<number> {
            return ViewModelUtil.GetDataValueAsNumber(this.Node);
        }

        public get Relevant(): KnockoutObservable<boolean> {
            return ViewModelUtil.GetRelevant(this.Node);
        }

        public get ReadOnly(): KnockoutObservable<boolean> {
            return ViewModelUtil.GetReadOnly(this.Node);
        }

        public get Required(): KnockoutObservable<boolean> {
            return ViewModelUtil.GetRequired(this.Node);
        }

        public get Valid(): KnockoutObservable<boolean> {
            return ViewModelUtil.GetValid(this.Node);
        }

        public get Type(): KnockoutObservable<string> {
            return ViewModelUtil.GetDataType(this.Node);
        }

        public get Appearance(): KnockoutObservable<string> {
            return ViewModelUtil.GetAppearance(this.Node);
        }

        public get Label(): Node {
            try {
                return ViewModelUtil.GetLabelNode(this.Node);
            } catch (ex) {
                ex.message = 'XFormsNodeViewModel.Label' + '"\nMessage: ' + ex.message;
                throw ex;
            }
        }

        public get LabelAppearance(): KnockoutObservable<string> {
            return this.Label != null ? ViewModelUtil.GetAppearance(this.Label) : null;
        }

        public get Help(): Node {
            return ViewModelUtil.GetHelpNode(this.Node);
        }

        public get Hint(): Node {
            return ViewModelUtil.GetHintNode(this.Node);
        }

        public get Alert(): Node {
            return ViewModelUtil.GetAlertNode(this.Node);
        }

        public get CountEnabled(): KnockoutObservable<number> {
            var self = this;
            return ko.pureComputed(() => ko.utils.arrayFilter(self.Contents(), _ => ViewModelUtil.GetRelevant(_)()).length);
        }

    }

}
