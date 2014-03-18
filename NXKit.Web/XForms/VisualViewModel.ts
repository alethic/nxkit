/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="../VisualViewModel.ts" />

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

        static GetValueAsString(visual: Visual): KnockoutComputed<string> {
            return ko.computed(() => {
                if (visual != null &&
                    visual.Properties['Value'] != null)
                    return visual.Properties['Value'].ValueAsString();
                else
                    return null;
            });
        }

        static GetRelevant(visual: Visual): KnockoutComputed<boolean> {
            return ko.computed(() => {
                if (visual != null &&
                    visual.Properties['Relevant'] != null)
                    return visual.Properties['Relevant'].ValueAsBoolean();
                else
                    return null;
            });
        }

        static GetAppearance(visual: Visual): KnockoutComputed<string> {
            return ko.computed(() => {
                if (visual != null &&
                    visual.Properties['Appearance'] != null)
                    return visual.Properties['Appearance'].ValueAsString();
                else
                    return null;
            });
        }

        static IsMetadataVisual(visual: Visual): boolean {
            return this.MetadataVisualTypes.some((_) =>
                visual.Type == _);
        }

        static GetLabel(visual: Visual): Visual {
            return ko.utils.arrayFirst(visual.Visuals(), (_: Visual) =>
                _.Type == 'NXKit.XForms.XFormsLabelVisual');
        }

        static GetHelp(visual: Visual): Visual {
            return ko.utils.arrayFirst(visual.Visuals(), (_: Visual) =>
                _.Type == 'NXKit.XForms.XFormsHelpVisual');
        }

        static GetHint(visual: Visual): Visual {
            return ko.utils.arrayFirst(visual.Visuals(), (_: Visual) =>
                _.Type == 'NXKit.XForms.XFormsHintVisual');
        }

        static GetAlert(visual: Visual): Visual {
            return ko.utils.arrayFirst(visual.Visuals(), (_: Visual) =>
                _.Type == 'NXKit.XForms.XFormsAlertVisual');
        }

        static IsControlVisual(visual: Visual): boolean {
            return this.ControlVisualTypes.some((_) =>
                visual.Type == _);
        }

        static HasControlVisual(visual: Visual): boolean {
            return visual.Visuals().some(_ =>
                this.IsControlVisual(_));
        }

        static GetControlVisuals(visual: Visual): Visual[] {
            return visual.Visuals().filter((_) =>
                this.IsControlVisual(_));
        }

        static GetContents(visual: Visual): Visual[] {
            return visual.Visuals().filter((_) =>
                !this.IsMetadataVisual(_));
        }

        constructor(context: KnockoutBindingContext, visual: Visual) {
            super(context, visual);
            var self = this;
        }

        get ValueAsString(): KnockoutComputed<string> {
            return VisualViewModel.GetValueAsString(this.Visual);
        }

        get Relevant(): KnockoutComputed<boolean> {
            return VisualViewModel.GetRelevant(this.Visual);
        }

        get Appearance(): KnockoutComputed<string> {
            return VisualViewModel.GetAppearance(this.Visual);
        }

        get Label(): Visual {
            return VisualViewModel.GetLabel(this.Visual);
        }

        get Help(): Visual {
            return VisualViewModel.GetHelp(this.Visual);
        }

        get Contents(): Visual[] {
            return VisualViewModel.GetContents(this.Visual);
        }

    }

}