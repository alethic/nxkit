/// <reference path="Scripts/typings/knockout/knockout.d.ts" />

module NXKit.Web.Utils {

    export function DeepEquals(a: any, b: any): boolean {
        for (var i in a) {
            if (a.hasOwnProperty(i)) {
                if (!b.hasOwnProperty(i))
                    return false;
                if (a[i] != b[i])
                    return false;
            }
        }

        for (var i in b) {
            if (b.hasOwnProperty(i)) {
                if (!a.hasOwnProperty(i))
                    return false;
                if (b[i] != a[i])
                    return false;
            }
        }

        return true;
    }

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

    export function GetLayoutManager(context: KnockoutBindingContext): LayoutManager {
        // advance through context until we find a layout manager
        var ctx = context;
        while (ctx != null) {
            if (ctx.$data instanceof LayoutManager)
                return <LayoutManager>ctx.$data;

            ctx = ctx.$parentContext || null;
        }

        // return the layout manager
        return new DefaultLayoutManager(context);
    }

    export function GetVisualTemplateData(data: any): any {
        // extract 

        var _: any = {};
        if (data == null)
            return _;

        // specified visual value
        if (data.visual != null &&
            data.visual instanceof Visual)
            _.visual = data.visual.Type;

        // value itself is a visual
        if (data instanceof Visual)
            _.visual = data.visual.Type;

        if (data.type != null)
            _.type = data.type;

        return _;
    }

}
