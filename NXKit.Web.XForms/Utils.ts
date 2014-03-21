module NXKit.Web.XForms.Utils {

    export function GetValue(visual: Visual): KnockoutComputed<any> {
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

    export function GetValueAsString(visual: Visual): KnockoutComputed<string> {
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
                    visual.Properties['Value'].ValueAsString(_);
            },
        });
    }

    export function GetValueAsBoolean(visual: Visual): KnockoutComputed<boolean> {
        return ko.computed<boolean>({
            read: () => {
                if (visual != null &&
                    visual.Properties['Value'] != null)
                    return visual.Properties['Value'].ValueAsBoolean();
                else
                    return null;
            },
            write: _ => {
                if (visual != null &&
                    visual.Properties['Value'] != null)
                    visual.Properties['Value'].ValueAsBoolean(_);
            },
        });
    }

    export function GetValueAsNumber(visual: Visual): KnockoutComputed<number> {
        return ko.computed<number>({
            read: () => {
                if (visual != null &&
                    visual.Properties['Value'] != null)
                    return visual.Properties['Value'].ValueAsNumber();
                else
                    return null;
            },
            write: _ => {
                if (visual != null &&
                    visual.Properties['Value'] != null)
                    visual.Properties['Value'].ValueAsNumber(_);
            },
        });
    }

    export function GetRelevant(visual: Visual): KnockoutComputed<boolean> {
        return ko.computed(() => {
            if (visual != null &&
                visual.Properties['Relevant'] != null)
                return visual.Properties['Relevant'].ValueAsBoolean();
            else
                return null;
        });
    }

    export function GetType(visual: Visual): KnockoutComputed<string> {
        return ko.computed(() => {
            if (visual != null &&
                visual.Properties['Type'] != null)
                return visual.Properties['Type'].ValueAsString();
            else
                return null;
        });
    }

    export function GetAppearance(visual: Visual): KnockoutComputed<string> {
        return ko.computed(() => {
            if (visual != null &&
                visual.Properties['Appearance'] != null)
                return visual.Properties['Appearance'].ValueAsString();
            else
                return null;
        });
    }

    export function IsMetadataVisual(visual: Visual): boolean {
        return XFormsVisualViewModel.MetadataVisualTypes
            .some((_) =>
                visual.Type == _);
    }

    export function GetLabel(visual: Visual): Visual {
        return ko.utils.arrayFirst(visual.Visuals(), (_: Visual) =>
            _.Type == 'NXKit.XForms.XFormsLabelVisual');
    }

    export function GetHelp(visual: Visual): Visual {
        return ko.utils.arrayFirst(visual.Visuals(), (_: Visual) =>
            _.Type == 'NXKit.XForms.XFormsHelpVisual');
    }

    export function GetHint(visual: Visual): Visual {
        return ko.utils.arrayFirst(visual.Visuals(), (_: Visual) =>
            _.Type == 'NXKit.XForms.XFormsHintVisual');
    }

    export function GetAlert(visual: Visual): Visual {
        return ko.utils.arrayFirst(visual.Visuals(), (_: Visual) =>
            _.Type == 'NXKit.XForms.XFormsAlertVisual');
    }

    export function IsControlVisual(visual: Visual): boolean {
        return this.ControlVisualTypes
            .some(_ =>
                visual.Type == _);
    }

    export function HasControlVisual(visual: Visual): boolean {
        return visual.Visuals()
            .some(_ =>
                IsControlVisual(_));
    }

    export function GetControlVisuals(visual: Visual): Visual[] {
        return visual.Visuals()
            .filter(_ =>
                IsControlVisual(_));
    }

    export function IsTransparentVisual(visual: Visual): boolean {
        return XFormsVisualViewModel.TransparentVisualTypes
            .some(_ =>
                visual.Type == _);
    }

    export function GetContents(visual: Visual): Visual[] {
        var l = visual.Visuals()
            .filter(_ =>
                !IsMetadataVisual(_));

        var r = new Array<Visual>();
        for (var i = 0; i < l.length; i++) {
            var v = l[i];
            if (IsTransparentVisual(v)) {
                var s = GetContents(v);
                for (var j = 0; j < s.length; j++)
                    r.push(s[j]);
            } else {
                r.push(v);
            }
        }

        return r;
    }

} 