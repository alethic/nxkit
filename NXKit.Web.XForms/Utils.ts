module NXKit.Web.XForms.Utils {

    export function GetValue(node: Node): KnockoutObservable<any> {
        return node.Value('NXKit.XForms.IValue', 'Value');
    }

    export function GetValueAsString(node: Node): KnockoutObservable<string> {
        return node.ValueAsString('NXKit.XForms.IValue', 'Value');
    }

    export function GetValueAsBoolean(node: Node): KnockoutObservable<boolean> {
        return node.ValueAsBoolean('NXKit.XForms.IValue', 'Value');
    }

    export function GetValueAsNumber(node: Node): KnockoutObservable<number> {
        return node.ValueAsNumber('NXKit.XForms.IValue', 'Value');
    }

    export function GetValueAsDate(node: Node): KnockoutObservable<Date> {
        return node.ValueAsDate('NXKit.XForms.IValue', 'Value');
    }

    export function GetRelevant(node: Node): KnockoutObservable<boolean> {
        return node.ValueAsBoolean('NXKit.XForms.IModelItemBinding', 'Relevant');
    }

    export function GetReadOnly(node: Node): KnockoutObservable<boolean> {
        return node.ValueAsBoolean('NXKit.XForms.IModelItemBinding', 'ReadOnly');
    }

    export function GetRequired(node: Node): KnockoutObservable<boolean> {
        return node.ValueAsBoolean('NXKit.XForms.IModelItemBinding', 'Required');
    }

    export function GetValid(node: Node): KnockoutObservable<boolean> {
        return node.ValueAsBoolean('NXKit.XForms.IModelItemBinding', 'Valid');
    }

    export function GetType(node: Node): KnockoutObservable<string> {
        return node.ValueAsString('NXKit.XForms.IModelItemBinding', 'DataType');
    }

    export function GetAppearance(node: Node): KnockoutObservable<string> {
        return ko.computed(() => {
            var p = node.Property('NXKit.XForms.IUIAppearance', "Appearance");
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
            _.Type == 'NXKit.XForms.Label');
    }

    export function GetHelp(node: Node): Node {
        return ko.utils.arrayFirst(node.Nodes(), (_: Node) =>
            _.Type == 'NXKit.XForms.Help');
    }

    export function GetHint(node: Node): Node {
        return ko.utils.arrayFirst(node.Nodes(), (_: Node) =>
            _.Type == 'NXKit.XForms.Hint');
    }

    export function GetAlert(node: Node): Node {
        return ko.utils.arrayFirst(node.Nodes(), (_: Node) =>
            _.Type == 'NXKit.XForms.Alert');
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