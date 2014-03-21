module NXKit.Web.XForms {

    export class XFormsVisualViewModel
        extends NXKit.Web.VisualViewModel {

        public static ControlVisualTypes = [
            'NXKit.XForms.XFormsInputVisual',
            'NXKit.XForms.XFormsRangeVisual',
            'NXKit.XForms.XFormsSelect1Visual',
            'NXKit.XForms.XFormsSelectVisual',
        ];

        public static MetadataVisualTypes = [
            'NXKit.XForms.XFormsLabelVisual',
            'NXKit.XForms.XFormsHelpVisual',
            'NXKit.XForms.XFormsHintVisual',
            'NXKit.XForms.XFormsAlertVisual',
        ];

        public static TransparentVisualTypes = [
            'NXKit.XForms.XFormsRepeatVisual',
            'NXKit.XForms.XFormsRepeatItemVisual',
        ];

        constructor(context: KnockoutBindingContext, visual: Visual) {
            super(context, visual);
        }

        public get Value(): KnockoutComputed<any> {
            return Utils.GetValue(this.Visual);
        }

        public get ValueAsString(): KnockoutComputed<string> {
            return Utils.GetValueAsString(this.Visual);
        }

        public get ValueAsBoolean(): KnockoutComputed<boolean> {
            return Utils.GetValueAsBoolean(this.Visual);
        }

        public get ValueAsNumber(): KnockoutComputed<number> {
            return Utils.GetValueAsNumber(this.Visual);
        }

        public get Relevant(): KnockoutComputed<boolean> {
            return Utils.GetRelevant(this.Visual);
        }

        public get ReadOnly(): KnockoutComputed<boolean> {
            return Utils.GetReadOnly(this.Visual);
        }

        public get Required(): KnockoutComputed<boolean> {
            return Utils.GetRequired(this.Visual);
        }

        public get Valid(): KnockoutComputed<boolean> {
            return Utils.GetValid(this.Visual);
        }

        public get Type(): KnockoutComputed<string> {
            return Utils.GetType(this.Visual);
        }

        public get Appearance(): KnockoutComputed<string> {
            return Utils.GetAppearance(this.Visual);
        }

        public get Label(): Visual {
            return Utils.GetLabel(this.Visual);
        }

        public get Help(): Visual {
            return Utils.GetHelp(this.Visual);
        }

        public get Hint(): Visual {
            return Utils.GetHint(this.Visual);
        }

        public get Contents(): Visual[] {
            return Utils.GetContents(this.Visual);
        }

    }

}