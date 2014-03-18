/// <reference path="Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="Scripts/typings/knockout/knockout.d.ts" />

module NXKit.Web.XForms {

    export class VisualViewModel extends NXKit.Web.VisualViewModel {

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

        static GetAppearance(visual): string {
            if (visual != null &&
                visual.Properties.Appearance != null &&
                visual.Properties.Appearance.Value() != null)
                return visual.Properties.Appearance.Value();
            else
                return "full";
        }

        static IsMetadataVisual(visual): boolean {
            return this.MetadataVisualTypes.some((_) =>
                visual.Type == _);
        }

        static GetLabel(visual): Visual {
            return ko.utils.arrayFirst(visual.Visuals(), (_: Visual) =>
                _.Type == 'NXKit.XForms.XFormsLabelVisual');
        }

        static GetHelp(visual): Visual {
            return ko.utils.arrayFirst(visual.Visuals(), (_: Visual) =>
                _.Type == 'NXKit.XForms.XFormsHelpVisual');
        }

        static GetHint(visual): Visual {
            return ko.utils.arrayFirst(visual.Visuals(), (_: Visual) =>
                _.Type == 'NXKit.XForms.XFormsHintVisual');
        }

        static GetAlert(visual): Visual {
            return ko.utils.arrayFirst(visual.Visuals(), (_: Visual) =>
                _.Type == 'NXKit.XForms.XFormsAlertVisual');
        }

        static IsControlVisual(visual): boolean {
            return this.ControlVisualTypes.some((_) =>
                visual.Type == _);
        }

        static HasControlVisual(visual): boolean {
            return visual.Visuals().some(_ =>
                this.IsControlVisual(_));
        }

        static GetControlVisuals(visual): KnockoutObservableArray<Visual> {
            return visual.Visuals.filter((_) =>
                this.IsControlVisual(_));
        }

        static GetContents(visual): KnockoutObservableArray<Visual> {
            return visual.Visuals.filter((_) =>
                !this.IsMetadataVisual(_));
        }

        constructor(context: KnockoutBindingContext, visual: Visual) {
            super(context, visual);
            var self = this;
        }

        get Label(): Visual {
            return VisualViewModel.GetLabel(this.Visual);
        }

        get Help(): Visual {
            return VisualViewModel.GetHelp(this.Visual);
        }

        get Contents(): KnockoutObservableArray<Visual> {
            return VisualViewModel.GetContents(this.Visual);
        }

        get Appearance(): string {
            return VisualViewModel.GetAppearance(this);
        }

    }

}