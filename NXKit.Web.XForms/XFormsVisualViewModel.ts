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

        get Value(): KnockoutComputed<any> {
            return Utils.GetValue(this.Visual);
        }

        get ValueAsString(): KnockoutComputed<string> {
            return Utils.GetValueAsString(this.Visual);
        }

        get Relevant(): KnockoutComputed<boolean> {
            return Utils.GetRelevant(this.Visual);
        }

        get Type(): KnockoutComputed<string> {
            return Utils.GetType(this.Visual);
        }

        get Appearance(): KnockoutComputed<string> {
            return Utils.GetAppearance(this.Visual);
        }

        get Label(): Visual {
            return Utils.GetLabel(this.Visual);
        }

        get Help(): Visual {
            return Utils.GetHelp(this.Visual);
        }

        get Hint(): Visual {
            return Utils.GetHint(this.Visual);
        }

        get Contents(): Visual[] {
            return Utils.GetContents(this.Visual);
        }

    }

}