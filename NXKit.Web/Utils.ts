module NXKit.Web.Utils {

    /**
     * Tests two objects for equality.
     */
    export function DeepEquals(a: any, b: any): boolean {

        if (a == null &&
            b === null)
            return true;

        if (typeof a !== typeof b)
            return false;

        if (typeof a === 'boolean' &&
            typeof b === 'boolean')
            return a === b;

        if (typeof a === 'string' &&
            typeof b === 'string')
            return a === b;

        if (typeof a === 'number' &&
            typeof b === 'number')
            return a === b;

        if (typeof a === 'function' &&
            typeof b === 'function')
            return a.toString() === b.toString();

        for (var i in a) {
            if (a.hasOwnProperty(i)) {
                if (!b.hasOwnProperty(i))
                    return false;
                if (!Utils.DeepEquals(a[i], b[i]))
                    return false;
            }
        }

        for (var i in b) {
            if (b.hasOwnProperty(i)) {
                if (!a.hasOwnProperty(i))
                    return false;
                if (!Utils.DeepEquals(b[i], a[i]))
                    return false;
            }
        }

        return true;
    }

    /**
     * Generates a unique identifier.
     */
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

    /**
     * Returns the entire context item chain from the specified context upwards.
     */
    export function GetContextItems(context: KnockoutBindingContext): any[] {
        return [context.$data].concat(context.$parents);
    }

    /**
     * Gets the layout manager in scope of the given binding context.
     */
    export function GetLayoutManager(context: KnockoutBindingContext): LayoutManager {
        return <LayoutManager>ko.utils.arrayFirst(GetContextItems(context), _ => _ instanceof LayoutManager);
    }

    /**
     * Gets the recommended view model for the given binding information.
     */
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

    /**
     * Extracts template index data from the given binding information.
     */
    export function GetTemplateBinding(valueAccessor: KnockoutObservable<any>, viewModel: any, bindingContext: KnockoutBindingContext): any {
        return GetLayoutManager(bindingContext).ParseTemplateBinding(valueAccessor, viewModel, bindingContext, {});
    }

    /**
     * Determines the named template from the given extracted data and context.
     */
    export function GetTemplateName(bindingContext: KnockoutBindingContext, data: any): string {
        return GetLayoutManager(bindingContext).GetTemplateName(data);
    }

}
