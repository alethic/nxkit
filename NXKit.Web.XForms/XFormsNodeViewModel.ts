NXKit.Web.ViewModelUtil.ControlNodeTypes.push(
    'NXKit.XForms.Input',
    'NXKit.XForms.Range',
    'NXKit.XForms.Select1',
    'NXKit.XForms.Select',
    'NXKit.XForms.TextArea');

NXKit.Web.ViewModelUtil.MetadataNodeTypes.push(
    'NXKit.XForms.Label',
    'NXKit.XForms.Help',
    'NXKit.XForms.Hint',
    'NXKit.XForms.Alert');

NXKit.Web.ViewModelUtil.TransparentNodeTypes.push(
    'NXKit.XForms.Repeat',
    'NXKit.XForms.RepeatItem');

module NXKit.Web.XForms {

    export class XFormsNodeViewModel
        extends NXKit.Web.NodeViewModel {

        constructor(context: KnockoutBindingContext, node: Node) {
            super(context, node);
        }

        public get Value(): KnockoutObservable<any> {
            return ViewModelUtil.GetValue(this.Node);
        }

        public get ValueAsString(): KnockoutObservable<string> {
            return ViewModelUtil.GetValueAsString(this.Node);
        }

        public get ValueAsBoolean(): KnockoutObservable<boolean> {
            return ViewModelUtil.GetValueAsBoolean(this.Node);
        }

        public get ValueAsNumber(): KnockoutObservable<number> {
            return ViewModelUtil.GetValueAsNumber(this.Node);
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
            return ViewModelUtil.GetType(this.Node);
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

        public get Help(): Node {
            return ViewModelUtil.GetHelpNode(this.Node);
        }

        public get Hint(): Node {
            return ViewModelUtil.GetHintNode(this.Node);
        }

        public get Alert(): Node {
            return ViewModelUtil.GetAlertNode(this.Node);
        }

    }

}