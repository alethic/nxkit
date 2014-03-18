/// <reference path="Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="View.ts" />

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

        constructor(context: KnockoutBindingContext, visual: Visual) {
            super(context, visual);
            var self = this;
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

        static GetRenderableContents(visual): KnockoutObservableArray<Visual> {
            return visual.Visuals.filter((_) =>
                !this.IsMetadataVisual(_));
        }

    }

}