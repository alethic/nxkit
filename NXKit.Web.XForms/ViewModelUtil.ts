NXKit.Web.ViewModelUtil.ControlNodes.push(
    '{http://www.w3.org/2002/xforms}input',
    '{http://www.w3.org/2002/xforms}range',
    '{http://www.w3.org/2002/xforms}select1',
    '{http://www.w3.org/2002/xforms}select',
    '{http://www.w3.org/2002/xforms}textarea');

NXKit.Web.ViewModelUtil.MetadataNodes.push(
    '{http://www.w3.org/2002/xforms}label',
    '{http://www.w3.org/2002/xforms}help',
    '{http://www.w3.org/2002/xforms}hint',
    '{http://www.w3.org/2002/xforms}alert');

NXKit.Web.ViewModelUtil.TransparentNodes.push(
    '{http://www.w3.org/2002/xforms}repeat',
    '{http://www.w3.org/2002/xforms}repeatItem');

module NXKit.Web.XForms.ViewModelUtil {

    function GetUIBinding(node: Node): KnockoutObservable<Node> {
        return node.Value('NXKit.XForms.IUIBindingNode', 'UIBinding')();
    }

    export function HasUIBinding(node: Node): boolean {
        return GetUIBinding(node)() != null;
    }

    export function GetValue(node: Node): KnockoutObservable<any> {
        if (!HasUIBinding(node))
            return null;

        return GetUIBinding(node)().Value('NXKit.XForms.UIBinding', 'Value');
    }

    export function GetValueAsString(node: Node): KnockoutObservable<string> {
        if (!HasUIBinding(node))
            return null;

        return GetUIBinding(node)().ValueAsString('NXKit.XForms.UIBinding', 'Value');
    }

    export function GetValueAsBoolean(node: Node): KnockoutObservable<boolean> {
        if (!HasUIBinding(node))
            return null;

        return GetUIBinding(node)().ValueAsBoolean('NXKit.XForms.UIBinding', 'Value');
    }

    export function GetValueAsNumber(node: Node): KnockoutObservable<number> {
        if (!HasUIBinding(node))
            return null;

        return GetUIBinding(node)().ValueAsNumber('NXKit.XForms.UIBinding', 'Value');
    }

    export function GetValueAsDate(node: Node): KnockoutObservable<Date> {
        if (!HasUIBinding(node))
            return null;

        return GetUIBinding(node)().ValueAsDate('NXKit.XForms.UIBinding', 'Value');
    }

    export function GetRelevant(node: Node): KnockoutObservable<boolean> {
        if (!HasUIBinding(node))
            return null;

        return GetUIBinding(node)().ValueAsBoolean('NXKit.XForms.UIBinding', 'Relevant');
    }

    export function GetReadOnly(node: Node): KnockoutObservable<boolean> {
        if (!HasUIBinding(node))
            return null;

        return GetUIBinding(node)().ValueAsBoolean('NXKit.XForms.UIBinding', 'ReadOnly');
    }

    export function GetRequired(node: Node): KnockoutObservable<boolean> {
        if (!HasUIBinding(node))
            return null;

        return GetUIBinding(node)().ValueAsBoolean('NXKit.XForms.UIBinding', 'Required');
    }

    export function GetValid(node: Node): KnockoutObservable<boolean> {
        if (!HasUIBinding(node))
            return null;

        return GetUIBinding(node)().ValueAsBoolean('NXKit.XForms.UIBinding', 'Valid');
    }

    export function GetType(node: Node): KnockoutObservable<string> {
        if (!HasUIBinding(node))
            return null;

        return GetUIBinding(node)().ValueAsString('NXKit.XForms.UIBinding', 'DataType');
    }

    export function GetAppearance(node: Node): KnockoutObservable<string> {
        return ko.computed(() => {
            var p = node.Property('NXKit.XForms.IUIAppearance', "Appearance");
            return p != null ? p.ValueAsString() : null;
        });
    }

    export function GetLabelNode(node: Node): Node {
        return ko.utils.arrayFirst(node.Nodes(), (_: Node) =>
            _.Name == '{http://www.w3.org/2002/xforms}label');
    }

    export function GetHelpNode(node: Node): Node {
        return ko.utils.arrayFirst(node.Nodes(), (_: Node) =>
            _.Name == '{http://www.w3.org/2002/xforms}help');
    }

    export function GetHintNode(node: Node): Node {
        return ko.utils.arrayFirst(node.Nodes(), (_: Node) =>
            _.Name == '{http://www.w3.org/2002/xforms}hint');
    }

    export function GetAlertNode(node: Node): Node {
        return ko.utils.arrayFirst(node.Nodes(), (_: Node) =>
            _.Name == '{http://www.w3.org/2002/xforms}alert');
    }

    export function GetValueNode(node: Node): Node {
        return ko.utils.arrayFirst(node.Nodes(), (_: Node) =>
            _.Name == '{http://www.w3.org/2002/xforms}value');
    }

}
