/// <reference path="Scripts/typings/knockout/knockout.d.ts" />

module NXKit.Web.Utils {

    export function GenerateGuid(): string {
        // http://www.ietf.org/rfc/rfc4122.txt

        var s = [];
        var hexDigits = "0123456789abcdef";
        for (var i = 0; i < 36; i++) {
            s[i] = hexDigits.substr(Math.floor(Math.random() * 0x10), 1);
        }
        s[14] = "4";  // bits 12-15 of the time_hi_and_version field to 0010
        s[19] = hexDigits.substr((s[19] & 0x3) | 0x8, 1);  // bits 6-7 of the clock_seq_hi_and_reserved to 01
        s[8] = s[13] = s[18] = s[23] = "-";

        return s.join("");
    }

    export function GetTemplateName(data: any, viewModel: any, context: KnockoutBindingContext): string {

        // if the passed context stores a layout manager, get the template from it
        if (context.$data instanceof LayoutManager)
            return (<LayoutManager>context.$data).GetTemplate(data);

        // otherwise search up the tree for the first layout manager
        var l = context.$parents;
        for (var i in l) {
            var p = l[i];
            if (p instanceof LayoutManager)
                return (<LayoutManager>p).GetTemplate(data);
        }

        return null;
    }

    export function GetTemplateViewModel(valueAccessor: KnockoutObservable<any>, viewModel: any, bindingContext: KnockoutBindingContext): any {
        var value = valueAccessor() || viewModel;

        // value itself is a visual
        if (value != null &&
            ko.unwrap(value) instanceof Visual)
            return ko.unwrap(value);

        // specified data value
        if (value != null &&
            value.data != null)
            return ko.unwrap(value.data);

        // specified visual value
        if (value != null &&
            value.visual != null &&
            ko.unwrap(value.visual) instanceof Visual)
            return ko.unwrap(value.visual);

        // default to existing context
        return null;
    }

    export function GetTemplateData(valueAccessor: KnockoutObservable<any>, viewModel: any, bindingContext: KnockoutBindingContext): any {
        // extract data to be used to search for a template
        var data: any = {};
        var value = valueAccessor();

        // value is itself a visual
        if (value != null &&
            ko.unwrap(value) instanceof Visual)
            return {
                visual: (<Visual>ko.unwrap(value)).Type,
            };

        // specified visual value
        if (value != null &&
            value.visual != null &&
            ko.unwrap(value.visual) instanceof Visual)
            data.visual = (<Visual>ko.unwrap(value.visual)).Type;

        if (data.visual == null)
            if (viewModel instanceof Visual)
                data.visual = (<Visual>viewModel).Type;

        // specified data type
        if (value != null &&
            value.type != null)
            data.type = ko.unwrap(value.type);

        return data;
    }

}
