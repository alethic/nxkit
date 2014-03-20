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

        static GetValue(visual: Visual): KnockoutComputed<any> {
            return ko.computed<any>({
                read: () => {
                    if (visual != null &&
                        visual.Properties['Value'] != null)
                        return visual.Properties['Value'].Value();
                    else
                        return null;
                },
                write: _ => {
                    if (visual != null &&
                        visual.Properties['Value'] != null)
                        visual.Properties['Value'].Value(_);
                },
            });
        }

        static GetValueAsString(visual: Visual): KnockoutComputed<string> {
            return ko.computed<string>({
                read: () => {
                    if (visual != null &&
                        visual.Properties['Value'] != null)
                        return visual.Properties['Value'].ValueAsString();
                    else
                        return null;
                },
                write: _ => {
                    if (visual != null &&
                        visual.Properties['Value'] != null)
                        visual.Properties['Value'].Value(_);
                },
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

        static GetType(visual: Visual): KnockoutComputed<string> {
            return ko.computed(() => {
                if (visual != null &&
                    visual.Properties['Type'] != null)
                    return visual.Properties['Type'].ValueAsString();
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
            return XFormsVisualViewModel.MetadataVisualTypes
                .some((_) =>
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
            return this.ControlVisualTypes
                .some(_ =>
                    visual.Type == _);
        }

        static HasControlVisual(visual: Visual): boolean {
            return visual.Visuals()
                .some(_ =>
                    XFormsVisualViewModel.IsControlVisual(_));
        }

        static GetControlVisuals(visual: Visual): Visual[] {
            return visual.Visuals()
                .filter(_ =>
                    XFormsVisualViewModel.IsControlVisual(_));
        }

        static IsTransparentVisual(visual: Visual): boolean {
            return XFormsVisualViewModel.TransparentVisualTypes
                .some(_ =>
                    visual.Type == _);
        }

        static GetContents(visual: Visual): Visual[] {
            var l = visual.Visuals()
                .filter(_ =>
                    !XFormsVisualViewModel.IsMetadataVisual(_));

            var r = new Array<Visual>();
            for (var i = 0; i < l.length; i++) {
                var v = l[i];
                if (XFormsVisualViewModel.IsTransparentVisual(v)) {
                    var s = XFormsVisualViewModel.GetContents(v);
                    for (var j = 0; j < s.length; j++)
                        r.push(s[j]);
                } else {
                    r.push(v);
                }
            }

            return r;
        }

        constructor(context: KnockoutBindingContext, visual: Visual) {
            super(context, visual);
        }

        get Value(): KnockoutComputed<any> {
            return XFormsVisualViewModel.GetValue(this.Visual);
        }

        get ValueAsString(): KnockoutComputed<string> {
            return XFormsVisualViewModel.GetValueAsString(this.Visual);
        }

        get Relevant(): KnockoutComputed<boolean> {
            return XFormsVisualViewModel.GetRelevant(this.Visual);
        }

        get Type(): KnockoutComputed<string> {
            return XFormsVisualViewModel.GetType(this.Visual);
        }

        get Appearance(): KnockoutComputed<string> {
            return XFormsVisualViewModel.GetAppearance(this.Visual);
        }

        get Label(): Visual {
            return XFormsVisualViewModel.GetLabel(this.Visual);
        }

        get Help(): Visual {
            return XFormsVisualViewModel.GetHelp(this.Visual);
        }

        get Hint(): Visual {
            return XFormsVisualViewModel.GetHint(this.Visual);
        }

        get Contents(): Visual[] {
            return XFormsVisualViewModel.GetContents(this.Visual);
        }

    }

}