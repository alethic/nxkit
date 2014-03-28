module NXKit.Web.XForms {

    export class XFormsNodeViewModel
        extends NXKit.Web.NodeViewModel {

        public static ControlNodeTypes = [
            'NXKit.XForms.Input',
            'NXKit.XForms.Range',
            'NXKit.XForms.Select1',
            'NXKit.XForms.SelectElement',
            'NXKit.XForms.TextAreaElement',
        ];

        public static MetadataNodeTypes = [
            'NXKit.XForms.Label',
            'NXKit.XForms.Help',
            'NXKit.XForms.Hint',
            'NXKit.XForms.Alert',
        ];

        public static TransparentNodeTypes = [
            'NXKit.XForms.RepeatElement',
            'NXKit.XForms.RepeatItemElement',
        ];

        constructor(context: KnockoutBindingContext, node: Node) {
            super(context, node);
        }

        public get Value(): KnockoutObservable<any> {
            return Utils.GetValue(this.Node);
        }

        public get ValueAsString(): KnockoutObservable<string> {
            return Utils.GetValueAsString(this.Node);
        }

        public get ValueAsBoolean(): KnockoutObservable<boolean> {
            return Utils.GetValueAsBoolean(this.Node);
        }

        public get ValueAsNumber(): KnockoutObservable<number> {
            return Utils.GetValueAsNumber(this.Node);
        }

        public get Relevant(): KnockoutObservable<boolean> {
            return Utils.GetRelevant(this.Node);
        }

        public get ReadOnly(): KnockoutObservable<boolean> {
            return Utils.GetReadOnly(this.Node);
        }

        public get Required(): KnockoutObservable<boolean> {
            return Utils.GetRequired(this.Node);
        }

        public get Valid(): KnockoutObservable<boolean> {
            return Utils.GetValid(this.Node);
        }

        public get Type(): KnockoutObservable<string> {
            return Utils.GetType(this.Node);
        }

        public get Appearance(): KnockoutObservable<string> {
            return Utils.GetAppearance(this.Node);
        }

        public get Label(): Node {
            return Utils.GetLabel(this.Node);
        }

        public get Help(): Node {
            return Utils.GetHelp(this.Node);
        }

        public get Hint(): Node {
            return Utils.GetHint(this.Node);
        }

        public get Alert(): Node {
            return Utils.GetAlert(this.Node);
        }

        public get Contents(): Node[] {
            return Utils.GetContents(this.Node);
        }

    }

}