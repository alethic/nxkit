NXKit.View.ViewModelUtil.ControlNodes.push(
    '{http://www.w3.org/2002/xforms}input',
    '{http://www.w3.org/2002/xforms}range',
    '{http://www.w3.org/2002/xforms}select1',
    '{http://www.w3.org/2002/xforms}select',
    '{http://www.w3.org/2002/xforms}textarea');

NXKit.View.ViewModelUtil.MetadataNodes.push(
    '{http://www.w3.org/2002/xforms}label',
    '{http://www.w3.org/2002/xforms}help',
    '{http://www.w3.org/2002/xforms}hint',
    '{http://www.w3.org/2002/xforms}alert');

//NXKit.View.ViewModelUtil.TransparentNodes.push(
//    '{http://www.w3.org/2002/xforms}repeat');

//NXKit.View.ViewModelUtil.TransparentNodePredicates.push(
//    // repeat items are transparent
//    (n: NXKit.View.Node) =>
//        n.Interfaces['NXKit.XForms.RepeatItem'] != null &&
//        n.Property('NXKit.XForms.RepeatItem', 'IsRepeatItem').ValueAsBoolean() == true);

module NXKit.View.XForms {

    export class Constants {

        public static UINode = "NXKit.XForms.IUINode";
        public static DataNode = "NXKit.XForms.IDataNode";

    }

}

module NXKit.View.XForms.ViewModelUtil {

    export function GetValue(node: Node): KnockoutObservable<string> {
        return node.Value;
    }

    export function IsDataNode(node: Node): boolean {
        return node.Interfaces[Constants.DataNode] != null;
    }

    export function GetDataValue(node: Node): KnockoutObservable<any> {
        return node.Property(Constants.DataNode, 'Value').ValueAsString;
    }

    export function GetDataValueAsString(node: Node): KnockoutObservable<string> {
        return node.Property(Constants.DataNode, 'Value').ValueAsString;
    }

    export function GetDataValueAsBoolean(node: Node): KnockoutObservable<boolean> {
        return node.Property(Constants.DataNode, 'Value').ValueAsBoolean;
    }

    export function GetDataValueAsNumber(node: Node): KnockoutObservable<number> {
        return node.Property(Constants.DataNode, 'Value').ValueAsNumber;
    }

    export function GetDataValueAsDate(node: Node): KnockoutObservable<Date> {
        return node.Property(Constants.DataNode, 'Value').ValueAsDate;
    }

    export function GetDataType(node: Node): KnockoutObservable<string> {
        var p = node.Property(Constants.DataNode, 'DataType');
        return p != null ? p.ValueAsString : null;
    }

    export function IsUINode(node: Node): boolean {
        return node.Interfaces[Constants.UINode] != null;
    }

    export function GetRelevant(node: Node): KnockoutObservable<boolean> {
        var p = node.Property(Constants.UINode, 'Relevant');
        return p != null ? p.ValueAsBoolean : null;
    }

    export function GetReadOnly(node: Node): KnockoutObservable<boolean> {
        var p = node.Property(Constants.UINode, 'ReadOnly');
        return p != null ? p.ValueAsBoolean : null;
    }

    export function GetRequired(node: Node): KnockoutObservable<boolean> {
        var p = node.Property(Constants.UINode, 'Required');
        return p != null ? p.ValueAsBoolean : null;
    }

    export function GetValid(node: Node): KnockoutObservable<boolean> {
        var p = node.Property(Constants.UINode, 'Valid');
        return p != null ? p.ValueAsBoolean : null;
    }

    export function GetAppearance(node: Node): KnockoutObservable<string> {
        var p = node.Property(Constants.UINode, 'Appearance');
        return p != null ? p.ValueAsString : null;
    }

    export function GetDataItem(node: Node): KnockoutObservable<any> {
        return ko.computed<any>(() => {
            return [
                GetValid(node)() ? 'valid' : 'invalid',
                GetRelevant(node)() ? 'enabled' : 'disabled',
                GetReadOnly(node)() ? 'readonly' : 'readwrite',
                GetRequired(node)() ? 'required' : 'optional',
            ].join(' ');
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
