NXKit.Web.ViewModelUtil.ControlNodeTypes.push(
    'NXKit.XForms.Input',
    'NXKit.XForms.Range',
    'NXKit.XForms.Select1',
    'NXKit.XForms.Select',
    'NXKit.XForms.TextArea');

NXKit.Web.ViewModelUtil.MetadataNodeTypes.push(
    'NXKit.XForms.Label',
    'NXKit.XForms.Help',
    'NXKit.XForms.Hint',
    'NXKit.XForms.Alert');

NXKit.Web.ViewModelUtil.TransparentNodeTypes.push(
    'NXKit.XForms.Repeat',
    'NXKit.XForms.RepeatItem');

module NXKit.Web.XForms.ViewModelUtil {

    export function GetValue(node: Node): KnockoutObservable<any> {
        return node.Value('NXKit.XForms.IModelItemValue', 'Value');
    }

    export function GetValueAsString(node: Node): KnockoutObservable<string> {
        return node.ValueAsString('NXKit.XForms.IModelItemValue', 'Value');
    }

    export function GetValueAsBoolean(node: Node): KnockoutObservable<boolean> {
        return node.ValueAsBoolean('NXKit.XForms.IModelItemValue', 'Value');
    }

    export function GetValueAsNumber(node: Node): KnockoutObservable<number> {
        return node.ValueAsNumber('NXKit.XForms.IModelItemValue', 'Value');
    }

    export function GetValueAsDate(node: Node): KnockoutObservable<Date> {
        return node.ValueAsDate('NXKit.XForms.IModelItemValue', 'Value');
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

    export function IsModelItemValue(node: Node): boolean {
        return node.Interfaces['NXKit.XForms.IModelItemValue'] != null;
    }

    export function IsModelItemBinding(node: Node): boolean {
        return node.Interfaces['NXKit.XForms.IModelItemBinding'] != null;
    }

    export function GetAppearance(node: Node): KnockoutObservable<string> {
        return ko.computed(() => {
            var p = node.Property('NXKit.XForms.IUIAppearance', "Appearance");
            return p != null ? p.ValueAsString() : null;
        });
    }

    export function GetLabelNode(node: Node): Node {
        return ko.utils.arrayFirst(node.Nodes(), (_: Node) =>
            _.Type == 'NXKit.XForms.Label');
    }

    export function GetHelpNode(node: Node): Node {
        return ko.utils.arrayFirst(node.Nodes(), (_: Node) =>
            _.Type == 'NXKit.XForms.Help');
    }

    export function GetHintNode(node: Node): Node {
        return ko.utils.arrayFirst(node.Nodes(), (_: Node) =>
            _.Type == 'NXKit.XForms.Hint');
    }

    export function GetAlertNode(node: Node): Node {
        return ko.utils.arrayFirst(node.Nodes(), (_: Node) =>
            _.Type == 'NXKit.XForms.Alert');
    }

    export function GetValueNode(node: Node): Node {
        return ko.utils.arrayFirst(node.Nodes(), (_: Node) =>
            _.Type == 'NXKit.XForms.Value');
    }

}
