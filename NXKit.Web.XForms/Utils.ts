module NXKit.Web.XForms.Utils {

    export function GetProperty(node: Node, name: string): Property {
        if (node != null &&
            node.Properties[name] != null)
            return node.Properties[name];
        else
            return null;
    }

    export function GetValue(node: Node): KnockoutComputed<any> {
        return ko.computed<any>({
            read: () => {
                var p = GetProperty(node, "Value");
                return p != null ? p.Value() : null;
            },
            write: _ => {
                var p = GetProperty(node, "Value");
                if (p != null)
                    p.Value(_);
            },
        });
    }

    export function GetValueAsString(node: Node): KnockoutComputed<string> {
        return ko.computed<string>({
            read: () => {
                var p = GetProperty(node, "Value");
                return p != null ? p.ValueAsString() : null;
            },
            write: _ => {
                var p = GetProperty(node, "Value");
                if (p != null)
                    p.ValueAsString(_);
            },
        });
    }

    export function GetValueAsBoolean(node: Node): KnockoutComputed<boolean> {
        return ko.computed<boolean>({
            read: () => {
                var p = GetProperty(node, "Value");
                return p != null ? p.ValueAsBoolean() : null;
            },
            write: _ => {
                var p = GetProperty(node, "Value");
                if (p != null)
                    p.ValueAsBoolean(_);
            },
        });
    }

    export function GetValueAsNumber(node: Node): KnockoutComputed<number> {
        return ko.computed<number>({
            read: () => {
                var p = GetProperty(node, "Value");
                return p != null ? p.ValueAsNumber() : null;
            },
            write: _ => {
                var p = GetProperty(node, "Value");
                if (p != null)
                    p.ValueAsNumber(_);
            },
        });
    }

    export function GetRelevant(node: Node): KnockoutComputed<boolean> {
        return ko.computed(() => {
            var p = GetProperty(node, "Relevant");
            return p != null ? p.ValueAsBoolean() : null;
        });
    }

    export function GetReadOnly(node: Node): KnockoutComputed<boolean> {
        return ko.computed(() => {
            var p = GetProperty(node, "ReadOnly");
            return p != null ? p.ValueAsBoolean() : null;
        });
    }

    export function GetRequired(node: Node): KnockoutComputed<boolean> {
        return ko.computed(() => {
            var p = GetProperty(node, "Required");
            return p != null ? p.ValueAsBoolean() : null;
        });
    }

    export function GetValid(node: Node): KnockoutComputed<boolean> {
        return ko.computed(() => {
            var p = GetProperty(node, "Valid");
            return p != null ? p.ValueAsBoolean() : null;
        });
    }

    export function GetType(node: Node): KnockoutComputed<string> {
        return ko.computed(() => {
            var p = GetProperty(node, "ItemType");
            return p != null ? p.ValueAsString() : null;
        });
    }

    export function GetAppearance(node: Node): KnockoutComputed<string> {
        return ko.computed(() => {
            var p = GetProperty(node, "Appearance");
            return p != null ? p.ValueAsString() : null;
        });
    }

    export function IsMetadataNode(node: Node): boolean {
        return XFormsNodeViewModel.MetadataNodeTypes
            .some((_) =>
                node.Type == _);
    }

    export function GetLabel(node: Node): Node {
        return ko.utils.arrayFirst(node.Nodes(), (_: Node) =>
            _.Type == 'NXKit.XForms.LabelElement');
    }

    export function GetHelp(node: Node): Node {
        return ko.utils.arrayFirst(node.Nodes(), (_: Node) =>
            _.Type == 'NXKit.XForms.HelpElement');
    }

    export function GetHint(node: Node): Node {
        return ko.utils.arrayFirst(node.Nodes(), (_: Node) =>
            _.Type == 'NXKit.XForms.HintElement');
    }

    export function GetAlert(node: Node): Node {
        return ko.utils.arrayFirst(node.Nodes(), (_: Node) =>
            _.Type == 'NXKit.XForms.AlertElement');
    }

    export function IsControlNode(node: Node): boolean {
        return this.ControlNodeTypes
            .some(_ =>
                node.Type == _);
    }

    export function HasControlNode(node: Node): boolean {
        return node.Nodes()
            .some(_ =>
                IsControlNode(_));
    }

    export function GetControlNodes(node: Node): Node[] {
        return node.Nodes()
            .filter(_ =>
                IsControlNode(_));
    }

    export function IsTransparentNode(node: Node): boolean {
        return XFormsNodeViewModel.TransparentNodeTypes
            .some(_ =>
                node.Type == _);
    }

    export function GetContents(node: Node): Node[] {
        var l = node.Nodes()
            .filter(_ =>
                !IsMetadataNode(_));

        var r = new Array<Node>();
        for (var i = 0; i < l.length; i++) {
            var v = l[i];
            if (IsTransparentNode(v)) {
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