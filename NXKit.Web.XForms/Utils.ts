module NXKit.Web.XForms.Utils {

    export function GetProperty(visual: Visual, name: string): Property {
        if (visual != null &&
            visual.Properties[name] != null)
            return visual.Properties[name];
        else
            return null;
    }

    export function GetValue(visual: Visual): KnockoutComputed<any> {
        return ko.computed<any>({
            read: () => {
                var p = GetProperty(visual, "Value");
                return p != null ? p.Value() : null;
            },
            write: _ => {
                var p = GetProperty(visual, "Value");
                if (p != null)
                    p.Value(_);
            },
        });
    }

    export function GetValueAsString(visual: Visual): KnockoutComputed<string> {
        return ko.computed<string>({
            read: () => {
                var p = GetProperty(visual, "Value");
                return p != null ? p.ValueAsString() : null;
            },
            write: _ => {
                var p = GetProperty(visual, "Value");
                if (p != null)
                    p.ValueAsString(_);
            },
        });
    }

    export function GetValueAsBoolean(visual: Visual): KnockoutComputed<boolean> {
        return ko.computed<boolean>({
            read: () => {
                var p = GetProperty(visual, "Value");
                return p != null ? p.ValueAsBoolean() : null;
            },
            write: _ => {
                var p = GetProperty(visual, "Value");
                if (p != null)
                    p.ValueAsBoolean(_);
            },
        });
    }

    export function GetValueAsNumber(visual: Visual): KnockoutComputed<number> {
        return ko.computed<number>({
            read: () => {
                var p = GetProperty(visual, "Value");
                return p != null ? p.ValueAsNumber() : null;
            },
            write: _ => {
                var p = GetProperty(visual, "Value");
                if (p != null)
                    p.ValueAsNumber(_);
            },
        });
    }

    export function GetRelevant(visual: Visual): KnockoutComputed<boolean> {
        return ko.computed(() => {
            var p = GetProperty(visual, "Relevant");
            return p != null ? p.ValueAsBoolean() : null;
        });
    }

    export function GetReadOnly(visual: Visual): KnockoutComputed<boolean> {
        return ko.computed(() => {
            var p = GetProperty(visual, "ReadOnly");
            return p != null ? p.ValueAsBoolean() : null;
        });
    }

    export function GetRequired(visual: Visual): KnockoutComputed<boolean> {
        return ko.computed(() => {
            var p = GetProperty(visual, "Required");
            return p != null ? p.ValueAsBoolean() : null;
        });
    }

    export function GetValid(visual: Visual): KnockoutComputed<boolean> {
        return ko.computed(() => {
            var p = GetProperty(visual, "Valid");
            return p != null ? p.ValueAsBoolean() : null;
        });
    }

    export function GetType(visual: Visual): KnockoutComputed<string> {
        return ko.computed(() => {
            var p = GetProperty(visual, "Type");
            return p != null ? p.ValueAsString() : null;
        });
    }

    export function GetAppearance(visual: Visual): KnockoutComputed<string> {
        return ko.computed(() => {
            var p = GetProperty(visual, "Appearance");
            return p != null ? p.ValueAsString() : null;
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